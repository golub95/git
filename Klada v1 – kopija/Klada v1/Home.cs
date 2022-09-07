﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using Klada_v3.Model;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Net.Mail;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;

namespace Klada_v3
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        HRKladeEntities db = new HRKladeEntities();
        private new List<OddsTable> Events = new List<OddsTable>();
        private readonly string SendingMail = "terry.ferit@gmail.com";
        private readonly string Password = "osusqhyeigynzuft";
        private readonly string ReceivingMail = "terry.ferit@gmail.com";
        private readonly string Subject = "Hit";
        double tolerance = 0.5, toleranceSum = 1.3;
        private void btn_run_Click(object sender, EventArgs e)
        {

            if (cb_IsprazniTablicu.Checked)
            {
                ResetIdentity(sender, e);
            }
            if (cb_SuperSport.Checked)
            {
                FormSuperSport formSuperSport = new FormSuperSport();
                formSuperSport.ShowDialog(this);
                formSuperSport.Dispose();
            }
            if (cb_Stanleybet.Checked)
            {
                FormStanleybet FormStanleybet = new FormStanleybet();
                FormStanleybet.ShowDialog(this);
                FormStanleybet.Dispose();
            }
            if (cb_PSK.Checked)
            {
                FormPSK formPSK = new FormPSK();
                formPSK.ShowDialog(this);
                formPSK.Dispose();

                //FormPSKCopy formPSKCopy = new FormPSKCopy();
                //formPSKCopy.ShowDialog(this);
                //formPSKCopy.Dispose();
            }
            if (cb_Germania.Checked)
            {
                FormGermania formGermania = new FormGermania();
                formGermania.ShowDialog(this);
                formGermania.Dispose();
            }
            if (cb_HL.Checked)
            {
                FormHL formHL = new FormHL();
                formHL.ShowDialog(this);
                formHL.Dispose();
            }
            if (cb_Calc.Checked)
            {
                //FindSimilar();
                MatchSystemIDs();
                FindIdenticalMatchFromOddsTable();
            }
            //Cef.Shutdown();
            Cef.ClearSchemeHandlerFactories();

        }

        private double CalculateOdds(CalcOddsTable hit)
        {

            double izracun1 = 0;
            double izracun2 = 0;
            double izracun3 = 0;
            
            izracun1 = (hit.Odd1>0) ? (1 / (0.95 * (double)hit.Odd1 - 0.095)) : (0);
            izracun2 = (hit.OddX>0) ? (1 / (0.95 * (double)hit.OddX - 0.095)) : (0);
            izracun3 = (hit.Odd2>0) ? (1 / (0.95 * (double)hit.Odd2 - 0.095)) : (0);

            if ((izracun1 + izracun2 + izracun3) < 1)
            {
                //MessageBox.Show(hit.EventID.ToString() + " " + (1- (izracun1 + izracun2 + izracun3)).ToString());
                SendMail(hit, izracun1 + izracun2 + izracun3);
            }

            return (izracun1 + izracun2 + izracun3);
        }

        private void SendMail(CalcOddsTable hit, double koef)
        {

            string message = "";


            message = hit.Home + " vs " + hit.Away + "\n\n" + hit.Klada1 + ": " + hit.Odd1 + "\n" + hit.KladaX + ": " + hit.OddX + "\n" + hit.Klada2 + ": " + hit.Odd2;

            if (message != "")
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;

                client.Credentials = new System.Net.NetworkCredential(SendingMail, Password);

                MailMessage mm = new MailMessage(SendingMail, ReceivingMail, Subject, message);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm); 

                //using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //{
                //    smtp.UseDefaultCredentials = false;
                //    smtp.Credentials = new NetworkCredential(SendingMail, Password);
                //    smtp.EnableSsl = true;
                //    client.DeliveryMethod = SmtpDeliveryMethod.Network;


                //    smtp.Send(mm);
                //}
            }

        }

        private void FindIdenticalMatchFromOddsTable()
        {
            //Foreach Event from OddsTable
            //Filter OddsTable Events by Current MatchSystemID And Get Largest Odds
            //Insert this Event to CalcOddTable
            List<OddsTable> AllEvents = new List<OddsTable>();
            List<CalcOddsTable> Hits = new List<CalcOddsTable>();

            AllEvents = db.OddsTable.ToList();
            foreach (var currentEvent in AllEvents) // find identical Event
            {
                if (currentEvent.HomeSystemID == null || currentEvent.HomeSystemID == Guid.Empty || currentEvent.AwaySystemID == null || currentEvent.AwaySystemID == Guid.Empty)
                    continue;

                CalcOddsTable Hit = new CalcOddsTable();

                Hit.Home = currentEvent.Home;
                Hit.Away = currentEvent.Away;
                Hit.SportType = currentEvent.SportType;
                Hit.SportTypeID = currentEvent.SportTypeID;
                Hit.EventTime = currentEvent.EventTime;
                Hit.EventDateTime = currentEvent.EventDateTime;

                OddsTable LargestOdd = new OddsTable();

                LargestOdd = AllEvents.Where(id => id.HomeSystemID == currentEvent.HomeSystemID && id.AwaySystemID == currentEvent.AwaySystemID && id.SportTypeID == currentEvent.SportTypeID).OrderByDescending(odd => odd.Odd1).FirstOrDefault();
                if (LargestOdd.Odd1 != null)
                { 
                    Hit.Odd1 = LargestOdd.Odd1.Value;
                    Hit.Klada1 = LargestOdd.KladaName;
                }
                LargestOdd = new OddsTable();
                LargestOdd = AllEvents.Where(id => id.HomeSystemID == currentEvent.HomeSystemID && id.AwaySystemID == currentEvent.AwaySystemID && id.SportTypeID == currentEvent.SportTypeID).OrderByDescending(odd => odd.OddX).FirstOrDefault();
                if (LargestOdd.OddX != null)
                {
                    Hit.OddX = LargestOdd.OddX.Value;
                    Hit.KladaX = LargestOdd.KladaName;
                }
                LargestOdd = new OddsTable();
                LargestOdd = AllEvents.Where(id => id.HomeSystemID == currentEvent.HomeSystemID && id.AwaySystemID == currentEvent.AwaySystemID && id.SportTypeID == currentEvent.SportTypeID).OrderByDescending(odd => odd.Odd2).FirstOrDefault();
                if (LargestOdd.Odd2 != null)
                {
                    Hit.Odd2 = LargestOdd.Odd2.Value;
                    Hit.Klada2 = LargestOdd.KladaName;
                }
                LargestOdd = new OddsTable();
                LargestOdd = AllEvents.Where(id => id.HomeSystemID == currentEvent.HomeSystemID && id.AwaySystemID == currentEvent.AwaySystemID && id.SportTypeID == currentEvent.SportTypeID).OrderByDescending(odd => odd.Odd1X).FirstOrDefault();
                if (LargestOdd.Odd1X != null)
                {
                    Hit.Odd1X = LargestOdd.Odd1X.Value;
                    Hit.Klada1X = LargestOdd.KladaName;
                }

                LargestOdd = new OddsTable();
                LargestOdd = AllEvents.Where(id => id.HomeSystemID == currentEvent.HomeSystemID && id.AwaySystemID == currentEvent.AwaySystemID && id.SportTypeID == currentEvent.SportTypeID).OrderByDescending(odd => odd.OddX2).FirstOrDefault();
                if (LargestOdd.OddX2 != null)
                {
                    Hit.OddX2 = LargestOdd.OddX2.Value;
                    Hit.KladaX2 = LargestOdd.KladaName;
                }
                LargestOdd = new OddsTable();
                LargestOdd = AllEvents.Where(id => id.HomeSystemID == currentEvent.HomeSystemID && id.AwaySystemID == currentEvent.AwaySystemID && id.SportTypeID == currentEvent.SportTypeID).OrderByDescending(odd => odd.Odd12).FirstOrDefault();
                if (LargestOdd.Odd12 != null)
                {
                    Hit.Odd12 = LargestOdd.OddX2.Value;
                    Hit.Klada12 = LargestOdd.KladaName;
                }
                Hit.CalcOdd = (decimal)CalculateOdds(Hit);

                Hit.EventID = currentEvent.KladaID;
                db.CalcOddsTable.Add(Hit);
                db.SaveChanges();
            }
        }

        protected void ResetIdentity(object sender, EventArgs e)
        {
            string efConnectionString = ConfigurationManager.ConnectionStrings["HRKladeEntities"].ConnectionString;

            #region Connection string is specific to Entity Framework and contains metadata.

            var builder = new EntityConnectionStringBuilder(efConnectionString);
            var regularConnectionString = builder.ProviderConnectionString;
            #endregion

            SqlConnection connection = new SqlConnection(regularConnectionString);
            string sqlStatement = "";
            connection.Open();

            for (int i = 0; i < 2; i++)
            {
                if(i == 0)
                    sqlStatement = "DELETE FROM OddsTable";
                if(i == 1)
                    sqlStatement = "DBCC CHECKIDENT ('OddsTable', RESEED, 0)";

                SqlCommand cmd = new SqlCommand(sqlStatement, connection);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            
            connection.Close();
        }

        /// <summary>
        /// Returns the number of steps required to transform the source string
        /// into the target string.
        /// </summary>
        private int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        /// <summary>
        /// Calculate percentage similarity of two strings
        /// <param name="source">Source String to Compare with</param>
        /// <param name="target">Targeted String to Compare</param>
        /// <returns>Return Similarity between two strings from 0 to 1.0</returns>
        /// </summary>
        private double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;

            source = source.ToLower();
            target = target.ToLower();
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        public static DateTime GetDatetimeFromString(string dayofWeek)
        {
            string date = string.Empty;
            string dayofWeekLetter = new String(dayofWeek.Where(Char.IsLetter).ToArray()); // get only chars from string
            int currentDayOfWeekInt = (int)DateTime.Today.DayOfWeek; // get today

            if (dayofWeekLetter == string.Empty && new string(dayofWeek.Where(Char.IsNumber).ToArray()) != string.Empty) // Case of "06.09." there is no letters
                date = dayofWeek;
            else if (dayofWeek != string.Empty)
            { 
                #region Case of "21.11 pon 20:00"
                string dateString = dayofWeek.Substring(0, dayofWeek.IndexOf(' ')); // get values from dayofWeek before first whitespace
                date = new string(dateString.Where(Char.IsNumber).ToArray()); // if date exist before first whitespace save date to string date

                #endregion case of "21.11 pon 20:00"
            }
            if (dayofWeek != string.Empty && dayofWeekLetter != string.Empty && date == string.Empty) // if exist dayofWeekLetter && date value is unknown
            { 
                #region Map day of week to INT

                int dayOfWeekInt = 0;
                switch (dayofWeekLetter.ToLower())
                {
                    case "pon":
                        dayOfWeekInt = 1;
                        break;
                    case "uto":
                        dayOfWeekInt = 2;
                        break;
                    case "sri":
                        dayOfWeekInt = 3;
                        break;
                    case "čet":
                        dayOfWeekInt = 4;
                        break;
                    case "pet":
                        dayOfWeekInt = 5;
                        break;
                    case "sub":
                        dayOfWeekInt = 6;
                        break;
                    case "ned":
                        dayOfWeekInt = 0;
                        break;
                }
                #endregion map day of week to INT

                #region Create date

                DateTime today = DateTime.Today;
                var dates = Enumerable.Range(0, 7).Select(days => today.AddDays(days)).ToList();
                DateTime result = new DateTime();

                if (dayOfWeekInt == (int)today.DayOfWeek) // if is today
                {
                    result = today;
                    date = today.Date.ToString();
                }
                if (date == string.Empty)
                { 
                    foreach (var day in dates)
                    {
                        if ((int)day.DayOfWeek == dayOfWeekInt)
                        {
                            result = day.Date;
                            break;
                        }
                    }
                }   
                #endregion create date

                // Add time from string
                int indexOfSecondWhitespace = dayofWeek.LastIndexOf(' '); // time is after second whitespace

                string timeString = dayofWeek.Substring(indexOfSecondWhitespace + 1); // create time string

                DateTime time = DateTime.ParseExact(timeString, "HH:mm",CultureInfo.InvariantCulture); // create time
                result = result.AddTicks(time.TimeOfDay.Ticks); // connect date and time

                return result;
            }
            else if (dayofWeek != string.Empty)// add year before first empty space
            {
                //Find first empty space
                string dateTimeString = string.Empty;
                DateTime dateResult;

                int currentyear = DateTime.Now.Year;

                if (dayofWeek.IndexOf(" ") > 0)
                    dateTimeString = dayofWeek.Insert(dayofWeek.IndexOf(" "), currentyear.ToString()); // insert year before time
                else 
                    dateTimeString = dayofWeek;

                if (!DateTime.TryParse(dateTimeString, CultureInfo.GetCultureInfo("hr-HR"), DateTimeStyles.None, out dateResult)) //create datetime | if false parsed maybe next year needed
                {
                    currentyear = DateTime.Now.AddYears(1).Year; // addd year to currentyear
                    dateTimeString = dayofWeek.Insert(dayofWeek.IndexOf(" "), currentyear.ToString()); // insert year before whitespace
                    dateResult = DateTime.Parse(dateTimeString, CultureInfo.GetCultureInfo("hr-HR")); // create datetime
                }
              
                return dateResult;
            }
            return DateTime.Now;
        }
        
        #region Find Similar
        #region  DOCUMENTATION
        /* 
        Nova tablica MatchSystemIDs
        prebaci polja iz tablice oklada onafterInsert:
        EventName(home i away iz tablice oklada - svaki zasebni redak)
        time
        matchType
        KladaID
        EventSystemID (novo polje)

        napraviti metodu koja će provjeriti dali u novoj tablici MatchSystemIDs postoji EventName s EventSystemID guidom.
        Ako postoji u tablicu oklada zapisati taj EventSystemID (home/away)jh.
        Ako ne postoji insertati rekord u MatchSystemIDs tablicu s praznim guidom

        Logika za prazne GUIDE:

        Korak prije izračuna oklada provjeri tablicu MatchSystemIDs.

        1. Filtriraj na sve one kojima je homeSystemID/awaySystemID  guid prazan
        2. Protrči kroz takve rekorde te 
        za svaki takav filtriraj sljedeće:
	        - kreiraj temp listu i nastavi logiku za svaki rekord
	        - ako lista nije prazna kreiraj im svima isti guid te ih spremi spremi u tabicu MatchSystemIDs
	        - Klada ID različit od trenutnog
	        - matchType jednak kao kod trenutnog (Nogomet/rukomet...
	        - datetime je u periodu plus/minus 4 sata od trenutnog
        Za svaki takav (treba provjeriti podudaranje naziva kluba)
	        - analizirati string dali se slaže sa trenutnim
	        - ako se slaže (pod određenim postotkom)ubaci ga u temp listu - kreiranu na početku prvog foreacha	
        */
        #endregion
        public Guid FindOrInsertToMatchSystemIDsTable(DateTime EventDateTime,string EventName,int SportTypeID,string KladaName)
        {
            MatchSystemIDs matchFound = new MatchSystemIDs();
            if (EventName != string.Empty && (matchFound = db.MatchSystemIDs.Where(m => m.EventName == EventName).FirstOrDefault()) != null && matchFound.EventSystemID != null && matchFound.EventSystemID != Guid.Empty)
                return matchFound.EventSystemID;
            else // insert record to MatchSystemIDs table
            {
                MatchSystemIDs newMatch = new MatchSystemIDs();
                newMatch.EventDateTime = EventDateTime;
                newMatch.EventName = EventName.Trim();
                newMatch.EventSportTypeID = SportTypeID;
                newMatch.KladaName = KladaName;
                newMatch.EventSystemID = Guid.Empty;

                db.MatchSystemIDs.Add(newMatch);
                db.SaveChanges();
            }
            return Guid.Empty;
        }
        public void MatchSystemIDs()
        {
            List <MatchSystemIDs> emptyGuids = new List<MatchSystemIDs>();
            emptyGuids = db.MatchSystemIDs.Where(m => m.EventSystemID == Guid.Empty).ToList();
            List<MatchSystemIDs> eventMatches = new List<MatchSystemIDs>();

            foreach (var currentEvent in emptyGuids)
            {
                if (eventMatches.Count > 0) // if list is not empty set same guid for all values and insert to DB
                {
                    var newGuid = Guid.NewGuid();
                    foreach (var eventMatch in eventMatches)
                    {
                        eventMatch.EventSystemID = newGuid;
                    }
                    db.SaveChanges();
                    eventMatches = new List<MatchSystemIDs>();
                }

                eventMatches.Add(currentEvent); // Insert currentEvent in hit list

                List<MatchSystemIDs> hits = new List<MatchSystemIDs>();
                hits = db.MatchSystemIDs.Where(m => m.KladaName != currentEvent.KladaName && m.EventSportTypeID == currentEvent.EventSportTypeID && m.EventDateTime == currentEvent.EventDateTime).ToList();

                foreach (var hit in hits)
                {
                    if (CalculateSimilarity(hit.EventName, currentEvent.EventName) > tolerance)
                    {
                        eventMatches.Add(hit);
                    }
                }
            }
        }
        #endregion find Similar

        public static int FindAndInsertSportTypeID (string eventType)
        {
            int result = 0;

            #region Insert Sports - EventID

            if (eventType.ToLower().Contains("nogomet") == true && eventType.ToLower().Contains("mali") == false && (eventType.ToLower().Contains("žene") == false || eventType.ToLower().Contains("zene") == false || eventType.ToLower().Contains("americki") == false || eventType.ToLower().Contains("američki") == false))
                return result = 1;
            if (eventType.ToLower().Contains("kosarka") == true || eventType.ToLower().Contains("košarka") == true)
                return result = 2;
            if (eventType.ToLower().Contains("tenis") == true && eventType.ToLower().Contains("stolni") == false)
                return result = 3;
            if (eventType.ToLower().Contains("hokej") == true && eventType.ToLower().Contains("travi") == false)
                return result = 4;
            if (eventType.ToLower().Contains("rukomet") == true)
                return result = 5;
            if (eventType.ToLower().Contains("baseball") == true || eventType.ToLower().Contains("bejzbol") == true)
                return result = 6;
            if (eventType.ToLower().Contains("boks") == true)
                return result = 7;
            if (eventType.ToLower().Contains("esports") == true || eventType.ToLower().Contains("esport") == true || eventType.ToLower().Contains("e sport") == true || eventType.ToLower().Contains("e-sports") == true || eventType.ToLower().Contains("e-sport") == true)
                return result = 8;
            if (eventType.ToLower().Contains("odbojka") == true)
                return result = 9;
            if (eventType.ToLower().Contains("pikado") == true)
                return result = 10;
            if (eventType.ToLower().Contains("rugby") == true || eventType.ToLower().Contains("ragbi") == true)
                return result = 11;
            if (eventType.ToLower().Contains("stol") == true && eventType.ToLower().Contains("tenis") == true) //stolni tenis
                return result = 12;
            if (eventType.ToLower().Contains("ultimate") == true || eventType.ToLower().Contains("fight") == true)
                return result = 13;
            if (eventType.ToLower().Contains("vaterpolo") == true)
                return result = 14;
            if (eventType.ToLower().Contains("hokej") == true && eventType.ToLower().Contains("trav") == true) // hokej na travi
                return result = 15;
            if ((eventType.ToLower().Contains("mali") == true && eventType.ToLower().Contains("nogomet")) || eventType.ToLower().Contains("futsal") == true) //futsal
                return result = 16;
            if (eventType.ToLower().Contains("kriket") == true)
                return result = 17;
            if (eventType.ToLower().Contains("nogomet") == true && (eventType.ToLower().Contains("žene") == true || eventType.ToLower().Contains("zene") == true))
                return result = 18;
            if (eventType.ToLower().Contains("mma") == true)
                return result = 19;
            if (eventType.ToLower().Contains("formula1") == true)
                return result = 20;
            if (eventType.ToLower().Contains("aussierules") == true)
                return result = 21;
            if (eventType.ToLower().Contains("motociklizam") == true)
                return result = 22;
            if (eventType.ToLower().Contains("lacrosse") == true)
                return result = 23;
            if (eventType.ToLower().Contains("floorball") == true)
                return result = 24;
            if (eventType.ToLower().Contains("badminton") == true)
                return result = 25;
            if (eventType.ToLower().Contains("atletika") == true)
                return result = 26;
            if (eventType.ToLower().Contains("borilačkisportovi") == true || eventType.ToLower().Contains("borilackisportovi") == true)
                return result = 27;
            if (eventType.ToLower().Contains("squash") == true)
                return result = 28;
            if (eventType.ToLower().Contains("šah") == true || eventType.ToLower().Contains("sah") == true)
                return result = 29;

            //// Query the database for the row to be updated.
            //var query =
            //    from ord in db.OddsTable
            //    where ord.KladaID == item.KladaID
            //    select ord;

            //query.FirstOrDefault().SportTypeID = eventID;

            //// Submit the changes to the database.
            //db.SaveChanges();

            #endregion

            return result;
        }
    }

    #region Common to all forms
    public static class ControlExtensions
    {
        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            //If you are planning on using a similar function in your own code then please be sure to
            //have a quick read over https://stackoverflow.com/questions/1874728/avoid-calling-invoke-when-the-control-is-disposed
            //No action
            if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
    #endregion common to all forms

}

