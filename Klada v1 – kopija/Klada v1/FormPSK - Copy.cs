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
using Klada_v1.Model;
using Application = System.Windows.Forms.Application;

namespace Klada_v1
{
   
    public partial class FormPSKCopy : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public object ControlSnapshot { get; private set; }
        public object AsyncContext { get; private set; }

        public FormPSKCopy()
        {
            InitializeComponent();
            InitializeChromium();

        }
        public void InitializeChromium()
        {
            // Create a browser component
            //chromeBrowser = new ChromiumWebBrowser("http://admin:@192.168.43.10/video.cgi");
            chromeBrowser = new ChromiumWebBrowser("https://www.psk.hr/oklade/nogomet");
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
            chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;

            chromeBrowser.LoadingStateChanged += (sender, args) =>
            {
                IFrame frame = null;
                frame = chromeBrowser.GetFocusedFrame();
                //Wait for the Page to finish loading
                Console.WriteLine("Loading State Changed GoBack {0} GoForward {1} CanReload {2} IsLoading {3}", args.CanGoBack, args.CanGoForward, args.CanReload, args.IsLoading);
                chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;

                if (args.CanReload && !args.IsLoading)
                {
                    //EvaluateScriptAsPromiseAsync calls Promise.resolve internally so even if your code doesn't
                    //return a Promise it will still execute successfully.
                    //var script = @"return (function() { return 1 + 1; })();";
                    //JavascriptResponse response = await frame.EvaluateScriptAsPromiseAsync(script);
                    //JavascriptResponse response = await frame.EvaluateScriptAsync(script);
                    // An example that gets the Document Height
                    //var task = frame.EvaluateScriptAsync("(function() { var body = document.body, html = document.documentElement; return  Math.max( body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight ); })();");

                    //Continue execution on the UI Thread
                    //task.ContinueWith(t =>
                    //{
                    //    if (!t.IsFaulted)
                    //    {
                    //        var response2 = t.Result;
                    //        //EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                    //    }
                    //}, TaskScheduler.FromCurrentSynchronizationContext());



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
                }
            };

            chromeBrowser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;


            //Wait for the MainFrame to finish loading
            chromeBrowser.FrameLoadEnd += (sender, args) =>
            {
                chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
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
                        chromeBrowser.FrameLoadEnd += OnBrowserFrameLoadEnd;
                        chromeBrowser.ViewSource();
                    }
                    if (args.HttpStatusCode == -101)
                    {
                        //finished, OK, streaming shut down
                        chromeBrowser.Reload();
                    }
                    if (args.HttpStatusCode == 0)
                    {
                        //The client request wasn't successful.
                        chromeBrowser.Reload();
                    }
                }
            };
        }

        private void OnBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs)
        {
            if (frameLoadEndEventArgs.Frame.IsMain /* && frameLoadEndEventArgs.Url == pageUrl*/)
            {
                frameLoadEndEventArgs.Browser.MainFrame.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");
                        /*ExecuteJavaScriptAsync(
                    "(function() { var el = document.querySelector('a[href=\"" + linkUrl +
                    "\"]'); if(el) { el.scrollIntoView(); } })();");*/
            }
        }
        public class BoundObject
        {
            public string MyProperty { get; set; }
            public void MyMethod()
            {
                // Do something really cool here.
            }

            public void TestCallback(IJavascriptCallback javascriptCallback)
            {
                const int taskDelay = 1500;

                Task.Run(async () =>
                {
                    await Task.Delay(taskDelay);

                    using (javascriptCallback)
                    {
                        //NOTE: Classes are not supported, simple structs are
                       //// var response = new CallbackResponseStruct("This callback from C# was delayed " + taskDelay + "ms");
                      //  await javascriptCallback.ExecuteAsync(response);
                    }
                });
            }
        }
        /// <summary>
        /// Execute some javascript code and return its output value
        /// </summary>
        /// <param name="script"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public object EvaluateScript(string script, object defaultValue, TimeSpan timeout)
        {
            var result = defaultValue;
            if (chromeBrowser.IsBrowserInitialized && !chromeBrowser.IsDisposed && !chromeBrowser.Disposing)
            {
                var task = chromeBrowser.EvaluateScriptAsync(script, timeout);
                var complete = task.ContinueWith(t => {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        result = response.Success ? (response.Result ?? "null") : response.Message;
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
                complete.Wait(); // Here comes the deadlock!
            }
            return result;
        }
        public async Task TestCorrect()
        {
            await Task.Run(async () => //Task.Run automatically unwraps nested Task types!
            {
                Console.WriteLine("Start");
                await Task.Delay(5000);
                Console.WriteLine("Done");
                chromeBrowser.ViewSource();
            });
            Console.WriteLine("All done");
        }

        private async Task AsyncCallerAsync()
        {
            //await Scroll();

      
        }

        private async Task Scroll()
        {
            Console.WriteLine("Start Scroll");
            //JavascriptResponse response = new JavascriptResponse();
            //var task = Task.Run(async () => await chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()"));
            //response = await chromeBrowser.EvaluateScriptAsync("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");
            //string script = string.Format("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()");
            //EvaluateScript(script, null,new TimeSpan());
   

            //response = await chromeBrowser.EvaluateScriptAsync(
            //    ("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()"));

            var task = await chromeBrowser.EvaluateScriptAsync(
                ("document.getElementById(\"sport-events-list-ajax-load-more\").scrollIntoView()"));
        }
        static int SetHeightFromDocument(ChromiumWebBrowser webControl)
        {
            webControl.EvaluateScriptAsync(@"(function() {
            var body = document.body,
                html = document.documentElement;

            var height = Math.max( body.scrollHeight, body.offsetHeight, 
                                   html.clientHeight, html.scrollHeight, html.offsetHeight );
            return height;
            })();")
                .ContinueWith(
                    height => {
                        webControl.Height = (int)height.Result.Result + 20; // Take possible scrollbar into acct
                    });

            return (webControl.Height);
        }
        private async Task GetDocSource()
        {
            chromeBrowser.ViewSource();
        }

        private void ChromeBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Console.WriteLine("end");
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Console.WriteLine(chromeBrowser.IsLoading);
        }

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var b = ((ChromiumWebBrowser)sender);

            this.InvokeOnUiThreadIfRequired(() => b.Focus());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = this.Text;
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                chromeBrowser.Load(url);
            }
        }

        public Task<HtmlAgilityPack.HtmlDocument> GetPSKDocument(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");
            IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("td.col-odds > a");
            IList<HtmlNode> matches = htmlDoc.QuerySelectorAll("div.event-name > span.market-name");
            //HtmlNode node = nodes[0].QuerySelector("td:eq(1)");

            // Using LINQ to parse HTML table smartly 
            var HTMLTableTRList = from table in htmlDoc.DocumentNode.SelectNodes("//html/body/div").Cast<HtmlNode>()
                                  from row in table.SelectNodes("//tr").Cast<HtmlNode>()
                                  from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
                                  select new { Cell_Text = cell.InnerText };
            #region Params

            bool hasOdd1 = false;
            bool hasOddX = false;
            bool hasOdd2 = false;
            bool hasOdd1X = false;
            bool hasOddX2 = false;
            bool hasOdd12 = false;
            bool hasVise = false;
            bool hasVrijeme = false;
            bool lastTableRow = false;
            int totalrows = 0;

            int counter = 0;

            HRKladeEntities db = new HRKladeEntities();
            OddsTable match = new OddsTable();
            #endregion

            foreach (var cell in HTMLTableTRList)
            {
                //clear string
                string currentItem = cell.Cell_Text.Replace("&nbsp;", " ").Replace("\n", "").Replace("\r", "").Trim();
                lastTableRow = false;

                #region Reset params
                if (currentItem.Contains("oklada"))
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
                    lastTableRow = false;
                    totalrows = 0;
                    counter = 0;
                    continue;
                }
                if (currentItem == string.Empty)
                {
                    counter = 0;
                    continue;
                }
                #endregion Reset params

                #region Set Odd Table

                switch (currentItem)
                {
                    case "1":
                        hasOdd1 = true;
                        counter++;
                        continue;
                    case "X":
                        hasOddX = true;
                        counter++;
                        continue;
                    case "2":
                        hasOdd2 = true;
                        counter++;
                        continue;
                    case "1X":
                        hasOdd1X = true;
                        counter++;
                        continue;
                    case "X2":
                        hasOddX2 = true;
                        counter++;
                        continue;
                    case "12":
                        hasOdd12 = true;
                        counter++;
                        continue;
                    case "više":
                        hasVise = true;
                        continue;
                    case "datum":
                        lastTableRow = true;
                        hasVrijeme = true;
                        totalrows = counter;
                        counter = 0;
                        continue;
                }
                #endregion Set Odd Table

                #region Save to Class
                if (!lastTableRow) // Save table values to database
                {
                    if (totalrows == 2) // there are only two odds
                    {
                        if (counter == 0 && currentItem != string.Empty)
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
                            match.Home = home.TrimEnd();
                            match.Away = away.TrimEnd();
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 1 && hasOdd1)
                        {
                            match.Odd1 = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 2 && hasOdd2)
                        {
                            match.Odd2 = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 3 && hasVise)
                        {
                            counter++;
                            continue;
                        }
                        if (counter == 4 && hasVrijeme)
                        {
                            match.EventTime = currentItem;
                            match.Created = DateTime.UtcNow;
                            match.KladaName = "PSK";
                            match.InPlay = false;
                            db.OddsTable.Add(match);
                            db.SaveChanges();
                            counter = 0;
                            counter++;
                            continue;
                        }
                    }
                    else if (totalrows == 6)
                    {
                        if (counter == 0 && currentItem != string.Empty)
                        {
                            var eventInfoNumber = new string(currentItem.Where(Char.IsNumber).ToArray());
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
                            match.Home = home.TrimEnd();
                            match.Away = away.TrimEnd();
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 1 && hasOdd1)
                        {
                            match.Odd1 = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 2 && hasOddX)
                        {
                            match.OddX = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 3 && hasOdd2)
                        {
                            match.Odd2 = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 4 && hasOdd1X)
                        {
                            match.Odd1X = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 5 && hasOddX2)
                        {
                            match.OddX2 = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 6 && hasOdd12)
                        {
                            match.Odd12 = Convert.ToDecimal(currentItem);
                            db.OddsTable.Add(match);
                            counter++;
                            continue;
                        }
                        if (counter == 7 && hasVise)
                        {
                            counter++;
                            continue;
                        }
                        if (counter == 8 && hasVrijeme)
                        {
                            match.EventTime = currentItem;
                            match.Created = DateTime.UtcNow;
                            match.KladaName = "PSK";
                            match.SportType = "Odbojka"; //TODO: get adress from chromium
                            match.InPlay = false;
                            db.OddsTable.Add(match);
                            db.SaveChanges();
                            counter = 0;
                            continue;
                        }
                    }
                }
                #endregion Save to Class

            }
            return null;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            this.Close();

            //Cef.Shutdown();
        }
    }


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
}
