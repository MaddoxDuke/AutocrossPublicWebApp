namespace AutocrossPublicWebApp.Models
{
    internal class ResultsModel
    {
        public string Name { get; set; }
        public string AutoxClass { get; set; }
        public string ClassPlacement { get; set; }
        public string PaxPlacement { get; set; }
        public float PaxTime { get; set; }
        public float RawTime { get; set; }
        public List<int> FinalTimes { get; set; }
    }
}
