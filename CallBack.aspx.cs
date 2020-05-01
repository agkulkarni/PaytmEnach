using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;

namespace PaytmEnach
{
    public partial class CallBack : System.Web.UI.Page
    {
        PaytmEnachEntities db = new PaytmEnachEntities();
        CommonUtils utils = new CommonUtils();
        //Easy easy = new Easy();
        //Slist HeaderList = new Slist();
        protected void Page_Load(object sender, EventArgs e)
        {
            //easy.SetOpt(CURLoption.CURLOPT_URL, "https://www.facebook.com/");
            //easy.SetOpt(CURLoption.CURLOPT_POST, true);
            //easy.Perform();
            string MandateFormURL = string.Empty;
            CommonUtils utils = new CommonUtils();
            //this.UploadMandateForm("63375");
            Response.Write(this.DownloadMandateForm("64977"));
            this.UploadMandateForm("64977");
            //var Req = Request.Form;
            foreach (var Req in Request.Form)
            {
                Response.Write(Req.ToString());
            }
            //if (Request.Form != null && Request.Form["respcode"].ToString() == "1" && Request.Form["status"].ToString() == "SUCCESS")
            //{
                
            //}
        }

        private string DownloadMandateForm(string SubsID)
        {
            SIPRegistrationData data = db.SIPRegistrationDatas.Where(a => a.SubsID == SubsID).FirstOrDefault();
            AccountDetail AccDet = db.AccountDetails.Where(a => a.ID == data.AccountDetails).FirstOrDefault();
            string MandateFormURL = string.Empty;
            string BodyJSON = "{\"mid\":\"" + PaytmCreds.MID + "\",\"subscriptionId\":\"" + SubsID + "\"}";
            string CheckSum = paytm.CheckSum.generateCheckSumByJson(PaytmCreds.MerchantKey, BodyJSON);
            string RequestJSON = "{\"head\":{\"version\":\"v1\",\"tokenType\":\"AES\",\"signature\":\"" + CheckSum + "\"},\"body\":" + BodyJSON + "}";
            string ServerURL = "https://securegw-stage.paytm.in/subscription/paper/mandate/downloadUrl";
            string ResponseJSONString = utils.CommunicateToServer(ServerURL, RequestJSON, "application/json");
            JObject ResponseJSON = JObject.Parse(ResponseJSONString);

            if (Convert.ToString(ResponseJSON["body"]["resultInfo"]["code"]) == "3006")
            {
                data.IsMandateFormDowloaded = true;
                AccDet.MandateFormDownloadURL = Convert.ToString(ResponseJSON["body"]["downloadUrl"]);
                MandateFormURL = Convert.ToString(ResponseJSON["body"]["downloadUrl"]);
            }
            else
                data.IsMandateFormDowloaded = false;

            data.MandateFormDownloadResultCode = ResponseJSON["body"]["resultInfo"]["code"].ToString();
            data.MandateFormDownloadResultStatus = ResponseJSON["body"]["resultInfo"]["status"].ToString();
            db.SaveChanges();

            return ResponseJSONString;
        }

        public void UploadMandateForm(string SubsID)
        {
            SIPRegistrationData data = db.SIPRegistrationDatas.Where(a => a.SubsID == SubsID).FirstOrDefault();
            AccountDetail AccDet = db.AccountDetails.Where(a => a.ID == data.AccountDetails).FirstOrDefault();
            string ServerURL = "https://securegw-stage.paytm.in/subscription/paper/mandate/uploadUrl";
            string BodyJSONString = "{\"mid\":\"" + PaytmCreds.MID + "\",\"subscriptionId\":\"" + SubsID + "\"}";
            string CheckSum = paytm.CheckSum.generateCheckSumByJson(PaytmCreds.MerchantKey, BodyJSONString);
            string RequestJSON = "{\"head\":{\"tokenType\":\"AES\",\"signature\":\"" + CheckSum + "\"},\"body\":" + BodyJSONString + "}";
            string ResponseJSONStr = utils.CommunicateToServer(ServerURL, RequestJSON, "application/json");
            JObject ResponseJSON = JObject.Parse(ResponseJSONStr);

            if (Convert.ToString(ResponseJSON["body"]["resultInfo"]["code"]) == "3006")
            {
                data.IsMandateFormUploaded = true;
                AccDet.MandateFormUploadURL = Convert.ToString(ResponseJSON["body"]["uploadUrl"]);
                AccDet.PolicyForm = Convert.ToString(ResponseJSON["body"]["policyForm"]);
                //MandateFormURL = Convert.ToString(Response["body"]["downloadUrl"]);
            }
            else
                data.IsMandateFormDowloaded = false;

            data.MandateFormUploadResultCode = Convert.ToString(ResponseJSON["body"]["resultInfo"]["code"]);
            data.MandateFormUploadResultStatus = Convert.ToString(ResponseJSON["body"]["resultInfo"]["status"]);
            db.SaveChanges();
            Response.Write(ResponseJSONStr);
        }
    }
}