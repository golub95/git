using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Klada_API.Kladionice.Favbet
{
    class Favbet
    {
        public static void API_Favbet()
        {
            var client = new RestClient("https://www.favbet.hr/frontend_api2/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("authority", "www.favbet.hr");
            request.AddHeader("accept", "*/*");
            request.AddHeader("accept-language", "en-US,en;q=0.9");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "text/plain;charset=UTF-8");
            request.AddHeader("cookie", "_gcl_au=1.1.1234079002.1661770600; _ga=GA1.2.1513967526.1661770600; affiliate_timestamp=2022-08-29T10:56:40.914Z; b_tag=a_63b_48c_brand_searchAffiliateId=32; type_r=ia; upstream=1; LANGUAGE=hr; AB_TEST_COOKIE={%22test-betslip-button%22:%22variant-a%22}; stickyCoockies=1662978995.426.3871.433432|163786b79aa5a20df757413a5fe83018; _gid=GA1.2.1782270648.1662978995; isBannerViewed-cookie-privacy-notification=true; COUNTRY-ID=HR; _gat_gtag_UA_162274005_2=1; PHPSESSID=64E6C59E22CE475E0E6D085EA1525AF80DAD3D22FF9D868A9A487F34A0526BBCE417DCB20D864DF3905C68ADC5905D8389354B015A2CB7D22CB49FD525A97719; sessionid=eyJfZXhwaXJlIjoxNjYzMDcxMDgwMjA0LCJfbWF4QWdlIjozNjAwMDAwfQ==; sessionid.sig=6_GehJhCRvdOA_9pj3khwRtIJ60; COUNTRY-ID=HR");
            request.AddHeader("origin", "https://www.favbet.hr");
            request.AddHeader("pragma", "no-cache");
            request.AddHeader("referer", "https://www.favbet.hr/hr/sports/sport/soccer/");
            request.AddHeader("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"104\", \"Opera GX\";v=\"90\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "no-cors");
            request.AddHeader("sec-fetch-site", "same-origin");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 OPR/90.0.4480.78";
            var body = @"{""jsonrpc"":""2.0"",""id"":83,""method"":""frontend/event/get"",""params"":{""by"":{""lang"":""hr"",""sport_id"":1,""head_markets"":true,""service_id"":0,""tz_diff"":7200,""time_range"":1189,""limit"":30,""offset"":30}}}";
            request.AddParameter("text/plain;charset=UTF-8", body,  ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);

            //using (var outStream = new StreamWriter(request.GetRequestStream()))
            //{
            //    outStream.Write(body); // this is request. add parameter (without this response is without data)
            //}

            // Get response
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            IRestResponse response = client.Execute(request);
            string resultText = "";
            //using (var reader = new StreamReader())
            //{
                resultText = client.Execute(request).Content;
            //}

            Program.DownloadData(resultText, "c:\\temp/API_Result_Favbet_Nogomet.txt");

            //ParsingJSONString(resultText);
        }

    }
}
