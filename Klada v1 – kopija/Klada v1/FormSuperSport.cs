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
using Klada_v3.Model;
using Application = System.Windows.Forms.Application;

namespace Klada_v3
{
    public partial class FormSuperSport : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        JavascriptResponse oldScrollPosition = new JavascriptResponse();
        string pageUrl = "https://www.supersport.hr/sport/dan/sve";

        public FormSuperSport()
        {
            InitializeComponent();
            InitializeChromium();
            chromeBrowser.FrameLoadEnd += WebBrowserFrameLoadEnded;
        }

        public void InitializeChromium()
        {
            Console.WriteLine("Create a browser component");
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(pageUrl);
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
            chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;

            chromeBrowser.LoadingStateChanged += (sender, args) =>
            {
                IFrame frame = null;
                frame = chromeBrowser.GetFocusedFrame();

                //var scroll = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync("scrollTo(0, document.body.scrollHeight)"));
                //Wait for the Page to finish loading
                Console.WriteLine("Loading State Changed GoBack {0} GoForward {1} CanReload {2} IsLoading {3}", args.CanGoBack, args.CanGoForward, args.CanReload, args.IsLoading);
                chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;

                if (args.CanReload && !args.IsLoading)
                {
                    //chromeBrowser.ExecuteScriptAsyncWhenPageLoaded("alert('All Resources Have Loaded');");

                    //EvaluateScriptAsPromiseAsync calls Promise.resolve internally so even if your code doesn't
                    //return a Promise it will still execute successfully.
                    //var script = @"return (function() { return 1 + 1; })();";
                    //JavascriptResponse response = await frame.EvaluateScriptAsPromiseAsync(script);
                    //JavascriptResponse response = await frame.EvaluateScriptAsync(script);

                    //var task = Task.Run(async () => { await Scroll(); });
                    //task.Wait();
                    //TestCorrect();
                    //await Task.WhenAll(Scroll(), TestCorrect());

                    //Task<JavascriptResponse> t = Task.Run(() => chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()"));
                    //await t.ContinueWith(async (t1) =>
                    // {
                    //     Console.WriteLine(t1.Result);
                    //     await GetDocSource();
                    // });
                    //if (!t.Status.Equals(TaskStatus.Running))
                    //{
                    //    Console.WriteLine("task t je zavrašen");
                    //}
                    //chromeBrowser.Reload();
                    //SetHeightFromDocument(chromeBrowser);
                    chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;
                    //chromeBrowser.ViewSource();
                }
            };

            chromeBrowser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;


            //Wait for the MainFrame to finish loading
            chromeBrowser.FrameLoadEnd += async (sender, args) =>
            {
                chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
                    //args.Frame.ExecuteJavaScriptAsync("alert('MainFrame finished loading');");
                    //chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");
                    chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;
                    Console.WriteLine("MainFrame finished loading Status code {0}", args.HttpStatusCode);
                    if (args.HttpStatusCode == 200)
                    {
                        //finished, OK, streaming end
                        //string finished = await Scroll();
                        //finished += finished;
                        //var task = Task.Run(async () => await Scroll());
                        //var result = task.Result;
                        //task.Start();
                        //AsyncCallerAsync();
                        //chromeBrowser.Reload();
                        //var scrollCompleated = chromeBrowser.EvaluateScriptAsync("scrollTo(0, document.body.scrollHeight)").ContinueWith(task=>

                        await ScrollAsync();


                        //await Task.Factory.StartNew(async () =>
                        // {
                        //     //chromeBrowser.EvaluateScriptAsync(scrollAndGetAttributeScript).ConfigureAwait(true);
                        //     await EvaluateJavaScriptSync(scrollAndGetAttributeScript);
                        // },
                        //TaskCreationOptions.LongRunning);
                        //await Task.Delay(60000);

                        //if (Scroll.IsCompleted)
                        //{
                        //    MessageBox.Show("Error");
                        //    MessageBox.Show($"{((dynamic)Scroll.IsCompleted)}");
                        //}
                        //MessageBox.Show($"{((dynamic)result.Result).Count}");

                        //Console.WriteLine("SCROLLLLINNNNGG STATUS" + " " + Scroll.Status);

                        chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;

                    }
                    if (args.HttpStatusCode == -101)
                    {
                        Console.WriteLine("finished, OK, streaming shut down");
                        //finished, OK, streaming shut down
                        chromeBrowser.Reload();
                    }
                    if (args.HttpStatusCode == 0)
                    {
                        Console.WriteLine("The client request wasn't successful");
                        //The client request wasn't successful.
                        chromeBrowser.Reload();
                    }
                    Console.WriteLine("ALL FINISHED");
                }

                Console.WriteLine("IsMain");
            };
            Console.WriteLine("Not InitializeChromium");
        }

        private void OnBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs)
        {
            if (frameLoadEndEventArgs.Frame.IsMain && frameLoadEndEventArgs.Url == pageUrl)
            {
                Console.WriteLine("OnBrowserFrameLoadEnd");
            }
            Console.WriteLine("Not OnBrowserFrameLoadEnd");
        }
        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var b = ((ChromiumWebBrowser)sender);

            this.InvokeOnUiThreadIfRequired(() => b.Focus());
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
                        //Klikni "Učitaj narednih" događaja [3]
                        JavascriptResponse afterClick = await chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName(\"buttonLoad\")[3].click()");

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

                                GetSSDocument(htmlDoc);
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

        private async Task ScrollAsync()
        {
            #region variables
            var scroll = @"
                            (function () {
                                scrollTo(0, document.body.scrollHeight)
                            })();";

            var currentScrollPositionScript = @"
                                (function() {
                                    var result = document.body.scrollHeight;
                                    return result;
                                })(); ";

            #endregion variables

            await Task.Delay(500);
            oldScrollPosition.Result = chromeBrowser.EvaluateScriptAsync(currentScrollPositionScript).Result.Result; // Scroll Position Before Scroll

            Task Scroll = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync(scroll).ContinueWith(waitScroll =>
            {
                int sleepTime = 10000; // in mills 
                Task GetAttribute = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync(currentScrollPositionScript)).ContinueWith(async waitAttribute =>
                {
                    // DOCUMENTATION:
                    // oldScrollPosition = scrollpositionbefore scroll and new position after scroll
                    // EvaluateScriptAsync(currentScrollPositionScript) is current scroll position.
                    // When oldScrollPosition result = EvaluateScriptAsync(currentScrollPositionScript) scrol position Result - this is last scroll
                    //while (Convert.ToInt32(oldScrollPosition.Result) != Convert.ToInt32(chromeBrowser.EvaluateScriptAsync(currentScrollPositionScript).Result.Result)) // compare position from old var and current scroll position /evaluate script
                    while (oldScrollPosition.Result != chromeBrowser.EvaluateScriptAsync(currentScrollPositionScript).Result.Result)
                    {
                        await Task.Delay(800);
                        oldScrollPosition.Result = chromeBrowser.EvaluateScriptAsync(currentScrollPositionScript).Result.Result; // Execute script Scroll Position and set value to old position var.
                        await Task.Delay(sleepTime);
                    }
                    // When scroll was finished get source

                    #region Get Source
                    //chromeBrowser.ViewSource();
                    var html = string.Empty;
                    await chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                    {
                        html = taskHtml.Result;

                        HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                        htmlDoc.LoadHtml(html);
                        //string path = "C:\\Temp/download.html";
                        //File.WriteAllText(path, html);

                        GetSSDocument(htmlDoc);
                    });

                    #endregion Get Source
                    chromeBrowser.BrowserCore.CloseBrowser(true);//e.Browser.CloseBrowser(true);
                    Cef.ClearSchemeHandlerFactories();
                    this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
                    //Shutdown before your application exists or it will hang.
                    Cef.Shutdown();

                    return (JavascriptResponse)this.oldScrollPosition;
                });

            }));
        }

        public Task<HtmlAgilityPack.HtmlDocument> GetSSDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            #region Nodes Collections

            var eventAllNodes = 
                "//div[contains(@class, 'sportska-liga nogomet')" +
                "or contains(@class,'sportska-liga kosarka') " +
                "or contains(@class, 'sportska-liga tenis') " +
                "or contains(@class, 'sportska-liga hokej')" +
                "or contains(@class, 'sportska-liga rukomet') " +
                "or contains(@class, 'sportska-liga baseball') " +
                "or contains(@class, 'sportska-liga boks') " +
                "or contains(@class, 'sportska-liga esports') " +
                "or contains(@class, 'sportska-liga odbojka') " +
                "or contains(@class, 'sportska-liga pikado') " +
                "or contains(@class, 'sportska-liga rugby') " +
                "or contains(@class, 'sportska-liga snooker') " +
                "or contains(@class, 'sportska-liga stolni-tenis') " +
                "or contains(@class, 'sportska-liga ultimate-fight') " +
                "or contains(@class, 'sportska-liga vaterpolo') " +
                "or contains(@class, 'sportska-liga hokej-na-travi') " +
                "or contains(@class, 'sportska-liga mali-nogomet') ] //div[contains(@class, 'panel-content')] | " +
                "//div[contains(@class, 'sportska-liga nogomet')" +
                "or contains(@class,'sportska-liga kosarka') " +
                "or contains(@class, 'sportska-liga tenis') " +
                "or contains(@class, 'sportska-liga hokej')" +
                "or contains(@class, 'sportska-liga rukomet') " +
                "or contains(@class, 'sportska-liga baseball') " +
                "or contains(@class, 'sportska-liga boks') " +
                "or contains(@class, 'sportska-liga esports') " +
                "or contains(@class, 'sportska-liga odbojka') " +
                "or contains(@class, 'sportska-liga pikado') " +
                "or contains(@class, 'sportska-liga rugby') " +
                "or contains(@class, 'sportska-liga snooker') " +
                "or contains(@class, 'sportska-liga stolni-tenis') " +
                "or contains(@class, 'sportska-liga ultimate-fight') " +
                "or contains(@class, 'sportska-liga vaterpolo') " +
                "or contains(@class, 'sportska-liga hokej-na-travi') " +
                "or contains(@class, 'sportska-liga mali-nogomet') ] //tr[contains(@class, 'ponuda')] | " +
                "//div[contains(@class, 'sportska-liga nogomet')" +
                "or contains(@class,'sportska-liga kosarka') " +
                "or contains(@class, 'sportska-liga tenis') " +
                "or contains(@class, 'sportska-liga hokej') " +
                "or contains(@class, 'sportska-liga rukomet') " +
                "or contains(@class, 'sportska-liga baseball') " +
                "or contains(@class, 'sportska-liga boks') " +
                "or contains(@class, 'sportska-liga esports') " +
                "or contains(@class, 'sportska-liga odbojka') " +
                "or contains(@class, 'sportska-liga pikado') " +
                "or contains(@class, 'sportska-liga rugby') " +
                "or contains(@class, 'sportska-liga snooker') " +
                "or contains(@class, 'sportska-liga stolni-tenis') " +
                "or contains(@class, 'sportska-liga ultimate-fight') " +
                "or contains(@class, 'sportska-liga vaterpolo') " +
                "or contains(@class, 'sportska-liga hokej-na-travi') " +
                "or contains(@class, 'sportska-liga mali-nogomet') ] //tr[contains(@class, 'tipovi')]";

            //var eventAllNodes = "//div[contains(@class, 'sportska-liga boks')]//tr[contains(@class, 'ponuda')] | //div[contains(@class, 'sportska-liga boks')] //tr[contains(@class, 'tipovi')]";

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
                if (node.Name == "div" && node.HasAttributes && node.Attributes[0].Value == "panel-content")
                    eventType = node.InnerText.Trim().Substring(0, node.InnerText.Trim().IndexOf("-"));
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

                            if (childItem.HasAttributes && childItem.InnerText.Length > 0 && (childItem.Attributes[0].Value == "sportska-tecaj td-shrink simple tecaj clickable" || childItem.Attributes[0].Value == "sportska-tecaj td-shrink simple tecaj"))
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
                                        match.Odd1 = decimal.Parse(childItem.InnerText, new NumberFormatInfo() { NumberDecimalSeparator = "," });

                                    }
                                    match.Odd1 = match.Odd1;
                                }

                                if (hasSecOdd && oddsCounter == 2 && !onlyTwoOddTypes) // X
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.OddX = 0;
                                    else
                                    {
                                        match.OddX = decimal.Parse(childItem.InnerText, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                                    }
                                    match.OddX = match.OddX;
                                }

                                if ((hasThirdOdd && oddsCounter == 3) || hasThirdOdd && onlyTwoOddTypes)// 2
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.Odd2 = 0;
                                    else
                                    {
                                        match.Odd2 = decimal.Parse(childItem.InnerText, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                                    }
                                    match.Odd2 = match.Odd2;
                                }

                                if (hasFourthOdd && oddsCounter == 4) // 1X
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.Odd1X = 0;
                                    else
                                    {
                                        match.Odd1X = decimal.Parse(childItem.InnerText, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                                    }
                                    match.Odd1X = match.Odd1X;
                                }

                                if (hasFifthOdd && oddsCounter == 5) // X2
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.OddX2 = 0;
                                    else
                                    {
                                        match.OddX2 = decimal.Parse(childItem.InnerText, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                                    }
                                    match.OddX2 = match.OddX2;
                                }

                                if (hasSixthOdd && oddsCounter == 6) // 12
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.Odd12 = 0;
                                    else
                                    {
                                        match.Odd12 = decimal.Parse(childItem.InnerText, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                                    }
                                    match.Odd12 = match.Odd12;
                                }

                                if (hasSeventhOdd && oddsCounter == 7) // F2
                                {
                                    if (childItem.InnerText.Contains("-") == true)
                                        match.OddF2 = 0;
                                    else
                                    {
                                        match.OddF2 = decimal.Parse(childItem.InnerText, new NumberFormatInfo() { NumberDecimalSeparator = "," });
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

                                match.Home = Regex.Replace (home, @"[!#$%&/()=?*]", string.Empty);
                                match.Away = Regex.Replace(away, @"[!#$%&/()=?*]", string.Empty);
                                if(match.KladaName == null)
                                    db.OddsTable.Add(match);
                                #endregion

                                #region Time
                                var time = childItem.LastChild.LastChild.InnerText;
                                if (time.Contains("&") == true)
                                    time = time.Remove(time.LastIndexOf('&'));

                                match.EventTime = time;
                                match.EventDateTime = Home.GetDatetimeFromString(time);

                                #endregion
                            }
                        }
                    }
                    if (match.Odd1 > 0 && (match.Odd2 > 0 || match.OddX > 0)) // ako nisu prazni koeficijenti spremi u bazu
                    {
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "Supersport";
                        match.InPlay = false;
                        match.SportType = eventType;
                        match.SportTypeID = Home.FindAndInsertSportTypeID(match.SportType);

                        if ((match.Odd1 != null && match.Odd2 != null) && (match.Home != null && match.Away != null))
                        { 
                            db.SaveChanges();
                            Home h = new Home(); //An  object reference is required for the non-static field, method, or property 
                            match.MatchSystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Home, match.Away, match.SportTypeID.Value, match.KladaName);
                        }
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
}

