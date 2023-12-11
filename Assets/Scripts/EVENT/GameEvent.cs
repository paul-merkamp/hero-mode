using System.Collections;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public int id;
    private GameEventController controller;

    public bool triggered = false;

    public void Start()
    {
        controller = FindAnyObjectByType<GameEventController>();
    }

    public void Trigger()
    {
        Debug.Log("Triggered event " + id);

        if (!triggered)
        {
            triggered = true;

            switch (id) {
            case 0:
                controller.TriggerEvent_0();
                break;
            case 1:
                controller.TriggerEvent_1();
                break;
            case 2:
                controller.TriggerEvent_2();
                break;
            case 3:
                controller.TriggerEvent_3();
                break;
            case 4:
                controller.TriggerEvent_4();
                break;
            case 5:
                controller.TriggerEvent_5();
                break;
            case 6:
                controller.TriggerEvent_6();
                break;
            case 7:
                controller.TriggerEvent_7();
                break;
            case 8:
                controller.TriggerEvent_8();
                break;
            }
        }
    }
}