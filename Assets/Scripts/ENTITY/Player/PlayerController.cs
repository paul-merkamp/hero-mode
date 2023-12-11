using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModeController playerModeController;
    private ResourceUIController resourceUIController;
    private HealthUIController healthUIController;

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
    public int keys = 0;
    public int bossKeys = 0;

    public AudioSource sfx;

    public bool invincible = false;
    public bool playerInputFrozen = false;
    public bool dead = false;
    public int deathCount = 0;

    private void Start()
    {
        List<PlayerController> existingPlayers = new List<PlayerController>(FindObjectsOfType<PlayerController>());
        foreach (PlayerController player in existingPlayers)
        {
            if (player != this)
            {
                transform.position = player.transform.position;
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

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
    }

    private void Update()
    {
        if (!invincible && !playerInputFrozen)
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
            if (Input.GetKeyDown(KeyCode.R))
            {
                // TODO: checkpoint
                UnityEngine.SceneManagement.SceneManager.LoadScene("BakosKitchen");
            }
        }
    }

    public void GainToken()
    {
        frogTokens++;
        resourceUIController.UpdateFrogCoinCount(frogTokens);
    }

    public void GainKey()
    {
        keys++;
        // resourceUIController.ShowKey(true);
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

    public GameObject deathScreenBG;
    public GameObject deathScreenUI;
    public TMP_Text deathTauntText;

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

    public void Die()
    {
        if (!dead)
        {
            playerModeController.SetCurrentMode(0);
            modeAnimators[0].SetBool("Dead", true);
            playerInputFrozen = true;
            dead = true;
            deathCount++;

            deathScreenBG.SetActive(true);
            deathScreenUI.SetActive(true);

            int tauntIndex = UnityEngine.Random.Range(-1, deadTaunts.Count);
            Debug.Log("Taunt index: " + tauntIndex);
            string tauntText = tauntIndex == -1 ? "<color=\"yellow\">" + rareDeadTaunts[UnityEngine.Random.Range(0, rareDeadTaunts.Count)] + "</color>" : deadTaunts[tauntIndex];
            deathTauntText.text = tauntText;
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
}
