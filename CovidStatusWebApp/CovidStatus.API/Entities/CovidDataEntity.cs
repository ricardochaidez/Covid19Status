using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidStatus.API.Entities
{
    public class CovidDataEntity
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
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
            public string cumulative_reported_deaths { get; set; }
            public double? cumulative_deaths { get; set; }
            public object cumulative_reported_tests { get; set; }
            public string cumulative_positive_tests { get; set; }
            public string area { get; set; }
            public double? cumulative_cases { get; set; }
            public double? reported_cases { get; set; }
            public string positive_tests { get; set; }
            public string cumulative_reported_cases { get; set; }
            public string area_type { get; set; }
            public double? reported_deaths { get; set; }
            public string total_tests { get; set; }
            public string deaths { get; set; }
            public object reported_tests { get; set; }
            public DateTime? date { get; set; }
            public string cases { get; set; }
            public int _id { get; set; }
            public string cumulative_total_tests { get; set; }
            public string population { get; set; }
        }

        public class Links
        {
            public string start { get; set; }
            public string next { get; set; }
        }

        public class Result
        {
            public string sort { get; set; }
            public bool include_total { get; set; }
            public string resource_id { get; set; }
            public List<Field> fields { get; set; }
            public string records_format { get; set; }
            public string q { get; set; }
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
