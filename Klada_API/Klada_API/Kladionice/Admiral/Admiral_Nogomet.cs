using Klada_API.Kladionice.Admiral.API_Class;
using Klada_API.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Linq;
using System.Globalization;
using RestSharp;
using System.Threading.Tasks;
using System;

namespace Klada_API.Kladionice.Admiral
{
    class Admiral_Nogomet
    {
        public static void API_Admiral()
        {
            var client = new RestClient("https://bettingapi.admiral.hr/api/webclient/competitionsWithEventsForSport/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("Accept-Language", "en-US,en;q=0.9");
            request.AddHeader("Cache-Control", "no-cache");
            //request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Language", "hr-HR");
            request.AddHeader("OfficeId", "138");
            request.AddHeader("Origin", "https://bettingweb.admiral.hr");
            request.AddHeader("Pragma", "no-cache");
            request.AddHeader("Referer", "https://bettingweb.admiral.hr/");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Site", "same-site");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 OPR/90.0.4480.78";
            request.AddHeader("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"104\", \"Opera GX\";v=\"90\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            var body = @"{""sportId"":1,""topN"":25,""skipN"":0,""isLive"":false,""dateFrom"":""2022-09-13T11:23:24.681"",""dateTo"":""2027-09-13T11:22:54.000"",""eventMappingTypes"":[2,1]}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            
            // Get response
            IRestResponse restResponse = client.Execute(request);

            string resultText = "";
            resultText = restResponse.Content;

            #region Save json2Disc
            //Console.WriteLine(restResponse.Content); // Will output the HTML contents of the requested page

            Program.DownloadData(resultText, "c:\\temp/API_Result_Admiral_Nogomet.txt");
            #endregion save json2Disc

            ParsingJSONString(resultText);
        }
        static void ParsingJSONString(string jsonData)
        {
            //JObject jsonObject = JObject.Parse(jsonData);
            AdmiralAPIList data = JsonConvert.DeserializeObject<AdmiralAPIList>(jsonData);
       
            SaveData2DB(data);
        }

        static void SaveData2DB(AdmiralAPIList dataFromJson)
        {
            HRKladeEntities db = new HRKladeEntities();

            foreach (var competitions in dataFromJson.competitions)
            {
                foreach (var _event in competitions.events)
                {
                    if (_event.bets == null)
                        continue;
                    bool saveEvent = true;

                    foreach (var bet in _event.bets.Where(b => b.betOutcomes.Count() > 0))
                    {
                        if (saveEvent)
                        { 
                            OddsTable OddsTable = new OddsTable();
                            #region Split string
                            var separator = _event.name.IndexOf("-", 2);
                            var home = _event.name;
                            var away = string.Empty;
                            if (separator > 0)
                            {
                                home = _event.name.Substring(0, separator);
                                away = _event.name.Substring(separator + 1);
                            }
                            #endregion split string

                            OddsTable.Home = home.Trim();
                            OddsTable.Away = away.Trim();
                            OddsTable.EventTime = _event.dateTime;
                            OddsTable.EventDateTime = Convert.ToDateTime(_event.dateTime);
                            OddsTable.SportType = _event.sportName;
                            OddsTable.Created = DateTime.Now;
                            OddsTable.KladaName = "Admiral";

                            string currentOdd = bet.betOutcomes.Where(odd => odd.name == "1").Select(s => s.odd).FirstOrDefault();
                            OddsTable.Odd1 = decimal.Parse((currentOdd != null) ? currentOdd : "0", new NumberFormatInfo() { NumberDecimalSeparator = "." });

                            currentOdd = bet.betOutcomes.Where(odd => odd.name == "X").Select(s => s.odd).FirstOrDefault();
                            OddsTable.OddX = decimal.Parse((currentOdd != null) ? currentOdd : "0", new NumberFormatInfo() { NumberDecimalSeparator = "." });

                            currentOdd = bet.betOutcomes.Where(odd => odd.name == "2").Select(s => s.odd).FirstOrDefault();
                            OddsTable.Odd2 = decimal.Parse((currentOdd != null) ? currentOdd : "0", new NumberFormatInfo() { NumberDecimalSeparator = "." });

                            currentOdd = bet.betOutcomes.Where(odd => odd.name == "1X").Select(s => s.odd).FirstOrDefault();
                            OddsTable.Odd1X = decimal.Parse((currentOdd != null)? currentOdd : "0", new NumberFormatInfo() { NumberDecimalSeparator = "." });

                            currentOdd = bet.betOutcomes.Where(odd => odd.name == "X2").Select(s => s.odd).FirstOrDefault();
                            OddsTable.OddX2 = decimal.Parse((currentOdd != null) ? currentOdd : "0", new NumberFormatInfo() { NumberDecimalSeparator = "." });

                            currentOdd = bet.betOutcomes.Where(odd => odd.name == "12").Select(s => s.odd).FirstOrDefault();
                            OddsTable.Odd12 = decimal.Parse((currentOdd != null) ? currentOdd : "0", new NumberFormatInfo() { NumberDecimalSeparator = "." });

                            currentOdd = bet.betOutcomes.Where(odd => odd.name == "F2").Select(s => s.odd).FirstOrDefault();
                            OddsTable.OddF2 = decimal.Parse((currentOdd != null) ? currentOdd : "0", new NumberFormatInfo() { NumberDecimalSeparator = "." });

                            db.OddsTables.Add(OddsTable);
                            db.SaveChanges();
                            saveEvent = false;
                        }
                    }
                }            
            }
        }
    }
}