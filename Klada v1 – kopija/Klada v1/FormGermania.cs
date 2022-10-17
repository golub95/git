using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Klada_v3.Model;
using Application = System.Windows.Forms.Application;

namespace Klada_v3
{
    public partial class FormGermania : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        static string pageUrl;
        static List<string> LinkList;
        JavascriptResponse clickLoadMore = new JavascriptResponse();
        JavascriptResponse isScrollFinished = new JavascriptResponse();
        #region Odd Parameters

        bool hasOdd1 = false;
        bool hasOddX = false;
        bool hasOdd2 = false;
        bool hasOdd1X = false;
        bool hasOddX2 = false;
        bool hasOdd12 = false;
        string dateTime = string.Empty;
        bool oddTableInitialization = false;
        bool oddTableSetCompleated = false;
        bool nextRecord = false;
        int totalrows = 0;
        string eventInfoNumber = string.Empty;
        string sportType = string.Empty;
        int RowCounter = 0;

        #endregion Odd Parameters

        public FormGermania()
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
            LinkList.Add("https://germaniasport.hr/hr#/date/all"); //
            //LinkList.Add("https://www.germaniasport.hr/hr#/betting/?sid=7");
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
                        
                        //JavascriptResponse afterClick = await chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName(\"buttonLoad\")[3].click()");
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
            //JavascriptResponse afterClick = await chromeBrowser.EvaluateScriptAsync("document.getElementsByClassName('buttonLoad').scrollIntoView()");

            #region variables

            var clickAndScrollScript = @"
                            (function () {
                                document.querySelector('.buttonLoad:last-child').click();
                                scrollTo(0, document.body.scrollHeight)
                                    var result = true;

                                    return result;
                            })();";

            var isScrollFinishedScript = @"
                                (function() {
                                    var result = document.querySelector('div.paginator.dark > div.remaining > span:nth-child(2)').textContent;
                      
                                    return result;
                                })(); ";

            #endregion variables

            await Task.Delay(500);

            Task Scroll = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync(clickAndScrollScript).ContinueWith(waitScroll =>
            {
                int sleepTime = 1500; // in mills 
                Task GetAttribute = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync(isScrollFinishedScript)).ContinueWith(async waitAttribute =>
                {
                    //DOCUMENTATION:
                    // clickAndScrollScript -> function click to button load and scroll down
                    // isScrollFinishedScript -> check if exist remaining odds - if no exist then download document
                    await Task.Delay(1500);
                    isScrollFinished.Result = waitAttribute.Result.Result;
                    while (Convert.ToInt32(isScrollFinished.Result) > 15) // compare position from old var and current scroll position /evaluate script
                    {
                        clickLoadMore.Result = chromeBrowser.EvaluateScriptAsync(clickAndScrollScript).Result.Result; // Execute script Scroll Position and set value to old position var.

                        await Task.Delay(sleepTime);

                        isScrollFinished.Result = chromeBrowser.EvaluateScriptAsync(isScrollFinishedScript).Result.Result;
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

                        GetGermaniaDocument(htmlDoc);

                    });
                    #endregion Get Source
                    chromeBrowser.BrowserCore.CloseBrowser(true);//e.Browser.CloseBrowser(true);
                    this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
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
        public Task<HtmlAgilityPack.HtmlDocument> GetGermaniaDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");
            //IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("td.col-odds > a");
            //IList<HtmlNode> matches = htmlDoc.QuerySelectorAll("div.event-name > span.market-name");
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");

            // Using LINQ to parse HTML table smartly 
            //var HTMLTableTRList = from table in htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'sportsoffer')]").Cast<HtmlNode>()
            //                      from row in table.SelectNodes("//div[contains(@class, 'partvar-min subgameheader')]").Cast<HtmlNode>() 
            //                      from cell in table.SelectNodes("//div[contains(@class, 'match botFlex')]").Cast<HtmlNode>()
            //                      select new { Cell_Text = table.InnerText };

            //var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'sportInfo')] | //div[@class = 'time'] | //div[contains(@class, 'subgameheader')]  | //a[contains(@class, 'pairs')]/div/span | //span[contains(@class, 'betting-regular-match')]").Cast<HtmlNode>();

            //div[contains(normalize-space(@class), 'xyz ng-binding ng-scope') and not(contains(@class, 'ng-hide'))]

            string xpathSportType = "//div[not(contains(@class, 'specComp'))]/div[contains(@class, 'sportInfo')]";
            string xpathTime = "//div[@class = 'time']";
            string xpathOddTypesPart1 = "//div[2]/div[contains(@class, 'partvar-min subgameheader')]";
            string xpathOddTypesPart2 = "//div[3]/div[contains(@class, 'partvar-min subgameheader')]";
            string xpathPairsPart1 = "//div[contains(@class, 'match-id')]//a[contains(@class, 'pairs')]/div/span[1]";
            string xpathPairsPart2 = "//div[contains(@class, 'match-id')]//a[contains(@class, 'pairs')]/div/span[2]";
            //string xpathOddsPart = "//span[contains(@class, 'betting-regular-match')]"; ;//"//div[contains(@class, 'partvar odds')]";
            string xpathOddsPart1 = "//div[1]/div[@class = 'partvar odds']";
            string xpathOddsPart2 = "//div[2]/div[@class = 'partvar odds']";
            var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes(xpathSportType + "|" + xpathTime + "|" + xpathOddTypesPart1 + "|" + xpathOddTypesPart2 + "|" + xpathPairsPart1 + "|" + xpathPairsPart2 + "|" + xpathOddsPart1 + "|" + xpathOddsPart2).Cast<HtmlNode>();

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

                if (node.HasAttributes && node.Attributes.First().Value.Contains("sportInfo")) // if New Sport Type
                {
                    #region Save record from previus sport type to DB
                    // Check if all string property in match class is null
                    bool isNullmatch = match.GetType().GetProperties()
                                    .Where(pi => pi.GetValue(match) is string)
                                    .All(p => p.GetValue(match) == null);

                    if (!isNullmatch) // If object is not null Save previus record to DB
                    {
                        match.EventTime = dateTime;
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "Germania";
                        match.SportType = sportType;
                        match.InPlay = false;
                        match.EventDateTime = Home.GetDatetimeFromString(dateTime);
                        match.SportTypeID = Home.FindAndInsertSportTypeID(match.SportType);

                        db.OddsTable.Add(match);

                        if ((match.Odd1 != null && match.Odd2 != null) && (match.Home != null && match.Away != null))
                        { 
                            Home h = new Home(); //An  object reference is required for the non-static field, method, or property 
                            match.HomeSystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Home, match.SportTypeID.Value, match.KladaName);
                            match.AwaySystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Away, match.SportTypeID.Value, match.KladaName);
                            db.SaveChanges();
                        }
                        match = new OddsTable();
                        RowCounter = 0;
                        dateTime = string.Empty;
                    }
                    #endregion save record from previus sport type to DB

                    #region filter Sport types
                    if (currentItem.ToLower().Contains("golovi") || currentItem.Contains("igrači"))
                    {
                        RowCounter = 0;
                        continue;
                    }

                    #endregion filter Sport types

                    sportType = Regex.Replace(currentItem, @"\s+", ""); ;
                    oddTableInitialization = true;
                    oddTableSetCompleated = false;
                    continue;
                }

                if (node.HasAttributes && node.Attributes.First().Value.Contains("time")) // This is new record save old record to DB
                {
                    // Check if all string property in match class is null
                    bool isNullmatch = match.GetType().GetProperties()
                                    .Where(pi => pi.GetValue(match) is string)
                                    .All(p => p.GetValue(match) == null);

                    if (!isNullmatch) // If object is not null Save previus record to DB
                    {
                        match.EventTime = dateTime;
                        match.Created = DateTime.UtcNow;
                        match.KladaName = "Germania";
                        match.SportType = sportType;
                        match.InPlay = false;
                        match.EventDateTime = Home.GetDatetimeFromString(dateTime);
                        match.SportTypeID = Home.FindAndInsertSportTypeID(match.SportType);

                        db.OddsTable.Add(match);

                        if ((match.Odd1 != null && match.Odd2 != null) && (match.Home != null && match.Away != null))
                        {
                            Home h = new Home(); //An  object reference is required for the non-static field, method, or property 
                            match.HomeSystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Home, match.SportTypeID.Value, match.KladaName);
                            match.AwaySystemID = h.FindOrInsertToMatchSystemIDsTable(match.EventDateTime.Value, match.Away, match.SportTypeID.Value, match.KladaName);
                            db.SaveChanges();
                        }
                        match = new OddsTable();
                        RowCounter = 0;
                        dateTime = string.Empty;
                    }
                    dateTime = currentItem;
                    nextRecord = true;
                    continue;
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
                    oddTableInitialization = false;
                    oddTableSetCompleated = false;
                    totalrows = 0;
                    RowCounter = 0;
                }
                #endregion Reset params

                #region Set Odd Table

                if (!oddTableSetCompleated)
                {
                    switch (currentItem)
                    {
                        case "1":
                            if (hasOdd1)
                                continue;
                            hasOdd1 = true;
                            RowCounter++;
                            continue;
                        case "X":
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
                    }
                   
                    totalrows = RowCounter;
                    if (totalrows > 0)
                        oddTableSetCompleated = true;
                    // In this moment we know that is Odd table completed
                    RowCounter = 0; // Reset row counter - prepare it for Odd values.
                }

                #region RowCounterFIX for sports
                if (sportType == "Baseball")
                    totalrows = 2;
                #endregion RowCounterFIX for sports

                #endregion Set Odd Table

                #region Save to Class
                //if (node.HasAttributes && node.Attributes.Count > 1 && node.Attributes[1].Value.Contains("partvar-min subgameheade"))
                //    continue;
                if (node.Attributes.Any(p => p.Value.Contains("partvar-min subgameheade"))) // If Any of Attributes contain class
                    continue;
                if (oddTableSetCompleated && nextRecord) // Save table values to database
                {
                    if (totalrows == 2) // there are only 1 X 2 Odds
                    {
                        if (RowCounter == 0 && currentItem != string.Empty && match.Home == null)
                        {
                            match.Home = currentItem.Trim();

                            db.OddsTable.Add(match);
                            RowCounter = 1;
                            continue;
                        }
                        if (RowCounter == 1 && currentItem != string.Empty && match.Away == null)
                        {
                            match.Away = currentItem.Trim();

                            db.OddsTable.Add(match);
                            RowCounter = 2;
                            continue;
                        }
                        if (RowCounter == 2 && hasOdd1 && match.Odd1 == null)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 3;
                                continue;
                            }
                            match.Odd1 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 3;
                            continue;
                        }
                        if (RowCounter == 3 && hasOdd2 && match.Odd2 == null)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                parsedValue = 0;
                            }
                            match.Odd2 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 4;
                            continue;
                        }
                    }
                    else if (totalrows == 3) // there are only 1 X 2 Odds
                    {
                        if (RowCounter == 0 && currentItem != string.Empty)
                        {
                            match.Home = currentItem.Trim();

                            db.OddsTable.Add(match);
                            RowCounter = 1;
                            continue;
                        }
                        if (RowCounter == 1 && currentItem != string.Empty)
                        {
                            match.Away = currentItem.Trim();

                            db.OddsTable.Add(match);
                            RowCounter = 2;
                            continue;
                        }
                        if (RowCounter == 2 && hasOdd1)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 3;
                                continue;
                            }
                            match.Odd1 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 3;
                            continue;
                        }
                        if (RowCounter == 3 && hasOddX)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 4;
                                continue;
                            }
                            match.OddX = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 4;
                            continue;
                        }
                        if (RowCounter == 4 && hasOdd2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                parsedValue = 0;
                            }
                            match.Odd2 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 5;
                            continue;
                        }
                    }
                    else if (totalrows >= 5)
                    {
                        if (RowCounter == 0 && currentItem != string.Empty)
                        {
                            match.Home = currentItem.Trim();

                            db.OddsTable.Add(match);
                            RowCounter = 1;
                            continue;
                        }
                        if (RowCounter == 1 && currentItem != string.Empty)
                        {
                            match.Away = currentItem.Trim();

                            db.OddsTable.Add(match);
                            RowCounter = 2;
                            continue;
                        }
                        if (RowCounter == 2 && hasOdd1)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 3;
                                continue;
                            }
                            match.Odd1 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 3;
                            continue;
                        }
                        if (RowCounter == 3 && hasOddX)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 4;
                                continue;
                            }
                            match.OddX = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 4;
                            continue;
                        }
                        if (RowCounter == 4 && hasOdd2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 5;
                                continue;
                            }
                            match.Odd2 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 5;
                            continue;
                        }
                        if (RowCounter == 5 && hasOdd1X)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 6;
                                continue;
                            }
                            match.Odd1X = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 6;
                            continue;
                        }
                        if (RowCounter == 6 && hasOdd12)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 7;
                                continue;
                            }
                            match.Odd12 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 7;
                            continue;
                        }
                        else if (RowCounter == 6 && hasOddX2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                RowCounter = 7;
                                continue;
                            }
                            match.OddX2 = parsedValue;
                            db.OddsTable.Add(match);
                            RowCounter = 7;
                            continue;
                        }
                        if (RowCounter == 7 && hasOddX2)
                        {
                            decimal parsedValue;
                            if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                            {
                                parsedValue = 0;  // even if there is no last off - The record must be saved
                            }
                            match.OddX2 = parsedValue;
                            db.OddsTable.Add(match);
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
                match.KladaName = "Germania";
                match.SportType = sportType;
                match.InPlay = false;
                match.EventDateTime = Home.GetDatetimeFromString(dateTime);
                match.SportTypeID = Home.FindAndInsertSportTypeID(match.SportType);

                db.OddsTable.Add(match);
                if ((match.Odd1 != null && match.Odd2 != null) && (match.Home != null && match.Away != null))
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
