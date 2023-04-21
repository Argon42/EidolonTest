using System.Threading.Tasks;
using UnityEngine;

internal class DebugAnalyticService : IAnalyticService
{
    public string ServiceName => "Debug";

    public Task<bool> TrySendEvents(string events)
    {
        Debug.Log(events);
        return Task.FromResult(true);
    }
}