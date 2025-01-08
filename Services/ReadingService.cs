using AutocrossPublicWebApp.Models;
using HtmlAgilityPack;
using System.Globalization;

namespace AutocrossPublicWebApp.Services
{
    public class ReadingService : ReadingModel
    {
        ReadingModel Reading = new ReadingModel();
        public void setYearDoc(int Year, bool paxAndRaw) {

			Console.WriteLine("Running setYearDoc Function.");

			Reading.Year = Year;
            Reading.PaxRaw = paxAndRaw;
            Reading.DocSize = 0;

            int currentYear = DateTime.Now.Year;
            string url = null;
            string[] urls = new string[12];

            if (Year == currentYear || Year == 2024)
            {
                url = "https://www.texasscca.org/solo/results/";
            }
            else url = "https://www.texasscca.org/Solo/results/past-results/";

            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            if (Reading.PaxRaw) { 
                for (int i = 1; i < 12; i++) {
                    if (htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[5]/a") != null)
                    { // td[5] instead of td[4] for the pax and raw link
                        urls[i - 1] = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[5]/a").Attributes["href"].Value; //links for final results
                        Reading.DocSize++;
                    }
                }
            
            } else {
				for (int i = 1; i < 12; i++) {
					if (htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[4]/a") != null) {
						urls[i - 1] = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[4]/a").Attributes["href"].Value; //links for final results
						Reading.DocSize++;
					}
				}
			}
            Console.WriteLine("DocSize is " + Reading.DocSize);
            Reading.SelectedDocs = new HtmlDocument[Reading.DocSize];

            for (int i = 1; i <= Reading.DocSize; i++)
            {
                httpClient = new HttpClient();
                html = httpClient.GetStringAsync(urls[i - 1]).Result;
                htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                Reading.SelectedDocs[i - 1] = htmlDocument;
            }
        }
        public void setTrNthChild(string Name, bool PaxRaw)
        {

			Console.WriteLine("Running setTrnthChild Function.");

            Reading.Name = Name;
            Reading.PaxRaw = PaxRaw;
			int searchNum = 350;
            Reading.TrNthChild = new int[Reading.DocSize];

            if (Reading.PaxRaw) {
                searchNum = 175;
                for (int j = 0; j < Reading.DocSize; j++)
                { // loop to locate row that contains name
                    Console.WriteLine("Searching doc #" + (j+1));
                    for (int i = 2; i < searchNum; i++)
                    {

                        if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + i + "]/td[5]") == null) break; // failsafe, exits for loop if null, this means it has reached the end of the webpage.


                        if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + i + "]/td[5]").InnerText.Contains(Name))
                        { // checks if name exists on each line.
                            Console.WriteLine("Name found on Doc #" + (j + 1));

                            Reading.TrNthChild[j] = i; // array for name addresses, correlates with each doc a name is found in. allows for easy lookup when displaying
                            searchNum = i + 100;
                            break;
                        }
                    }
                    if (Reading.TrNthChild[j] == 0) Console.WriteLine("Name not found on Doc #" + (j + 1));
                    Console.WriteLine("Loading... (" + (j + 1) + "/" + Reading.DocSize + ")");
                    Console.WriteLine(Reading.TrNthChild[j]);
                }
            
            } else {
				for (int j = 0; j < Reading.DocSize; j++) { // loop to locate row that contains name
					Console.WriteLine("Searching doc #" + (j+1));
					for (int i = 1; i < searchNum; i++) {

						if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + i + "]") == null) break; // failsafe, exits for loop if null, this means it has reached the end of the webpage.

						if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + i + "]").InnerText.Contains(Name)) { // checks if name exists on each line.
							Console.WriteLine("Name found on Doc #" + (j + 1));

							Reading.TrNthChild[j] = i; // array for name addresses, correlates with each doc a name is found in. allows for easy lookup when displaying
							searchNum = i + 100;
							break;
						}
					}
					if (Reading.TrNthChild[j] == 0) Console.WriteLine("Name not found on Doc #" + (j + 1));
					Console.WriteLine("Loading... (" + (j + 1) + "/" + Reading.DocSize + ")");
				}
			}


        }        public List<EventResult> saveToEventResult() {

			int eventCount = 0;

            List<EventResult> results = new List<EventResult>();            

			for (int j = 0; j < Reading.DocSize; j++) {

                string temp = "";

                EventResult result = new EventResult();

                result.Name = Reading.Name;
                result.Year = Reading.Year;
                result.PaxRaw = Reading.PaxRaw;
                result.DidNotParticipate = false;

                while (Reading.TrNthChild[j] == 0) {
					eventCount++;

					if (j == Reading.DocSize - 1) break;
					else j++;
				}
                result.EventNum = j + 1;

                // Can be used to check if not participated in any events
                if (eventCount == Reading.DocSize) {

                    result.AutoxClass = "DNP";
                    results.Add(result);

                    return results;
                }

                if (j == Reading.DocSize - 1 && Reading.TrNthChild[j] == 0) {
                    result.DidNotParticipate = true;
                    break;
                }

                result.FinalTimes = new List<string>();

                if (!Reading.PaxRaw) { //searches final times

                    result.AutoxClass = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[2]").InnerText);

                    for (int i = 7; i <= 9; i++) { //loop for run times.

                        temp = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[" + i + "]").InnerText);
                        if (!String.IsNullOrWhiteSpace(temp) && temp.Length >= 9) temp = temp.Substring(0, 9); // ensures only the times get added.
                        result.FinalTimes.Add(temp);

                    }
                    for (int i = 7; i <= 9; i++) {
                        temp = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + (Reading.TrNthChild[j] + 1) + "]/td[" + i + "]").InnerText);// time results, second row.
                        if (!String.IsNullOrWhiteSpace(temp) && temp.Length >= 9) temp = temp.Substring(0, 9);
                        result.FinalTimes.Add(temp);
                    }

					result.ClassPlacement = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[1]").InnerText);
				} else { // searches paxRaw times

                    result.AutoxClass = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[3]").InnerText);
					result.PaxTime = (float.Parse(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[9]").InnerText, CultureInfo.InvariantCulture.NumberFormat));
					result.RawTime = (float.Parse(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[7]").InnerText, CultureInfo.InvariantCulture.NumberFormat));
					result.PaxPlacement = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[1]").InnerText);
					result.ClassPlacement = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[2]").InnerText);

				}
                if (!result.DidNotParticipate) results.Add(result);
              
            }
            return results;
        }
        public void Search(ReadingModel result) {
            setYearDoc(result.Year, result.PaxRaw);
            setTrNthChild(result.Name, result.PaxRaw);
        }
    }
}
