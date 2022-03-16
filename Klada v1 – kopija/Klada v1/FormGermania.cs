using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class FormGermania : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public FormGermania()
        {
            InitializeComponent();
            InitializeChromium();

            chromeBrowser.FrameLoadEnd += WebBrowserFrameLoadEndeds;
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            chromeBrowser = new ChromiumWebBrowser("https://www.germaniasport.hr/hr#/date/all");
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
        }

        public void WebBrowserFrameLoadEndeds(object sender, FrameLoadEndEventArgs e)
        {
            // Kada se učita cijeli html napravi htmlDoc iz taskHtml rezultata
            if (e.Frame.IsMain)
            {
                bool isScrollFinished = false;
                chromeBrowser.LoadingStateChanged += async (s, args) =>
                {
                    //Wait for the Page to finish loading
                    if (!args.IsLoading)
                    {
                        // Vremenska odgoda za učitavanje html-a
                        DateTime startTime = DateTime.UtcNow;
                        DateTime? endDate = null;
                        while (endDate == null)
                        {
                            #region Scroll
                            JavascriptResponse scroll = await chromeBrowser.EvaluateScriptAsync("scrollTo(0, document.body.scrollHeight)");
                            if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > 2500 && !isScrollFinished)
                            {
                                //Klikni "Učitaj narednih" događaja [3]
                                JavascriptResponse afterClick = await chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName(\"buttonLoad\")[3].click()");

                                //for (int i = 1000; i < 10000000; i += 350)
                                for (int i = 1000; i < 1000000; i += 250)
                                {
                                    string script = string.Format("scrollTo(0," + i + ")");
                                    JavascriptResponse scroll3 = await chromeBrowser.EvaluateScriptAsync(script);
                                }
                                isScrollFinished = true;
                                startTime = DateTime.UtcNow;
                            }
                            #endregion

                            // Uđi u metodu WebBrowserFrameLoadEnded nakon određenog vremena
                            if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > 70000)
                            {
                                var taskHtml = chromeBrowser.GetBrowser().MainFrame.GetSourceAsync();

                                string html = taskHtml.Result;
                                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                                htmlDoc.LoadHtml(html);

                                chromeBrowser.ViewSource();
                                GetGermaniaDocument(htmlDoc);
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
        public Task<HtmlAgilityPack.HtmlDocument> GetGermaniaDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            #region Nodes Collections

            var eventAllNodes = "//div[contains(@class, 'part1 sportInfo')] | //div[contains(@class, 'match botFlex')]";

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
            int oddsCounter = 0;
            int oddTypeCounter = 0;
            var sportType = "";
            #endregion

            HRKladeEntities db = new HRKladeEntities();
            OddsTable match = new OddsTable();

            foreach (var node in EventAllNodes)
            {
                int homeFlag = 0;
                if (node.Attributes[0].Value == "part1 sportInfo")
                {
                    sportType = node.InnerText.Trim();
                }
                else if (node.HasChildNodes && sportType.ToLower().Contains("duel") == false)
                {
                    foreach (var childItem in node.ChildNodes)
                    {

                        if (childItem.HasAttributes)
                        {
                            if(childItem.Attributes[0].Value == "part1") // Events and Times
                            {
                                foreach (var childChildItem in childItem.ChildNodes)
                                {
                                    if(childChildItem.Name == "div" && childChildItem.HasAttributes && childChildItem.Attributes[0].Value == "infos")
                                        match.SportType = sportType + " " +childChildItem.InnerText.Trim();
                                    if (childChildItem.Name == "a")
                                    {
                                        foreach (var item in childChildItem.ChildNodes)
                                        {
                                            if(item.Name == "span")
                                            {
                                                homeFlag++;
                                                if (item.HasAttributes && item.Attributes[0].Value == "time")
                                                    match.EventTime = item.InnerText.Trim();
                                                if (!item.HasAttributes && homeFlag == 2)
                                                {
                                                    match.Home = Regex.Replace(item.InnerText.Trim(), @"[!#$%&/()=?*]", string.Empty);
                                                }
                                                if (!item.HasAttributes && homeFlag == 3)
                                                {
                                                    match.Away = Regex.Replace(item.InnerText.Trim(), @"[!#$%&/()=?*]", string.Empty);
                                                }
                                            }

                                            }

                                    }
                                }
                            }

                            #region Odd Types 1,X,2,1X,X2,12,F2
                            if (childItem.Attributes[0].Value == "part2") // provjeri koji tipovi postoje
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
                                }
                                #endregion
                                foreach (var childChildItem in childItem.ChildNodes)
                                {
                                    if(childChildItem.HasAttributes && childChildItem.Attributes[0].Value == "part2wrapper")
                                    {
                                        foreach (var item in childChildItem.ChildNodes)
                                        {
                                            if(item.Name == "div")
                                            {
                                                foreach (var span in item.ChildNodes)
                                                {
                                                    if(span.HasAttributes && span.Attributes[0].Value == "top-name")
                                                    {
                                                        #region Set params if exist on site

                                                        if (span.InnerText == "1")
                                                            hasFirstOdd = true;
                                                        if (span.InnerText == "X")
                                                            hasSecOdd = true;
                                                        if (span.InnerText == "2")
                                                            hasThirdOdd = true;
                                                        if (span.InnerText == "1X")
                                                            hasFourthOdd = true;
                                                        if (span.InnerText == "X2")
                                                            hasFifthOdd = true;
                                                        if (span.InnerText == "12")
                                                            hasSixthOdd = true;
                                                        #endregion
                                                    }
                                                    if (!span.HasAttributes)
                                                    {
                                                        #region Odds

                                                        if (hasFirstOdd) // 1
                                                        {
                                                            if (span.InnerText.Contains("-") == true)
                                                                match.Odd1 = 0;
                                                            else
                                                            {
                                                                match.Odd1 = decimal.Parse(span.InnerText, CultureInfo.InvariantCulture);
                                                            }
                                                            match.Odd1 = match.Odd1;
                                                            hasFirstOdd = false;
                                                        }

                                                        if (hasSecOdd) // X
                                                        {
                                                            if (span.InnerText.Contains("-") == true)
                                                                match.OddX = 0;
                                                            else
                                                            {
                                                                match.OddX = decimal.Parse(span.InnerText, CultureInfo.InvariantCulture);
                                                            }
                                                            match.OddX = match.OddX;
                                                            hasSecOdd = false;
                                                        }

                                                        if (hasThirdOdd)// 2
                                                        {
                                                            if (span.InnerText.Contains("-") == true)
                                                                match.Odd2 = 0;
                                                            else
                                                            {
                                                                match.Odd2 = decimal.Parse(span.InnerText, CultureInfo.InvariantCulture);
                                                            }
                                                            match.Odd2 = match.Odd2;
                                                            hasThirdOdd = false;
                                                        }

                                                        if (hasFourthOdd) // 1X
                                                        {
                                                            if (span.InnerText.Contains("-") == true)
                                                                match.Odd1X = 0;
                                                            else
                                                            {
                                                                match.Odd1X = decimal.Parse(span.InnerText, CultureInfo.InvariantCulture);
                                                            }
                                                            match.Odd1X = match.Odd1X;
                                                            hasFourthOdd = false;
                                                        }

                                                        if (hasFifthOdd) // X2
                                                        {
                                                            if (span.InnerText.Contains("-") == true)
                                                                match.OddX2 = 0;
                                                            else
                                                            {
                                                                match.OddX2 = decimal.Parse(span.InnerText, CultureInfo.InvariantCulture);
                                                            }
                                                            match.OddX2 = match.OddX2;
                                                            hasFifthOdd = false;
                                                        }

                                                        if (hasSixthOdd) // 12
                                                        {
                                                            if (span.InnerText.Contains("-") == true)
                                                                match.Odd12 = 0;
                                                            else
                                                            {
                                                                match.Odd12 = decimal.Parse(span.InnerText, CultureInfo.InvariantCulture);
                                                            }
                                                            match.Odd12 = match.Odd12;
                                                            hasSixthOdd = false;
                                                        }

                                                        if (hasSeventhOdd) // F2
                                                        {
                                                            if (span.InnerText.Contains("-") == true)
                                                                match.OddF2 = 0;
                                                            else
                                                            {
                                                                match.OddF2 = decimal.Parse(span.InnerText, CultureInfo.InvariantCulture);
                                                            }
                                                            match.OddF2 = match.OddF2;
                                                            hasSeventhOdd = false;
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    if (match.Odd1 > 0 && (match.Odd2 > 0 || match.OddX > 0)) // ako nisu prazni koeficijenti spremi u bazu
                    {
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "Germania";
                        match.InPlay = false;
                        db.OddsTable.Add(match);
                        db.SaveChanges();
                    }

                    #region Reset params

                    oddsCounter = 0;
                    #region Reset params
                    if (oddsCounter == 0)
                    {
                        oddTypeCounter = 0;
                        match = new OddsTable();
                    }
                    #endregion

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
