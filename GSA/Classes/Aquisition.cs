using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.Classes
{
    internal class Aquisition
    {
        

        public DateTime start_time { get; set; }
        public DateTime finish_time { get; set; }
        public string road_type { get; set; }
        public string weather { get; set; }
        public string road_conditions { get; set; }
        public string driver { get; set; }
        public string curves { get; set; }
        public string visibility_conditions { get; set; }
        public string volume_of_traffic { get; set; }
        public string period_of_the_day { get; set; }
        public Aquisition()
        {
            this.start_time = DateTime.Now;
            this.finish_time = DateTime.Now;
            this.road_type = "Rural";
            this.weather = "Sunny";
            this.road_conditions = "Clean";
            this.driver = "Matheus";
            this.curves = "c1";
            this.visibility_conditions = "Clean";
            this.volume_of_traffic = "Free flow";
            this.period_of_the_day = "Day";
        }
        public Aquisition(DateTime start_time, DateTime finish_time, string road_type, string weather, string road_condition, string driver, string curve, string visibility_conditions, string volume_of_traffic)
        {
            this.start_time = start_time;
            this.finish_time = finish_time;
            this.road_type = road_type;
            this.weather = weather;
            this.road_conditions = road_condition;
            this.driver = driver;
            this.curves = curve;
            this.visibility_conditions = visibility_conditions;
            this.volume_of_traffic = volume_of_traffic;
        }

        internal void CopyValues(Aquisition current_aquisition_state)
        {
            
            this.road_type = current_aquisition_state.road_type;
            this.weather = current_aquisition_state.weather;
            this.road_conditions = current_aquisition_state.road_conditions;
            this.driver = current_aquisition_state.driver;
            this.curves = current_aquisition_state.curves;
            this.visibility_conditions = current_aquisition_state.visibility_conditions;
            this.volume_of_traffic = current_aquisition_state.volume_of_traffic;
            this.period_of_the_day = current_aquisition_state.period_of_the_day;
        }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public string[] ListViewString()
        {
            string[] myString = ToListString();
            return myString;
        }
        public string[] ToListString()
        {
            string[] array_list = new[]
            {
                start_time.ToString(),
                finish_time.ToString(),
                road_type,
                weather,
                road_conditions,
                driver,
                curves,
                visibility_conditions,
                volume_of_traffic,
                period_of_the_day,
            };
            return array_list;
        }

        public static implicit operator string(Aquisition aqst)
        {

            string v = $"{aqst.road_conditions},{aqst.driver},{aqst.curves},{aqst.visibility_conditions},{aqst.volume_of_traffic},{aqst.period_of_the_day}";
            return $"{aqst.start_time},{aqst.finish_time},{aqst.road_type},{aqst.weather}," +
                v;
        }
      
    }
}
