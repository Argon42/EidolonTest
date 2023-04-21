using System;
using Common;
using Eidolon.Analytic.Analytic;
using Eidolon.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Eidolon
{
    public class EventServiceHandler : MonoBehaviour
    {
        [SerializeField] private int cooldownBeforeSend = 30;
        [SerializeField] private string serverUrl = "http://localhost:5234/Analytic";
        private EventService _eventService;

        private void Start()
        {
            IRepository repository = new PlayerPrefsRepository();
            IAnalyticService analyticService = new AnalyticService(serverUrl);
            _eventService = new EventService(repository, analyticService,
                cooldownBeforeSend: TimeSpan.FromSeconds(cooldownBeforeSend));
            _eventService.Initialize();
        }

        private void Update()
        {
            _eventService.Update();
        }

        public void TrackEvent(string type, string data)
        {
            _eventService.TrackEvent(type, data);
        }
    }
}