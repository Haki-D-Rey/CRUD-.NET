using System.Data.SqlClient;

namespace SCELL.Model
{
    public class DBConection
    {
        private static SqlConnectionStringBuilder sqlConnectionSB = new SqlConnectionStringBuilder();
        public string GetConnectionString() {

            sqlConnectionSB.DataSource = "DESKTOP-T1IOV7M\\SQLEXPRESS";
            sqlConnectionSB.InitialCatalog = "SCELLBD";
            sqlConnectionSB.UserID = "sa";
            sqlConnectionSB.Password = "Admin12345";
            sqlConnectionSB.ConnectRetryCount = 2;
            sqlConnectionSB.ConnectRetryInterval = 10;
            sqlConnectionSB.IntegratedSecurity = false;
            sqlConnectionSB.Encrypt = false;
            sqlConnectionSB.ConnectTimeout = 30;

            return sqlConnectionSB.ToString(); ;
        }

    }
}
