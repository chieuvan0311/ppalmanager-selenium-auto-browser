using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYPAL.RandomData
{
    public class Get_Random_Password
    {
        public string Get_One_Random_Password()
        {
            int random_0_95 = new Random().Next(0, 95);
            string firstWord = new Upper_First_Letter_Words_96().Get_96_First_Words_List()[random_0_95];
            //Second word without upper letter
            int random_0_94 = new Random().Next(0, 94);
            string secondWord = new Second_Words_95().Get_95_Second_Words_List()[random_0_94];
            //get date in month
            string date_1_30 = new Random().Next(1, 30).ToString();
            //get month
            string month_1_12 = new Random().Next(1, 12).ToString();
            //get special characters
            int random_0_14 = new Random().Next(0, 14);
            string specialCharacter = new Special_Characters_15().Get_15_SpecialCharacters_List()[random_0_14];

            return firstWord + secondWord + date_1_30 + month_1_12 + specialCharacter;
        }
    }
}
