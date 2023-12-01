using System.Collections.Generic;
using UnityEngine;

public class PlayerModeController : MonoBehaviour
{
    public PlayerMode currentMode = PlayerMode.Sword;

    public enum PlayerMode
    {
        Sword,
        Big,
        Tiny,
        Magic
    }

    public List<GameObject> modeGOs;
    public List<GameObject> playerForms;

    private PlayerController player;

    private MagicController magic;

    public void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
        magic = FindAnyObjectByType<MagicController>();

        magic.Disable();
    }

    public void SwitchModes(int adder)
    {
        int modeCount = System.Enum.GetValues(typeof(PlayerMode)).Length;
        int newMode = (int)currentMode + adder;
        if (newMode < 0)
        {
            newMode = modeCount - 1;
        }
        else if (newMode >= modeCount)
        {
            newMode = 0;
        }
        currentMode = (PlayerMode)newMode;

        for (int i = 0; i < modeGOs.Count; i++)
        {
            modeGOs[i].SetActive(i == newMode);
            playerForms[i].SetActive(i == newMode);
        }

        // Set player speed depending on mode
        switch (currentMode)
        {
            case PlayerMode.Sword:
                player.moveSpeed = 1f;
                break;
            case PlayerMode.Big:
                player.moveSpeed = 0.7f;
                break;
            case PlayerMode.Tiny:
                player.moveSpeed = 1.2f;
                break;
            case PlayerMode.Magic:
                player.moveSpeed = 0.9f;
                break;
        }

        if (currentMode == PlayerMode.Magic)
        {
            magic.Enable();
        }
        else
        {
            magic.Disable();
        }
    }
}
