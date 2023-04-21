using System.Threading.Tasks;

namespace Eidolon.Analytic
{
    public interface IAnalyticService
    {
        string ServiceName { get; }
        Task<bool> TrySendEvents(string events);
    }
}