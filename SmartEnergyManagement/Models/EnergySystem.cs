using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace SmartEnergyManagement.Models
{
    public class EnergySystem : INotifyPropertyChanged
    {
        private double _windPower;
        private double _solarPower;
        private double _totalPower;
        private double _batteryCapacity;
        private double _batteryPercentage;
        private double _householdPower;
        private Timer _updateTimer;
        private Random _random;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SecurityParameter> SecurityParameters { get; set; }

        public double WindPower
        {
            get => _windPower;
            set
            {
                if (_windPower != value)
                {
                    _windPower = value;
                    OnPropertyChanged();
                    UpdateTotalPower();
                }
            }
        }

        public double SolarPower
        {
            get => _solarPower;
            set
            {
                if (_solarPower != value)
                {
                    _solarPower = value;
                    OnPropertyChanged();
                    UpdateTotalPower();
                }
            }
        }

        public double TotalPower
        {
            get => _totalPower;
            set
            {
                if (_totalPower != value)
                {
                    _totalPower = value;
                    OnPropertyChanged();
                }
            }
        }

        public double BatteryCapacity
        {
            get => _batteryCapacity;
            set
            {
                if (_batteryCapacity != value)
                {
                    _batteryCapacity = value;
                    OnPropertyChanged();
                    UpdateBatteryPercentage();
                }
            }
        }

        public double BatteryPercentage
        {
            get => _batteryPercentage;
            set
            {
                if (_batteryPercentage != value)
                {
                    _batteryPercentage = value;
                    OnPropertyChanged();
                }
            }
        }

        public double HouseholdPower
        {
            get => _householdPower;
            set
            {
                if (_householdPower != value)
                {
                    _householdPower = value;
                    OnPropertyChanged();
                }
            }
        }

        public EnergySystem()
        {
            _random = new Random();
            SecurityParameters = new ObservableCollection<SecurityParameter>
            {
                new SecurityParameter { Name = "温度", Value = 25, MinValue = 0, MaxValue = 60, Unit = "°C" },
                new SecurityParameter { Name = "电压", Value = 220, MinValue = 210, MaxValue = 230, Unit = "V" },
                new SecurityParameter { Name = "电流", Value = 5, MinValue = 0, MaxValue = 15, Unit = "A" }
            };

            InitializeSystem();
            StartMonitoring();
        }

        private void InitializeSystem()
        {
            WindPower = _random.NextDouble() * 10;
            SolarPower = _random.NextDouble() * 15;
            BatteryCapacity = 100;
            HouseholdPower = _random.NextDouble() * 8;
        }

        private void StartMonitoring()
        {
            _updateTimer = new Timer(1000);
            _updateTimer.Elapsed += UpdateTimerElapsed;
            _updateTimer.Start();
        }

        private void UpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateEnergyParameters();
            UpdateSecurityParameters();
        }

        private void UpdateEnergyParameters()
        {
            WindPower = Math.Max(0, WindPower + (_random.NextDouble() - 0.5) * 2);
            SolarPower = Math.Max(0, SolarPower + (_random.NextDouble() - 0.5) * 3);
            
            double powerDifference = TotalPower - HouseholdPower;
            
            if (powerDifference > 0)
            {
                BatteryCapacity = Math.Min(100, BatteryCapacity + powerDifference * 0.1);
            }
            else
            {
                double powerFromBattery = Math.Min(Math.Abs(powerDifference), BatteryCapacity);
                BatteryCapacity -= powerFromBattery;
            }
            
            HouseholdPower = Math.Max(0, HouseholdPower + (_random.NextDouble() - 0.5) * 1.5);
        }

        private void UpdateSecurityParameters()
        {
            foreach (var param in SecurityParameters)
            {
                double variation = (_random.NextDouble() - 0.5) * param.MaxValue * 0.05;
                param.Value = Math.Max(param.MinValue, Math.Min(param.MaxValue, param.Value + variation));
            }
        }

        private void UpdateTotalPower()
        {
            TotalPower = WindPower + SolarPower;
        }

        private void UpdateBatteryPercentage()
        {
            BatteryPercentage = BatteryCapacity;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SecurityParameter : INotifyPropertyChanged
    {
        private string _name;
        private double _value;
        private double _minValue;
        private double _maxValue;
        private string _unit;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNormal));
                    OnPropertyChanged(nameof(StatusColor));
                }
            }
        }

        public double MinValue
        {
            get => _minValue;
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNormal));
                    OnPropertyChanged(nameof(StatusColor));
                }
            }
        }

        public double MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNormal));
                    OnPropertyChanged(nameof(StatusColor));
                }
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNormal => Value >= MinValue && Value <= MaxValue;

        public string StatusColor => IsNormal ? "Green" : "Red";

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
    