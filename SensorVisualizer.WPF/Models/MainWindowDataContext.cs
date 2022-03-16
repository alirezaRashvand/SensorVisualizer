using SensorVisualizer.Core.Models;
using System.Collections.ObjectModel;

namespace SensorVisualizer.WPF.Models
{
    public class MainWindowDataContext : PropertyChangedModel
    {
        private bool _sensorsLoading;
        private bool _sensorViewLoading;
        private bool _shouldSelect;

        public PagingContextModel PagingContext { get; }
        public ObservableCollection<Sensor> Sensors { get; set; }
        public ObservableCollection<MapPoint> CustomPoints { get; set; }
        public bool SensorsLoading
        {
            get => _sensorsLoading;
            set
            {
                _sensorsLoading = value;
                OnPropertyChanged();
            }
        }
        public bool SensorViewLoading
        {
            get => _sensorViewLoading;
            set
            {
                _sensorViewLoading = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldSelect
        {
            get => _shouldSelect;
            set
            {
                _shouldSelect = value;
                OnPropertyChanged();
            }
        }

        public MainWindowDataContext()
        {
            PagingContext = new PagingContextModel();
            Sensors = new ObservableCollection<Sensor>();
            CustomPoints = new ObservableCollection<MapPoint>();
            _sensorsLoading = true;
            _shouldSelect = true;
        }
    }
}
