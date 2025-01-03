namespace AutocrossPublicWebApp.Models
{
    public class EventResult {
        public bool DidNotParticipate { get; set; }
        public int EventNum { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public bool PaxRaw { get; set; }
        public string AutoxClass { get; set; }
        public string ClassPlacement { get; set; }
        public string? PaxPlacement { get; set; }
        public float? PaxTime { get; set; }
        public float? RawTime { get; set; }
        public List<string>? FinalTimes { get; set; }
    }
}
