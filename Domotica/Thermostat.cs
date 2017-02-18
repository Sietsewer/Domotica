using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Domotica
{
    class Thermostat : INotifyPropertyChanged
    {
        private static GpioController gpioController;

        private DependencyObject dependencyObject;

        private SynchronizationContext syncContext;

        public event PropertyChangedEventHandler PropertyChanged;

        private const int RELAY_3 = 6;
        private const int RELAY_4 = 12;
        private const int RELAY_LIVING_ROOM = 13;
        private const int RELAY_COMMON = 19;
        private const int BOILER_HEATING = 26;

        private GpioPin _Relay3;

        private GpioPin _Relay4;

        private GpioPin _RelayLivingRoom;

        private GpioPin _RelayCommon;

        private GpioPin _BoilerHeating;


        async void _valueChanged(GpioPin pin, GpioPinValueChangedEventArgs args)
        {
            if (pin == _BoilerHeating)
            {

                await dependencyObject.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                     this.PropertyChanged(this, new PropertyChangedEventArgs("BoilerOn"));
                 });
            }
        }

        public Thermostat(DependencyObject dependencyObject)
        {
            this.dependencyObject = dependencyObject;
            syncContext = SynchronizationContext.Current;

            gpioController = GpioController.GetDefault();

            if (gpioController != null)
            {
                try
                {
                    _Relay3 = gpioController.OpenPin(RELAY_3);
                    _Relay3.SetDriveMode(GpioPinDriveMode.Output);
                    _Relay3.Write(GpioPinValue.Low);
                    _Relay3.ValueChanged += _valueChanged;

                    _Relay4 = gpioController.OpenPin(RELAY_4);
                    _Relay4.SetDriveMode(GpioPinDriveMode.Output);
                    _Relay4.Write(GpioPinValue.Low);
                    _Relay4.ValueChanged += _valueChanged;


                    _RelayLivingRoom = gpioController.OpenPin(RELAY_LIVING_ROOM);
                    _RelayLivingRoom.SetDriveMode(GpioPinDriveMode.Output);
                    _RelayLivingRoom.Write(GpioPinValue.Low);
                    _RelayLivingRoom.ValueChanged += _valueChanged;



                    _RelayCommon = gpioController.OpenPin(RELAY_COMMON);
                    _RelayCommon.SetDriveMode(GpioPinDriveMode.Output);
                    _RelayCommon.Write(GpioPinValue.Low);
                    _RelayCommon.ValueChanged += _valueChanged;


                    _BoilerHeating = gpioController.OpenPin(BOILER_HEATING);
                    _BoilerHeating.ValueChanged += _valueChanged;
                    _BoilerHeating.SetDriveMode(GpioPinDriveMode.Input);

                }
                catch (Exception e)
                {
                    gpioController = null;
                }
            }
        }

        public bool Relay3
        {
            get
            {
                if (gpioController != null)
                {
                    return _Relay3.Read() == GpioPinValue.High;
                }
                return false;
            }
            set
            {
                if (gpioController != null)
                {
                    _Relay3.Write(value ? GpioPinValue.High : GpioPinValue.Low);
                }
                PropertyChanged(this, new PropertyChangedEventArgs("Relay3"));

            }
        }
        public bool Relay4
        {
            get
            {
                if (gpioController != null)
                {
                    return _Relay4.Read() == GpioPinValue.High;
                }
                return false;
            }
            set
            {
                if (gpioController != null)
                {
                    _Relay4.Write(value ? GpioPinValue.High : GpioPinValue.Low);
                }
                PropertyChanged(this, new PropertyChangedEventArgs("Relay4"));

            }
        }

        public bool RelayLivingRoom
        {
            get
            {
                if (gpioController != null)
                {
                    return _RelayLivingRoom.Read() == GpioPinValue.High;
                }
                return false;
            }
            set
            {
                if (gpioController != null)
                {
                    _RelayLivingRoom.Write(value ? GpioPinValue.High : GpioPinValue.Low);
                }
                PropertyChanged(this, new PropertyChangedEventArgs("RelayLivingRoom"));

            }
        }

        public bool RelayCommon
        {
            get
            {
                if (gpioController != null)
                {
                    return _RelayCommon.Read() == GpioPinValue.High;
                }
                return false;
            }
            set
            {
                if (gpioController != null)
                {
                    _RelayCommon.Write(value ? GpioPinValue.High : GpioPinValue.Low);
                }
                PropertyChanged(this, new PropertyChangedEventArgs("RelayCommon"));


            }
        }

        public bool BoilerOn
        {
            get
            {
                if (gpioController != null)
                {
                    return _BoilerHeating.Read() == GpioPinValue.High;
                }
                return false;
            }
        }

    }
}
