using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;
using Klada_v1.Model;
using Application = System.Windows.Forms.Application;

namespace Klada_v1
{
    public partial class FormPSK : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public FormPSK()
        {
            InitializeComponent();
            InitializeChromiumAsync();

            chromeBrowser.FrameLoadEnd += WebBrowserFrameLoadEndeds;

            Console.WriteLine("Finished");
        }

        public async Task InitializeChromiumAsync()
        {
            CefSettings settings = new CefSettings();
            chromeBrowser = new ChromiumWebBrowser("https://www.psk.hr/oklade/nogomet");
            //chromeBrowser = new ChromiumWebBrowser();
            //await chromeBrowser.LoadPageAsync("https://www.psk.hr/oklade/nogomet");

            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            Console.WriteLine("initialization");
        }

        public void WebBrowserFrameLoadEndeds(object sender, FrameLoadEndEventArgs e)
        {
            JavascriptResponse response = new JavascriptResponse();

            Console.WriteLine("WebBrowserFrameLoadEndeds");

            //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
            chromeBrowser.LoadingStateChanged += async (s, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    //chromeBrowser.EvaluateScriptAsync("alert('All Resources Have Loaded');");
                    Console.WriteLine("IsLoading");

                    //Console.WriteLine("Awaiting task");
                    //await writeAsync();
                    //Console.WriteLine("Task finished");
                    //Console.WriteLine("Awaiting scroll");
                    //await Scrolling();
                    //Console.WriteLine("scroll finished");
                    //ScrollAsync();

                    //response = chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()").Result;
                    //getScrollPositionTask.ContinueWith(t =>
                    //{
                    //    var response = t.Result;
                    //    if (response.Success)
                    //    {
                    //        Console.WriteLine("Current scroll position is " + response.Result);
                    //        Task.Delay(2000).ContinueWith(t2 =>
                    //        {
                    //            chromeBrowser.ViewSource();
                    //        });
                    //    }
                    //}).Wait();
                    if (Scroll().IsFaulted)
                    {
                        chromeBrowser.ViewSource();
                    }

                    //await CopySourceToClipBoardAsync(chromeBrowser);

                    //var htmlSource = await chromeBrowser.GetSourceAsync();
                    //var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.txt");
                    //TextWriter sw = new StreamWriter(filePath);
                    //sw.Write(htmlSource);


                    //SetHeightFromDocument(chromeBrowser);
                    //var task = chromeBrowser.EvaluateScriptAsync("(function() { var body = document.body, html = document.documentElement; return  Math.max( body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight ); })();");

                    // var taskHtml = chromeBrowser.GetBrowser().MainFrame.GetSourceAsync();
                    //Console.WriteLine(taskHtml);
                    //Task<string> doc = chromeBrowser.GetSourceAsync();
                    //chromeBrowser.ViewSource();
                }
                else
                {
                    chromeBrowser.ViewSource();
                }
            };
            Console.WriteLine(response);
            //Wait for the MainFrame to finish loading
            chromeBrowser.FrameLoadEnd += async (s, args) =>
            {
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
                    Console.WriteLine("IsMain");
                    //ScrollAsync();

                    //Console.WriteLine("MainFrame finished loading");

                    //args.Frame.ExecuteJavaScriptAsync("alert('MainFrame finished loading');");
                }
            };
            Console.WriteLine("dva");
        }

        Task Scrolling()
        {
            //JavascriptResponse response = new JavascriptResponse();
            //var task = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()"));
            //response = await chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");
            //string script = string.Format("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");
            //EvaluateScript(script, null,new TimeSpan());


            //response = await chromeBrowser.EvaluateScriptAsync(
            //    ("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()"));

            //await chromeBrowser.EvaluateScriptAsync(
            //    ("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()"));
            return Task.Run(() => Scroll());

        }

        public async Task Scroll()
        {
            chromeBrowser.ExecuteScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");

            //Task<JavascriptResponse> getScrollPositionTask = chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");
            //getScrollPositionTask.ContinueWith(t => {
            //    var response = t.Result;
            //    if (response.Success)
            //    {
            //        Console.WriteLine("Current scroll position is " + response.Result);
            //        Task.Delay(2000).ContinueWith(t2 => {
            //            chromeBrowser.ViewSource();
            //        });
            //    }
            //}).Wait();

            //var frameIdent = chromeBrowser.GetBrowser().GetFrameIdentifiers();
            //var result = chromeBrowser.GetBrowser().GetFrame(frameIdent.Last()).GetSourceAsync().Result;
            //result.ToString();
        }
        static int SetHeightFromDocument(ChromiumWebBrowser webControl)
        {
            webControl.EvaluateScriptAsync(@"(function() {
            var $window = $(window),
                scrollBottom, elementBottom, parentTopOffset;

            if (this.$scrollTarget.get(0) === $window.get(0)) {
                scrollBottom = this.$parent.height() + this.$scrollTarget.scrollTop() + 1; /* rounding fix */
                elementBottom = this.$el.offset().top + this.$el.height();
            } else {
                scrollBottom = this.$parent.height();
                parentTopOffset = this.$parent.offset() ? this.$parent.offset().top : 0;
                elementBottom = this.$el.offset().top - parentTopOffset + this.$el.height();
            }

            scrollBottom += this.options.heightOffset || 0;

            var willLoadMore = scrollBottom >= elementBottom;

            if (willLoadMore) {
                this._doLoadContent();
            }
        }")
                .ContinueWith(
                    height => {
                        if (height.Result.Result != null)
                        {
                            //webControl.Height = (int)height.Result.Result + 20; // Take possible scrollbar into acct
                            webControl.ExecuteScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");

                        }
                    });

            return (webControl.Height);
        }
        Task writeAsync()
        {
            return Task.Run(() => write());
        }

        void write()
        {
            Thread.Sleep(10000);
        }
        public async Task CopySourceToClipBoardAsync(ChromiumWebBrowser currentBrowser)
        {
            var htmlSource = await currentBrowser.GetSourceAsync();
        }
       
        public Task<HtmlAgilityPack.HtmlDocument> GetPSKDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            #region Nodes Collections

            var sccerEventNodes = "//div[contains(@class, 'psk-sport-group sport-group-type-soccer js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-ice-hockey js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-basketball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-handball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-tennis js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-volleyball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-baseball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-futsal js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-table-tennis js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-cricket js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-rugby js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-motosport js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-darts js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-cycling js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-aussie-rules js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-e-sports js-psk-event-container expanded') ] //div[contains(@class, 'event-col event-col-header')] | " + // HOME/ AWAY AND TIME NODES
                                  "//div[contains(@class, 'psk-sport-group sport-group-type-soccer js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-ice-hockey js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-basketball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-handball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-tennis js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-volleyball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-baseball js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-futsal js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-table-tennis js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-cricket js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-rugby js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-motosport js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-darts js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-cycling js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-aussie-rules js-psk-event-container expanded')" +
                                  "or contains(@class, 'psk-sport-group sport-group-type-e-sports js-psk-event-container expanded') ] //div[contains(@class, 'event-col event-col-bets bet-offer-list')] "; // ODD / OddType NODES
            //var sccerEventNodes = "//div[contains(@class, 'psk-sport-group sport-group-type-tennis js-psk-event-container expanded')] //div[contains(@class, 'event-col event-col-header')] | //div[contains(@class, 'psk-sport-group sport-group-type-tennis js-psk-event-container expanded')] //div[contains(@class, 'event-col event-col-bets bet-offer-list')] "; ONLY ONE SPORT
            HtmlNodeCollection EventAllNodes = htmlDoc.DocumentNode.SelectNodes(sccerEventNodes); // Nogomet Home & Away
            #endregion

            #region Save stufs on new document

            #region Params

            bool hasThirdOdd = false;
            string eventType = "";

            #endregion

            HRKladeEntities db = new HRKladeEntities();
            OddsTable match = new OddsTable();
            int oddscounter = 0;
            foreach (var node in EventAllNodes)
            {
                if (node.Attributes[0].Value == "event-col event-col-header")
                {
                    match = new OddsTable();
                }
                if (node.HasChildNodes)
                {
                    foreach (var childItem in node.ChildNodes)
                    {
                        if (childItem.Name == "div" && childItem.HasAttributes && childItem.Attributes[0].Value == "header-group-title")
                        {
                            eventType = Regex.Replace(childItem.InnerText.Trim(), @"\s+", ""); 
                        }
                            
                        #region Home And Away
                        if (childItem.HasAttributes && childItem.Attributes[0].Value == "event-header-center" && eventType.ToLower().Contains("duel") == false) // Home & Away
                        {
                            if (childItem.HasChildNodes)
                            {
                                foreach (var childChildNode in childItem.ChildNodes)
                                {
                                    if (childChildNode.Name == "a")
                                    {
                                        foreach (var HomeAndAway in childChildNode.ChildNodes)
                                        {
                                            if (HomeAndAway.Name == "div")
                                            {
                                                if (HomeAndAway.HasAttributes && HomeAndAway.Attributes[0].Value == "event-header-team top")
                                                {
                                                    match.Home = Regex.Replace(HomeAndAway.InnerHtml.Trim(), @"[!#$%&/()=?*]", string.Empty);
                                                }
                                                else if (HomeAndAway.HasAttributes && HomeAndAway.Attributes[0].Value == "event-header-team bottom")
                                                {
                                                    match.Away = Regex.Replace(HomeAndAway.InnerHtml.Trim(), @"[!#$%&/()=?*]", string.Empty);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        #endregion

                        #region Time
                        if (childItem.HasAttributes && childItem.Attributes[0].Value == "event-header-right") // TIME
                        {
                            match.EventTime = childItem.InnerText.Trim();
                        }
                        #endregion

                        #region OddTypes & Odds
                        
                        if (node.HasAttributes && node.Attributes[0].Value == "event-col event-col-bets bet-offer-list") // Odds & OddTypes
                        {
                            string oddTypee = null;
                            foreach (var oddNode in childItem.ChildNodes)
                            {
                                if (oddNode.HasAttributes)
                                {
                                    HtmlAttribute oddTypes = oddNode.Attributes["data-pick"]; // Tipovi oklada
                                    if (oddTypes != null)
                                    {
                                        oddTypee = oddTypes.Value;
                                    }
                                }
                                if (oddNode.Name == "div")// ODDS
                                {
                                    #region Reset params
                                    if (oddscounter == 0)
                                    {
                                        match.Odd1 = 0;
                                        match.OddX = 0;
                                        match.Odd2 = 0;
                                        match.Odd1X = 0;
                                        match.OddX2 = 0;
                                        match.Odd12 = 0;
                                        match.OddF2 = 0;
                                    }
                                    #endregion
                                    if (oddscounter == 1 && oddTypee == "2")
                                        oddscounter = oddscounter + 1;

                                    oddscounter++;
                                    if (oddTypee == "1" && oddscounter == 1) // 1
                                    {
                                        if (oddNode.InnerText.Trim().Contains("-") == true)
                                            match.Odd1 = 0;
                                        else
                                        {
                                            match.Odd1 = Convert.ToDecimal(oddNode.InnerText);
                                        }
                                        match.Odd1 = match.Odd1;
                                    }

                                    if (oddTypee == "X" && oddscounter == 2) // X
                                    {
                                        if (oddNode.InnerText.Trim().Contains("-") == true)
                                            match.OddX = 0;
                                        else
                                        {
                                            match.OddX = Convert.ToDecimal(oddNode.InnerText);
                                        }
                                        match.OddX = match.OddX;
                                    }

                                    if ((oddTypee == "2" && oddscounter == 3) || hasThirdOdd)// 2
                                    {
                                        if (oddNode.InnerText.Trim().Contains("-") == true)
                                            match.Odd2 = 0;
                                        else
                                        {
                                            match.Odd2 = Convert.ToDecimal(oddNode.InnerText);
                                        }
                                        match.Odd2 = match.Odd2;
                                    }

                                    if (oddTypee == "1X" && oddscounter == 4) // 1X
                                    {
                                        if (oddNode.InnerText.Trim().Contains("-") == true)
                                            match.Odd1X = 0;
                                        else
                                        {
                                            match.Odd1X = Convert.ToDecimal(oddNode.InnerText);
                                        }
                                        match.Odd1X = match.Odd1X;
                                    }

                                    if (oddTypee == "X2" && oddscounter == 5) // X2
                                    {
                                        if (oddNode.InnerText.Trim().Contains("-") == true)
                                            match.OddX2 = 0;
                                        else
                                        {
                                            match.OddX2 = Convert.ToDecimal(oddNode.InnerText);
                                        }
                                        match.OddX2 = match.OddX2;
                                    }

                                    if (oddTypee == "12" && oddscounter == 6) // 12
                                    {
                                        if (oddNode.InnerText.Trim().Contains("-") == true)
                                            match.Odd12 = 0;
                                        else
                                        {
                                            match.Odd12 = Convert.ToDecimal(oddNode.InnerText);
                                        }
                                        match.Odd12 = match.Odd12;
                                    }

                                    if (oddTypee == "F2" && oddscounter == 7) // F2
                                    {
                                        if (oddNode.InnerText.Trim().Contains("-") == true)
                                            match.OddF2 = 0;
                                        else
                                        {
                                            match.OddF2 = Convert.ToDecimal(oddNode.InnerText);
                                        }
                                        match.OddF2 = match.OddF2;
                                    }
                                    //ADD TO DB
                                }
                            }
                        }
                        if(match.Odd1 > 0 && (match.Odd2 > 0 || match.OddX > 0) && match.Home != null)
                        db.OddsTable.Add(match);
                    }
                        #endregion
                }
                    if (match.KladaName == null && match.Odd1 > 0 && (match.Odd2 > 0 || match.OddX > 0) && match.Home != null) // ako nisu prazni koeficijenti spremi u bazu
                    {
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "PSK";
                        match.InPlay = false;
                        match.SportType = eventType;
                        db.SaveChanges();
                    }

                #region Reset params

                oddscounter = 0; 
                #endregion
            }

            #endregion

            return null;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            this.Close();

            //Cef.Shutdown();
        }
    }
    public static class WebBrowserExtensions
    {
        public static Task LoadPageAsync(this IWebBrowser browser, string address = null)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    //Important that the continuation runs async using TaskCreationOptions.RunContinuationsAsynchronously
                    tcs.TrySetResult(true);
                    Console.WriteLine("if");
                    browser.ExecuteScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");

                }
                else
                {
                    Console.WriteLine("else");
                }
            };

            browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }
            return tcs.Task;
        }
     
    }
}
