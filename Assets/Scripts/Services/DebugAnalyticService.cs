using System.Threading.Tasks;
using Eidolon.Analytic.Analytic;
using UnityEngine;

namespace Eidolon.Services
{
    public class DebugAnalyticService : IAnalyticService
    {
        public string ServiceName => "Debug";

        public Task<bool> TrySendEvents(string events)
        {
            Debug.Log(events);
            return Task.FromResult(true);
        }
    }
}