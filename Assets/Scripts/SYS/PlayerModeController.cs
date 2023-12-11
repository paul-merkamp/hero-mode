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

    public List<PlayerMode> unlockedModes;

    public List<GameObject> modeGOs;
    public List<GameObject> playerForms;

    private PlayerController player;

    private MagicController magic;

    public void Awake()
    {
        follower = FindObjectOfType<PlayerFollower>();

        player = FindAnyObjectByType<PlayerController>();
        magic = FindAnyObjectByType<MagicController>();

        unlockedModes = new List<PlayerMode>
        {
            PlayerMode.Sword
        };
    }

    public void SwitchModes(int adder)
    {
        int oldMode = (int)currentMode;
        int modeCount = unlockedModes.Count;
        int newMode = CalculateNewMode(adder, modeCount);

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

    private int CalculateNewMode(int adder, int modeCount)
    {
        int newMode = (int)currentMode + adder;
        if (newMode < 0)
        {
            newMode = modeCount - 1;
        }
        else if (newMode >= modeCount)
        {
            newMode = 0;
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
