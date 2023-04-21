using System;
using UnityEngine;

public class EventServiceHandler : MonoBehaviour
{
    private EventService _eventService;
    [SerializeField] private int cooldownBeforeSend = 30;

    private void Start()
    {
        IRepository repository = new PlayerPrefsRepository();
        IAnalyticService analyticService = new DebugAnalyticService();
        _eventService = new EventService(repository, analyticService,
            cooldownBeforeSend: TimeSpan.FromSeconds(cooldownBeforeSend));
        _eventService.Initialize();
    }

    private void Update()
    {
        _eventService.Update();
    }

    private void OnDestroy()
    {
        _eventService.Deinitialize();
    }

    public void TrackEvent(string type, string data)
    {
        _eventService.TrackEvent(type, data);
    }
}