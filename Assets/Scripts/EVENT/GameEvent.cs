using System.Collections;
using UnityEngine;

// this whole class is redundant. whoops. in too deep now.
public class GameEvent : MonoBehaviour
{
    public int id;
    private GameEventController controller;

    public bool triggered = false;

    public void Start()
    {
        controller = FindAnyObjectByType<GameEventController>();
    }

    [System.Obsolete]
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
            case 9:
                controller.TriggerEvent_9();
                break;
            case 10:
                controller.TriggerEvent_10();
                break;
            case 11:
                // compiler error
                // controller.TriggerEvent_11();
                break;
            case 12:
                controller.TriggerEvent_12();
                break;
            case 15:
                controller.TriggerEvent_15();
                break;
            default:
                Debug.Log("Event " + id + " not found");
                break;
            }
        }
    }
}