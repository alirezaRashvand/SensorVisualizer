using SensorVisualizer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SensorVisualizer.WPF
{
    /// <summary>
    /// Interaction logic for MapPointWindow.xaml
    /// </summary>
    public partial class MapPointWindow : Window
    {
        public MapPoint MapPoint { get; private set; }
        public bool Saved { get; private set; }
        private readonly Regex decimalNumberRegex;

        public MapPointWindow(MapPoint mapPoint = null)
        {
            Saved = false;
            MapPoint = mapPoint ?? new MapPoint();
            Title = mapPoint == null ? "Create map point" : "Edit map point";
            InitializeComponent();
            decimalNumberRegex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
        }

        private void DecimalTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            e.Handled = !decimalNumberRegex.IsMatch(textBox.Text.Insert(textBox.SelectionStart, e.Text));
        }

        private void Save_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtLongitude.Text) || string.IsNullOrWhiteSpace(txtLatitude.Text))
            {
                _ = MessageBox.Show(this, "All fields are required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Saved = true;
            Close();
        }

        private void Cancel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
