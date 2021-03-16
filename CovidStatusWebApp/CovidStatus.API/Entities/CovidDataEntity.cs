using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidStatus.API.Entities
{
    public class CovidDataEntity
    {
        public class Field
        {
            public string type { get; set; }
            public string id { get; set; }
        }

        public class Record
        {
            public string area { get; set; }
            public double? deaths { get; set; }
            public double? reported_cases { get; set; }
            public double? rank { get; set; }
            public string area_type { get; set; }
            public double? reported_deaths { get; set; }
            public string total_tests { get; set; }
            public string reported_tests { get; set; }
            public string date { get; set; }
            public double? cases { get; set; }
            public int _id { get; set; }
            public string positive_tests { get; set; }
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
