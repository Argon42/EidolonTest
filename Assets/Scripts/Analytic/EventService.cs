using System;
using System.Collections.Generic;
using Common;
using Newtonsoft.Json;

namespace Eidolon.Analytic.Analytic
{
    public class EventService
    {
        [Serializable]
        private struct AnalyticEvent
        {
            public string type;
            public string data;

            public AnalyticEvent(string type, string data)
            {
                this.type = type;
                this.data = data;
            }
        }

        [Serializable]
        private class AnalyticSaveData
        {
            public List<AnalyticEvent> events;
        }

        private readonly string _analyticEventsKey;
        private readonly IRepository _repository;
        private readonly IAnalyticService _analyticService;
        private readonly TimeSpan _cooldownBeforeSend;
        private readonly List<AnalyticEvent> _events = new();
        private DateTime _lastTryUpdate;
        private readonly List<AnalyticEvent> _cash = new();

        public EventService(IRepository repository, IAnalyticService analyticService, TimeSpan cooldownBeforeSend)
        {
            _repository = repository;
            _analyticService = analyticService;
            _cooldownBeforeSend = cooldownBeforeSend;
            _analyticEventsKey = $"AnalyticEvents_{_analyticService.ServiceName}";
        }

        public void Initialize()
        {
            var json = _repository.LoadText(_analyticEventsKey);
            var save = JsonConvert.DeserializeObject<AnalyticSaveData>(json);
            if (save is { events: { } })
                _events.InsertRange(0, save.events);
            if (_events.Count > 0)
                SendEvents();
        }

        public void TrackEvent(string type, string data)
        {
            var analyticEvent = new AnalyticEvent(type, data);
            TrackEvent(analyticEvent);
        }

        public void Update()
        {
            TrySendEvents();
        }

        private void SaveData()
        {
            AnalyticSaveData data = new() { events = _events };
            string json = JsonConvert.SerializeObject(data);
            _repository.SaveText(_analyticEventsKey, json);
        }

        private async void SendEvents()
        {
            _cash.AddRange(_events);
            _lastTryUpdate = DateTime.Now;
            string json = JsonConvert.SerializeObject(new AnalyticSaveData { events = _cash });
            if (await _analyticService.TrySendEvents(json))
            {
                _events.RemoveRange(0, _cash.Count);
                _cash.Clear();
                SaveData();
                return;
            }
            _cash.Clear();
        }

        private void TrackEvent(AnalyticEvent analyticEvent)
        {
            if (_events.Count == 0)
                _lastTryUpdate = DateTime.Now;
            _events.Add(analyticEvent);
            SaveData();
            TrySendEvents();
        }

        private void TrySendEvents()
        {
            if (_events.Count > 0 && DateTime.Now - _lastTryUpdate >= _cooldownBeforeSend)
                SendEvents();
        }
    }
}