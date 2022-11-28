namespace StockPOS.Settings
{
    public class MysqlDbSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public string ConnectionString 
        { 
            get
            {
                return $"server={Host};database={Database};user={User};password={Password};Convert Zero Datetime=True;TreatTinyAsBoolean=false;";
            }
        }
    }
}