using WellTestAnalysis.Models;

namespace WellTestAnalysis.App.Data
{
    public interface IRepository
    {
        void Add(AnalysisResult obj);

        IList<AnalysisResult> GetAll();
        bool CommitToDatabase();
    }
}
