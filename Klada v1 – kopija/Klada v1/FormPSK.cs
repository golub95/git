using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using HtmlAgilityPack.CssSelectors.NetCore;
using Klada_v3.Model;
using Application = System.Windows.Forms.Application;

namespace Klada_v3
{
    public partial class FormPSK : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        static string pageUrl;
        static List<string> LinkList;
        JavascriptResponse oldScrollPosition = new JavascriptResponse();

        #region Odd Parameters

        bool hasOdd1 = false;
        bool hasOddX = false;
        bool hasOdd2 = false;
        bool hasOdd1X = false;
        bool hasOddX2 = false;
        bool hasOdd12 = false;
        bool hasVise = false;
        bool hasVrijeme = false;
        bool oddTableInitialization = false;
        bool oddTableSetCompleated = false;
        bool doNotSaveEvent = false;
        int totalrows = 0;
        string eventInfoNumber = string.Empty;
        string sportType = string.Empty;
        string dateTime = string.Empty;
        int RowCounter = 0;

        #endregion Odd Parameters

        public FormPSK()
        {
            InitializeComponent();
            InsertLinksToList();

            foreach (string link in LinkList)
            {
                pageUrl = link;
                InitializeChromium();
            }
        }

        private void InsertLinksToList()
        {
            LinkList = new List<string>();
            LinkList.Add("https://www.psk.hr/?rateFrom=1.1&rateTo=1000#");
            //LinkList.Add("https://www.psk.hr/oklade/nogomet");
            //LinkList.Add("https://www.psk.hr/oklade/kosarka");
            //LinkList.Add("https://www.psk.hr/oklade/tenis");
            //LinkList.Add("https://www.psk.hr/oklade/hokej");
            //LinkList.Add("https://www.psk.hr/oklade/rukomet");
            //LinkList.Add("https://www.psk.hr/oklade/baseball");
            //LinkList.Add("https://www.psk.hr/oklade/odbojka");
            //LinkList.Add("https://www.psk.hr/oklade/americki-nogomet");
            //LinkList.Add("https://www.psk.hr/oklade/mma");
            //LinkList.Add("https://www.psk.hr/oklade/boks");
            //LinkList.Add("https://www.psk.hr/oklade/futsal");
            //LinkList.Add("https://www.psk.hr/oklade/curling");
            //LinkList.Add("https://www.psk.hr/oklade/australski-nogomet");
            //LinkList.Add("https://www.psk.hr/oklade/e-sport-cs-go");
            //LinkList.Add("https://www.psk.hr/oklade/e-sport-lol");
            //LinkList.Add("https://www.psk.hr/oklade/e-sport-dota2");
            //LinkList.Add("https://www.psk.hr/oklade/e-sport-rainbow-six");
            //LinkList.Add("https://www.psk.hr/oklade/e-sport-ostali"); 
            //LinkList.Add("https://www.psk.hr/oklade/formula-1");
            //LinkList.Add("https://www.psk.hr/oklade/floorball");
            //LinkList.Add("https://www.psk.hr/oklade/k-1");
            //LinkList.Add("https://www.psk.hr/oklade/kriket");
            //LinkList.Add("https://www.psk.hr/oklade/lacrosse");
            //LinkList.Add("https://www.psk.hr/oklade/hokej-na-travi");
            //LinkList.Add("https://www.psk.hr/oklade/rugby");
            //LinkList.Add("https://www.psk.hr/oklade/snooker");
            //LinkList.Add("https://www.psk.hr/oklade/stolni-tenis");
            //LinkList.Add("https://www.psk.hr/oklade/pikado");
            //LinkList.Add("https://www.psk.hr/oklade/duel");
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
        public async Task<string> EvaluateJavaScriptSync(string jsScript)
        {
            string jsonFromJS = null;
            if (chromeBrowser.CanExecuteJavascriptInMainFrame && jsScript != null)
            {
                var response = await chromeBrowser.EvaluateScriptAsync(jsScript);
                if (response.Success && response.Result != null)
                {
                    jsonFromJS = response.Result.ToString();
                }
            }
            return jsonFromJS;
        }
        private void OnBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs)
        {
            if (frameLoadEndEventArgs.Frame.IsMain && frameLoadEndEventArgs.Url == pageUrl)
            {
                Console.WriteLine("OnBrowserFrameLoadEnd");
            }
            Console.WriteLine("Not OnBrowserFrameLoadEnd");
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
                int sleepTime = 7000; // in mills 
                Task GetAttribute = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync(currentScrollPositionScript)).ContinueWith(async waitAttribute =>
                {
                    await Task.Delay(500);
                    // DOCUMENTATION:
                    // oldScrollPosition = scrollpositionbefore scroll and new position after scroll
                    // EvaluateScriptAsync(currentScrollPositionScript) is current scroll position.
                    // When oldScrollPosition result = EvaluateScriptAsync(currentScrollPositionScript) scrol position Result - this is last scroll
                    while (Convert.ToInt32(oldScrollPosition.Result) != Convert.ToInt32(chromeBrowser.EvaluateScriptAsync(currentScrollPositionScript).Result.Result)) // compare position from old var and current scroll position /evaluate script
                    {
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

                        GetPSKDocument(htmlDoc);
                    });

                    #endregion Get Source
                    chromeBrowser.BrowserCore.CloseBrowser(true);//e.Browser.CloseBrowser(true);
                    this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
                    //Shutdown before your application exists or it will hang.
                });

            }));
        }

        private void ChromeBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Console.WriteLine("end");
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Console.WriteLine("OnLoadingStateChanged");
        }

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var b = ((ChromiumWebBrowser)sender);

            this.InvokeOnUiThreadIfRequired(() => b.Focus());
        }

        public Task<HtmlAgilityPack.HtmlDocument> GetPSKDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");
            //IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("td.col-odds > a");
            //IList<HtmlNode> matches = htmlDoc.QuerySelectorAll("div.event-name > span.market-name");
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");

            // Using LINQ to parse HTML table smartly 
            string xpathSportType = "//h2[@class = 'breadcrumbed-title']/span[@class = 'title-part']/a/span[@class = 'sport-name ']";//"//h2[@class = 'breadcrumbed-title']/span/a/span[@class = 'sport-name']";//"//span[@class = 'competition-name']"
            string xpathOddTypesPart = "//span[@class =  'odds-name']";
            string xpathPairsPart = "//div[@class =  'event-name']"; // col-title
            string xpathOddsPart = "//span[@class = 'odds-value']"; ;//"//div[contains(@class, 'partvar odds')]";
            string xpathTime = "//span[@class = 'event-datetime']";

            var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes(xpathSportType + "|" + xpathOddTypesPart  + "|" + xpathPairsPart + "|" + xpathOddsPart + "|" + xpathTime).Cast<HtmlNode>();


            #region Save list to txt Test ONLY 
            // For Testing
            //var HTMLTableTRListHumanReadable = from table in HTMLTableTRList select new { Cell_Text = table.InnerText };
            //string combinedString = string.Join(",", HTMLTableTRListHumanReadable);
            //string path = "C:\\Temp/download.txt";
            //File.WriteAllText(path, combinedString);

            #endregion Save list to txt Test ONLY

            HRKladeEntities db = new HRKladeEntities();
            OddsTable match = new OddsTable();

            foreach (var node in HTMLTableTRList)
            {
                //clear string
                string currentItem = node.InnerText.Replace("&nbsp;", " ").Replace("\n", "").Replace("\r", "").Trim();

                if (currentItem == string.Empty) // SpeedUP Algoritam
                {
                    RowCounter = 0;
                    continue;
                }
                if (node.HasAttributes && node.Attributes.First().Value.Contains("sport-name")) // if New Sport Type
                {
                    sportType = Regex.Replace(currentItem, @"\s+", "");
                    oddTableInitialization = true;
                    oddTableSetCompleated = false;
                    doNotSaveEvent = false;
                    if (currentItem == "Duel")
                        doNotSaveEvent = true;
                    continue;
                }

                if (node.HasAttributes && node.Attributes.First().Value.Contains("event-datetime")) // This is new record save old record to DB
                {
                    dateTime = currentItem;
                    // Check if all string property in match class is null
                    bool isNullmatch = match.GetType().GetProperties()
                                    .Where(pi => pi.GetValue(match) is string)
                                    .All(p => p.GetValue(match) == null);

                    if (!isNullmatch) // If object is not null Save previus record to DB
                    {
                        match.EventTime = dateTime;
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "PSK";
                        match.SportType = sportType;
                        match.InPlay = false;
                        match.EventDateTime = Home.GetDatetimeFromString(dateTime);
                        match.SportTypeID = Home.FindAndInsertSportTypeID(match.SportType);

                        db.OddsTable.Add(match);
                        if (!doNotSaveEvent || (match.Odd1 != null && match.Odd2 != null) && (match.Home != null && match.Away != null))
                        { 
                            Home h = new Home(); //An  object reference is required for the non-static field, method, or property 
                            match.HomeSystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Home, match.SportTypeID.Value, match.KladaName);
                            match.AwaySystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Away, match.SportTypeID.Value, match.KladaName);
                            db.SaveChanges();
                        }
                        match = new OddsTable();
                        dateTime = string.Empty;
                        RowCounter = 0;
                    }

                    continue;
                }

                if (currentItem == "1") // This is Odds table first Initialization
                {
                    oddTableInitialization = true;
                    oddTableSetCompleated = false;
                }

                #region Reset params
                if (!oddTableSetCompleated & oddTableInitialization) // -> Only first time
                {
                    //This is new table
                    hasOdd1 = false;
                    hasOddX = false;
                    hasOdd2 = false;
                    hasOdd1X = false;
                    hasOddX2 = false;
                    hasOdd12 = false;
                    hasVise = false;
                    hasVrijeme = false;
                    oddTableInitialization = false;
                    oddTableSetCompleated = false;
                    totalrows = 0;
                }

                #endregion Reset params

                #region Set Odd Table
                if (!oddTableSetCompleated) // oddTableInitialization = TRUE in reset parameters!
                {
                    switch (currentItem)
                    {
                        case "1":
                            if (hasOdd1)
                                continue;
                            hasOdd1 = true;
                            RowCounter++;
                            continue;
                        case "X"://Neriješeno
                            if (hasOddX)
                                continue;
                            hasOddX = true;
                            RowCounter++;
                            continue;
                        case "Neriješeno":// BOKS
                            if (hasOddX)
                                continue;
                            hasOddX = true;
                            RowCounter++;
                            continue;
                        case "2":
                            if (hasOdd2)
                                continue;
                            hasOdd2 = true;
                            RowCounter++;
                            continue;
                        case "1X":
                            if (hasOdd1X)
                                continue;
                            hasOdd1X = true;
                            RowCounter++;
                            continue;
                        case "X2":
                            if (hasOddX2)
                                continue;
                            hasOddX2 = true;
                            RowCounter++;
                            continue;
                        case "12":
                            if (hasOdd12)
                                continue;
                            hasOdd12 = true;
                            RowCounter++;
                            continue;
                        case "više":
                            if (hasVise)
                                continue;
                            hasVise = true;
                            continue;
                        case "datum":
                            if (hasVrijeme)
                                continue;
                            hasVrijeme = true;
                            continue;
                    }
                    totalrows = RowCounter;
                    if (totalrows > 0)
                        oddTableSetCompleated = true;
                    // In this moment we know that is Odd table completed
                    RowCounter = 0; // Reset row counter - prepare it for Odd values.
                }

                #endregion Set Odd Table

                #region Save to Class
                if (totalrows == 1)
                    continue;

                if (oddTableSetCompleated) // Save table values to database
                {
                    if (totalrows == 2) // there are only two odds
                    {
                        if (RowCounter == 0 && currentItem != string.Empty)
                        {
                            var separator = currentItem.IndexOf(" - ", 2); // index of separator home and away event
                            var home = string.Empty;
                            var away = string.Empty;

                            if (separator > 0)
                            {
                                home = currentItem.Substring(0, separator);
                                away = currentItem.Substring(separator + 2);
                            }
                            var doubleEmptySpaces = away.IndexOf("   "); // index of two spaces in the string
                            if (away != string.Empty && doubleEmptySpaces > 0)
                            {
                                away = away.Substring(0, doubleEmptySpaces); // This will remove all text after two whitespace
                            }
                            match.Home = home.Trim();
                            match.Away = away.Trim();
                            db.OddsTable.Add(match);
                            RowCounter = 1;
                            continue;
                        }
                        if (RowCounter == 1 && hasOdd1)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd1 = Math.Abs(parsedValue); 
                            db.OddsTable.Add(match);
                            RowCounter = 2;
                            continue;
                        }
                        if (RowCounter == 2 && hasOdd2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd2 = Math.Abs(parsedValue); 
                            db.OddsTable.Add(match);
                            RowCounter = 3;
                            continue;
                        }
                        if (RowCounter == 3 && hasVise)
                        {
                            RowCounter = 4;
                            continue;
                        }
                       
                    }
                    else if (totalrows == 3) // there are only three odds
                    {
                        if (RowCounter == 0 && currentItem != string.Empty)
                        {
                            var separator = currentItem.IndexOf(" - ", 2); // index of separator home and away event
                            var home = string.Empty;
                            var away = string.Empty;

                            if (separator > 0)
                            {
                                home = currentItem.Substring(0, separator);
                                away = currentItem.Substring(separator + 2);
                            }
                            var doubleEmptySpaces = away.IndexOf("   "); // index of two spaces in the string
                            if (away != string.Empty && doubleEmptySpaces > 0)
                            {
                                away = away.Substring(0, doubleEmptySpaces); // This will remove all text after two whitespace
                            }
                            match.Home = home.Trim();
                            match.Away = away.Trim();
                            db.OddsTable.Add(match);
                            RowCounter = 1;
                            continue;
                        }
                        if (RowCounter == 1 && hasOdd1)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd1 = Math.Abs(parsedValue);
                            db.OddsTable.Add(match);
                            RowCounter = 2;
                            continue;
                        }
                        if (RowCounter == 2 && hasOddX)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.OddX = Math.Abs(parsedValue);
                            db.OddsTable.Add(match);
                            RowCounter = 3;
                            continue;
                        }
                        if (RowCounter == 3 && hasOdd2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd2 = Math.Abs(parsedValue); 
                            db.OddsTable.Add(match);
                            RowCounter = 4;
                            continue;
                        }

                    }
                    else if (totalrows == 6)
                    {
                        if (RowCounter == 0 && currentItem != string.Empty)
                        {
                            eventInfoNumber = new string(currentItem.Where(Char.IsNumber).ToArray());
                            var separator = currentItem.IndexOf(" - "); // index of separator home and away event
                            var home = string.Empty;
                            var away = string.Empty;

                            if (separator > 0)
                            {
                                home = currentItem.Substring(0, separator);
                                away = currentItem.Substring(separator + 2);
                            }
                            var doubleEmptySpaces = away.IndexOf("   "); // index of two spaces in the string
                            if (away != string.Empty && doubleEmptySpaces > 0)
                            {
                                away = away.Substring(0, doubleEmptySpaces); // This will remove all text after two whitespace
                            }
                            match.Home = home.Trim();
                            match.Away = away.Trim();
                            db.OddsTable.Add(match);
                            RowCounter = 1;
                            continue;
                        }
                        if (RowCounter == 1 && hasOdd1)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd1 = Math.Abs(parsedValue); ;
                            db.OddsTable.Add(match);
                            RowCounter = 2;
                            continue;
                        }
                        if (RowCounter == 2 && hasOddX)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.OddX = Math.Abs(parsedValue); ;
                            db.OddsTable.Add(match);
                            RowCounter = 3;
                            continue;
                        }
                        if (RowCounter == 3 && hasOdd2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd2 = Math.Abs(parsedValue); ;
                            db.OddsTable.Add(match);
                            RowCounter = 4;
                            continue;
                        }
                        if (RowCounter == 4 && hasOdd1X)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd1X = Math.Abs(parsedValue); ;
                            db.OddsTable.Add(match);
                            RowCounter = 5;
                            continue;
                        }
                        if (RowCounter == 5 && hasOddX2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.OddX2 = Math.Abs(parsedValue); ;
                            db.OddsTable.Add(match);
                            RowCounter = 6;
                            continue;
                        }
                        if (RowCounter == 6 && hasOdd12)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 0;
                                continue;
                            }
                            match.Odd12 = Math.Abs(parsedValue); ;
                            db.OddsTable.Add(match);
                            RowCounter = 7;
                            continue;
                        }
                        if (RowCounter == 7 && hasVise)
                        {
                            RowCounter = 8;
                            continue;
                        }
                    }
                }
                #endregion Save to Class

            }
            //Save last match

            // Check if all string property in match class is null
            if (!match.GetType().GetProperties()
                            .Where(pi => pi.GetValue(match) is string)
                            .All(p => p.GetValue(match) == null)) // If object is not null Save previus record
            {
                match.EventTime = dateTime;
                match.Created = DateTime.UtcNow;
                match.KladaName = "PSK";
                match.SportType = sportType;
                match.InPlay = false;
                match.EventDateTime = Home.GetDatetimeFromString(dateTime);
                match.SportTypeID = Home.FindAndInsertSportTypeID(match.SportType);

                db.OddsTable.Add(match);
                if (!doNotSaveEvent || (match.Odd1 != null && match.Odd2 != null) && (match.Home != null && match.Away != null))
                { 
                    Home h = new Home(); //An  object reference is required for the non-static field, method, or property 
                    match.HomeSystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Home, match.SportTypeID.Value, match.KladaName);
                    match.AwaySystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Away, match.SportTypeID.Value, match.KladaName);
                    db.SaveChanges();
                }
                match = new OddsTable();
                RowCounter = 0;
            }
            return null;
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region This is closing Forms and Browser
            Cef.ClearSchemeHandlerFactories();
            Dispose();
            this.Close();
            //Cef.Shutdown();
            #endregion
        }
    }
}
