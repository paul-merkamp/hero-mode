using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerModeController playerModeController;
    private ResourceUIController resourceUIController;
    private HealthUIController healthUIController;

    public PauseMenu configMenu;

    private readonly List<GameObject> modeGOs = new List<GameObject>();
    private readonly List<Animator> modeAnimators = new List<Animator>();
    private readonly List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    
    // Entity values
    public int health = 3;
    public int maxHealth = 3;
    public float iFrameLength;
    public AudioClip takeDamageSFX;
    public float knockbackForce = 0.1f;

    // Player values
    public int frogTokens = 0;
    public int bossKeys = 0;

    public AudioSource sfx;

    public bool invincible = false;
    public bool playerInputFrozen = false;
    public bool modeSwitchFrozen = false;
    public bool dead = false;
    public int deathCount = 0;

    public GameObject deathScreenBG;
    public GameObject deathScreenUI;
    public TMP_Text deathTauntText;

    public MusicController music;

    public List<string> temporarilyCollectedItems = new List<string>();

    public bool ignoreCheckpointPos = false;

    private void Awake()
    {
        if (PlayerData.saveDataExists)
        {
            LoadData();
        }
    }

    public AudioClip overworld;
    public AudioClip overworldPass2;

    public AudioClip pickupMaxHealthSFX;

    private void Start()
    {
        if (!ignoreCheckpointPos) ForcePlayerPosition(PlayerData.lastCheckpointPosition);

        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        if (playerControllers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        sfx = GameObject.Find("SFX/SFX_Player").GetComponent<AudioSource>();

        playerModeController = FindObjectOfType<PlayerModeController>();
        resourceUIController = FindObjectOfType<ResourceUIController>();
        healthUIController = FindObjectOfType<HealthUIController>();

        Transform formsTransform = transform.GetChild(0).transform;
        int childCount = formsTransform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject modeGO = formsTransform.GetChild(i).gameObject;
            modeGOs.Add(modeGO);

            Animator animator = modeGO.transform.GetChild(0).GetComponent<Animator>();
            if (animator != null)
            {
                modeAnimators.Add(animator); // Add animator to the list
            }

            SpriteRenderer spriteRenderer = modeGO.transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderers.Add(spriteRenderer);
        }

        foreach(string item in PlayerData.permanentlyCollectedItems)
        {
            Destroy(GameObject.Find(item));
        }

        music = GameObject.Find("SYS/SYS_MusicController").GetComponent<MusicController>();
        music.PlaySequentially(overworld, overworldPass2);
    }

    private bool paused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)
            {
                configMenu.Pause();
                paused = true;
                playerInputFrozen = true;

                Time.timeScale = 0;
            }
            else {
                configMenu.Resume();
                paused = false;
                playerInputFrozen = false;

                Time.timeScale = 1;
            }
        }

        if (!invincible && !playerInputFrozen && !modeSwitchFrozen && !modeGOs[4].GetComponent<PlayerForm>().onStealthCooldown)
        {
            // Cycle through modes
            if (Input.GetKeyDown(KeyCode.N))
            {
                playerModeController.SwitchModes(-1);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                playerModeController.SwitchModes(1);            
            }
        }

        if (dead)
        {
            // prevent shenanigans, idk how else to accomplish this
            // without taking a ton of time to refactor stuff
            playerInputFrozen = true;

            if (Input.GetKeyDown(KeyCode.R))
            {
                // TODO: checkpoint

                SceneManager.LoadScene("BakosKitchen", LoadSceneMode.Single);
                
                Time.timeScale = 1;
            }
        }
    }

    public void GainToken()
    {
        frogTokens++;
        resourceUIController.UpdateFrogCoinCount(frogTokens);
    }

    public void GainBossKey()
    {
        bossKeys++;
        resourceUIController.ShowBossKey(true);
    }

    public void TogglePlayerControl()
    {
        playerInputFrozen = !playerInputFrozen;
    }

    public void ToggleModeSwitching()
    {
        modeSwitchFrozen = !modeSwitchFrozen;
    }

    private List<String> deadTaunts = new List<String>{
        "Wow, you died!",
        "You fought well!",
        "You died! Try again!",
        "Hope you're having fun! :)",
        "Not a very useful mode!",
        "You can do it!",
        "You can't win them all!",
        "Was the prophecy wrong? No, surely not...",
        "You'll get it next time!",
        "Get back up, hero!",
        "Keep trying! :)",
        "I believe in you!",
        "Try going Big Mode instead!",
        "Try not getting hit!",
        "Try dodging their attacks!",
        "A warrior's death, to be sure!"
    };

    private List<String> rareDeadTaunts = new List<String>{
        "Bennett Foddy would have a field day with this...",
        "I'm pretty sure that was actually a bug... sorry...",
        "This is going in my cringe compilation.",
        "Maybe you can't do it! Maybe you're WEAK!",
        "Snake? Snake?! SNAAAAAAAAKE!",
        "You've met with a terrible fate, haven't you?",
        "Here, give me the controller. Please, I'm begging you.",
        "Wishlist Animal Well!",
        "Wishlist Clone Clicker!",
        "Brought to you by videogamedunkey!",
        "Brought to you by Hidden Wish Studios!",
    };

    [Obsolete]
    public void Die()
    {
        if (!dead)
        {
            playerModeController.SetCurrentMode(0);
            modeAnimators[0].SetBool("Dead", true);
            playerInputFrozen = true;
            dead = true;
            PlayerData.deathCount++;

            deathScreenBG.SetActiveRecursively(true);
            deathScreenUI.SetActiveRecursively(true);

            int tauntIndex = UnityEngine.Random.Range(-1, deadTaunts.Count);
            Debug.Log("Taunt index: " + tauntIndex);
            string tauntText = tauntIndex == -1 ? "<color=\"yellow\">" + rareDeadTaunts[UnityEngine.Random.Range(0, rareDeadTaunts.Count)] + "</color>" : deadTaunts[tauntIndex];
            deathTauntText.text = tauntText;

            MusicController music = GameObject.Find("SYS/SYS_MusicController").GetComponent<MusicController>();
            music.CutToSong(music.deathSong);
        }
    }

    public void UpdateHeartsUI()
    {
        healthUIController.UpdateHeartsUI();
    }

    public void Respawn()
    {
        dead = false;
        health = maxHealth;
        healthUIController.UpdateHeartsUI();
        playerInputFrozen = false;
        modeAnimators[0].SetBool("Dead", false);
    }

    public void SaveData(Vector2 checkpointPosition)
    {
        PlayerData.lastCheckpointPosition = checkpointPosition;

        PlayerData.saveDataExists = true;

        PlayerData.maxHealth = maxHealth;
        PlayerData.frogTokens = frogTokens;
        PlayerData.bossKeys = bossKeys;
        PlayerData.deathCount = deathCount;

        // uniques
        FrogMan frogMan = FindAnyObjectByType<FrogMan>();
        PlayerData.frogManQuestCompleted = frogMan.frogManQuestCompleted;

        GreenDemonAI greenDemonAI = FindAnyObjectByType<GreenDemonAI>();
        PlayerData.greenDemonDefeated = greenDemonAI.defeated;

        SurvivalSectionController survivalSectionController = FindAnyObjectByType<SurvivalSectionController>();
        PlayerData.survivalSectionCompleted = survivalSectionController.survivalSectionCompleted;

        foreach(string item in temporarilyCollectedItems)
        {
            PlayerData.permanentlyCollectedItems.Add(item);
        }

        temporarilyCollectedItems = new List<string>();

        PlayerData.unlockedModes = playerModeController.unlockedModes;
    }

    public void LoadData()
    {
        maxHealth = PlayerData.maxHealth;
        health = maxHealth;

        frogTokens = PlayerData.frogTokens;
        bossKeys = PlayerData.bossKeys;
        deathCount = PlayerData.deathCount;

        // all uniques check against PlayerData by themselves
        // but survival is different since nothing is despawned or disabled
        // we have to fail those checks as they happen

        SurvivalSectionController survivalSectionController = FindAnyObjectByType<SurvivalSectionController>();
        survivalSectionController.survivalSectionCompleted = PlayerData.survivalSectionCompleted;

        // unlocked modes are grabbed by the controller
    }

    public void ForcePlayerPosition(Vector2 position)
    {
        transform.position = position;

        foreach(GameObject modeGO in modeGOs)
        {
            modeGO.transform.localPosition = Vector2.zero;
        }
    }
}
