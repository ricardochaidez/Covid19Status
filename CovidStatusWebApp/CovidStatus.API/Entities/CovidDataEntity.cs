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
            public double rank { get; set; }
            public int? CUMULATIVE_REPORTED_DEATHS { get; set; }
            public int? CUMULATIVE_DEATHS { get; set; }
            public int? CUMULATIVE_REPORTED_TESTS { get; set; }
            public int? CUMULATIVE_POSITIVE_TESTS { get; set; }
            public string AREA { get; set; }
            public int? POSITIVE_TESTS { get; set; }
            public int? CUMULATIVE_CASES { get; set; }
            public int? REPORTED_CASES { get; set; }
            public string CUMULATIVE_REPORTED_CASES { get; set; }
            public string AREA_TYPE { get; set; }
            public int? DEATHS { get; set; }
            public int? TOTAL_TESTS { get; set; }
            public int? REPORTED_TESTS { get; set; }
            public int? REPORTED_DEATHS { get; set; }
            public DateTime? DATE { get; set; }
            public int? CASES { get; set; }
            public int _id { get; set; }
            public int? CUMULATIVE_TOTAL_TESTS { get; set; }
            public int? POPULATION { get; set; }
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
