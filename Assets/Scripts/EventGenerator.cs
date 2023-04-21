using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    [SerializeField] private EventServiceHandler eventServiceHandler;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            eventServiceHandler.TrackEvent("levelStart", $"level:{Random.Range(0, 100)}");

        if (Input.GetKey(KeyCode.S))
            eventServiceHandler.TrackEvent("getAward", $"award:{Random.Range(0, 100)}");

        if (Input.GetKey(KeyCode.D))
            eventServiceHandler.TrackEvent("spendCoins", $"coins:{Random.Range(0, 100)}");
    }
}