using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidStatus.API.Entities
{
    public class CovidHospitalDataEntity
    {
        public class Info
        {
            public string notes { get; set; }
            public string type_override { get; set; }
            public string label { get; set; }
        }

        public class Field
        {
            public string type { get; set; }
            public string id { get; set; }
            public Info info { get; set; }
        }

        public class Record
        {
            public double? icu_covid_confirmed_patients { get; set; }
            public double? icu_suspected_covid_patients { get; set; }
            public object hospitalized_covid_patients { get; set; }
            public double? hospitalized_suspected_covid_patients { get; set; }
            public double? icu_available_beds { get; set; }
            public string county { get; set; }
            public double? hospitalized_covid_confirmed_patients { get; set; }
            public int _id { get; set; }
            public object all_hospital_beds { get; set; }
            public DateTime todays_date { get; set; }
        }

        public class Links
        {
            public string start { get; set; }
            public string next { get; set; }
        }

        public class Result
        {
            public bool include_total { get; set; }
            public string resource_id { get; set; }
            public List<Field> fields { get; set; }
            public string records_format { get; set; }
            public List<Record> records { get; set; }
            public int limit { get; set; }
            public Links _links { get; set; }
            public int total { get; set; }
        }

        public class Root
        {
            public string help { get; set; }
            public bool success { get; set; }
            public Result result { get; set; }
        }
    }
}
