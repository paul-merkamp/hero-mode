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
        player.invincible = true;
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
        player.invincible = false;
    }

    //
    // Green demon death
    //

    public void TriggerEvent_1()
    {
        player.invincible = true;
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/GreenDemon"));

        dialogController.TriggerDialog(new List<string>{
            "\"Noooooooooooooooo!\"",
            "\"I just wanted to eat you!\""
        });

        StartCoroutine(Event_1(5.5f));
    }

    IEnumerator Event_1(float delay)
    {
        GameObject demon = GameObject.Find("ENTITY/GreenDemon");

        // wait for dialogue to finish
        yield return new WaitForSeconds(delay);

        // play death animation / disable demon GO
        demon.GetComponent<GreenDemonAI>().DeathAnimation();
        yield return new WaitForSeconds(3);

        // look at the door
        VcamDampToTarget(GameObject.Find("ENTITY/GreenDemon_eventUnlockedDoorTarget"));
        yield return new WaitForSeconds(1);

        // open the door
        demon.GetComponent<GreenDemonAI>().DisableOnDefeat();
        yield return new WaitForSeconds(1.5f);

        // return to the player
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
        player.invincible = false;
    }

    //
    // Gain Big Mode
    //

    public void TriggerEvent_2()
    {
        player.invincible = true;
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
        player.invincible = true;
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
        player.invincible = true;
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
    // Skeleton summoning room
    //

    public void TriggerEvent_5()
    {
        dialogController.TriggerDialog(new List<string>{
            "\"Get 'em, boys!\"",
        });

        StartCoroutine(Event_5());
    }

    IEnumerator Event_5()
    {
        yield return new WaitForSeconds(1f);

        for (int i=0; i<GameObject.Find("ENTITY/SummoningRoomEnemies").transform.childCount; i++)
        {
            GameObject enemy = GameObject.Find("ENTITY/SummoningRoomEnemies").transform.GetChild(i).gameObject;
            enemy.SetActive(true);
        }
    }

    //
    // Green demon enter phase 2
    //

    public void TriggerEvent_6()
    {
        player.invincible = true;
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/GreenDemon"));

        dialogController.TriggerDialog(new List<string>{
            "\"You think you won?\"",
            "\"Well beat this, stupid!\"",
        });

        StartCoroutine(Event_6());
    }

    IEnumerator Event_6()
    {
        yield return new WaitForSeconds(4f);

        for (int i=0; i<GameObject.Find("ENTITY/GreenDemonBackupEnemies").transform.childCount; i++)
        {
            GameObject enemy = GameObject.Find("ENTITY/GreenDemonBackupEnemies").transform.GetChild(i).gameObject;
            enemy.SetActive(true);
        }

        player.transform.position = new Vector2(-10.19f, -0.9f);
    
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
        player.invincible = false;
    }

    //
    // Vampire intro
    //

    public void TriggerEvent_7()
    {
        player.invincible = true;
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/Vampire"));

        dialogController.TriggerDialog(new List<string>{
            "\"I'm bored... you could be a bit of fun.\"",
            "\"I'll let you pass if you can survive my minions.\"",
            "\"Eliminate <color=#9c1ce6>10</color> of them, and I'll give you a prize.\"",
        });

        StartCoroutine(Event_7());
    }

    IEnumerator Event_7()
    {
        yield return new WaitForSeconds(12f);

        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
        player.invincible = false;

        for (int i=0; i<GameObject.Find("ENTITY/VampireSpawners").transform.childCount; i++)
        {
            Spawner spawner = GameObject.Find("ENTITY/VampireSpawners").transform.GetChild(i).gameObject.GetComponent<Spawner>();
            spawner.StartSpawning();
        }

        GameObject.Find("UI/UI_SurvivalDisplay").transform.GetChild(0).gameObject.SetActive(true);
        FindAnyObjectByType<SurvivalSectionController>().StartSection();
    }

    //
    // Finished vampire room - no kills
    //

    public void TriggerEvent_8()
    {
        player.invincible = true;
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/Vampire"));

        dialogController.TriggerDialog(new List<string>{
            "\"... you didn't kill ANY of them???\"",
            "\"How bland... well, a deal's a deal...\"",
        });

        StartCoroutine(Event_8());
    }

    IEnumerator Event_8()
    {
        // wait for dialog to finish
        yield return new WaitForSeconds(6f);

        // move to gate
        VcamDampToTarget(GameObject.Find("ENV/ENV_Objects/RedDungeon/FreezerGate"));
        yield return new WaitForSeconds(2f);

        // Show gate open
        GameObject.Find("ENV/ENV_Objects/RedDungeon/FreezerGate").SetActive(false);
        yield return new WaitForSeconds(1f);

        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
        player.invincible = false;
    }

    //
    // Finished vampire room - not enough kills
    //

    public void TriggerEvent_9()
    {
        player.invincible = true;
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/Vampire"));

        dialogController.TriggerDialog(new List<string>{
            "\"A fair performance, but you leave me wanting more.\"",
            "\"Go on, then, scram, hero.\"",
        });

        StartCoroutine(Event_9());
    }

    IEnumerator Event_9()
    {
        // wait for dialog to finish
        yield return new WaitForSeconds(6f);

        // move to gate
        VcamDampToTarget(GameObject.Find("ENV/ENV_Objects/RedDungeon/FreezerGate"));
        yield return new WaitForSeconds(2f);

        // Show gate open
        GameObject.Find("ENV/ENV_Objects/RedDungeon/FreezerGate").SetActive(false);
        yield return new WaitForSeconds(1f);

        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
        player.invincible = false;
    }

    //
    // Finished vampire room - enough kills
    //

    public void TriggerEvent_10()
    {
        player.invincible = true;
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamDampToTarget(GameObject.Find("ENTITY/Vampire"));

        dialogController.TriggerDialog(new List<string>{
            "\"Well done, hero, well done!\"",
            "\"You've earned a marvelous prize.\"",
        });

        StartCoroutine(Event_10());
    }

    IEnumerator Event_10()
    {
        // wait for dialog to finish
        yield return new WaitForSeconds(6f);

        // move to gate
        VcamDampToTarget(GameObject.Find("ENV/ENV_Objects/RedDungeon/FreezerGate"));
        yield return new WaitForSeconds(2f);

        // spawn prize
        // setactive doesn't actually work on child objects, ignore obsolete warning
        GameObject.Find("ENV/ENV_Objects/RedDungeon/SurvivalPrizeHeartContainer").SetActiveRecursively(true);

        // Show gate open
        GameObject.Find("ENV/ENV_Objects/RedDungeon/FreezerGate").SetActive(false);
        yield return new WaitForSeconds(1f);

        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        VcamReset();
        player.invincible = false;
    }

    //
    // Helpers
    // 

    IEnumerator Dialog_Coroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.TogglePlayerControl();
        blackBarController.ToggleBlackBars();
        player.invincible = false;
        VcamReset();
    }
}