<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Subscription.aspx.cs" Inherits="PaytmEnach.Subscription" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>e-NACH Subscription</h2>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-lg-6">
                <div class="row form-group">
                    <div class="col-lg-3">
                        <asp:Label ID="lblSIPDay" runat="server" Text="SIP Day"></asp:Label>
                    </div>
                    <div class ="col-lg-9">
                        <asp:DropDownList ID="drpSIPDay" runat="server" OnTextChanged="drpSIPDay_TextChanged" AutoPostBack="true" CssClass="form-control">
                            <asp:ListItem Value="0" Text="--Select--" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="1" Text="1" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="15" Text="15" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="20" Text="20" Enabled="true"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvdrpSIPDay" runat="server" ControlToValidate="drpSIPDay" Display="Dynamic" ErrorMessage="Select SIP day" ValidationGroup="SIPValidation" InitialValue="0" CssClass="text-danger" />
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-3">
                        <asp:Label ID="lblSIPYears" runat="server" Text="SIP Years" />
                    </div>
                    <div class="col-lg-9">
                        <asp:DropDownList ID="drpSIPYears" runat="server" OnTextChanged="drpSIPYears_TextChanged" AutoPostBack="true" CssClass="form-control">
                            <asp:ListItem Text="--Select--" Value="0" Enabled="true" />
                            <asp:ListItem Text="1" Value="1" Enabled="true" />
                            <asp:ListItem Text="2" Value="2" Enabled="true" />
                            <asp:ListItem Text="3" Value="3" Enabled="true" />
                            <asp:ListItem Text="4" Value="4" Enabled="true" />
                            <asp:ListItem Text="5" Value="5" Enabled="true" />
                            <asp:ListItem Text="6" Value="6" Enabled="true" />
                            <asp:ListItem Text="7" Value="7" Enabled="true" />
                            <asp:ListItem Text="8" Value="8" Enabled="true" />
                            <asp:ListItem Text="9" Value="9" Enabled="true" />
                            <asp:ListItem Text="10" Value="10" Enabled="true" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvdrpSIPYears" runat="server" ControlToValidate="drpSIPYears" Display="Dynamic" ErrorMessage="Select SIP years" InitialValue="0" ValidationGroup="SIPValidation" CssClass="text-danger" />
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-3">
                        <asp:Label ID="lblFrequency" runat="server" Text="Frequency" />
                    </div>
                    <div class="col-lg-9">
                        <asp:DropDownList ID="drpFrequency" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0" Text="--Select--" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="1" Text="1" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4" Enabled="true"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5" Enabled="true"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvdrpFrequency" runat="server" ControlToValidate="drpFrequency" Display="Dynamic" ErrorMessage="Select SIP frequency" ValidationGroup="SIPValidation" InitialValue="0" CssClass="text-danger" />
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-3">
                        <asp:Label ID="lblAmount" runat="server" Text="Amount">
                        </asp:Label>
                    </div>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtAmount" CssClass="form-control" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvtxtAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="Enter SIP amount" ValidationGroup="SIPValidation" CssClass="text-danger" />
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="row form-group">
                    <div class="col-lg-3">
                        <asp:Label ID="lblSIPStartDate" runat="server" Text="Start Date" />
                    </div>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtSIPStartDate" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-3">
                        <asp:Label ID="lblSIPEndDate" runat="server" Text="SIP End Date" />
                    </div>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtSIPEndDate" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-lg-3">
                        <asp:Label ID="lblSIPFreqencyUnit" runat="server" Text="Frequency Unit" />
                    </div>
                    <div class="col-lg-9">
                        <asp:DropDownList ID="drpSIPFrequencyUnit" runat="server" CssClass="form-control">
                            <asp:ListItem Text="--Select--" Value="" Enabled="true" />
                            <asp:ListItem Text="Monthly" Value="MONTH" Enabled="true" />
                            <asp:ListItem Text="Quarterly" Value="QUARTER" Enabled="true" />
                            <asp:ListItem Text="Yearly" Value="YEAR" Enabled="true" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvdrpSIPFrequencyUnit" runat="server" ControlToValidate="drpSIPFrequencyUnit" Display="Dynamic" ErrorMessage="Select SIP frequency unit" ValidationGroup="SIPValidation" CssClass="text-danger" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <asp:Button ID="btnCreateSIP" runat="server" CssClass="btn btn-primary" ValidationGroup="SIPValidation" OnClick="btnCreateSIP_Click" Text="Create Subscription" />
            </div>
        </div>
    </div>
    <asp:Button ID="btnDummy" Text="click me" runat="server" style="display:none;" />
    <ajax:ModalPopupExtender ID="mpeBankDetails" runat="server" DropShadow="true" PopupControlID="pnlbankDetails" TargetControlID="btnDummy" CancelControlID="btnCancel" Enabled="true" />
    <asp:Panel ID="pnlbankDetails" runat="server" style="width:1000px;display:none;">
                <div class="modal-dialog" style="max-width:none !important">
                    <div class="modal-content">
                        <div class="modal-header">
                            <div class="modal-title">
                                <h3>Bank Details</h3>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="updBankingDtls" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <div class="row form-group">
                                                <div class="col-lg-4">
                                                    <asp:Label ID="lblrblMandateModes" runat="server" Text="Mandate Mode" Font-Bold="true" />
                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:RadioButtonList ID="rblMandateModes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblMandateModes_SelectedIndexChanged" RepeatDirection="Vertical" RepeatLayout="Flow">
                                                        <asp:ListItem Text="e-Mandate" Value="E_MANDATE" Enabled="true" />
                                                        <asp:ListItem Text="Paper Mandate" Value="PAPER_MANDATE" Enabled="true" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator ID="rfvrblMandateModes" Display="Dynamic" ValidationGroup="ValBankDetails" runat="server" ControlToValidate="rblMandateModes" ErrorMessage="Select mandate mode" CssClass="text-danger" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col-lg-4">
                                                    <asp:Label ID="lblBankDD" Font-Bold="true" runat="server" Text="Select Bank" />
                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:DropDownList ID="BankDD" CssClass="form-control" OnSelectedIndexChanged="BankDD_SelectedIndexChanged" Enabled="false" AutoPostBack="true" runat="server" DataValueField="BankCode" DataTextField="BankName">
                                                        <asp:ListItem Text="--Select--" Value="" Enabled="true" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col-lg-4">
                                                    <asp:Label runat="server" Font-Bold="true" ID="lblAccountNumber" Text="Account No." />
                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:TextBox runat="server" Enabled="false" CssClass="form-control" ID="txtAccountNo" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtAccountNo" runat="server" Display="Dynamic" ControlToValidate="txtAccountNo" ErrorMessage="Account No. is mandatory" CssClass="text-danger" ValidationGroup="ValBankDetails" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col-lg-4">
                                                    <asp:Label runat="server" Font-Bold="true" ID="lblAccType" Text="Account Type" />
                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:DropDownList ID="drpAccType" Enabled="false" CssClass="form-control" runat="server">
                                                        <asp:ListItem Value="" Text="--Select--" Selected="True" Enabled="true"></asp:ListItem>
                                                        <asp:ListItem Value="ISA" Text="Individual Savings Account" Enabled="true"></asp:ListItem>
                                                        <asp:ListItem Value="CA" Text="Current Account" Enabled="true"></asp:ListItem>
                                                        <asp:ListItem Value="OTHERS" Text="Others" Enabled="true"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvdrpAcctype" InitialValue="" Display="Dynamic" ValidationGroup="ValBankDetails" runat="server" ControlToValidate="drpAccType" ErrorMessage="Choose account type" CssClass="text-danger" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6">
                                            <div class="row form-group">
                                                <div class="col-lg-4">
                                                    <asp:Label Font-Bold="true" ID="lblIFSCCode" runat="server" Text="IFSC Code" />
                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:TextBox ID="txtIFSCCode" Enabled="false" CssClass="form-control" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtIFSCCode" ValidationGroup="ValBankDetails" Display="Dynamic" runat="server" ControlToValidate="txtIFSCCode" ErrorMessage="IFSC Code is mandatory" CssClass="text-danger" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col-lg-4">
                                                    <asp:Label runat="server" Font-Bold="true" ID="lblUserName" Text="User Name" />
                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:TextBox runat="server" Enabled="false" CssClass="form-control" ID="txtUserName" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtUserName" ValidationGroup="ValBankDetails" Display="Dynamic" runat="server" ControlToValidate="txtUserName" ErrorMessage="User Name is mandatory" CssClass="text-danger" />
                                                </div>
                                            </div>
                                            <div runat="server" id="pnlAuthMode" visible="false" class="row form-group">
                                                <div class="col-lg-4">
                                                    <asp:Label runat="server" Font-Bold="true" ID="lblrblAuthType" Text="Authorization Mode" />
                                                </div>
                                                <div class="col-lg-8">
                                                    <asp:RadioButtonList ID="rblAuthType" runat="server" DataTextField="AuthModeText" DataValueField="AuthModeVal" RepeatLayout="Flow"  RepeatDirection="Vertical" />
                                                    <asp:RequiredFieldValidator ID="rfvrblAuthType" ValidationGroup="ValBankDetails" Display="Dynamic" runat="server" ControlToValidate="rblAuthType" ErrorMessage="Select authorization mode" CssClass="text-danger" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID = "BankDD" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID = "rblMandateModes" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="modal-footer">
                            <asp:HiddenField ID="hdfTxnToken" runat="server" />
                            <asp:HiddenField ID="hdfSubsID" runat="server" />
                            <asp:HiddenField ID="hdfOrderID" runat="server" />
                            <asp:HiddenField ID="hdfRegData" runat="server" />
                            <asp:Button ID="btnAuthSubs" runat="server" Text="Authenticate Subscription" CssClass="btn btn-primary" ValidationGroup="ValBankDetails" OnClick="btnAuthSubs_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" />
                        </div>
                    </div>
                </div>
    </asp:Panel>
    <script type="text/javascript">
        function ClearAccountDetails() {
            var TxtAccountNo = document.getElementById('<%=txtAccountNo.ClientID %>');
            var TxtIFSCCode = document.getElementById('<%=txtIFSCCode.ClientID %>');
            var TxtUserName = document.getElementById('<%=txtUserName.ClientID %>');
            var DrpAccountType = document.getElementById('<%=drpAccType.ClientID %>');
            var DrpBanksList = document.getElementById('<%=BankDD.ClientID %>');

            TxtAccountNo.value = "";
            TxtAccountNo.disabled = true;
            TxtIFSCCode.disabled = true;
            TxtUserName.disabled = true;
            TxtIFSCCode.value = "";
            TxtUserName.value = "";
            DrpAccountType.selectedIndex = 0;
            DrpBanksList.selectedIndex = 0;
            DrpAccountType.disabled = true;
            DrpBanksList.disabled = true;
            $("span[id$=<%=rblMandateModes.ClientID %>] input:radio:checked").removeAttr("checked")

            var RblAuthModes = document.getElementById('<%=pnlAuthMode.ClientID %>');

            if (RblAuthModes != null)
                RblAuthModes.style.visibility = "hidden";
        }

        document.getElementById('<%=btnCancel.ClientID %>').onclick = ClearAccountDetails();
    </script>
</asp:Content>
