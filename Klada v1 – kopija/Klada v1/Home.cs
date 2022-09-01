using System;
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
        private readonly string Password = "FeRiT.2018";
        private readonly string ReceivingMail = "terry.ferit@gmail.com";
        private readonly string Subject = "Hit";

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
                FindSimilar();
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
                //TODO:client.Send(mm);
            }

        }

        private void FindSimilar()
        {
            double tolerance = 0.5, toleranceSum = 1.3;
            List<CalcOddsTable> Hits = new List<CalcOddsTable>();



            bool hitFlag = false;

            Events = db.OddsTable.ToList();

            int numberOfEvents = Events.Count();

            double test1, test2;

            #region Insert Sports - EventID
            OddsTable match = new OddsTable();

            foreach (var item in Events)
            {
                int eventID = 0;
                string eventType = item.SportType.ToLower();


                if (eventType.ToLower().Contains("nogomet") == true && eventType.ToLower().Contains("mali") == false && (eventType.ToLower().Contains("žene") == false || eventType.ToLower().Contains("zene") ==false))
                    eventID= 1;
                if (eventType.ToLower().Contains("kosarka") == true || eventType.ToLower().Contains("košarka") == true)
                    eventID= 2;
                if (eventType.ToLower().Contains("tenis") == true && eventType.ToLower().Contains("stolni") == false)
                    eventID= 3;
                if (eventType.ToLower().Contains("hokej") == true && eventType.ToLower().Contains("travi") == false)
                    eventID= 4;
                if (eventType.ToLower().Contains("rukomet") == true)
                    eventID= 5;
                if (eventType.ToLower().Contains("baseball") == true || eventType.ToLower().Contains("bejzbol") == true)
                    eventID= 6;
                if (eventType.ToLower().Contains("boks") == true)
                    eventID= 7;
                if (eventType.ToLower().Contains("esports") == true || eventType.ToLower().Contains("esport") == true || eventType.ToLower().Contains("e sport") == true || eventType.ToLower().Contains("e-sports") == true)
                    eventID= 8;
                if (eventType.ToLower().Contains("odbojka") == true)
                    eventID= 9;
                if (eventType.ToLower().Contains("pikado") == true)
                    eventID= 10;
                if (eventType.ToLower().Contains("rugby") == true || eventType.ToLower().Contains("ragbi") == true)
                    eventID= 11;
                if (eventType.ToLower().Contains("stol") == true && eventType.ToLower().Contains("tenis") == true) //stolni tenis
                    eventID= 12;
                if (eventType.ToLower().Contains("ultimate") == true || eventType.ToLower().Contains("fight") == true)
                    eventID= 13;
                if (eventType.ToLower().Contains("vaterpolo") == true)
                    eventID= 14;
                if (eventType.ToLower().Contains("hokej") == true && eventType.ToLower().Contains("trav") == true) // hokej na travi
                    eventID= 15;
                if ((eventType.ToLower().Contains("mali") == true && eventType.ToLower().Contains("nogomet")) || eventType.ToLower().Contains("futsal") == true) //futsal
                    eventID= 16;
                if (eventType.ToLower().Contains("kriket") == true)
                    eventID = 17;
                if (eventType.ToLower().Contains("nogomet") == true && (eventType.ToLower().Contains("žene") == true || eventType.ToLower().Contains("zene") == true))
                    eventID = 18;

                // Query the database for the row to be updated.
                var query =
                    from ord in db.OddsTable
                    where ord.KladaID == item.KladaID
                    select ord;

                query.FirstOrDefault().SportTypeID = eventID;

                // Submit the changes to the database.
                db.SaveChanges();

            }
            #endregion




            int EventID = 0;
            string Home = "";
            string Away = "";
            string AllHome = "";
            string AllAway = "";
            string EventTime = "";
            Nullable<decimal> Odd1 = (decimal) 0.0;
            string Klada1 = "";
            Nullable<decimal> OddX = (decimal)0.0;
            string KladaX = "";
            Nullable<decimal> Odd2 = (decimal)0.0;
            string Klada2 = "";
            Nullable<decimal> Odd1X = (decimal)0.0;
            string Klada1X = "";
            Nullable<decimal> OddX2 = (decimal)0.0;
            string KladaX2 = "";
            Nullable<decimal> Odd12 = (decimal)0.0;
            string Klada12 = "";

            for (int FirstEvent = 0; FirstEvent < numberOfEvents; FirstEvent++)
            {
                for (int SecondEvent = FirstEvent + 1; SecondEvent < numberOfEvents; SecondEvent++)
                {
                    string firstEventDay = Events[FirstEvent].EventTime.Substring(0, 3);
                    string secEventDay = Events[SecondEvent].EventTime.Substring(0, 3);

                    if (firstEventDay.ToLower().Contains(secEventDay.ToLower()) == false || Events[FirstEvent].SportTypeID != Events[SecondEvent].SportTypeID || (Events[FirstEvent].KladaName == Events[SecondEvent].KladaName) || Events[FirstEvent].InPlay==true)
                    {
                        continue;
                    }
                    if (!hitFlag)
                    {
                        test1 = CalculateSimilarity(Events[FirstEvent].Home, Events[SecondEvent].Home);
                        test2 = CalculateSimilarity(Events[FirstEvent].Away, Events[SecondEvent].Away);
                        if ((test1 > tolerance && test2 > tolerance) && (test1 + test2 >= toleranceSum))
                        {
                            Events[SecondEvent].InPlay = true;
                            hitFlag = true;
                            AllHome = Events[FirstEvent].Home + ';' + Events[SecondEvent].Home;
                            AllAway = Events[FirstEvent].Away + ';' + Events[SecondEvent].Away;

                            EventID = Events[FirstEvent].KladaID;
                            Home = Events[FirstEvent].Home;
                            Away = Events[FirstEvent].Away;
                            EventTime = Events[FirstEvent].EventTime;
                            Odd1 = (Events[FirstEvent].Odd1 > Events[SecondEvent].Odd1) ? (Events[FirstEvent].Odd1) : (Events[SecondEvent].Odd1);
                            Klada1 = (Events[FirstEvent].Odd1 > Events[SecondEvent].Odd1) ? (Events[FirstEvent].KladaName) : (Events[SecondEvent].KladaName);
                            OddX = (Events[FirstEvent].OddX > Events[SecondEvent].OddX) ? (Events[FirstEvent].OddX) : (Events[SecondEvent].OddX);
                            KladaX = (Events[FirstEvent].OddX > Events[SecondEvent].OddX) ? (Events[FirstEvent].KladaName) : (Events[SecondEvent].KladaName);
                            Odd2 = (Events[FirstEvent].Odd2 > Events[SecondEvent].Odd2) ? (Events[FirstEvent].Odd2) : (Events[SecondEvent].Odd2);
                            Klada2 = (Events[FirstEvent].Odd2 > Events[SecondEvent].Odd2) ? (Events[FirstEvent].KladaName) : (Events[SecondEvent].KladaName);
                            Odd1X = (Events[FirstEvent].Odd1X > Events[SecondEvent].Odd1X) ? (Events[FirstEvent].Odd1X) : (Events[SecondEvent].Odd1X);
                            Klada1X = (Events[FirstEvent].Odd1X > Events[SecondEvent].Odd1X) ? (Events[FirstEvent].KladaName) : (Events[SecondEvent].KladaName);
                            OddX2 = (Events[FirstEvent].OddX2 > Events[SecondEvent].OddX2) ? (Events[FirstEvent].OddX2) : (Events[SecondEvent].OddX2);
                            KladaX2 = (Events[FirstEvent].OddX2 > Events[SecondEvent].OddX2) ? (Events[FirstEvent].KladaName) : (Events[SecondEvent].KladaName);
                            Odd12 = (Events[FirstEvent].Odd12 > Events[SecondEvent].Odd12) ? (Events[FirstEvent].Odd12) : (Events[SecondEvent].Odd12);
                            Klada12 = (Events[FirstEvent].Odd12 > Events[SecondEvent].Odd12) ? (Events[FirstEvent].KladaName) : (Events[SecondEvent].KladaName);

                            
                        }
                    }
                    else
                    {
                        string[] HomeGroup = AllHome.Split(';');
                        string[] AwayGroup = AllAway.Split(';');
                        var groupLenght = HomeGroup.Length;
                        if (AwayGroup.Length < HomeGroup.Length)
                        {
                            groupLenght = AwayGroup.Length;
                        }
                        for (int i = 0; i < groupLenght; i++)
                        {

                            test1 = CalculateSimilarity(HomeGroup[i], Events[SecondEvent].Home);
                            test2 = CalculateSimilarity(AwayGroup[i], Events[SecondEvent].Away);
                            if ((test1 > tolerance && test2 > tolerance) && (test1+test2>= toleranceSum))
                            {
                                AllHome += ';' + Events[SecondEvent].Home;
                                AllAway += ';' + Events[SecondEvent].Away;

                                Odd1 = (Odd1 > Events[SecondEvent].Odd1) ? (Odd1) : (Events[SecondEvent].Odd1);
                                Klada1 = (Odd1 > Events[SecondEvent].Odd1) ? (Klada1) : (Events[SecondEvent].KladaName);
                                OddX = (OddX > Events[SecondEvent].OddX) ? (OddX) : (Events[SecondEvent].OddX);
                                KladaX = (OddX > Events[SecondEvent].OddX) ? (KladaX) : (Events[SecondEvent].KladaName);
                                Odd2 = (Odd2 > Events[SecondEvent].Odd2) ? (Odd2) : (Events[SecondEvent].Odd2);
                                Klada2 = (Odd2 > Events[SecondEvent].Odd2) ? (Klada2) : (Events[SecondEvent].KladaName);
                                Odd1X = (Odd1X > Events[SecondEvent].Odd1X) ? (Odd1X) : (Events[SecondEvent].Odd1X);
                                Klada1X = (Odd1X > Events[SecondEvent].Odd1X) ? (Klada1X) : (Events[SecondEvent].KladaName);
                                OddX2 = (OddX2 > Events[SecondEvent].OddX2) ? (OddX2) : (Events[SecondEvent].OddX2);
                                KladaX2 = (OddX2 > Events[SecondEvent].OddX2) ? (KladaX2) : (Events[SecondEvent].KladaName);
                                Odd12 = (Odd12 > Events[SecondEvent].Odd12) ? (Odd12) : (Events[SecondEvent].Odd12);
                                Klada12 = (Odd12 > Events[SecondEvent].Odd12) ? (Klada12) : (Events[SecondEvent].KladaName);
                                break;
                            }
                        }

                    }

                }
                if (hitFlag)
                {
                    hitFlag = false;

                    CalcOddsTable Hit = new CalcOddsTable(EventID, Home, Away, EventTime, Odd1, Klada1, OddX, KladaX, Odd2, Klada2, Odd1X, Klada1X, OddX2, KladaX2, Odd12, Klada12);
                    Hit.CalcOdd = (decimal)CalculateOdds(Hit);
                    Hits.Add(Hit);
                }
            }

            List<CalcOddsTable> SimilarEvents = new List<CalcOddsTable>();
            SimilarEvents = db.CalcOddsTable.ToList();
            if (SimilarEvents.Count > 0)
            {
                foreach (var SEvent in SimilarEvents)
                {
                    db.CalcOddsTable.Remove(SEvent);
                }
                db.SaveChanges();
            }

            foreach (var hit in Hits)
            {
                try
                {
                    db.CalcOddsTable.Add(hit);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
                
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
            source = source.ToLower();
            target = target.ToLower();
            if ((source == null) || (target == null)) return 0.0;
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

                #region Calculate dateTime

                //if (currentDayOfWeekInt == dayOfWeekInt)
                //    dayOfWeekInt = currentDayOfWeekInt;
                //else if (currentDayOfWeekInt > dayOfWeekInt)
                //    dayOfWeekInt = 7 -(currentDayOfWeekInt - dayOfWeekInt); // if dayOfWeekInt = 2 and dayOfWeekInt = 2 next monday currentDay + 6  => 7-(2-1)

                //DateTime result = DateTime.Today.AddDays(dayOfWeekInt - currentDayOfWeekInt); // Replace string with date
               
                #endregion calculate dateTime

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

        public static DateTime ParseString2DateTime(string dateTimeString)
        {
            //string date = "12/28/2019";

            DateTime dateTime;
            string[] validformats = new[] { "MM/dd/yyyy", "dd-MM-yyyy HH:mm:ss, fff", "yyyy/MM/dd", "MM/dd/yyyy HH:mm:ss",
                                        "MM/dd/yyyy hh:mm tt", "yyyy-MM-dd HH:mm:ss, fff","dd-MM-yyyy HH:mm" };

            CultureInfo provider = CultureInfo.InvariantCulture;

            if (DateTime.TryParseExact(dateTimeString, validformats, provider,
                                        DateTimeStyles.None, out dateTime))
            {
                Console.WriteLine("The specified date is valid: " + dateTime);
            }
            else
            {
                Console.WriteLine("Unable to parse the specified date");
            }
            /*
                Output: The specified date is valid: 28-12-2019 12.00.00 AM
            */
            return dateTime;
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

