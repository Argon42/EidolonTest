using System.Threading.Tasks;

public interface IAnalyticService
{
    string ServiceName { get; }
    Task<bool> TrySendEvents(string events);
}