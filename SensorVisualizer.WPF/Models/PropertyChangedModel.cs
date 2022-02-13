using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SensorVisualizer.WPF.Models
{
    public abstract class PropertyChangedModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void PropertiesChanged(params string[] names)
        {
            foreach (string item in names)
                OnPropertyChanged(item);
        }
    }
}
