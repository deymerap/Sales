namespace Sales.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Text;

    public static class RegexHelper
    {
        public static bool IsValidEmailAddr(string pvStrEmail)
        {
            try
            {
                var vStrEmail =new MailAddress(pvStrEmail);
                return true;
            }
            catch(FormatException)
            {
                return false;
            }
        }
    }
}
