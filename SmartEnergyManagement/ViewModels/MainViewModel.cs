using SmartEnergyManagement.Models;
using System.Windows.Input;

namespace SmartEnergyManagement.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private EnergySystem _energySystem;
        private ICommand _refreshCommand;

        public EnergySystem EnergySystem
        {
            get => _energySystem;
            set
            {
                if (_energySystem != value)
                {
                    _energySystem = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(
                        param => RefreshData(),
                        param => true
                    );
                }
                return _refreshCommand;
            }
        }

        public MainViewModel()
        {
            EnergySystem = new EnergySystem();
        }

        private void RefreshData()
        {
            // 手动刷新数据（虽然系统已经自动刷新）
            OnPropertyChanged(nameof(EnergySystem));
        }
    }

    public class ViewModelBase : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly System.Action<object> _execute;
        private readonly System.Predicate<object> _canExecute;

        public event System.EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(System.Action<object> execute, System.Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new System.ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
    