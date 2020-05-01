using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Newtonsoft.Json.Linq;

namespace PaytmEnach
{
    public partial class Subscription : System.Web.UI.Page
    {
        private PaytmEnachEntities db = new PaytmEnachEntities();
        CommonUtils utils = new CommonUtils();
        protected void Page_Load(object sender, EventArgs e)
        {
            //mpeBankDetails.Show();s
            //Response.Write(utils.GetTransactionStatus("63387", "2"));
        }

        protected void drpSIPDay_TextChanged(object sender, EventArgs e)
        {
            DateTime NextMonthDate = System.DateTime.Now.AddMonths(1);
            txtSIPStartDate.Text = Convert.ToDateTime(NextMonthDate.Year + "-" + NextMonthDate.Month + "-" + drpSIPDay.SelectedValue).ToString("dd/MM/yyyy");
        }

        protected void btnCreateSIP_Click(object sender, EventArgs e)
        {
            string OrderID = string.Empty;
            string ServerURL = string.Empty;
            string TxnAmount = string.Empty;
            string Mobile = string.Empty;
            string Email = string.Empty;
            string CallbackUrl = string.Empty;
            string SubsRequestBodyJSON = string.Empty;
            string checksum = string.Empty;
            string SubsRequestJSON = string.Empty;
            string SubsInitResponseJSON = string.Empty;
            string PaymentOptions = string.Empty;
            UserMaster Owner = db.UserMasters.Where(a => a.ID == 2).FirstOrDefault();
            SIPData SubsData = new SIPData();
            string[] SIPStartDate = txtSIPStartDate.Text.Split('/');
            string[] SIPEnddate = txtSIPEndDate.Text.Split('/');
            SubsData.StartDate = SIPStartDate[2] + "-" + SIPStartDate[1] + "-" + SIPStartDate[0];
            SubsData.EndDate = SIPEnddate[2] + "-" + SIPEnddate[1] + "-" + SIPEnddate[0];
            SubsData.Frequency = Convert.ToInt32(drpFrequency.SelectedValue);
            SubsData.FrequencyUnit = db.FrequencyUnitMasters.Where(a => a.FrequencyUnit == drpSIPFrequencyUnit.SelectedValue).Select(b => b.ID).SingleOrDefault();
            SubsData.Amount = Convert.ToDecimal(txtAmount.Text);
            SubsData.CustomerID = Owner.ID;
            db.SIPDatas.Add(SubsData);
            db.SaveChanges();
            OrderID = "ON" + SubsData.ID;
            ServerURL = "https://securegw-stage.paytm.in/subscription/create?mid=" + PaytmCreds.MID + "&orderId=" + OrderID;
            TxnAmount = Convert.ToDouble(txtAmount.Text).ToString("0.00");
            Mobile = Owner.Mobile;
            Email = Owner.Email;
            CallbackUrl = "https://localhost:44300/CallBack.aspx";

            SubsRequestBodyJSON = "{\"requestType\":\"NATIVE_SUBSCRIPTION\",\"mid\":\"" + PaytmCreds.MID + "\",\"orderId\":\"" + OrderID + "\",\"websiteName\":\"" + PaytmCreds.WEBSITE + "\",\"subscriptionAmountType\":\"VARIABLE\",\"subscriptionMaxAmount\":\"" + TxnAmount + "\",\"subscriptionEnableRetry\":\"0\",\"subscriptionRetryCount\":\"5\",\"subscriptionFrequencyUnit\":\"MONTH\",\"subscriptionFrequency\":\"" + SubsData.Frequency.ToString() + "\",\"subscriptionStartDate\":\"" + SubsData.StartDate + "\",\"subscriptionExpiryDate\":\"" + SubsData.EndDate + "\",\"subscriptionGraceDays\":\"0\",\"callbackUrl\":\"" + CallbackUrl + "\",\"txnAmount\":{\"value\":\"0\",\"currency\":\"INR\"},\"userInfo\":{\"custId\":\"" + SubsData.CustomerID.ToString() + "\"}}"; //Newtonsoft.Json.JsonConvert.SerializeObject(SubsRequestBody);
            checksum = paytm.CheckSum.generateCheckSumByJson(PaytmCreds.MerchantKey, SubsRequestBodyJSON);
            //JObject SubsRequest = new JObject(new JProperty("head", new JObject(new JProperty("clientId", PaytmCreds.MerchantKey.ToString()), new JProperty("version", "v1"), new JProperty("channelId", PaytmCreds.CHANNEL_ID), new JProperty("signature", checksum))), new JProperty("body", SubsRequestBody));
            SubsRequestJSON = "{\"head\":{\"clientId\":\"" + PaytmCreds.MerchantKey + "\",\"version\":\"v1\",\"channelId\":\"" + PaytmCreds.CHANNEL_ID + "\",\"signature\":\"" + checksum + "\"},\"body\":" + SubsRequestBodyJSON + "}"; //Newtonsoft.Json.JsonConvert.SerializeObject(SubsRequest);
            SubsInitResponseJSON = utils.CommunicateToServer(ServerURL, SubsRequestJSON, "application/json");

            if (!string.IsNullOrEmpty(SubsInitResponseJSON))
            {
                JObject SubsInitResponse = JObject.Parse(SubsInitResponseJSON);
                SIPRegistrationData sIPRegData = new SIPRegistrationData();
                sIPRegData.SIPID = SubsData.ID;
                sIPRegData.InsertDate = System.DateTime.Now;
                sIPRegData.InsertUserID = SubsData.CustomerID;
                sIPRegData.OrderID = OrderID;
                sIPRegData.UpdateDate = System.DateTime.Now;
                sIPRegData.UpdateUserID = Owner.ID;
                sIPRegData.TranInitResultCode = Convert.ToString(SubsInitResponse["body"]["resultInfo"]["resultCode"]);
                sIPRegData.TranInitResultStatus = Convert.ToString(SubsInitResponse["body"]["resultInfo"]["resultCode"]) != "0000" ? Convert.ToString(SubsInitResponse["body"]["resultInfo"]["resultStatus"]) + "(" + Convert.ToString(SubsInitResponse["body"]["resultInfo"]["resultMsg"]) + ")" : Convert.ToString(SubsInitResponse["body"]["resultInfo"]["resultStatus"]);

                if (Convert.ToString(SubsInitResponse["body"]["resultInfo"]["resultCode"]) == "0000" && Convert.ToString(SubsInitResponse["body"]["resultInfo"]["resultStatus"]) == "S")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "subsinitsuccess", "javascript: alert('Subscription initiated successfully, please enter bank details to proceed further');", true);
                    sIPRegData.SubsID = Convert.ToString(SubsInitResponse["body"]["subscriptionId"]);
                    sIPRegData.txnToken = Convert.ToString(SubsInitResponse["body"]["txnToken"]);
                    sIPRegData.IsTransactionInit = true;
                    PaymentOptions = this.GetPaymentOptions(Convert.ToString(SubsInitResponse["body"]["txnToken"]), OrderID);
                    if (!string.IsNullOrEmpty(PaymentOptions))
                    {
                        JObject Options = JObject.Parse(PaymentOptions);
                        JArray PayModes = JArray.Parse(Options["body"]["merchantPayOption"]["paymentModes"].ToString());
                        JArray ChannelOptions = JArray.Parse(PayModes.AsJEnumerable().Where(a => a["displayName"].ToString() == "BANK_MANDATE").Select(b => b["payChannelOptions"]).First().ToString());
                        JArray MandateAuthModes = new JArray();

                        JObject obj;
                        foreach (var option in ChannelOptions.AsJEnumerable().Where(a => a["mandateMode"].ToString() == "E_MANDATE" && Convert.ToBoolean(a["isDisabled"]["status"]) == false && Convert.ToBoolean(a["hasLowSuccess"]["status"]) == false))
                        {
                            obj = new JObject();
                            obj.Add("BankCode", option["channelCode"]);
                            obj.Add("AuthModes", option["mandateAuthMode"]);
                            MandateAuthModes.Add(obj);
                        }

                        Session["MandateAuthModes"] = MandateAuthModes;

                        JArray BanksList = new JArray();

                        foreach (var option in ChannelOptions.AsJEnumerable().Where(a => Convert.ToBoolean(a["isDisabled"]["status"]) == false && Convert.ToBoolean(a["hasLowSuccess"]["status"]) == false))
                        {
                            obj = new JObject();
                            obj.Add("MandateMode", option["mandateMode"]);
                            obj.Add("BankCode", option["channelCode"].ToString());
                            obj.Add("BankName", option["channelName"].ToString());
                            BanksList.Add(obj);
                        }

                        Session["BanksList"] = BanksList;

                        db.SIPRegistrationDatas.Add(sIPRegData);
                        db.SaveChanges();

                        hdfOrderID.Value = OrderID;
                        hdfSubsID.Value = sIPRegData.SubsID;
                        hdfTxnToken.Value = sIPRegData.txnToken;
                        hdfRegData.Value = sIPRegData.ID.ToString();
                        mpeBankDetails.Show();
                    }
                    else
                    {
                        sIPRegData.IsTransactionInit = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "subsfail", "javascript: alert('Subscription registration failed due to technical reasons');", true);
                    }
                }   
                else
                {
                    sIPRegData.IsTransactionInit = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "subsfail", "javascript: alert('Subscription registration failed due to technical reasons');", true);
                }
            }
            else
            {
                db.SIPDatas.Remove(SubsData);
                db.SaveChanges();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "subsfail", "javascript: alert('Subscription registration failed due to technical reasons');", true);
            }
        }

        protected void drpSIPYears_TextChanged(object sender, EventArgs e)
        {
            string[] SIPStartDate = txtSIPStartDate.Text.Split('/');
            DateTime SIPEndDate = Convert.ToDateTime(SIPStartDate[2] + "-" + SIPStartDate[1] + "-" + SIPStartDate[0]).AddYears(Convert.ToInt32(drpSIPYears.SelectedValue));
            txtSIPEndDate.Text = SIPEndDate.ToString("dd/MM/yyyy");
        }

        protected void btnAuthSubs_Click(object sender, EventArgs e)
        {
            AuthenticateMandateRequest(hdfTxnToken.Value, hdfSubsID.Value, hdfOrderID.Value, Convert.ToInt64(hdfRegData.Value));
        }
        private string GetPaymentOptions(string TxnToken, string OrderID)
        {
            string PaymentOptions = string.Empty;
            string ServerURL = "https://securegw-stage.paytm.in/fetchPaymentOptions?mid=" + PaytmCreds.MID + "&orderId=" + OrderID;
            JObject PaymentOptionsRequest = new JObject(new JProperty("head", new JObject(new JProperty("txnToken", TxnToken), new JProperty("channelId", PaytmCreds.CHANNEL_ID))));
            string ServerResponse = utils.CommunicateToServer(ServerURL, Newtonsoft.Json.JsonConvert.SerializeObject(PaymentOptionsRequest), "application/json");
            JObject PaymentOptiosResponse = JObject.Parse(ServerResponse);

            if (PaymentOptiosResponse["body"]["resultInfo"]["resultCode"].ToString() == "0000" && PaymentOptiosResponse["body"]["resultInfo"]["resultStatus"].ToString() == "S")
            {
                PaymentOptions = PaymentOptiosResponse.ToString();
            }
            return PaymentOptions;
        }

        public void AuthenticateMandateRequest(string TxnToken, string SubsID, string OrderID, long RegData)
        {
            string resp = string.Empty;
            AccountDetail account = new AccountDetail();
            account.BankCode = BankDD.SelectedValue;
            account.BankName = BankDD.SelectedItem.Text;
            account.AccountNumber = txtAccountNo.Text;
            account.AccountType = drpAccType.SelectedValue;
            account.AuthMode = rblMandateModes.SelectedValue == "PAPER_MANDATE" ? "NET_BANKING" : rblAuthType.SelectedValue;
            account.AccHolderName = txtUserName.Text;
            account.IFSCCode = txtIFSCCode.Text;
            account.MandateMode = rblMandateModes.SelectedValue;

            string MandateAuthMode = string.Empty;

            if (rblMandateModes.SelectedValue == "PAPER_MANDATE")
                MandateAuthMode = "NET_BANKING";
            else
                MandateAuthMode = rblAuthType.SelectedValue;

            db.AccountDetails.Add(account);
            db.SaveChanges();
            SIPRegistrationData regData = db.SIPRegistrationDatas.Where(a => a.ID == (int)RegData).Select(b => b).FirstOrDefault();
            regData.AccountDetails = account.ID;
            db.SaveChanges();
            string ServerURL = "https://securegw-stage.paytm.in/order/pay?mid=" + PaytmCreds.MID + "&amp;orderId=" + OrderID;
            Response.Write("<html>");
            Response.Write("<head>");
            Response.Write("<title>");
            Response.Write("Authenticate Subscription");
            Response.Write("</title>");
            Response.Write("</head>");
            Response.Write("<body>");
            Response.Write("<center>");
            Response.Write("<h1> Please do not refresh this page...</h1>");
            Response.Write("</center>");
            Response.Write("<form method='POST' name='paytm_form' action='" + ServerURL + "'>");
            Response.Write("<input type='hidden' name='txnToken' value='" + TxnToken + "'>");
            Response.Write("<input type='hidden' name='SUBSCRIPTION_ID' value='" + SubsID + "'>");
            Response.Write("<input type='hidden' name='paymentMode' value='BANK_MANDATE'>");
            Response.Write("<input type = 'hidden' name='AUTH_MODE'  value='USRPWD'> ");
            Response.Write("<input type='hidden' name='account_number' value='" + txtAccountNo.Text.Trim() + "'>");
            Response.Write("<input type='hidden' name='bankIfsc' value='" + txtIFSCCode.Text.Trim() + "'>");
            Response.Write("<input type='hidden' name='USER_NAME' value='" + txtUserName.Text.Trim() + "'>");
            Response.Write("<input type='hidden' name='ACCOUNT_TYPE' value='" + drpAccType.SelectedValue.Trim() + "'>");
            Response.Write("<input type='hidden' name='channelCode' value='" + BankDD.SelectedValue + "'>");
            Response.Write("<input type='hidden' name='channelId' value='" + PaytmCreds.CHANNEL_ID + "'>");
            Response.Write("<input type='hidden' name='mandateAuthMode' value='" + MandateAuthMode + "'>");
            Response.Write("<script type='text/javascript'>");
            Response.Write("document.paytm_form.submit();");
            Response.Write("</script>");
            Response.Write("</body>");
            Response.Write("</html>");
            Response.Flush();
            Response.End();
        }

        protected void rblMandateModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAccountDetails();

            DataTable dt = new DataTable();
            dt.Columns.Add("BankCode", typeof(string));
            dt.Columns.Add("BankName", typeof(string));

            JArray BanksList = (JArray)Session["BanksList"];

            foreach (var option in BanksList.AsJEnumerable().Where(a => a["MandateMode"].ToString() == rblMandateModes.SelectedValue))
            {
                dt.Rows.Add(new object[] { option["BankCode"].ToString(), option["BankName"].ToString() });
            }

            ListItem InitItem = new ListItem("--Select--", "");

            BankDD.DataSource = dt;
            BankDD.DataBind();
            BankDD.Items.Insert(0, InitItem);
            BankDD.Enabled = true;
        }

        private void ClearAccountDetails()
        {
            txtAccountNo.Text = string.Empty;
            txtIFSCCode.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtAccountNo.Enabled = false;
            txtIFSCCode.Enabled = false;
            txtUserName.Enabled = false;
            BankDD.DataSource = null;
            BankDD.DataBind();
            BankDD.Enabled = false;
            drpAccType.SelectedValue = "";
            drpAccType.Enabled = false;
            rblAuthType.DataSource = null; ;
            rblAuthType.DataBind();
            pnlAuthMode.Visible = false;
        }

        private void EnableAccountDetails()
        {
            txtAccountNo.Enabled = true;
            txtIFSCCode.Enabled = true;
            txtUserName.Enabled = true;
            drpAccType.Enabled = true;
        }

        protected void BankDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblMandateModes.SelectedValue == "E_MANDATE")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("AuthModeText", typeof(string));
                dt.Columns.Add("AuthModeVal", typeof(string));
                JArray MandateAuthModes = (JArray)Session["MandateAuthModes"];

                JArray SelectedAuthModes = (JArray)MandateAuthModes.Where(a => a["BankCode"].ToString() == BankDD.SelectedValue).Select(b => b["AuthModes"]).First();
                foreach (var AuthMode in SelectedAuthModes)
                    dt.Rows.Add(new object[] { AuthMode.ToString().Replace("_", " "), AuthMode });

                rblAuthType.DataSource = dt;
                rblAuthType.DataBind();
                pnlAuthMode.Visible = true;
            }
            else
            {
                rblAuthType.DataSource = null;
                rblAuthType.DataBind();
                pnlAuthMode.Visible = false;
            }

            EnableAccountDetails();
        }
    }
}