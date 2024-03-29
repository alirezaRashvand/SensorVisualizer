﻿using SensorVisualizer.Core;
using SensorVisualizer.Core.Clients;
using SensorVisualizer.Core.Clients.OpenWeatherMap;
using SensorVisualizer.Core.Clients.OpenWeatherMap.Models;
using SensorVisualizer.Core.Models;
using SensorVisualizer.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensorVisualizer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowDataContext Model => DataContext as MainWindowDataContext;
        private readonly ISensorVisualizerClient client;
        private readonly OpenWeatherMapClient weatherMapClient;
        private CancellationTokenSource _sensorViewTokenSource;
        private int mapZoomLevel = 18;

        public MainWindow()
        {
            InitializeComponent();
            client = new TestSensorVisualizerClient();
            weatherMapClient = new OpenWeatherMapClient(AppConsts.OpenWeatherMapApiKey);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = FetchSensors(true);
        }

        private void PreviosPageButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Model.PagingContext.CurrentPage > 1)
            {
                Model.PagingContext.CurrentPage--;
                _ = FetchSensors();
            }
        }

        private void NextPageButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Model.PagingContext.CurrentPage != Model.PagingContext.TotalPages)
            {
                Model.PagingContext.CurrentPage++;
                _ = FetchSensors();
            }
        }

        private void Search_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _ = FetchSensors(true);
        }

        private void Refresh_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _ = FetchSensors(true);
        }

        private void SensorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem is Sensor selected && selected != null)
            {
                CustomPointsListBox.SelectedItem = null;
                Model.ShouldSelect = false;
                if (Model.SensorViewLoading)
                    _sensorViewTokenSource.Cancel();
                else
                    Model.SensorViewLoading = true;
                if (_sensorViewTokenSource != null) _sensorViewTokenSource.Dispose();
                _sensorViewTokenSource = new CancellationTokenSource();
                GetSensorViewInformationsAsync(selected.Id, _sensorViewTokenSource.Token).ContinueWith(OnSensorViewModelCompleted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                Model.SensorViewLoading = false;
                if (CustomPointsListBox.SelectedItem == null) Model.ShouldSelect = true;
            }
        }

        private Task FetchSensors(bool refreshed = false)
        {

            if (!Model.SensorsLoading) Model.SensorsLoading = true;
            int currentPage = refreshed ? 1 : Model.PagingContext.CurrentPage;
            int skipCount = currentPage == 0 ? 0 : (currentPage - 1) * AppConsts.SensorsPageSize;
            return client.GetSensorsAsync(skipCount, AppConsts.SensorsPageSize, Search_textbox.Text).ContinueWith(OnSensorsFetchingDone, refreshed, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task<SensorViewModel> GetSensorViewInformationsAsync(int id, CancellationToken cancellationToken)
        {
            Sensor sensor = await client.GetSensorAsync(id, cancellationToken);
            CurrentWeather weatherInfo = await weatherMapClient.CurrentWeatherAsync(sensor.Latitude, sensor.Longitude, cancellationToken);
            return new SensorViewModel(sensor, weatherInfo);
        }

        private void OnSensorsFetchingDone(Task<PagedList<Sensor>> sensorsTask, object refreshed)
        {
            if (sensorsTask.IsCompletedSuccessfully)
            {
                PagedList<Sensor> result = sensorsTask.Result;
                Model.Sensors.Clear();
                foreach (Sensor item in result.Data)
                    Model.Sensors.Add(item);
                if ((bool)refreshed)
                    Model.PagingContext.TotalPages = (result.TotalCount / AppConsts.SensorsPageSize) + Convert.ToInt32(result.TotalCount % AppConsts.SensorsPageSize != 0);
                Model.SensorsLoading = false;
            }
            else
            {
                _ = MessageBox.Show("Cannot fetch sensors...\nTrying again...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _ = FetchSensors();
            }
        }

        private void OnSensorViewModelCompleted(Task<SensorViewModel> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                SensorViewModel result = task.Result;
                SensorViewLocationImage.ImageSource = new BitmapImage(MapBoxStaticImageGenerator.GenerateStaticImageUri(result.Sensor.Longitude, result.Sensor.Latitude, 18, 500, 500));
                SensorViewTitle.Text = result.Sensor.Name;
                SensorViewLocation.Text = $"({result.Sensor.Latitude:F}, {result.Sensor.Longitude:F}) {result.WeatherInfo.CityName}";
                SensorViewMeasuredTemp.Text = $"{result.Sensor.Temperature.ToString("F", CultureInfo.InvariantCulture)} °C";
                SensorViewRealTemp.Text = $"{result.WeatherInfo.Temperature.Temp.ToString("F", CultureInfo.InvariantCulture)} °C";
                Model.SensorViewLoading = false;
            }
            else
            {
                if (!task.IsCanceled)
                    _ = MessageBox.Show("Cannot fetch sensor informations...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnMappointViewModelCompleted(Task<MapPointWeatherModel> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                MapPointWeatherModel result = task.Result;
                SensorViewLocationImage.ImageSource = new BitmapImage(MapBoxStaticImageGenerator.GenerateStaticImageUri(result.MapPoint.Longitude, result.MapPoint.Latitude, mapZoomLevel, 500, 500));
                SensorViewTitle.Text = result.MapPoint.Name;
                SensorViewLocation.Text = $"({result.MapPoint.Latitude:F}, {result.MapPoint.Longitude:F}) {result.Weather.CityName}";
                SensorViewMeasuredTemp.Text = "N/A";
                SensorViewRealTemp.Text = $"{result.Weather.Temperature.Temp.ToString("F", CultureInfo.InvariantCulture)} °C";
                Model.SensorViewLoading = false;
            }
            else
            {
                if (!task.IsCanceled)
                    _ = MessageBox.Show("Cannot fetch weather informations...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CustomPointAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapPointWindow form = new();
            _ = form.ShowDialog();
            if (form.Saved) Model.CustomPoints.Add(form.MapPoint);
        }

        private void CustomPointEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CustomPointsListBox.SelectedItem == null)
            {
                _ = MessageBox.Show("Select potint to edit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MapPointWindow form = new(CustomPointsListBox.SelectedItem as MapPoint);
            _ = form.ShowDialog();
        }

        private void CustomPointDelete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CustomPointsListBox.SelectedItem == null)
            {
                _ = MessageBox.Show("Select potint", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("You shure?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Model.CustomPoints.Remove(CustomPointsListBox.SelectedItem as MapPoint);
        }

        private void CustomPointsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem is MapPoint selected && selected != null)
            {
                SensorsListBox.SelectedItem = null;
                Model.ShouldSelect = false;
                if (Model.SensorViewLoading)
                    _sensorViewTokenSource.Cancel();
                else
                    Model.SensorViewLoading = true;
                if (_sensorViewTokenSource != null) _sensorViewTokenSource.Dispose();
                _sensorViewTokenSource = new CancellationTokenSource();
                GetMapPointViewInformationsAsync(selected, _sensorViewTokenSource.Token).ContinueWith(OnMappointViewModelCompleted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                Model.SensorViewLoading = false;
                if (SensorsListBox.SelectedItem == null) Model.ShouldSelect = true;
            }
        }

        private async Task<MapPointWeatherModel> GetMapPointViewInformationsAsync(MapPoint selected, CancellationToken token)
        {
            return new MapPointWeatherModel
            {
                MapPoint = selected,
                Weather = await weatherMapClient.CurrentWeatherAsync(selected.Latitude, selected.Longitude, _sensorViewTokenSource.Token)
            };
        }

        private void MapZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (mapZoomLevel-- == AppConsts.minZoomLevel) MapZoomOut.IsEnabled = false;
            MapZoomIn.IsEnabled = true;
            //mapZoomLevel = Math.Clamp(--mapZoomLevel, AppConsts.minZoomLevel, AppConsts.maxZoomLevel);
            if (CustomPointsListBox.SelectedItem is MapPoint point && point != null)
                SensorViewLocationImage.ImageSource = new BitmapImage(MapBoxStaticImageGenerator.GenerateStaticImageUri(point.Longitude, point.Latitude, mapZoomLevel, 500, 500));
            else if (SensorsListBox.SelectedItem is Sensor sensor && sensor != null)
                SensorViewLocationImage.ImageSource = new BitmapImage(MapBoxStaticImageGenerator.GenerateStaticImageUri(sensor.Longitude, sensor.Latitude, mapZoomLevel, 500, 500));
            else
                _ = MessageBox.Show("Select point or sensor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void MapZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (mapZoomLevel++ == AppConsts.maxZoomLevel) MapZoomIn.IsEnabled = false;
            MapZoomOut.IsEnabled = true;
            //mapZoomLevel = Math.Clamp(++mapZoomLevel, AppConsts.minZoomLevel, AppConsts.maxZoomLevel);
            if (CustomPointsListBox.SelectedItem is MapPoint point && point != null)
                SensorViewLocationImage.ImageSource = new BitmapImage(MapBoxStaticImageGenerator.GenerateStaticImageUri(point.Longitude, point.Latitude, mapZoomLevel, 500, 500));
            else if (SensorsListBox.SelectedItem is Sensor sensor && sensor != null)
                SensorViewLocationImage.ImageSource = new BitmapImage(MapBoxStaticImageGenerator.GenerateStaticImageUri(sensor.Longitude, sensor.Latitude, mapZoomLevel, 500, 500));
            else
                _ = MessageBox.Show("Select point or sensor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
