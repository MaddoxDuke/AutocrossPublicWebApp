using HtmlAgilityPack;

namespace AutocrossPublicWebApp.Models
{
    public class ReadingModel
    {

        public string Name { get; set; }
        public HtmlDocument[] SelectedDocs { get; set; }
        public int DocSize { get; set; }
        public int Year { get; set; }
        public int[] TrNthChild { get; set; }
        public bool PaxRaw { get; set; }
    }
}
