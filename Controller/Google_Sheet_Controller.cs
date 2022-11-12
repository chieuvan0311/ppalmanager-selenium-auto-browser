using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using PAYPAL.DataConnection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading;

namespace PAYPAL.Controller
{
    public class Google_Sheet_Controller
    {
        string driver_link_id = null;
        string data_sheet = null;
        string save_accounts_sheet = null;
        string inuput_paypal_sheet = null;
        string update_proxy_sheet = null;

        public IList<IList<Object>> Get_New_Accounts_List()
        {
            using (PaypalDbContext db = new PaypalDbContext())
            {
                driver_link_id = db.Admins.Where(x => x.Name == "Google_Sheet_ID").FirstOrDefault().Value;
                inuput_paypal_sheet = db.Admins.Where(x => x.Name == "Input_Accounts_Sheet_Name").FirstOrDefault().Value;
            }
            SheetsService service = Authorize_GoogleApp_For_SheetsService();
            String range = inuput_paypal_sheet + "!A4:I";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(driver_link_id, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> getValues = response.Values;
            return getValues;
        }

        public IList<IList<Object>> Get_Proxy_List()
        {
            using (PaypalDbContext db = new PaypalDbContext())
            {
                driver_link_id = db.Admins.Where(x => x.Name == "Google_Sheet_ID").FirstOrDefault().Value;
                update_proxy_sheet = db.Admins.Where(x => x.Name == "Update_Proxy_Sheet_Name").FirstOrDefault().Value;
            }
            SheetsService service = Authorize_GoogleApp_For_SheetsService();
            String range = update_proxy_sheet + "!A4:A";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(driver_link_id, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> getValues = response.Values;
            return getValues;
        }

        public void Update_Database ()
        {
            List<Account> accounts = null;
            using (PaypalDbContext db = new PaypalDbContext())
            {
                driver_link_id = db.Admins.Where(x => x.Name == "Google_Sheet_ID").FirstOrDefault().Value;
                data_sheet = db.Admins.Where(x => x.Name == "Database_Sheet_Name").FirstOrDefault().Value;
                accounts = db.Accounts.ToList();
            }
            // SpreadhSheetID of above created document.  

            var service = Authorize_GoogleApp_For_SheetsService();
            string google_sheet_range = data_sheet + "!A2:AH";
            IList<IList<Object>> objNeRecords = Database_Accounts_Transfer(accounts);


            int database_list_row_count = accounts.Count;

            int google_sheet_row_total = Get_Total_GoogleSheet_Row(driver_link_id, google_sheet_range);
            if (google_sheet_row_total > database_list_row_count) //Clear phần data dư thừa
            {
                string row_index = (database_list_row_count + 2).ToString();
                string clear_from_row = data_sheet + "!A" + row_index + ":AH";
                int total_clear_row = google_sheet_row_total - database_list_row_count;
                IList<IList<Object>> empty_model = Get_GoogleSheet_Empty_Model(total_clear_row);
                Clear_Sheet_Range_InBatch(empty_model, driver_link_id, clear_from_row, service);
            }
            Update_GoogleSheet_InBatch(objNeRecords, driver_link_id, google_sheet_range, service);
        }

        public void Update_Del_Account ()
        {
            List<Del_Account> del_accounts = null;
            PaypalDbContext db = new PaypalDbContext();
            driver_link_id = db.Admins.Where(x => x.Name == "Google_Sheet_ID").FirstOrDefault().Value;
            save_accounts_sheet = db.Admins.Where(x => x.Name == "Del_Accounts_Sheet_Name").FirstOrDefault().Value;
            del_accounts = db.Del_Account.ToList();

            var service = Authorize_GoogleApp_For_SheetsService();
            string google_sheet_range = save_accounts_sheet + "!A2:AH";
            IList<IList<Object>> objNeRecords = Del_Accounts_Transfer(del_accounts);

            int sheet_data_row = Get_Total_GoogleSheet_Row(driver_link_id, google_sheet_range);

            if (sheet_data_row > del_accounts.Count) //Clear phần data dư thừa
            {
                string row_index = (sheet_data_row + 2).ToString();
                string clear_from_row = save_accounts_sheet + "!A" + row_index + ":AH";
                int total_clear_row = sheet_data_row - del_accounts.Count;
                IList<IList<Object>> empty_model = Get_GoogleSheet_Empty_Model(total_clear_row);
                Clear_Sheet_Range_InBatch(empty_model, driver_link_id, clear_from_row, service);
            }
            Update_GoogleSheet_InBatch(objNeRecords, driver_link_id, google_sheet_range, service);
        }


        public void Save_Del_Accounts (List<Account> accounts)
        {
            using (PaypalDbContext db = new PaypalDbContext())
            {
                driver_link_id = db.Admins.Where(x => x.Name == "Google_Sheet_ID").FirstOrDefault().Value;
                save_accounts_sheet = db.Admins.Where(x => x.Name == "Del_Accounts_Sheet_Name").FirstOrDefault().Value;
            }

            var service = Authorize_GoogleApp_For_SheetsService();
            string range = save_accounts_sheet + "!A2:AH";
            int total_row = Get_Total_GoogleSheet_Row(driver_link_id, range);
            string row_index = (total_row + 2).ToString();
            string add_from_row = save_accounts_sheet + "!A" + row_index + ":AH";
            IList<IList<Object>> objNeRecords = Database_Accounts_Transfer(accounts);
            Update_GoogleSheet_InBatch(objNeRecords, driver_link_id, add_from_row, service);
        }

        private static SheetsService Authorize_GoogleApp_For_SheetsService()
        {
            // If modifying these scopes, delete your previously saved credentials  
            // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json  
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            string ApplicationName = "Google Sheets API .NET Quickstart";
            UserCredential credential;
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("MyAppsToken")).Result;

            }

            // Create Google Sheets API service.  
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        private static int Get_Total_GoogleSheet_Row(string spreadsheetId, string range)
        {
            int totalRow = 0;
            SheetsService service = Authorize_GoogleApp_For_SheetsService();
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> getValues = response.Values;
            if (getValues != null)
            {
                totalRow = getValues.Count;
            }
            return totalRow;
        }

        private static IList<IList<Object>> Database_Accounts_Transfer (List<Account> accounts)
        {
            List<IList<Object>> row_list_data = new List<IList<Object>>();

            for (int i = 0; i < accounts.Count; i++)
            {
                IList<Object> row_data = new List<Object>();

                row_data.Add(i + 1); //Index 1
                if (!string.IsNullOrEmpty(accounts[i].Email)) { row_data.Add(accounts[i].Email); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Balance)) { row_data.Add(accounts[i].Balance); } else { row_data.Add(""); }   
                if (!string.IsNullOrEmpty(accounts[i].TransactionTotal)) { row_data.Add(accounts[i].TransactionTotal); } else { row_data.Add(""); }
                if (accounts[i].Profile_Created_Time != null) { row_data.Add(accounts[i].Profile_Created_Time.ToString()); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Notification)) { row_data.Add(accounts[i].Notification); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].UpdatedDateTime)) { row_data.Add(accounts[i].UpdatedDateTime); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].AccPassword)) { row_data.Add(accounts[i].AccPassword); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].TwoFA)) { row_data.Add(accounts[i].TwoFA); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].EmailPassword)) { row_data.Add(accounts[i].EmailPassword); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Email_2FA)) { row_data.Add(accounts[i].Email_2FA); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Proxy)) { row_data.Add(accounts[i].Proxy); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].AccName)) { row_data.Add(accounts[i].AccName); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].AccBirthDay)) { row_data.Add(accounts[i].AccBirthDay); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Address)) { row_data.Add(accounts[i].Address); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Phone)) { row_data.Add(accounts[i].Phone); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].BankCard)) { row_data.Add(accounts[i].BankCard); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].SeQuestion1)) { row_data.Add(accounts[i].SeQuestion1); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].SeQuestion2)) { row_data.Add(accounts[i].SeQuestion2); } else { row_data.Add(""); }
                if (accounts[i].Acc_ON_OFF == true) { row_data.Add("ON"); } else { row_data.Add("OFF"); }

                if (!string.IsNullOrEmpty(accounts[i].AccType)) { row_data.Add(accounts[i].AccType); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].RecoveryEmail)) { row_data.Add(accounts[i].RecoveryEmail); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].ForwordToEmail)) { row_data.Add(accounts[i].ForwordToEmail); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].SecondEmail)) { row_data.Add(accounts[i].SecondEmail); } else { row_data.Add(""); }
                if (accounts[i].Profile_Save == true) { row_data.Add("YES"); } else { row_data.Add("NO"); }

                if (!string.IsNullOrEmpty(accounts[i].AccPassword_Old)) { row_data.Add(accounts[i].AccPassword_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].EmailPassword_Old)) { row_data.Add(accounts[i].EmailPassword_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Acc_2FA_Old)) { row_data.Add(accounts[i].Acc_2FA_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Email_2FA_Old)) { row_data.Add(accounts[i].Email_2FA_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].RecoveryEmail_Old)) { row_data.Add(accounts[i].RecoveryEmail_Old); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Questions_Old)) { row_data.Add(accounts[i].Questions_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Random_AccPassword)) { row_data.Add(accounts[i].Random_AccPassword); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Random_EmailPassword)) { row_data.Add(accounts[i].Random_EmailPassword); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Random_Questions)) { row_data.Add(accounts[i].Random_Questions); } else { row_data.Add(""); }

                row_list_data.Add(row_data);
            }
            return row_list_data;
        }

        private static IList<IList<Object>> Del_Accounts_Transfer(List<Del_Account> accounts)
        {
            List<IList<Object>> row_list_data = new List<IList<Object>>();

            for (int i = 0; i < accounts.Count; i++)
            {
                IList<Object> row_data = new List<Object>();

                row_data.Add(i + 1); //Index 1
                if (!string.IsNullOrEmpty(accounts[i].Email)) { row_data.Add(accounts[i].Email); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Balance)) { row_data.Add(accounts[i].Balance); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].TransactionTotal)) { row_data.Add(accounts[i].TransactionTotal); } else { row_data.Add(""); }
                if (accounts[i].Profile_Created_Time != null) { row_data.Add(accounts[i].Profile_Created_Time.ToString()); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Notification)) { row_data.Add(accounts[i].Notification); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].UpdatedDateTime)) { row_data.Add(accounts[i].UpdatedDateTime); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].AccPassword)) { row_data.Add(accounts[i].AccPassword); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].TwoFA)) { row_data.Add(accounts[i].TwoFA); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].EmailPassword)) { row_data.Add(accounts[i].EmailPassword); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Email_2FA)) { row_data.Add(accounts[i].Email_2FA); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Proxy)) { row_data.Add(accounts[i].Proxy); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].AccName)) { row_data.Add(accounts[i].AccName); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].AccBirthDay)) { row_data.Add(accounts[i].AccBirthDay); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Address)) { row_data.Add(accounts[i].Address); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Phone)) { row_data.Add(accounts[i].Phone); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].BankCard)) { row_data.Add(accounts[i].BankCard); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].SeQuestion1)) { row_data.Add(accounts[i].SeQuestion1); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].SeQuestion2)) { row_data.Add(accounts[i].SeQuestion2); } else { row_data.Add(""); }
                if (accounts[i].Acc_ON_OFF == true) { row_data.Add("ON"); } else { row_data.Add("OFF"); }

                if (!string.IsNullOrEmpty(accounts[i].AccType)) { row_data.Add(accounts[i].AccType); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].RecoveryEmail)) { row_data.Add(accounts[i].RecoveryEmail); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].ForwordToEmail)) { row_data.Add(accounts[i].ForwordToEmail); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].SecondEmail)) { row_data.Add(accounts[i].SecondEmail); } else { row_data.Add(""); }
                if (accounts[i].Profile_Save == true) { row_data.Add("YES"); } else { row_data.Add("NO"); }

                if (!string.IsNullOrEmpty(accounts[i].AccPassword_Old)) { row_data.Add(accounts[i].AccPassword_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].EmailPassword_Old)) { row_data.Add(accounts[i].EmailPassword_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Acc_2FA_Old)) { row_data.Add(accounts[i].Acc_2FA_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Email_2FA_Old)) { row_data.Add(accounts[i].Email_2FA_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].RecoveryEmail_Old)) { row_data.Add(accounts[i].RecoveryEmail_Old); } else { row_data.Add(""); }

                if (!string.IsNullOrEmpty(accounts[i].Questions_Old)) { row_data.Add(accounts[i].Questions_Old); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Random_AccPassword)) { row_data.Add(accounts[i].Random_AccPassword); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Random_EmailPassword)) { row_data.Add(accounts[i].Random_EmailPassword); } else { row_data.Add(""); }
                if (!string.IsNullOrEmpty(accounts[i].Random_Questions)) { row_data.Add(accounts[i].Random_Questions); } else { row_data.Add(""); }

                row_list_data.Add(row_data);
            }
            return row_list_data;
        }

        private static IList<IList<Object>> Get_GoogleSheet_Empty_Model(int row_total)
        {
            List<IList<Object>> empty_row_list = new List<IList<Object>>();
            string empty_cell = "";
            for (int i = 1; i <= row_total; i++)
            {
                IList<Object> empty_row = new List<Object>();
                for (int col = 1; col <= 34; col++)
                {
                    empty_row.Add(empty_cell);
                }
                empty_row_list.Add(empty_row);
            }
            return empty_row_list;
        }

        private static void Update_GoogleSheet_InBatch(IList<IList<Object>> values, string spreadsheetId, string from_row, SheetsService service)
        {
            SpreadsheetsResource.ValuesResource.UpdateRequest request =
               service.Spreadsheets.Values.Update(new ValueRange() { Values = values }, spreadsheetId, from_row);

            //request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOption.INSERTROWS;
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            var response = request.Execute();
        }

        private static void Clear_Sheet_Range_InBatch(IList<IList<Object>> values, string spreadsheetId, string clear_from_row, SheetsService service)
        {
            SpreadsheetsResource.ValuesResource.UpdateRequest request =
               service.Spreadsheets.Values.Update(new ValueRange() { Values = values }, spreadsheetId, clear_from_row);

            //request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOption.INSERTROWS;
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            var response = request.Execute();
        }
    }
}
