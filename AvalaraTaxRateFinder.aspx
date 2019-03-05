<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AvalaraTaxRateFinder.aspx.cs" Inherits="AvalaraTaxRateFinder" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <style type="text/css">
        .submitbutton {
            background-color: #3333cc;
            border: none;
            color: white;
            padding-left: 20px;
            padding-right: 20px;
            padding-top: 1px;
            padding-bottom: 1px;
            text-align: center;
            vertical-align: middle;
            text-decoration: none;
            display: inline-block;
            font-size: 14px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 0px;
            margin-bottom: 0px;
            cursor: pointer;
            box-shadow: 2px 2px 5px #888;
            -webkit-transition-duration: 0.4s; /* Safari */
            transition-duration: 0.4s;
            border: 1px solid black;
        }

        .submitbutton:hover {
            background-color: #4CAF50;
            color: white;
        }

        .txtbox {
            width: 100px;
            height: 15px;
        }

        .side-by-side {
            float: left;
            padding: 0px 5px;
        }
        #taxresults {
            height: 208px;
            width: 370px;
        }
        #taxresults0 {
            height: 228px;
            width: 647px;
        }
    </style>
</head>

<body>

    <div style="margin-left: 50px;">
        <form id="form1" runat="server">
            <fieldset class="side-by-side" style="width: 370px; height: 248px">
                <legend>Tax Rate Finder:</legend>
                    <br />
                <div class="side-by-side">Zip Code:</div>
                <div class="side-by-side">
                    <asp:TextBox ID="tbx_Zipcode" runat="server" class="txtbox" required = "True"></asp:TextBox>
                </div>
                <div class="side-by-side">
                    <asp:Button ID="btn_FindRate" runat="server" OnClick="Btn_FindRate_Click" Text="Find Rate" class="submitbutton" />
                </div>
                    <br />
                    <br />
                <div class="side-by-side">
                    <asp:Label ID="lbl_BadZipcode" runat="server" Text="That Was Not A Valid Zipcode. Please Try Again" ForeColor="Red"></asp:Label>
                </div>
                    <br />
            </fieldset>
            <fieldset class="side-by-side" style="width: 370px; height: 248px">
                <legend>Results:</legend>
                <div class="side-by-side" runat="server" id="taxresults">
                    <asp:Label ID="lbl_zipcode" runat="server" Text=""></asp:Label>
                        <br />
                    <asp:Label ID="lbl_Country" runat="server" Text=""></asp:Label>
                        <br />
                    <asp:Label ID="lbl_State" runat="server" Text=""></asp:Label>
                         <br />
                    <asp:Label ID="lbl_County" runat="server" Text=""></asp:Label>
                         <br />
                    <asp:Label ID="lbl_City" runat="server" Text=""></asp:Label>

                    <asp:Label ID="lbl_TaxGroupCode" runat="server" Text=""></asp:Label>
                         <br />
                    <asp:Label ID="lbl_TaxRate" runat="server" Text=""></asp:Label>
                         <br />
                </div>
            </fieldset>
        </form>
    </div>
</body>
</html>
