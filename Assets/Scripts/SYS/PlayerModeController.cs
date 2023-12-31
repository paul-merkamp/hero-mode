using System.Collections.Generic;
using UnityEngine;

public class PlayerModeController : MonoBehaviour
{
    private PlayerFollower follower;
    public Animator modePanelAnimator;

    public PlayerMode currentMode = PlayerMode.Sword;

    public enum PlayerMode
    {
        Sword,
        Big,
        Tiny,
        Magic,
        Stealth
    }

    private int modesCount = 5;

    public List<PlayerMode> unlockedModes;

    public List<GameObject> modeGOs;
    private List<GameObject> playerForms;

    private MagicController magic;

    public void Start()
    {
        follower = FindObjectOfType<PlayerFollower>();

        magic = FindAnyObjectByType<MagicController>();

        unlockedModes = PlayerData.unlockedModes;

        playerForms = new List<GameObject>();

        // Get all child GameObjects of Player/Forms
        Transform formsTransform = GameObject.Find("Player/Forms").transform;
        for (int i = 0; i < formsTransform.childCount; i++)
        {
            playerForms.Add(formsTransform.GetChild(i).gameObject);
        }
    }

    public void SwitchModes(int adder)
    {
        int oldMode = (int)currentMode;
        int newMode = CalculateNewMode(adder);

        SetCurrentMode(newMode);

        if (oldMode != newMode)
            modePanelAnimator.SetTrigger("Display");
    }

    public void SetCurrentMode(int newMode)
    {
        int oldMode = (int)currentMode;

        currentMode = (PlayerMode)newMode;

        // update position of the new form to the old form
        playerForms[newMode].transform.position = playerForms[oldMode].transform.position;

        for (int i = 0; i < modeGOs.Count; i++)
        {
            modeGOs[i].SetActive(i == newMode);
            playerForms[i].SetActive(i == newMode);
        }

        if (currentMode == PlayerMode.Magic)
        {
            magic.Enable();
        }
        else
        {
            magic.Disable();
        }

        follower.target = playerForms[newMode];
    }

    private int CalculateNewMode(int adder)
    {
        int newMode = (int)currentMode + adder;
        while (!unlockedModes.Contains((PlayerMode)newMode))
        {
            newMode += adder;
            if (newMode < 0)
            {
                newMode = modesCount - 1;
            }
            else if (newMode >= modesCount)
            {
                newMode = 0;
            }
        }
        return newMode;
    }

    public void UnlockMode(PlayerMode mode)
    {
        if (!unlockedModes.Contains(mode))
        {
            unlockedModes.Add(mode);
        }

        SetCurrentMode((int)mode);
    }
}
