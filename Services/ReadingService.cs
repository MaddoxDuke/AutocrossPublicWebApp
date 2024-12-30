using AutocrossPublicWebApp.Models;
using AutocrossPublicWebApp.Controllers;
using HtmlAgilityPack;
using System.Diagnostics.Tracing;
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
            int currentYear = DateTime.Now.Year;
            string url = null;
            string[] urls = new string[12];

            if (Year < currentYear - 10 || Year > currentYear)
            {
                Console.WriteLine("Year entered is invalid.");
                return;
            }
            if (Year == currentYear)
            {
                url = "https://www.texasscca.org/solo/results/";
            }
            else url = "https://www.texasscca.org/Solo/results/past-results/";

            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            if (Reading.PaxRaw) { 
                for (int i = 1; i < 12; i++)
                {
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

            Console.WriteLine(Reading.TrNthChild[0]);

            if (Reading.PaxRaw) { 
                for (int j = 0; j < Reading.DocSize; j++)
                { // loop to locate row that contains name
                    Console.WriteLine("Searching doc #" + (j+1));
                    for (int i = 1; i < searchNum; i++)
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
                    Console.WriteLine(Reading.TrNthChild[j] + "\n");
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


        }
        public EventResult saveToEventResult(EventResult resultModel) {

			int eventCount = 0;
            string temp = "";
           
			resultModel.FinalTimes = new List<string>();
            resultModel.ClassPlacement = new List<string>();
            resultModel.PaxPlacement = new List<string>();
            resultModel.AutoxClass = new List<string>();
            resultModel.PaxTime = new List<float>();
            resultModel.RawTime = new List<float>();


			for (int j = 0; j < Reading.DocSize; j++) {
				while (Reading.TrNthChild[j] == 0) {
					eventCount++;
					if (j == Reading.DocSize - 1) break;
					else j++;
				}

				if (eventCount == Reading.DocSize) Console.WriteLine(Reading.Name + " did not participate in any events for the year " + Reading.Year);

				if (j == Reading.DocSize - 1 && Reading.TrNthChild[j] == 0) break;

				if (!Reading.PaxRaw) {

					resultModel.AutoxClass.Add(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[2]").InnerText);

					for (int i = 7; i <= 9; i++) {
						temp = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[" + i + "]").InnerText);
                        resultModel.FinalTimes.Add(temp);
						temp = (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + (Reading.TrNthChild[j] + 1) + "]/td[" + i + "]").InnerText);// time results, second row.
						resultModel.FinalTimes.Add(temp);
					}

					resultModel.ClassPlacement.Add(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[1]").InnerText);
				} else {

					resultModel.AutoxClass.Add(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[3]").InnerText);

					resultModel.PaxTime.Add(float.Parse(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[9]").InnerText, CultureInfo.InvariantCulture.NumberFormat));
					resultModel.RawTime.Add(float.Parse(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[7]").InnerText, CultureInfo.InvariantCulture.NumberFormat));

					resultModel.PaxPlacement.Add(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[1]").InnerText);
					resultModel.ClassPlacement.Add(Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[2]").InnerText);

				}
			}
            if (Reading.PaxRaw) for (int i = 0; i < resultModel.FinalTimes.Count; i++) Console.WriteLine(resultModel.FinalTimes[i]);
            return resultModel;
        }
        public void Search(EventResult result) {
            setYearDoc(result.Year, result.PaxRaw);
            setTrNthChild(result.Name, result.PaxRaw);
        }
    }
}
