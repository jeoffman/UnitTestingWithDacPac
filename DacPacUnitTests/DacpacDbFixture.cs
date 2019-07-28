using MartinCostello.SqlLocalDb;
using Microsoft.SqlServer.Dac;
using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DacPacUnitTests
{
    public class DacpacDbFixture : IDisposable
    {
        public static int NumberOfTimesDacpacWasApplied { get; set; } = 0;

        ISqlLocalDbInstanceInfo _instance;
        ISqlLocalDbInstanceManager _manager;
        SqlLocalDbApi _localDb;

        public string ConnectionString { get; set; }
        public string EfConnectionString { get; set; }
        public string LocalDbInstanceName { get; set; }

        public const string DatabaseUserName = "User";
        public const string DatabasePassword = "Password123456";

        public DacpacDbFixture()
        {
            Console.WriteLine("Running fixture constructor...");

            _localDb = new SqlLocalDbApi();
            DateTime nowUtc = DateTime.UtcNow;
            LocalDbInstanceName = $"{nowUtc.ToString("yyyyMMddHHmmssFFFFFFF")}";    //something mostly unique
            _instance = _localDb.GetOrCreateInstance(LocalDbInstanceName);
            _manager = _instance.Manage();

            if (!_instance.IsRunning)
                _manager.Start();

            var packagePath = "SimpleDb.dacpac";

            var deployOptions = new DacDeployOptions
            {
                CreateNewDatabase = true,
                GenerateSmartDefaults = true,
            };
            deployOptions.SqlCommandVariableValues["LoginName"] = DatabaseUserName;
            deployOptions.SqlCommandVariableValues["Password"] = DatabasePassword;

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = _manager.NamedPipe;
            csb.IntegratedSecurity = true;
            var databaseName = "SimpleDbUnitTest";
            var debugConnectionString = csb.ConnectionString;
            var dacServices = new DacServices(debugConnectionString);

            using (var package = DacPackage.Load(packagePath))
            {
                dacServices.Deploy(package, databaseName, true, deployOptions);
            }
            csb.InitialCatalog = databaseName;
            //csb.UserID = DatabaseUserName;
            //csb.Password = DatabasePassword;
            //csb.IntegratedSecurity = false;
            ConnectionString = csb.ConnectionString;

            EntityConnectionStringBuilder ecsb = new EntityConnectionStringBuilder();
			string nameOfConnectionString = "SimpleDbModel";  //NOTE: HACK: this must match the name of my Entity Framework model (the .edmx guy)
			string providerName = "System.Data.SqlClient";
            ecsb.Provider = providerName;
            ecsb.ProviderConnectionString = csb.ConnectionString;
            ecsb.Metadata = $"res://*/{nameOfConnectionString}.csdl|res://*/{nameOfConnectionString}.ssdl|res://*/{nameOfConnectionString}.msl";
            EfConnectionString = ecsb.ConnectionString;

            NumberOfTimesDacpacWasApplied++;
            Debug.WriteLine($">> The DACPAC has been applied {NumberOfTimesDacpacWasApplied} times");
            Console.WriteLine($">> The DACPAC has been applied {NumberOfTimesDacpacWasApplied} times");
        }

        public void Dispose()
        {
            _manager?.Stop();
            _localDb.DeleteInstance(LocalDbInstanceName);
            _localDb?.Dispose();
        }
    }
}
