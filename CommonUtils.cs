using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PaytmEnach
{
    public class CommonUtils
    {
        public string CommunicateToServer(string serverURL, string content, string contentType)
        {
            string ServerResponse = string.Empty;

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(serverURL);
                request.Method = "POST";
                request.ContentType = contentType;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(content);
                    sw.Flush();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    ServerResponse = sr.ReadToEnd();
                }

                response.Close();
            }
            catch (Exception ex)
            {

            }

            return ServerResponse;
        }

        internal string GetTransactionStatus(string SubsID, string CustID)
        {
            string BodyJSON = "{\"subsId\":\"" + SubsID + "\",\"mid\":\"" + PaytmCreds.MID + "\",\"custId\":\"" + CustID + "\"}";
            string Checksum = paytm.CheckSum.generateCheckSumByJson(PaytmCreds.MerchantKey, BodyJSON);
            string RequestJSON = "{\"head\":{\"version\":\"v1\",\"tokenType\":\"AES\",\"signature\":\"" + Checksum + "\"},\"body\":" + BodyJSON + "}";
            string ServerURL = "https://securegw-stage.paytm.in/subscription/checkStatus";
            string ServerResponse = this.CommunicateToServer(ServerURL, RequestJSON, "application/json");
            return ServerResponse;
        }
    }
}