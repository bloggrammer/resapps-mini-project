using NHibernate;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.App.Data
{
    public class Repository : IRepository
    {

        public Repository(ISession session) => _session = session;
        public void Add(AnalysisResult obj) => _session.SaveOrUpdate(obj);

        public IList<AnalysisResult> GetAll() => _session.Query<AnalysisResult>().ToList();

        public bool CommitToDatabase()
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    if (transaction.IsActive)
                    {
                        _session.Flush();
                        transaction.Commit();
                        _session.Flush();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    IOService.LogDB(ex.Message);
                    transaction.Rollback();
                    return false;
                }
            }

        }
        public void Rollback(ITransaction transaction)
        {
            try
            {
                if (transaction.IsActive)
                    transaction.Rollback();
            }
            finally
            {
                _session.Dispose();
            }
        }

        protected ISession _session;
    }
}
