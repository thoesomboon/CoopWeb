/////#define InternalTest
//#define InternalParallelRun
//#define UAT
#define production
using System.Data.EntityClient;
using System.Data.SqlClient;
using EnCryptDeCryptData;

namespace APIConnection
{
    public class APIConnection
    {
        /// <summary>
        /// Get provider to connect database server.
        /// </summary>
#if Develop
        const string server = "SERVERC44";
        const string catalog = "CoopWeb_V2";
        const string user = "telepro";
        const string data = "%&#$!\"^c_V[Vc";
        const string key = "ADAMS";

#elif InternalTest
        const string server = "SERVERC44";
        const string catalog = "CoopWeb_V2_UAT";
        const string user = "telepro";
        const string data = "%&#$!\"^c_V[Vc";
        const string key = "ADAMS";

#elif InternalParallelRun
        const string server = "SERVERC44";
        const string catalog = "CoopWeb_V2_Parallel";
        const string user = "telepro";
        const string data = "%&#$!\"^c_V[Vc";
        const string key = "ADAMS";

#elif UAT
        /*UAT*/
        const string server = "192.168.1.114";
        const string catalog = "CoopWeb_V2";
        const string user = "telepro";
        const string data = "%&#$!\"^c_V[Vc";
        const string key = "ADAMS";

#elif production
        /*Production*/
        const string server = "LAPTOP-6L4BN0S0";
        const string catalog = "CoopWeb";
        const string user = "CoopWeb";
        const string dataDeC = "Suwan210223";
        //const string data = "%&#$!\"^c_V[Vc";
        //const string key = "ADAMS";

#endif

        public static SqlConnection ApplicationServicesConnection()
        {
            EnCryptDeCrypt edC = new EnCryptDeCrypt();
            //string dataDeC = edC.DeCrypt(data, key);

            SqlConnection c = new SqlConnection
            {
                ConnectionString =
                "Data Source=" + server + ";Initial Catalog=" + catalog + ";Persist Security Info=True;Connection Timeout=18000;MultipleActiveResultSets=True;User ID=" + user + ";Password=" + dataDeC,
                FireInfoMessageEventOnUserErrors = false
            };
            return c;
        }
        public static SqlConnection SessionStringsConnection()
        {
            EnCryptDeCrypt edC = new EnCryptDeCrypt();
            //string dataDeC = edC.DeCrypt(data, key);

            SqlConnection c = new SqlConnection
            {
                ConnectionString =
                "Data Source=" + server + ";Initial Catalog=SESSIONDB;Persist Security Info=True;User ID=" + user + ";Password=" + dataDeC,
                FireInfoMessageEventOnUserErrors = false
            };
            return c;
        }
        public static string CoopWebEntitiesConnection()
        {
            EnCryptDeCrypt edC = new EnCryptDeCrypt();
            //string dataDeC = edC.DeCrypt(data, key);

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source. 
            sqlBuilder.DataSource = server;
            sqlBuilder.InitialCatalog = catalog;
            sqlBuilder.UserID = user;
            sqlBuilder.Password = dataDeC;
            //sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.ApplicationName = "EntityFramework";
            sqlBuilder.ConnectTimeout = 500000;

            // Build the SqlConnection connection string. 
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder. 
            var entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name. 
            entityBuilder.Provider = "System.Data.SqlClient";
            // Set the provider-specific connection string. 
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location. 
            entityBuilder.Metadata = @"res://*/Entities.CoopWeb.csdl|res://*/Entities.CoopWeb.ssdl|res://*/Entities.CoopWeb.msl";

            return entityBuilder.ToString();
        }
    }
}
