using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domotica
{

    public class GpioScheduleDay : INotifyPropertyChanged
    {
        [JsonProperty]
        private string _DayName;
        [JsonIgnore]
        public string DayName
        {
            get
            {
                return _DayName;
            }
            set
            {
                _DayName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DayName"));
                MainPage.SerializeObjects();

            }
        }
        [JsonProperty]
        private ObservableCollection<GpioScheduleItem> _Schedule;
        [JsonIgnore]
        public ObservableCollection<GpioScheduleItem> Schedule
        {
            get
            {
                return _Schedule;
            }
            set
            {
                _Schedule = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Schedule"));
                MainPage.SerializeObjects();

            }
        }

        public GpioScheduleDay(string dayName = "dayName", int initialSize = 1)
        {
            _DayName = dayName;
            _Schedule = new ObservableCollection<GpioScheduleItem>();
            for (int i = 0; i < initialSize; i++)
            {
                _Schedule.Add(new GpioScheduleItem(_Schedule));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
