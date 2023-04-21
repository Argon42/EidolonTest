using System;
using UnityEngine;

public class EventServiceHandler : MonoBehaviour
{
    private EventService _eventService;
    [SerializeField] private int cooldownBeforeSend = 30;
    [SerializeField] private string uri = "http://localhost:5234/Analytic";

    private void Start()
    {
        IRepository repository = new PlayerPrefsRepository();
        IAnalyticService analyticService = new AnalyticService(uri);
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