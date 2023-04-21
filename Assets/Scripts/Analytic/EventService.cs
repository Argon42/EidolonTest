using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class EventService
{
    [Serializable]
    private class AnalyticSaveData
    {
        public List<AnalyticEvent> events;
    }

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

    public void Deinitialize()
    {
        AnalyticSaveData data = new() { events = _events };
        string json = JsonConvert.SerializeObject(data);
        _repository.SaveText(_analyticEventsKey, json);
    }

    public void Initialize()
    {
        var json = _repository.LoadText(_analyticEventsKey);
        var save = JsonConvert.DeserializeObject<AnalyticSaveData>(json);
        if (save is { events: { } })
            _events.InsertRange(0, save.events);
        if (_events.Count > 0)
            SendEvents();
        _lastTryUpdate = DateTime.Now;
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

    private async void SendEvents()
    {
        _cash.AddRange(_events);
        _events.Clear();
        _lastTryUpdate = DateTime.Now;
        string json = JsonConvert.SerializeObject(new AnalyticSaveData { events = _cash });
        if (await _analyticService.TrySendEvents(json))
            return;
        _events.InsertRange(0, _cash);
    }

    private void TrackEvent(AnalyticEvent analyticEvent)
    {
        _events.Add(analyticEvent);
        TrySendEvents();
    }

    private void TrySendEvents()
    {
        if (_events.Count > 0 && DateTime.Now - _lastTryUpdate >= _cooldownBeforeSend)
            SendEvents();
    }
}