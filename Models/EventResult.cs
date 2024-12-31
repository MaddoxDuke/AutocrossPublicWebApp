
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutocrossPublicWebApp.Models
{
    public class EventResult {
        public List<int> EventNum { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public bool PaxRaw { get; set; }
        public List<string> AutoxClass { get; set; }
        public List<string> ClassPlacement { get; set; }
        public List<string>? PaxPlacement { get; set; }
        public List<float>? PaxTime { get; set; }
        public List<float>? RawTime { get; set; }
        public List<string>? FinalTimes { get; set; }
    }
}
