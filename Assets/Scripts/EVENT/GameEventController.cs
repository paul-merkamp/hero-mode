using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameEventController : MonoBehaviour
{
    private BlackBarController blackBarController;
    private PlayerController player;
    private GameObject playerFollower;
    private DialogController dialogController;

    private CinemachineVirtualCamera vcam;

    public void Start()
    {
        blackBarController = FindAnyObjectByType<BlackBarController>();
        player = FindAnyObjectByType<PlayerController>();
        playerFollower = GameObject.Find("ENTITY/PlayerFollower");
        dialogController = FindAnyObjectByType<DialogController>();

        vcam = GameObject.Find("CAMERA/Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    public void VcamDampToTarget(GameObject target)
    {
        vcam.Follow = target.transform;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1f;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1f;
    }

    public void VcamReset()
    {
        vcam.Follow = playerFollower.transform;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0f;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0f;
    }

    // Event Triggers

    //
    // Green demon intro
    //

    public void TriggerEvent_0()
    {
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/GreenDemon"));

        dialogController.TriggerDialog(new List<string>{
            "\"Hero treat, yummy yummy!\"",
            "\"Me bring Chef Bako knight flesh!\""
        });

        StartCoroutine(Event_0(7f));
    }

    IEnumerator Event_0(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
        GameObject.Find("ENTITY/GreenDemon").GetComponent<GreenDemonAI>().StartAttacking();
    }

    //
    // Green demon death
    //

    public void TriggerEvent_1()
    {
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/GreenDemon"));

        dialogController.TriggerDialog(new List<string>{
            "\"Nooooo!\""
        });

        StartCoroutine(Event_1(5.5f));
    }

    IEnumerator Event_1(float delay)
    {
        GameObject demon = GameObject.Find("ENTITY/GreenDemon");

        // wait for dialogue to finish
        yield return new WaitForSeconds(delay);

        // destroy the demon
        demon.GetComponent<GreenDemonAI>().DeathAnimation();
        yield return new WaitForSeconds(1);

        // look at the door
        VcamDampToTarget(GameObject.Find("ENTITY/GreenDemon_eventUnlockedDoorTarget"));
        yield return new WaitForSeconds(1);

        // open the door
        demon.GetComponent<GreenDemonAI>().DisableOnDefeat();
        yield return new WaitForSeconds(1);

        // return to the player
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
    }

    //
    // Gain Big Mode
    //

    public void TriggerEvent_2()
    {
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();

        dialogController.TriggerDialog(new List<string>{
            "The <color=\"red\">red page</color> has granted you the power of <color=\"red\">Big Mode</color>!",
            "Press <color=\"red\">N</color> or <color=\"red\">M</color> to switch between modes.",
        });

        StartCoroutine(Dialog_Coroutine(9f));
    }

    //
    // Bako intro
    //

    public void TriggerEvent_3()
    {
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/BakoBoss_eventTarget"));

        dialogController.TriggerDialog(new List<string>{
            "\"You've sealed your <color=\"red\">doom</color> by coming here!\"",
            "\"Prepare to be my restaurant's <color=\"red\">NEXT SPECIAL</color>!\"",
        });

        StartCoroutine(Dialog_Coroutine(8.5f));
    }

    //
    // Bako death
    //

    public void TriggerEvent_4()
    {
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/BakoBoss_eventTarget"));

        dialogController.TriggerDialog(new List<string>{
            "\"No...\"",
            "\"I can't believe I've been defeated by a mere <color=\"red\">snack</color>!\"",
        });

        StartCoroutine(Dialog_Coroutine(8.5f));
    }

    //
    // Helpers
    // 

    IEnumerator Dialog_Coroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
    }
}
