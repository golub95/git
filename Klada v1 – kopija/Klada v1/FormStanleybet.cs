using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;
using Klada_v1.Model;
using Application = System.Windows.Forms.Application;

namespace Klada_v1
{
    public partial class FormStanleybet : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public FormStanleybet()
        {
            InitializeComponent();
            InitializeChromium();
            #region First time open browser
            ////First time open browser
            //chromeBrowser.LoadingStateChanged += (s, args) =>
            //{
            //    //Wait for the Page to finish loading
            //    if (!args.IsLoading)
            //    {
            //        // Ovdje se otvori stranica ali učitano je samo prvih nekoliko tablica sa događajima
            //        // klikni nešto ili skorolaj dolje

            //        //chromeBrowser.ViewSource();
            //    }
            //};
            #endregion
            chromeBrowser.FrameLoadEnd += WebBrowserFrameLoadEndeds;
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            //Cef.Initialize(settings); KOM
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("http://www.stanleybet.hr/?p=o&d=88");
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

        }

        public void WebBrowserFrameLoadEndeds(object sender, FrameLoadEndEventArgs e)
        {
            // Kada se učita cijeli html napravi htmlDoc iz taskHtml rezultata
            if (e.Frame.IsMain)
            {
                chromeBrowser.LoadingStateChanged += async (s, args) =>
                {
                    //Wait for the Page to finish loading
                    if (!args.IsLoading)
                    {
                        //Klikni "Učitaj narednih" događaja [3]
                        //JavascriptResponse afterClick = await chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName(\"buttonLoad\")[3].click()");

                        // Vremenska odgoda za učitavanje html-a
                        DateTime startTime = DateTime.UtcNow;
                        DateTime? endDate = null;
                        while (endDate == null)
                        {
                            #region Scroll

                            if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > 1500)
                            {

                                //JavascriptResponse scroll1 = await chromeBrowser.EvaluateScriptAsync("scrollTo(0,document.documentElement.scrollHeight)"); // scroll-a do dna kad se učita dokument
                                //JavascriptResponse scroll2 = await chromeBrowser.EvaluateScriptAsync("scrollTo(0,document.body.scrollHeight)"); // scroll-a do dna (ali nekada i dok nije učitana stranica)
                                //JavascriptResponse scroll1i2 = await chromeBrowser.EvaluateScriptAsync("scrollTo(0, document.body.scrollHeight || document.documentElement.scrollHeight)");

                                for (int i = 1000; i < 100000; i += 150)
                                {
                                    string script = string.Format("scrollTo(0," + i + ")");
                                    JavascriptResponse scroll3 = await chromeBrowser.EvaluateScriptAsync(script);
                                }

                            }
                            #endregion

                            // Uđi u metodu WebBrowserFrameLoadEnded nakon određenog vremena
                            if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > 5000)
                            {
                                var taskHtml = chromeBrowser.GetBrowser().MainFrame.GetSourceAsync();

                                string html = taskHtml.Result;
                                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                                htmlDoc.LoadHtml(html);

                                GetSteanlyDocument(htmlDoc);
                                endDate = DateTime.UtcNow;
                            }
                        }
                    }
                    e.Browser.CloseBrowser(true);
                    Cef.ClearSchemeHandlerFactories();
                    this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
                };
            }
        }

        public Task<HtmlAgilityPack.HtmlDocument> GetSteanlyDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            #region Nodes Collections

            var eventAllNodes = "//h2 | //a[contains(@class, 'switch')] | //div[contains(@class, 'result  razrada-01')] //tr";

            HtmlNodeCollection EventAllNodes = htmlDoc.DocumentNode.SelectNodes(eventAllNodes);

            #endregion

            #region Save stufs on new document

            #region Params
            bool hasFirstOdd = false;
            bool hasSecOdd = false;
            bool hasThirdOdd = false;
            bool hasFourthOdd = false;
            bool hasFifthOdd = false;
            bool hasSixthOdd = false;
            bool hasSeventhOdd = false;
            bool onlyTwoOddTypes = false;
            int oddsCounter = 0;
            int oddTypeCounter = 0;
            #endregion

            HRKladeEntities db = new HRKladeEntities();
            OddsTable match = new OddsTable();

            var eventDay = "";

            string eventType = "";

            foreach (var node in EventAllNodes)
            {
                if (node.Name == "a" && node.HasAttributes && node.Attributes[0].Value == "switch") // EventType
                    eventType = node.InnerText.Trim();
                if (node.Name == "h2")// EventTime custom logic
                {
                    eventDay = node.InnerText.Substring(0, 3); //get first 3 char from string
                    var date = Regex.Replace(node.InnerText, "[A-ZČa-zč ]", ""); // remove chars from string

                    DateTime enteredDate = DateTime.Parse(date); // parse string to datetime
                    if (DateTime.Now.AddDays(4) < enteredDate) 
                    {
                        eventDay = eventDay + " " + date.Substring(0, 5);
                    }
                }

                if (node.HasChildNodes && eventType.ToLower().Contains("duel")== false)
                {
                    foreach (var childItem in node.ChildNodes)
                    {

                        if (childItem.InnerText.Length > 0)
                        {
                            #region Odd Types 1,X,2,1X,X2,12,F2
                            if (childItem.HasAttributes && (childItem.Attributes[0].Value == "coef" || childItem.Attributes[0].Value == "coef last")) // provjeri koji tipovi postoje
                            {
                                #region Reset params
                                if (oddTypeCounter == 0)
                                {
                                    hasFirstOdd = false;
                                    hasSecOdd = false;
                                    hasThirdOdd = false;
                                    hasFourthOdd = false;
                                    hasFifthOdd = false;
                                    hasSixthOdd = false;
                                    hasSeventhOdd = false;
                                    onlyTwoOddTypes = false;
                                }
                                oddTypeCounter++;
                                #endregion

                                #region Set params if exist on site

                                if (childItem.InnerText == "1")
                                    hasFirstOdd = true;
                                if (childItem.InnerText == "X")
                                    hasSecOdd = true;
                                if (childItem.InnerText == "2")
                                    hasThirdOdd = true;
                                if (childItem.InnerText == "1X")
                                    hasFourthOdd = true;
                                if (childItem.InnerText == "X2")
                                    hasFifthOdd = true;
                                if (childItem.InnerText == "12")
                                    hasSixthOdd = true;
                                if (childItem.InnerText == "H1-1")
                                    hasSeventhOdd = true;

                                if (childItem.Attributes[0].Value == "coef last" && childItem.InnerText == "2")
                                    onlyTwoOddTypes = true;

                                #endregion

                            }
                            #endregion

                            #region Odds

                            if (childItem.InnerText.Length > 0 && childItem.HasAttributes && (childItem.Attributes[0].Value.Contains("ticket_action(this)") == true))
                            {
                                #region Reset params
                                if (oddsCounter == 0)
                                {
                                    oddTypeCounter = 0;
                                    match.Odd1 = 0;
                                    match.OddX = 0;
                                    match.Odd2 = 0;
                                    match.Odd1X = 0;
                                    match.OddX2 = 0;
                                    match.Odd12 = 0;
                                    match.OddF2 = 0;
                                }
                                #endregion

                                oddsCounter++;
                                if (hasFirstOdd && oddsCounter == 1) // 1
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.Odd1 = 0;
                                    else
                                    {
                                        match.Odd1 = Convert.ToDecimal(childItem.InnerText);
                                    }
                                    match.Odd1 = match.Odd1;
                                }

                                if (hasSecOdd && oddsCounter == 2 && !onlyTwoOddTypes) // X
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.OddX = 0;
                                    else
                                    {
                                        match.OddX = Convert.ToDecimal(childItem.InnerText);
                                    }
                                    match.OddX = match.OddX;
                                }

                                if ((hasThirdOdd && oddsCounter == 3) || hasThirdOdd && onlyTwoOddTypes)// 2
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.Odd2 = 0;
                                    else
                                    {
                                        match.Odd2 = Convert.ToDecimal(childItem.InnerText);
                                    }
                                    match.Odd2 = match.Odd2;
                                }

                                if (hasFourthOdd && oddsCounter == 4) // 1X
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.Odd1X = 0;
                                    else
                                    {
                                        match.Odd1X = Convert.ToDecimal(childItem.InnerText);
                                    }
                                    match.Odd1X = match.Odd1X;
                                }

                                if (hasFifthOdd && oddsCounter == 5) // X2
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.OddX2 = 0;
                                    else
                                    {
                                        match.OddX2 = Convert.ToDecimal(childItem.InnerText);
                                    }
                                    match.OddX2 = match.OddX2;
                                }

                                if (hasSixthOdd && oddsCounter == 6) // 12
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.Odd12 = 0;
                                    else
                                    {
                                        match.Odd12 = Convert.ToDecimal(childItem.InnerText);
                                    }
                                    match.Odd12 = match.Odd12;
                                }

                                if (hasSeventhOdd && oddsCounter == 7) // F2
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.OddF2 = 0;
                                    else
                                    {
                                        match.OddF2 = Convert.ToDecimal(childItem.InnerText);
                                    }
                                    match.OddF2 = match.OddF2;
                                }
                            }
                            #endregion

                            // Events and Times
                            if (childItem.HasAttributes && childItem.Attributes[0].Value == "name" && childItem.InnerText.Length > 0)  //Home, Away and Time nodes
                            {
                                #region Home and Away
                                string homeAndaway = childItem.InnerText;
                                var output = Regex.Replace(homeAndaway, @" ?\(.*?\)", string.Empty); // delete chars in brackets

                                string[] tokens = output.Split('-');
                                string home = tokens[0];
                                string away = tokens[1];

                                if (home.Contains("Chicago Sky") == true)
                                    homeAndaway = "bla";

                                match.Home = Regex.Replace(home, @"[!#$%&/()=?*]", string.Empty);
                                match.Away = Regex.Replace(away, @"[!#$%&/()=?*]", string.Empty);
                                db.OddsTable.Add(match);
                                #endregion

                            }
                            if (childItem.HasAttributes && (childItem.Attributes[0].Value == "date-zone" || childItem.Attributes[0].Value == "time") && childItem.InnerText.Length > 0)  //Home, Away and Time nodes
                            {
                                #region Time
                                var time = childItem.InnerText;
                                if (time.Contains("&") == true)
                                    time = time.Remove(time.LastIndexOf('&'));

                                //ako sadrži dan uzmi taj dan, a ne onaj iz naslova
                                if (time.Contains("pon") == true || time.Contains("uto") == true || time.Contains("sri") == true || time.Contains("cet") == true || time.Contains("pet") == true || time.Contains("sub") == true || time.Contains("ned") == true)
                                {
                                    match.EventTime = time;
                                }
                                else
                                {
                                    string onlyTime = Regex.Replace(time, "[A-Za-z ]", ""); // remove all char
                                    match.EventTime = eventDay + " " + onlyTime;
                                }

                                
                                #endregion

                            }
                        }
                    }
                    if (match.Odd1 > 0 && (match.Odd2 > 0 || match.OddX > 0)) // ako nisu prazni koeficijenti spremi u bazu
                    {
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "Stanleybet";
                        match.InPlay = false;
                        match.SportType = eventType;
                        db.SaveChanges();
                    }

                    #region Reset params

                    oddsCounter = 0;

                    #endregion
                }
            }
            #endregion

            return null;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region This is closing Forms and Browser
            // Koristi na predposljednjoj formi
            Dispose();
            this.Close();
            //Cef.Shutdown();
            #endregion
        }
    }
}
