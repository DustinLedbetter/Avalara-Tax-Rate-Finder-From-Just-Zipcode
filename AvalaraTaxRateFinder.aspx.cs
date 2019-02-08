/***************************************************************************************************************************************************
*                                                 GOD First                                                                                        *
* Author: Dustin Ledbetter                                                                                                                         *
* Release Date: 1-15-2019                                                                                                                          *
* Last Edited:  1-16-2019                                                                                                                          *
* Version: 1.0                                                                                                                                     *
* Purpose: This is a web app that allows a user to give a zipcode and get the returned tax rate for that zipcode                                   *
***************************************************************************************************************************************************/

/*
    USING Avalara.AvaTax.RestClient (These are added From NuGet Package Management)
    1. Avalara.AvaTax.RestClient.net45.dll
    2. Newtonsoft.Json.9.0.1
*/

using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Avalara.AvaTax.RestClient;

public partial class AvalaraTaxRateFinder : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        // Hide the results field till the user clicks the find rates button
        taxresults.Visible = false;

        // Setup options for when a bad zipcode is entered
        lbl_BadZipcode.Visible = false;
        tbx_Zipcode.BorderColor = Color.Empty;
        tbx_Zipcode.BorderStyle = BorderStyle.NotSet;
    }

    protected void Btn_FindRate_Click(object sender, EventArgs e)
    {

        // Place in a try catch bloack incase the number passed is valid, but it is not a valid zipcode
        try
        {


            // clear all labels out




            // Test if user specified zipcode is valid or not
            if (IsValidZipCode(tbx_Zipcode.Text) == true)
            {

                // Used to see if we are sending a US zipcode or a Canadian zipcode
                bool isLetter = !string.IsNullOrEmpty(tbx_Zipcode.Text) && char.IsLetter(tbx_Zipcode.Text[0]);

                // Variables sent in Avalara call
                var zipcode = tbx_Zipcode.Text;
                decimal totalAmountDecimal = 100.00m;

                // Create client to be setup with user defined environment variable
                var client = new AvaTaxClient("", "", null, null);

                // Set the Avalara environment to be on Sandbox
                client = new AvaTaxClient("AvalaraTaxExtension", "1.0", Environment.MachineName, AvaTaxEnvironment.Sandbox)
                    .WithSecurity($"nulled for security", $"nulled for security");

                // This creates the transaction that reaches out to alavara and gets the amount of tax for the user based on info we send
                var transaction = new TransactionBuilder(client, $"nulled for security", DocumentType.SalesOrder, $"nulled for security")

                    // Pass the zipcode user passed from form 
                    .WithAddress(TransactionAddressType.SingleLocation, null, null, null, null, null, zipcode, null)

                    // Pass the amount of money to calculate tax on (This amount doesn't matter here as it is just used to get the RATE)
                    // (This value is not used after this for rate, but we must pass something to Avalara)
                    .WithLine(totalAmountDecimal)

                    // Run transaction
                    .Create();


                //--------------------------------------------
                // Retrieve more values for county, state, etc
                //--------------------------------------------


                // First we need to get access to the Transaction JSON data
                var js = new JavaScriptSerializer();
                var d = js.Deserialize<dynamic>(transaction.ToString());

                // Check if we have a canadian zipcode
                if (isLetter == true)
                {
                    // Get the data for each field we want to add to the labels

                    var county = "<b>Country: </b>" + d["lines"][0]["details"][0]["jurisName"];
                    var stateAssignedNo = "<b>Tax Code: </b>" + d["lines"][0]["details"][0]["taxAuthorityTypeId"];

                    // Set our labels to the values we retrieved
                    //Label1.Text = transaction.ToString();      // Transaction.ToString() displays the entire contents of the JSON file returned from avalara
                    lbl_zipcode.Text = "<b>Zipcode: </b>" + tbx_Zipcode.Text;
                    lbl_Country.Text = county;
                    lbl_State.Text = stateAssignedNo;
                    lbl_County.Text = "<b>Total Tax: </b>" + Convert.ToString(d["totalTax"]);
                    // Show the results to the user
                    taxresults.Visible = true;
                }
                // else assume it is US
                else
                {
                    // Get the data for each field we want to add to the labels
                    var country = "<b>Country: </b>" + d["lines"][0]["details"][0]["country"];
                    var state = "<b>State: </b>" + d["lines"][0]["details"][0]["jurisName"];
                    var county = "<b>County: </b>" + d["lines"][0]["details"][1]["jurisName"];
                    var stateAssignedNo = "<b>Tax Code: </b>" + d["lines"][0]["details"][1]["stateAssignedNo"];

                    // Set our labels to the values we retrieved
                    //Label1.Text = transaction.ToString();      // Transaction.ToString() displays the entire contents of the JSON file returned from avalara
                    lbl_zipcode.Text = "<b>Zipcode: </b>" + tbx_Zipcode.Text;
                    lbl_Country.Text = country;
                    lbl_State.Text = state;
                    lbl_County.Text = county;
                    lbl_TaxGroupCode.Text = stateAssignedNo;

                    // Show the results to the user
                    lbl_TaxRate.Text = Convert.ToString(d["totalTax"]);
                    taxresults.Visible = true;
                    lbl_County.Visible = true;
                    lbl_TaxGroupCode.Visible = true;
                }
            }
            else
            {
                // Alert user that the zipcode entered was not valid
                tbx_Zipcode.BorderColor = Color.Red;
                tbx_Zipcode.BorderStyle = BorderStyle.Solid;
                lbl_BadZipcode.Visible = true;
            }
        }
        catch
        {
            // Alert user that the zipcode entered was not valid
            taxresults.Visible = false;
            tbx_Zipcode.BorderColor = Color.Red;
            tbx_Zipcode.BorderStyle = BorderStyle.Solid;
            lbl_BadZipcode.Visible = true;
            lbl_County.Visible = false;
            lbl_TaxGroupCode.Visible = false;
        }
    }

    // Used to test if user specified zipcode is valid or not
    private bool IsValidZipCode(string zipCode)
    {
        // Our regex for zipcodes
        var _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
        var _caZipRegEx = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";


        // Check the user given zipcode against the regex to see if valid or not
        var validZipCode = true;
        if ((!Regex.Match(zipCode, _usZipRegEx).Success) && (!Regex.Match(zipCode, _caZipRegEx).Success))
        {
            validZipCode = false;
        }

        return validZipCode;
    }

    //end of the class: AvalaraTaxRateFinder
    //end of the file
}