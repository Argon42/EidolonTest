using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    [SerializeField] private EventServiceHandler eventServiceHandler;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            FireStartLevelEvent();

        if (Input.GetKey(KeyCode.S))
            FireGetAwardEvent();

        if (Input.GetKey(KeyCode.D))
            FireSpendCoinsEvent();
    }

    public void FireSpendCoinsEvent() =>
        eventServiceHandler.TrackEvent("spendCoins", $"coins:{Random.Range(0, 100)}");

    public void FireGetAwardEvent() =>
        eventServiceHandler.TrackEvent("getAward", $"award:{Random.Range(0, 100)}");

    public void FireStartLevelEvent() =>
        eventServiceHandler.TrackEvent("levelStart", $"level:{Random.Range(0, 100)}");
}