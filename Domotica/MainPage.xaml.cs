using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Domotica
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private byte REGISTER_ID;

        public ObservableCollection<GpioScheduleDay> gpioSchedule;

        private const string SCHEDULE_KEY = "schedule";

        private static ApplicationDataContainer localSettings;

        private static MainPage mp;

        private Thermostat thermostat;
        public MainPage()
        {
            mp = this;
            gpioSchedule = new ObservableCollection<GpioScheduleDay>();

            gpioSchedule.Add(new GpioScheduleDay("Maandag"));
            gpioSchedule.Add(new GpioScheduleDay("Dinsdag"));
            gpioSchedule.Add(new GpioScheduleDay("Woensdag"));
            gpioSchedule.Add(new GpioScheduleDay("Donderdag"));
            gpioSchedule.Add(new GpioScheduleDay("Vrijdag"));
            gpioSchedule.Add(new GpioScheduleDay("Zaterdag"));
            gpioSchedule.Add(new GpioScheduleDay("Zondag"));

            localSettings = ApplicationData.Current.LocalSettings;
            object scheduleData = localSettings.Values[SCHEDULE_KEY];
            if(scheduleData != null)
            {
                string data = localSettings.Values[SCHEDULE_KEY] as string;
                gpioSchedule = JsonConvert.DeserializeObject<ObservableCollection<GpioScheduleDay>>(data);
            }
            else // No data, save default
            {
                string data = JsonConvert.SerializeObject(gpioSchedule);
                localSettings.Values[SCHEDULE_KEY] = data;
            }




            this.InitializeComponent();
            thermostat = new Thermostat(this);
            SwitchGrid.DataContext = thermostat;


            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler<object>(TimerTick);
            timer.Start();
        }

        private void TimerTick (object sender, object obj)
        {
            Clock.Text = string.Format(new CultureInfo("nl-NL"),"{0:dddd,\td-M-yyyy\tHH:mm:ss}", DateTime.Now);
        }

        DispatcherTimer timer;

        public static void SerializeObjects()
        {
            string data = JsonConvert.SerializeObject(mp.gpioSchedule);
            localSettings.Values[SCHEDULE_KEY] = data;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /*
        public static async Task<IEnumerable<I2cDevice>> FindDevicesAsync()
        {
            IList<byte> returnValue = new List<byte>();
            IList<I2cDevice> devices = new List<I2cDevice>();
            // *** 
            // *** Get a selector string that will return all I2C controllers on the system 
            // *** 
            string aqs = I2cDevice.GetDeviceSelector();
            // *** 
            // *** Find the I2C bus controller device with our selector string 
            // *** 
            var dis = await DeviceInformation.FindAllAsync(aqs).AsTask();
            if (dis.Count > 0)
            {
                const int minimumAddress = 0;
                const int maximumAddress = 255;
                for (byte address = minimumAddress; address <= maximumAddress; address++)
                {
                    var settings = new I2cConnectionSettings(address);
                    settings.BusSpeed = I2cBusSpeed.FastMode;
                    settings.SharingMode = I2cSharingMode.Shared;
                    // *** 
                    // *** Create an I2cDevice with our selected bus controller and I2C settings 
                    // *** 
                    using (I2cDevice device = await I2cDevice.FromIdAsync(dis[0].Id, settings))
                    {
                        if (device != null)
                        {
                            try
                            {
                                byte[] writeBuffer = new byte[1] { 0 };
                                device.Write(writeBuffer);
                                // *** 
                                // *** If no exception is thrown, there is 
                                // *** a devie at this address. 
                                // *** 
                                returnValue.Add(address);
                                devices.Add(device);
                            }
                            catch
                            {
                                // *** 
                                // *** If the address is invalid, an exception will be thrown. 
                                // *** 
                            }
                        }
                    }
                }
            }
            return devices;
        }*/

        private async void Grid_Loading(FrameworkElement sender, object args)
        {
           /* List<I2cDevice> devices = (await FindDevicesAsync()).ToList();
            DevicesControl.ItemsSource = devices;*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void dayList_Loaded(object sender, RoutedEventArgs e)
        {
            dayList.ItemsSource = gpioSchedule;
            dayList.SelectedIndex = 0;
            ScheduleList.ItemsSource = (dayList.SelectedItem as GpioScheduleDay).Schedule;
        }

        private void addElementButton_Click(object sender, RoutedEventArgs e)
        {
            (ScheduleList.ItemsSource as ObservableCollection<GpioScheduleItem>).Add(new GpioScheduleItem(ScheduleList.ItemsSource as ObservableCollection<GpioScheduleItem>));
        }

        private void removeElementButton_Click(object sender, RoutedEventArgs e)
        {
            (ScheduleList.ItemsSource as ObservableCollection<GpioScheduleItem>).Remove((ScheduleList.SelectedItem as GpioScheduleItem));
        }

        private void dayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScheduleList.ItemsSource = (dayList.SelectedItem as GpioScheduleDay).Schedule;
        }

        private void ScheduleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
        }
    }
}
