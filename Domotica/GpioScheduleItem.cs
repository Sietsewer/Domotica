using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Domotica
{
    public class GpioScheduleItem : INotifyPropertyChanged
    {
        private ObservableCollection<GpioScheduleItem> parentList;

        public GpioScheduleItem (ObservableCollection<GpioScheduleItem> parentList)
        {
            this.parentList = parentList;
        }

        [JsonProperty]
        private TimeSpan _Time;
        [JsonIgnore]
        public TimeSpan Time
        {
            get
            {
                return _Time;
            }
            set
            {
                _Time = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Time"));
                MainPage.SerializeObjects();

            }
        }

        [JsonProperty]
        private bool _Common;
        [JsonIgnore]
        public bool Common
        {
            get
            {
                return _Common;
            }
            set
            {
                _Common = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Common"));
                MainPage.SerializeObjects();
            }
        }

        [JsonProperty]
        private bool _Livingroom;
        [JsonIgnore]
        public bool Livingroom
        {
            get
            {
                return _Livingroom;
            }
            set
            {
                _Livingroom = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Livingroom"));
                MainPage.SerializeObjects();

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void removeItem (object sender, RoutedEventArgs e)
        {
            parentList.Remove(this);
        }
    }
}
