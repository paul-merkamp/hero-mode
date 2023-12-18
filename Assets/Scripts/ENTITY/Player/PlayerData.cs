using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static bool saveDataExists;

    public static int maxHealth = 3;
    public static int frogTokens = 0;
    public static int bossKeys = 0;
    public static int deathCount = 0;
    
    public static Vector2 lastCheckpointPosition = new Vector2(-4.49f, -17.73f);

    public static List<string> permanentlyCollectedItems = new List<string>();

    public static bool frogManQuestCompleted = false;
    public static bool greenDemonDefeated = false;
    public static bool survivalSectionCompleted = false;
    public static bool bossDoorUnlocked = false;

    public static List<PlayerModeController.PlayerMode> unlockedModes = new List<PlayerModeController.PlayerMode>
    {
        PlayerModeController.PlayerMode.Sword
    };
}