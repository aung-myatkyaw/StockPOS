namespace StockPOS.Util
{
    public static class GeneralUtility
    {

        public static string ConvertSystemDateStringFormat(string aDate)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(aDate))
                {
                    if (aDate.Length == 8 && int.TryParse(aDate, out _))
                    {
                        return aDate;
                    }

                    if (aDate.Length != 10)
                    {
                        throw new Exception("Date format is incorrect");
                    }

                    if (aDate.Contains('/') || aDate.Contains('.'))
                    {
                        string[] l_strDate = aDate.Contains('/') ? aDate.Split('/') : aDate.Split('.');
                        int l_TryPass1 = int.Parse(l_strDate[2]);
                        int l_TryPass2 = int.Parse(l_strDate[0]);
                        int l_TryPass3 = int.Parse(l_strDate[1]);

                        string l_Date = l_strDate[2] + l_strDate[1] + l_strDate[0];
                        return l_Date;
                    }
                    else
                    {
                        return aDate;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        // ... The rest of the methods

        public static DateTime? ConvertDateFormat(string aDateString)
        {
            try
            {
                if (string.IsNullOrEmpty(aDateString))
                {
                    return null;
                }

                int l_year = Convert.ToInt32(aDateString.Substring(0, 4));
                int l_month = Convert.ToInt32(aDateString.Substring(4, 2));
                int l_day = Convert.ToInt32(aDateString.Substring(6, 2));

                DateTime l_date = new DateTime(l_year, l_month, l_day);
                return l_date.Date;
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ConvertDateFormatNew(string aDateString)
        {
            try
            {
                if (string.IsNullOrEmpty(aDateString))
                {
                    return null;
                }

                int l_year = Convert.ToInt32(aDateString.Substring(6, 4));
                int l_month = Convert.ToInt32(aDateString.Substring(3, 2));
                int l_day = Convert.ToInt32(aDateString.Substring(0, 2));

                DateTime l_date = new DateTime(l_year, l_month, l_day);
                return l_date.Date;
            }
            catch
            {
                return null;
            }
        }

        public static string ConvertSystemTimeFormat(DateTime aDate)
        {
            string l_Time = "";
            l_Time += aDate.Hour.ToString("00");
            l_Time += aDate.Minute.ToString("00");
            l_Time += aDate.Second.ToString("00");
            return l_Time;
        }

        public static string ServerDate => ConvertSystemDateStringFormat(DateTime.Now.ToString());

        public static string ServerTime => ConvertSystemTimeFormat(DateTime.Now);

        public static bool CheckDateFormat(string Datestr)
        {
            if (!DateTime.TryParseExact(Datestr, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                return false;
            }
            return true;
        }

        public static string Now_DateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");
            }
        }

        public static string Now_Date
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        public static string GeneratedKey
        {
            get
            {
                System.Threading.Thread.Sleep(100);
                DateTime l_CurrentDate = DateTime.Now;
                return ConvertSystemDateStringFormat(l_CurrentDate.ToString("dd/MM/yyyy")) +
                    ConvertSystemTimeFormat(l_CurrentDate) +
                    l_CurrentDate.Millisecond.ToString("0000");
            }
        }
        public static string GenerateGuid
        {
            get
            {

                return Guid.NewGuid().ToString();

            }
        }
        public static string GenerateGuid_WithDate
        {
            get
            {

                return $"{DateTime.Now.ToString("ddMMyyyyHHmm").ToString()}-" + Guid.NewGuid().ToString();

            }
        }

        public static class RegEx
        {
            public const string DMY = @"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";
        }
    }
}
