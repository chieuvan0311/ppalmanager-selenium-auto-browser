using PAYPAL.DataConnection;
using PAYPAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.DataTransfer
{
    public class Account_Table_Data
    {
        public List<AccountModel> Get_Account_Table_Data (List<Account> listAcc)
        {
            List<AccountModel> listTB = new List<AccountModel>();
            for (int i = 0; i < listAcc.Count; i++)
            {
                AccountModel tb = new AccountModel();

                //Các cột thường sử dụng
                tb.STT = i + 1; //index 0
                tb.ID = listAcc[i].ID; //index 1 - luôn ẩn
                tb.Email = listAcc[i].Email; //index 2
                tb.Balance = listAcc[i].Balance; //index 3
                tb.TransactionTotal = listAcc[i].TransactionTotal; //index 4
                try { if (listAcc[i].Profile_Created_Time != null) { tb.Profile_Created_Time = listAcc[i].Profile_Created_Time.ToString(); } } catch { } //index 5
                tb.Notification = listAcc[i].Notification; //index 6           
                tb.SessionResult = listAcc[i].SessionResult; //index 7
                tb.UpdatedDateTime = listAcc[i].UpdatedDateTime; //index 8

                //Không thường xuyên quan sát
                tb.AccPassword = listAcc[i].AccPassword; //index 9
                tb.TwoFA = listAcc[i].TwoFA; //index 10
                tb.EmailPassword = listAcc[i].EmailPassword; //index 11
                tb.Proxy = listAcc[i].Proxy; //index 12
                if (listAcc[i].Profile == true) { tb.Profile = "YES"; } else { tb.Profile = "NO"; } //index 13
                tb.ProfileId = listAcc[i].ProfileId; //index 14
                tb.Email_2FA = listAcc[i].Email_2FA; //index 15
                                                             
                tb.AccName = listAcc[i].AccName; //index 16
                tb.AccBirthDay = listAcc[i].AccBirthDay; //index 17
                tb.Address = listAcc[i].Address; //index 18
                tb.Phone = listAcc[i].Phone; //index 19
                tb.BankCard = listAcc[i].BankCard; //index 20
                tb.SeQuestion1 = listAcc[i].SeQuestion1; //index 21
                tb.SeQuestion2 = listAcc[i].SeQuestion2; //index 22
                
                tb.AccType = listAcc[i].AccType; //index 23             
                tb.AccOtherType = listAcc[i].AccOtherType; //index 24
                tb.AccCategory = listAcc[i].AccCategory; //index 25
                tb.InputtedDate = listAcc[i].InputtedDate; //index 26
                tb.EmailType = listAcc[i].EmailType; //index 27
                tb.RecoveryEmail = listAcc[i].RecoveryEmail; //index 28
                tb.ForwordToEmail = listAcc[i].ForwordToEmail;  // Index 29
                tb.SecondEmail = listAcc[i].SecondEmail;// Index 30
                tb.ProxyStatus = listAcc[i].ProxyStatus;// Index 31
                if (listAcc[i].Acc_ON_OFF == true) { tb.Acc_ON_OFF = "ON"; } else { tb.Acc_ON_OFF = "OFF"; } //Index 32
                if (listAcc[i].Set_ChangedPassPP == true){ tb.Set_ChangedPassPP = "YES"; } else { tb.Set_ChangedPassPP = "NO"; } //Index 33
                if (listAcc[i].Set_ChangedPassEmail == true) { tb.Set_ChangedPassEmail = "YES"; } else { tb.Set_ChangedPassEmail = "NO"; } //Index 34
                if (listAcc[i].Set_Add_RecoveryEmail == true) { tb.Set_Add_RecoveryEmail = "YES"; } else { tb.Set_Add_RecoveryEmail = "NO"; } //Index 35
                if (listAcc[i].Set_Deleted_FwEmail == true) { tb.Set_Deleted_FwEmail = "YES"; } else { tb.Set_Deleted_FwEmail = "NO"; } //Index 36
                if (listAcc[i].Set_Add_New_FwEmail == true) { tb.Set_Add_New_FwEmail = "YES"; } else { tb.Set_Add_New_FwEmail = "NO"; } //Index 37
                if (listAcc[i].Set_2FA == true) { tb.Set_2FA = "YES"; } else { tb.Set_2FA = "NO"; }  //Index 38
                if (listAcc[i].Set_Questions == true) { tb.Set_Questions = "YES"; } else { tb.Set_Questions = "NO"; } //Index 39
                if (listAcc[i].Profile_Save == true) { tb.Profile_Save = "YES"; } else { tb.Profile_Save = "NO"; }  //Index 40
                tb.AccPassword_Old = listAcc[i].AccPassword_Old;// Index 41          
                tb.EmailPassword_Old = listAcc[i].EmailPassword_Old;// Index 42          
     
                listTB.Add(tb);
            }
            return listTB;
        }
    }
}
