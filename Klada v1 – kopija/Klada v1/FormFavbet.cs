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
    public partial class FormFavbet : Form
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
        string previusSportType = string.Empty;
        int RowCounter = 0;
        string lastOddType = string.Empty;

        #endregion Odd Parameters

        public FormFavbet()
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
            //LINK: https://www.favbet.hr/hr/sports/sport/soccer/?timeFilter=%7B"all"%3A"all"%7D
            LinkList.Add("https://www.favbet.hr/hr/sports/sport/soccer/?timeFilter=%7B\"all\"%3A\"all\"%7D");
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
                                    scrollTo(0, document.body.scrollHeight);
                                    document.querySelector('#root > div > div > div > div.Box_box__2h4Jp.Page_pageWrapper__2WiEK.Box_justify_center__1-I-N > div > div:nth-child(2) > div:nth-child(5) > div:nth-child(3) > div > div.Box_box__2h4Jp.ButtonLoader_showMoreWrapper__34SHp.Box_justify_center__1-I-N > button').click();
                                    var result = true;

                                    return result;
                            })();";

            var isScrollFinishedScript = @"
                                (function() {
                                    var result = document.querySelector('#root > div > div > div > div.Box_box__2h4Jp.Page_pageWrapper__2WiEK.Box_justify_center__1-I-N > div > div:nth-child(2) > div:nth-child(5) > div:nth-child(3) > div > div.Box_box__2h4Jp.ButtonLoader_showMoreWrapper__34SHp.Box_justify_center__1-I-N > button').textContent == 'Prikaži više';
                      
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
                    //await Task.Delay(1500);
                    isScrollFinished.Result = waitAttribute.Result.Result;
                    while (Convert.ToBoolean(chromeBrowser.EvaluateScriptAsync(clickAndScrollScript).Result.Result) == true) // compare position from old var and current scroll position /evaluate script
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

                        GetFavbetDocumentAsync(htmlDoc);
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
        public Task<HtmlAgilityPack.HtmlDocument> GetFavbetDocumentAsync(HtmlAgilityPack.HtmlDocument htmlDoc)
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
            //string xpathSportType = "//div[contains(@class, 'EventsLineTitle_title')]/span";
            //string xpathTime = "//div[contains(@class, 'EventDate_eventDate')]";//span[@class = 'time']";
            //string xpathDate = "//div[contains(@class, 'EventTime_time')]";
            //string xpathOddTypesPart = "//div[contains(@class, 'HeadGroup_marketParamHead')]/span";//"//div[@class = 'tip']";
            //string xpathPairsPart = "//div[contains(@class, 'EventParticipants')]/div";//"//div[@class = 'odd']";
            //string xpathOddsPart = "//div[contains(@class, 'eventSubRowContainer')]/div/div";//"//td[@class = 'match']//span[last()]"; //Get last span

            string xpathSportType = "//div[contains(@class, 'EventsLineTitle_title')]";//"//div[contains(@class, 'EventBody_container')]";
            string xpathOddTypesPart1 = "//div[1]/span[contains(@class, 'HeadGroup_typeName')]";
            string xpathOddTypesPart2 = "//div[2]/span[contains(@class, 'HeadGroup_typeName')]";
            string xpathDate = "//div[contains(@class, 'EventDate_table')]";
            string xpathTime = "//div[contains(@class, 'EventTime_table')]";
            string xpathPairsPart = "//div[contains(@class, 'EventParticipants_participantMain')]";
            string xpathOddsPart1 = "//div[1]/div/div/div/div/div/span[contains(@class, '_coef')] | //div[2]/div/div/div/div/div/span[contains(@class, '_coef')]";
            string xpathOddsPart2 = "//div[2]/div/div/div/div/div/span[contains(@class, '_coef')]";

            //var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes(xpathOddsPart1);
            var HTMLTableTRList = htmlDoc.DocumentNode.SelectNodes(xpathSportType + "|" + xpathOddTypesPart1 + "|" + xpathOddTypesPart2 + "|" + xpathDate + "|" + xpathTime + "|" + xpathPairsPart + "|" + xpathOddsPart1).Cast<HtmlNode>();

            // Using LINQ to parse HTML table smartly 
            //var HTMLTableTRList = from table in htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'Box_box')]").Cast<HtmlNode>()
            //                      from row in table.SelectNodes(xpathOddTypesPart1 + "|" + xpathOddTypesPart2).Cast<HtmlNode>()
            //                      from cell in table.SelectNodes(xpathSportType + "|" + xpathDate + "|" + xpathTime + "|" + xpathPairsPart + "|" + xpathOddsPart1 + "|" + xpathOddsPart2).Cast<HtmlNode>()
            //                      select new { Cell_Text = cell.InnerText };

            #region New Way
            //IList<OddsTable> results = new List<OddsTable>();
            //foreach (var node in htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'Box_box')]"))
            //{
            //    var record = new OddsTable();
            //    record.Home = node.SelectSingleNode("//div[contains(@class, 'EventParticipants_participantMain')]").InnerText;
            //    record.SportType = node.SelectSingleNode("//div[contains(@class, 'EventsLineTitle_title')]").InnerText;
            //    record.EventTime = node.SelectSingleNode("//div[contains(@class, 'EventDate_table')]").InnerText;
            //    results.Add(record);
            //}
            #endregion new Way

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
                /*
                if (node.HasAttributes && node.Attributes.First().Value.Contains("sport-header")) // if New Sport Type
                {
                    #region filter Sport types


                    #endregion filter Sport types
                    previusSportType = sportType;
                    sportType = Regex.Replace(currentItem, @"\s+", "");
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
                        match.KladaName = "HL";
                        match.SportType = previusSportType; // new sport type is generated for new Event (date) 
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

                    dateTime = currentItem;
                    nextRecord = true;
                    RowCounter = 0;

                    #region Reset params

                    //This is new Event
                    hasOdd1 = false;
                    hasOddX = false;
                    hasOdd2 = false;
                    hasOdd1X = false;
                    hasOddX2 = false;
                    hasOdd12 = false;
                    #endregion Reset params

                    continue;
                }
                */
                RowCounter++; // foreach new node increase row counter
                previusSportType = sportType; // set current sport type to previus sport type (last event in table sportType bugfix)

                #region Set Odd Table
                switch (currentItem)
                {
                    case "1":
                        if (hasOdd1)
                            continue;
                        hasOdd1 = true;
                        lastOddType = currentItem;
                        continue;
                    case "X":
                        if (hasOddX)
                            continue;
                        hasOddX = true;
                        lastOddType = currentItem;
                        continue;
                    case "2":
                        if (hasOdd2)
                            continue;
                        hasOdd2 = true;
                        lastOddType = currentItem;
                        continue;
                    case "1X":
                        if (hasOdd1X)
                            continue;
                        hasOdd1X = true;
                        lastOddType = currentItem;
                        continue;
                    case "X2":
                        if (hasOddX2)
                            continue;
                        hasOddX2 = true;
                        lastOddType = currentItem;
                        continue;
                    case "12":
                        if (hasOdd12)
                            continue;
                        hasOdd12 = true;
                        lastOddType = currentItem;
                        continue;
                }

                //    totalrows = RowCounter;
                //    if (totalrows > 0)
                //        oddTableSetCompleated = true;
                //    // In this moment we know that is Odd table completed
                //        RowCounter = 0; // Reset row counter - prepare it for Odd values.

                #endregion Set Odd Table

                #region Save to Class
                if (!nextRecord)
                    continue;

                if (RowCounter == 1 && currentItem != string.Empty)
                {
                    var separator = currentItem.IndexOf("-", 2); // index of separator home and away event
                    var home = currentItem;
                    var away = string.Empty;

                    if (separator > 0)
                    {
                        home = currentItem.Substring(0, separator);
                        away = currentItem.Substring(separator + 1);
                    }
                    var doubleEmptySpaces = away.IndexOf("   "); // index of two spaces in the string
                    if (away != string.Empty && doubleEmptySpaces > 0)
                    {
                        away = away.Substring(0, doubleEmptySpaces); // This will remove all text after two whitespace
                    }
                    match.Home = home.Trim();
                    match.Away = away.Trim();
                    db.OddsTable.Add(match);
                    continue;
                }
                
                if (lastOddType == "1" && hasOdd1)
                {
                    decimal parsedValue;
                    if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                    {
                        continue;
                    }
                    match.Odd1 = decimal.Parse(currentItem, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                    db.OddsTable.Add(match);
                    continue;
                }
                if (lastOddType == "X" && hasOddX)
                {
                    decimal parsedValue;
                    if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                    {
                        continue;
                    }
                    match.OddX = decimal.Parse(currentItem, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                    db.OddsTable.Add(match);
                    continue;
                }
                if (lastOddType == "2" && hasOdd2)
                {
                    decimal parsedValue;
                    if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                    {
                        continue;
                    }
                    match.Odd2 = decimal.Parse(currentItem, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                    db.OddsTable.Add(match);
                    continue;
                }
                if (lastOddType == "1X" && hasOdd1X)
                {
                    decimal parsedValue;
                    if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                    {
                        continue;
                    }
                    match.Odd1X = decimal.Parse(currentItem, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                    db.OddsTable.Add(match);
                    continue;
                }
                if (lastOddType == "X2" && hasOddX2)
                {
                    decimal parsedValue;
                    if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                    {
                        continue;
                    }
                    match.OddX2 = decimal.Parse(currentItem, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                    db.OddsTable.Add(match);
                    continue;
                }
                if (lastOddType == "12" && hasOdd12)
                {
                    decimal parsedValue;
                    if (!Decimal.TryParse(currentItem, out parsedValue)) // Number returns true, 
                    {
                        continue;
                    }
                    match.Odd12 = decimal.Parse(currentItem, new NumberFormatInfo() { NumberDecimalSeparator = "," });
                    db.OddsTable.Add(match);
                    continue;
                }
            }
            
            #endregion Save to Class

            //Save last match

            // Check if all string property in match class is null
            if (!match.GetType().GetProperties()
                            .Where(pi => pi.GetValue(match) is string)
                            .All(p => p.GetValue(match) == null)) // If object is not null Save previus record
            {
                match.EventTime = dateTime;
                match.Created = DateTime.UtcNow;
                match.KladaName = "HL";
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
            // Koristi na predposljednjoj formi
            Dispose();
            this.Close();
            //Cef.Shutdown();
            #endregion
        }
    }
}
