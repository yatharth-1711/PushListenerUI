
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTest.FrameWork
{
    public static class CommonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static string signOnErrors = string.Empty;

        /// <summary>
        /// This will check if required validation will pass
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsRequiredValidationPass(string str)
        {
            return string.IsNullOrEmpty(str) ? false : true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool ISEmailFormateProper(string email)
        {
            System.Text.RegularExpressions.Regex expr = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            return expr.IsMatch(email) ? true : false;
        }

        /// <summary>
        /// This will check if Length validation will pass
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLengthValid(string str)
        {
            return str.Length > 30 ? false : true;
        }


        /// <summary>
        /// This will check if Password validation will pass
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPasswordValid(string str)
        {

            return (str.Length >= 6 && str.Length <= 30) && (isAlphaNumeric(str)) ? true : false;
        }

        /// <summary>
        /// If any Alpha Numeric
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static Boolean isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,30}$");
            return rg.IsMatch(strToCheck);
        }

        /// <summary>
        /// This will convrt the List to String 
        /// You can give \n, . any line braker 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertListToString(List<string> listStr, string lineBraker)
        {
            string combindedString = string.Join(lineBraker, listStr.ToArray());
            return combindedString;
        }

        /// <summary>
        /// This will return Admin Status as per the Role ID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>


        /// <summary>
        /// Forget Password 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetRandomPassword()
        {
            return "Admin@123";
        }

        /// <summary>
        /// Forget Password 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// This will check the object properties on by and give list of non similar properties
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static List<string> GetChangedProperties(Object A, Object B)
        {
            if (A.GetType() != B.GetType())
            {
                throw new System.InvalidOperationException("Objects of different Type");
            }
            List<string> changedProperties = ElaborateChangedProperties(A.GetType().GetProperties(), B.GetType().GetProperties(), A, B);
            return changedProperties;
        }

        /// <summary>
        /// This will check the object properties on by and give list of non similar properties
        /// </summary>
        /// <param name="pA"></param>
        /// <param name="pB"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static List<string> ElaborateChangedProperties(PropertyInfo[] pA, PropertyInfo[] pB, Object A, Object B)
        {
            Boolean isADateTimeType = false;
            Boolean isBDateTimeType = false;
            List<string> changedProperties = new List<string>();
            foreach (PropertyInfo info in pA)
            {
                object propValueA = info.GetValue(A, null);
                isADateTimeType = (propValueA != null && propValueA.GetType() == typeof(DateTime)) ? true : false;
                object propValueB = info.GetValue(B, null);
                isBDateTimeType = (propValueB != null && propValueB.GetType() == typeof(DateTime)) ? true : false;
                if (!object.Equals(propValueA, propValueB))
                {
                    if (!(isADateTimeType || isBDateTimeType))
                        changedProperties.Add(info.Name);
                }
            }
            return changedProperties;
        }
        public static void DisplayDLMSSignONError()
        {
            MessageBox.Show($"{CommonHelper.signOnErrors} There is some problem in Sign ON DLMS.", "DLMS Sign ON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void DisplayErrorMessage(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
