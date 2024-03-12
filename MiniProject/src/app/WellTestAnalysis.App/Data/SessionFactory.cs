using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using WellTestAnalysis.App.Data.Mapping;

namespace WellTestAnalysis.App.Data
{
    public class SessionFactory
    {
        public SessionFactory(string connectionString)
        {
            _sessionFactory = BuildSessionFactory_SQLite(connectionString);
        }

        public ISession GetSession() => _sessionFactory.OpenSession();
        public ISession ReOpen() => _sessionFactory.OpenSession();

        private ISessionFactory BuildSessionFactory_SQLite(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(typeof(TestInfoMap).Assembly))
                    .ExposeConfiguration(cfg => {
                        new SchemaUpdate(cfg).Execute(true, true);
                        cfg.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, "2000");
                    });
            return configuration.BuildSessionFactory();
        }

        private readonly ISessionFactory _sessionFactory;
    }
}
