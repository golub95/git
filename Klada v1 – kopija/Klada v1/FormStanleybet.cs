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
    public partial class FormStanleybet : Form
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

        public FormStanleybet()
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
            LinkList.Add("https://www.admiral.hr/app/sport?sport=NOGOMET");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/tennis/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/basketball/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/ice-hockey2/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/volleyball/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/handball/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/water-polo/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/athletics/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/american-football/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/aussie-rules/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/baseball/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/biathlon/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/boxing/");
            LinkList.Add("https://www.favbet.hr/hr/e-sports/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/e-basketball/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/playstation-games/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/floorball/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/formula-1/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/golf/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/cricket/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/mma/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/beach-football/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/darts/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/squash/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/politics/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/rugby-union/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/wrestling/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/rugby-league/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/snooker/");
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/table-tennis/");
            //LinkList.Add("https://www.favbet.hr/hr/sports/sport/specials/");
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

                        chromeBrowser.ViewSource();

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
            //document.querySelector('.page-item:last-child').click() WORKING
            //document.querySelector('li.page-item:first-child').textContent WORKING
            //document.querySelector('li.page-item:nth-child(4)').textContent WORKING 
            //document.querySelector('li.page-item.disabled:nth-child(5)').textContent WORKING 
            //document.getElementsByClassName('.page-item disabled:last-child')
            //document.getElementsByClassName("item-option")[9].click() SVE

            var clickAndScrollScript = @"
                                (function () {
                                    var result = document.body.scrollHeight;
                                    
                                    scrollTo(0, document.body.scrollHeight);
                                    if (document.readyState === 'complete' || document.readyState === 'loaded') 
                                    {
                                        document.getElementsByClassName('item - option')[9].click()
                                        document.querySelector('.page-item:last-child').click();
                                    }
                                    
                                    return true;
                                })();";

            var isScrollFinishedScript = @"
                                (function() {
                                    var nodes = document.querySelectorAll('li.page-item').length.valueOf() - 2;
                                    
                                    var beforelast = document.querySelectorAll('li.page-item')[nodes].textContent;
                                  
                                    return true;
                                })(); ";

            #endregion variables

            await Task.Delay(5000);

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
                    while (isScrollFinished.Success) // compare position from old var and current scroll position /evaluate script
                    {
                        clickLoadMore.Result = chromeBrowser.EvaluateScriptAsync(clickAndScrollScript).Result.Result; // Execute script Scroll Position and set value to old position var.

                        await Task.Delay(sleepTime);

                        isScrollFinished.Result = chromeBrowser.EvaluateScriptAsync(isScrollFinishedScript).Result.Result;
                    }
                    // When scroll was finished get source

                    #region Get Source
                    var html = string.Empty;
                    await chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                    {
                        html = taskHtml.Result;

                        HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                        htmlDoc.LoadHtml(html);
                        string path = "C:\\Temp/download.html";
                        File.WriteAllText(path, html);

                        GetAdmiralDocument(htmlDoc);
                    });

                    #endregion Get Source
                    chromeBrowser.BrowserCore.CloseBrowser(true);//e.Browser.CloseBrowser(true);
                    Cef.ClearSchemeHandlerFactories();
                    this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
                    //Shutdown before your application exists or it will hang.
                    Cef.Shutdown();

                    return (JavascriptResponse)clickLoadMore;
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
        public Task<HtmlAgilityPack.HtmlDocument> GetAdmiralDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            #region Linq Example
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");
            //IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("td.col-odds > a");
            //IList<HtmlNode> matches = htmlDoc.QuerySelectorAll("div.event-name > span.market-name");
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");

            // Using LINQ to parse HTML table smartly 
            //var HTMLTableTRList = from table in htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'sportsoffer')]").Cast<HtmlNode>()
            //                      from row in table.SelectNodes("//div[contains(@class, 'partvar-min subgameheader')]").Cast<HtmlNode>() 
            //                      from cell in table.SelectNodes("//div[contains(@class, 'match botFlex')]").Cast<HtmlNode>()
            //                      select new { Cell_Text = table.InnerText };

            // Using LINQ to parse HTML table smartly 
            //var HTMLTableTRList = from table in htmlDoc.DocumentNode.SelectNodes("//html/body/div").Cast<HtmlNode>()
            //                      from row in table.SelectNodes("//tr").Cast<HtmlNode>()
            //                      from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
            //                      select new { Cell_Text = cell.InnerText };

            //string xpathSportType = "//div[contains(@class, 'sportInfo')]"; // from link
            //string xpathTime = "//span[contains(@class, 'time')]";
            //string xpathOddTypesPart = "//span[contains(@class, 'ng-star-inserted')]";
            //string xpathPairsPart1 = "//span[contains(@class, 'home')]";
            //string xpathPairsPart2 = "//span[contains(@class, 'away')]";
            //string xpathOddsPart = "//span/@data-market";//"//div[contains(@class, 'partvar odds')]";
            //var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes(xpathTime + "|" + xpathOddTypesPart + "|" + xpathPairsPart1 + "|" + xpathPairsPart2 + "|" + xpathOddsPart).Cast<HtmlNode>();
            #endregion linq Example

            //string xpathSportType = "//div[contains(@class, 'sportInfo')]"; // from link
            string xpathTime = "//span[contains(@_ngcontent-dhj-c95 class, 'time')]";
            string xpathOddTypesPart = "//div[contains(@_ngcontent-dhj-c96 class, 'bet-type-col')]/span";
            string xpathPairsPart1 = "//div[contains(@_ngcontent-dhj-c95 class, 'home')]/span";
            string xpathPairsPart2 = "//div[contains(@_ngcontent-dhj-c95 class, 'away')]/span";
            string xpathOddsPart = "//span/@data-market";//"//div[contains(@class, 'partvar odds')]";
            //var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes(xpathTime + "|" + xpathOddTypesPart + "|" + xpathPairsPart1 + "|" + xpathPairsPart2 + "|" + xpathOddsPart).Cast<HtmlNode>();

            var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes(xpathTime).Cast<HtmlNode>();
            #region Save list to txt Test ONLY 
            // For Testing
            var HTMLTableTRListHumanReadable = from table in HTMLTableTRList select new { Cell_Text = table.InnerText };
            string combinedString = string.Join(",", HTMLTableTRListHumanReadable);
            string path = "C:\\Temp/download.txt";
            File.WriteAllText(path, combinedString);

            #endregion Save list to txt Test ONLY

            HRKladeEntities db = new HRKladeEntities();
            OddsTable match = new OddsTable();

            foreach (var node in HTMLTableTRList)
            {
                //clear string
                string currentItem = node.InnerText.Replace("&nbsp;", " ").Replace("\n", "").Replace("\r", "").Trim();

            //    if (node.HasAttributes && node.Attributes.First().Value.Contains("sportInfo")) // if New Sport Type
            //    {
            //        #region filter Sport types
            //        if (currentItem.Contains("fantasy"))
            //        {
            //            RowCounter = 0;
            //            continue;
            //        }

            //        #endregion filter Sport types

            //        sportType = Regex.Replace(currentItem, @"\s+", ""); ;
            //        oddTableInitialization = true;
            //        oddTableSetCompleated = false;
            //        continue;
            //    }

            //    if (node.HasAttributes && node.Attributes.First().Value.Contains("time")) // This is new record save old record to DB
            //    {
            //        // Check if all string property in match class is null
            //        bool isNullmatch = match.GetType().GetProperties()
            //                        .Where(pi => pi.GetValue(match) is string)
            //                        .All(p => p.GetValue(match) == null);

            //        if (!isNullmatch) // If object is not null Save previus record to DB
            //        {
            //            match.EventTime = dateTime;
            //            match.Created = DateTime.UtcNow;
            //            match.KladaName = "Germania";
            //            match.SportType = sportType;
            //            match.InPlay = false;
            //            match.EventDateTime = Home.GetDatetimeFromString(dateTime);

            //            db.OddsTable.Add(match);
            //            db.SaveChanges();
            //            match = new OddsTable();
            //            RowCounter = 0;
            //        }

            //        dateTime = currentItem;
            //        nextRecord = true;
            //        continue;
            //    }

            //    #region Reset params
            //    if (!oddTableSetCompleated & oddTableInitialization) // -> Only first time
            //    {
            //        //This is new table
            //        hasOdd1 = false;
            //        hasOddX = false;
            //        hasOdd2 = false;
            //        hasOdd1X = false;
            //        hasOddX2 = false;
            //        hasOdd12 = false;
            //        dateTime = string.Empty;
            //        oddTableInitialization = false;
            //        oddTableSetCompleated = false;
            //        totalrows = 0;
            //        RowCounter = 0;
            //    }

            //    #endregion Reset params

            //    #region Set Odd Table
            //    if (!oddTableSetCompleated)
            //    {
            //        switch (currentItem)
            //        {
            //            case "1":
            //                if (hasOdd1)
            //                    continue;
            //                hasOdd1 = true;
            //                RowCounter++;
            //                continue;
            //            case "X":
            //                if (hasOddX)
            //                    continue;
            //                hasOddX = true;
            //                RowCounter++;
            //                continue;
            //            case "2":
            //                if (hasOdd2)
            //                    continue;
            //                hasOdd2 = true;
            //                RowCounter++;
            //                continue;
            //            case "1X":
            //                if (hasOdd1X)
            //                    continue;
            //                hasOdd1X = true;
            //                RowCounter++;
            //                continue;
            //            case "X2":
            //                if (hasOddX2)
            //                    continue;
            //                hasOddX2 = true;
            //                RowCounter++;
            //                continue;
            //            case "12":
            //                if (hasOdd12)
            //                    continue;
            //                hasOdd12 = true;
            //                RowCounter++;
            //                continue;
            //        }

            //        totalrows = RowCounter;
            //        if (totalrows > 0)
            //            oddTableSetCompleated = true;
            //        // In this moment we know that is Odd table completed
            //        RowCounter = 0; // Reset row counter - prepare it for Odd values.
            //    }

            //    #region RowCounterFIX for sports
            //    if (sportType == "Baseball")
            //        totalrows = 2;

            //    #endregion RowCounterFIX for sports

            //    #endregion Set Odd Table

            //    #region Save to Class
            //    if (oddTableSetCompleated && nextRecord) // Save table values to database
            //    {
            //        if (totalrows == 2) // there are only 1 X 2 Odds
            //        {
            //            if (RowCounter == 0 && currentItem != string.Empty && match.Home == null)
            //            {
            //                match.Home = currentItem;

            //                db.OddsTable.Add(match);
            //                RowCounter = 1;
            //                continue;
            //            }
            //            if (RowCounter == 1 && currentItem != string.Empty && match.Away == null)
            //            {
            //                match.Away = currentItem;

            //                db.OddsTable.Add(match);
            //                RowCounter = 2;
            //                continue;
            //            }
            //            if (RowCounter == 2 && hasOdd1 && match.Odd1 == null)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 3;
            //                    continue;
            //                }
            //                match.Odd1 = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 3;
            //                continue;
            //            }
            //            if (RowCounter == 3 && hasOdd2 && match.Odd2 == null)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    parsedValue = 0;
            //                }
            //                match.Odd2 = parsedValue;
            //                db.OddsTable.Add(match);

            //                continue;
            //            }
            //        }
            //        else if (totalrows == 3) // there are only 1 X 2 Odds
            //        {
            //            if (RowCounter == 0 && currentItem != string.Empty)
            //            {
            //                match.Home = currentItem;

            //                db.OddsTable.Add(match);
            //                RowCounter = 1;
            //                continue;
            //            }
            //            if (RowCounter == 1 && currentItem != string.Empty)
            //            {
            //                match.Away = currentItem;

            //                db.OddsTable.Add(match);
            //                RowCounter = 2;
            //                continue;
            //            }
            //            if (RowCounter == 2 && hasOdd1)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 3;
            //                    continue;
            //                }
            //                match.Odd1 = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 3;
            //                continue;
            //            }
            //            if (RowCounter == 3 && hasOddX)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 4;
            //                    continue;
            //                }
            //                match.OddX = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 4;
            //                continue;
            //            }
            //            if (RowCounter == 4 && hasOdd2)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    parsedValue = 0;
            //                }
            //                match.Odd2 = parsedValue;
            //                db.OddsTable.Add(match);

            //                continue;
            //            }
            //        }
            //        else if (totalrows >= 5)
            //        {
            //            if (RowCounter == 0 && currentItem != string.Empty)
            //            {
            //                match.Home = currentItem;

            //                db.OddsTable.Add(match);
            //                RowCounter = 1;
            //                continue;
            //            }
            //            if (RowCounter == 1 && currentItem != string.Empty)
            //            {
            //                match.Away = currentItem;

            //                db.OddsTable.Add(match);
            //                RowCounter = 2;
            //                continue;
            //            }
            //            if (RowCounter == 2 && hasOdd1)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 3;
            //                    continue;
            //                }
            //                match.Odd1 = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 3;
            //                continue;
            //            }
            //            if (RowCounter == 3 && hasOddX)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 4;
            //                    continue;
            //                }
            //                match.OddX = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 4;
            //                continue;
            //            }
            //            if (RowCounter == 4 && hasOdd2)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 5;
            //                    continue;
            //                }
            //                match.Odd2 = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 5;
            //                continue;
            //            }
            //            if (RowCounter == 5 && hasOdd1X)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 6;
            //                    continue;
            //                }
            //                match.Odd1X = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 6;
            //                continue;
            //            }
            //            if (RowCounter == 6 && hasOdd12)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 7;
            //                    continue;
            //                }
            //                match.Odd12 = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 7;
            //                continue;
            //            }
            //            else if (RowCounter == 6 && hasOddX2)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    RowCounter = 7;
            //                    continue;
            //                }
            //                match.OddX2 = parsedValue;
            //                db.OddsTable.Add(match);
            //                RowCounter = 7;
            //                continue;
            //            }
            //            if (RowCounter == 7 && hasOddX2)
            //            {
            //                decimal parsedValue;
            //                if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
            //                {
            //                    parsedValue = 0;  // even if there is no last off - The record must be saved
            //                }
            //                match.OddX2 = parsedValue;
            //                db.OddsTable.Add(match);

            //                continue;
            //            }
            //        }
            //    }
            //    #endregion Save to Class

            //}
            ////Save last match

            //// Check if all string property in match class is null
            //if (!match.GetType().GetProperties()
            //                .Where(pi => pi.GetValue(match) is string)
            //                .All(p => p.GetValue(match) == null)) // If object is not null Save previus record
            //{
            //    match.EventTime = dateTime;
            //    match.Created = DateTime.UtcNow;
            //    match.KladaName = "Germania";
            //    match.SportType = sportType;
            //    match.InPlay = false;
            //    match.EventDateTime = Home.GetDatetimeFromString(dateTime);

            //    db.OddsTable.Add(match);
            //    db.SaveChanges();
            //    match = new OddsTable();
            //    RowCounter = 0;
            }
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
