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
    public partial class FormHL : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public FormHL()
        {
            InitializeComponent();
            InitializeChromium();
            chromeBrowser.FrameLoadEnd += WebBrowserFrameLoadEnded;
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            //Cef.Initialize(settings); KOM
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("https://www.lutrija.hr/crobet?game=sport#/sport/CustomOffer=All");
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

        }

        public void WebBrowserFrameLoadEnded(object sender, FrameLoadEndEventArgs e)
        {
            // Kada se učita cijeli html napravi htmlDoc iz taskHtml rezultata
            if (e.Frame.IsMain)
            {
                chromeBrowser.LoadingStateChanged += async (s, args) =>
                {
                    //Wait for the Page to finish loading
                    if (!args.IsLoading)
                    {
                        //Klikni "Učitaj narednih" događaja [2]
                        JavascriptResponse afterClick = await chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName(\"buttonLoad\")[2].click()");
                        //Klikni "Učitaj narednih" događaja [3]
                        afterClick = await chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName(\"buttonLoad\")[3].click()");

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

                                for (int i = 1000; i < 100000; i += 50)
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

                                //GetHLDocument(htmlDoc);
                                GetDoc1(htmlDoc);
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
        static List<AchievementLayout> GetDoc1(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = htmlDoc;
            List<AchievementLayout> achievements = new List<AchievementLayout>();
            //Console.WriteLine(GameName);
            Console.WriteLine();
            foreach (HtmlNode li in doc.DocumentNode.SelectNodes("//li"))
            {
                AchievementLayout al = new AchievementLayout();
                if (li.InnerHtml.Contains("-achievement") && li.InnerHtml.Contains("data-bf"))
                {
                    var part1 = li.SelectSingleNode(".//a").InnerText;
                    string hrefValue = li.SelectSingleNode(".//a").GetAttributeValue("href", string.Empty).SplitWithString("/")[2];
                    var part2 = li.SelectSingleNode(".//p").GetAttributeValue("data-bf", string.Empty);
                    var part3 = li.SelectSingleNode(".//p").InnerText;
                    var score = part2.SplitWithString(" ")[0];
                    al.Name = part1;
                    al.Details = part3;
                    al.Score = score;
                    al.Icon = "";
                    if (li.OuterHtml.Contains("(Secret)"))
                    {
                        al.IsSecret = true;
                    }
                    else
                    {
                        al.IsSecret = false;
                    }

                    achievements.Add(al);
                }
            }
            return achievements;
        }
            public Task<HtmlAgilityPack.HtmlDocument> GetHLDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            #region Nodes Collections

            var eventAllNodes =
                "//div[contains(@class, 'sport-header nogomet')" +
                "or contains(@class,'sport-header kosarka') " +
                "or contains(@class, 'sport-header tenis') " +
                "or contains(@class, 'sport-header hokej')" +
                "or contains(@class, 'sport-header rukomet') " +
                "or contains(@class, 'sport-header baseball') " +
                "or contains(@class, 'sport-header boks') " +
                "or contains(@class, 'sport-header esports') " +
                "or contains(@class, 'sport-header odbojka') " +
                "or contains(@class, 'sport-header pikado') " +
                "or contains(@class, 'sport-header ragbi') " +
                "or contains(@class, 'sport-header snooker') " +
                "or contains(@class, 'sport-header stolni_tenis') " +
                "or contains(@class, 'sport-header ultimate-fight') " +
                "or contains(@class, 'sport-header vaterpolo') " +
                "or contains(@class, 'sport-header hokej-na-travi') " +
                "or contains(@class, 'competition mbs') " +
                "or contains(@class, 'sport-header mali-nogomet') ]" +
            " | //div[contains(@class, 'competition nogomet')" +
                "or contains(@class,'competition kosarka') " +
                "or contains(@class, 'competition tenis') " +
                "or contains(@class, 'competition hokej')" +
                "or contains(@class, 'competition rukomet') " +
                "or contains(@class, 'competition baseball') " +
                "or contains(@class, 'competition boks') " +
                "or contains(@class, 'competition esports') " +
                "or contains(@class, 'competition odbojka') " +
                "or contains(@class, 'competition pikado') " +
                "or contains(@class, 'competition ragbi') " +
                "or contains(@class, 'competition snooker') " +
                "or contains(@class, 'competition stolni_tenis') " +
                "or contains(@class, 'competition ultimate-fight') " +
                "or contains(@class, 'competition vaterpolo') " +
                "or contains(@class, 'competition hokej-na-travi') " +
                "or contains(@class, 'competition mbs') " +
                "or contains(@class, 'competition mali-nogomet') ] // tr[contains(@class, 'event') ]";

            //var eventAllNodes = "//div[contains(@class, 'competition boks')]//tr[contains(@class, 'ponuda')] | //div[contains(@class, 'competition boks')] //tr[contains(@class, 'tipovi')]";

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
            var eventType = "";

            foreach (var node in EventAllNodes)
            {
                if (node.Name == "div" && node.HasAttributes && node.Attributes[0].Value.Contains("header") == true)
                    eventType = node.InnerText.Trim();
                if (node.HasChildNodes && eventType.ToLower().Contains("duel") == false)
                {
                    foreach (var childItem in node.ChildNodes)
                    {

                        if (childItem.InnerHtml.Length > 0)
                        {
                            #region Odd Types 1,X,2,1X,X2,12,F2
                            if (childItem.HasAttributes && childItem.Attributes[0].Value == "tip td-shrink") // provjeri koji tipovi postoje
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
                                if (childItem.InnerText == "f+2")
                                    hasSeventhOdd = true;

                                if (node.InnerText == "12")
                                    onlyTwoOddTypes = true;
                                #endregion

                            }
                            #endregion

                            #region Odds

                            if (childItem.InnerText.Length > 0 && (childItem.HasAttributes && (childItem.Attributes[0].Value == "sportska-tecaj td-shrink simple tecaj clickable" || childItem.Attributes[0].Value == "sportska-tecaj td-shrink simple tecaj")))
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
                            if (childItem.HasAttributes && (childItem.Attributes[0].Value == "ponuda-info clickable" || childItem.Attributes[0].Value == "ponuda-info") && childItem.InnerText.Length > 0)  //Home, Away and Time nodes
                            {
                                #region Home and Away
                                string homeAndaway = childItem.LastChild.FirstChild.InnerText;
                                string away = "";
                                var output = Regex.Replace(homeAndaway, @" ?\(.*?\)", string.Empty); // delete chars in brackets

                                string[] tokens = output.Split('-');
                                string home = tokens[0];
                                if (homeAndaway.Contains("-") == true)
                                {
                                    away = tokens[1];
                                }

                                match.Home = Regex.Replace(home, @"[!#$%&/()=?*]", string.Empty);
                                match.Away = Regex.Replace(away, @"[!#$%&/()=?*]", string.Empty);
                                if (match.KladaName == null)
                                    db.OddsTable.Add(match);
                                #endregion

                                #region Time
                                var time = childItem.LastChild.LastChild.InnerText;
                                if (time.Contains("&") == true)
                                    time = time.Remove(time.LastIndexOf('&'));

                                match.EventTime = time;
                                #endregion
                            }
                        }
                    }
                    if (match.Odd1 > 0 && (match.Odd2 > 0 || match.OddX > 0)) // ako nisu prazni koeficijenti spremi u bazu
                    {
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "HL";
                        match.InPlay = false;
                        match.SportType = eventType;
                        db.SaveChanges();
                        match = new OddsTable();
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

    #region My temp
    public class TestMyExample
    {
        static List<List<AchievementLayout>> AllCheevos = new List<List<AchievementLayout>>();
        static List<string> Games = new List<string> { "Grand-Theft-Auto-V", "Grand-Theft-Auto-4" };
        public static void Main2()
        {
            foreach (var item in Games)
            {
                List<AchievementLayout> cheevos = GetGameAchievements(item);
                AllCheevos.Add(cheevos);
            }
        }

        static List<AchievementLayout> GetGameAchievements(string GameName)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = web.Load("https://www.trueachievements.com/game/" + GameName + "/achievements");
            List<AchievementLayout> achievements = new List<AchievementLayout>();
            Console.WriteLine(GameName);
            Console.WriteLine();
            foreach (HtmlNode li in doc.DocumentNode.SelectNodes("//li"))
            {
                AchievementLayout al = new AchievementLayout();
                if (li.InnerHtml.Contains("-achievement") && li.InnerHtml.Contains("data-bf"))
                {
                    var part1 = li.SelectSingleNode(".//a").InnerText;
                    string hrefValue = li.SelectSingleNode(".//a").GetAttributeValue("href", string.Empty).SplitWithString("/")[2];
                    var part2 = li.SelectSingleNode(".//p").GetAttributeValue("data-bf", string.Empty);
                    var part3 = li.SelectSingleNode(".//p").InnerText;
                    var score = part2.SplitWithString(" ")[0];
                    al.Name = part1;
                    al.Details = part3;
                    al.Score = score;
                    al.Icon = "";
                    if (li.OuterHtml.Contains("(Secret)"))
                    {
                        al.IsSecret = true;
                    }
                    else
                    {
                        al.IsSecret = false;
                    }

                    achievements.Add(al);
                }
            }

            Console.WriteLine(GameName + " [" + achievements.Count + "]");
            return achievements;
        }
    }

    [System.Serializable]
    public class AchievementLayout
    {
        public string Name;
        public string Details;
        public string Score;
        public string Icon;
        public bool IsSecret;
        public bool CanBeMissed;
    }

    static class StringExtensions
    {
        public static string[] SplitWithString(this string text, string msg)
        {
            return text.Split(new string[] { msg }, StringSplitOptions.None);
        }
    }
    #endregion
}

