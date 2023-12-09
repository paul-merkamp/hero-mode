using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModeController playerModeController;
    private ResourceUIController resourceUIController;

    private readonly List<GameObject> modeGOs = new List<GameObject>();
    private readonly List<Animator> modeAnimators = new List<Animator>();
    private readonly List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    
    // Entity values
    public int health;
    public float iFrameLength;
    public AudioClip takeDamageSFX;
    public float knockbackForce = 0.1f;

    // Player values
    public int frogTokens = 0;
    public int keys = 0;
    public int bossKeys = 0;

    public SpriteRenderer spriteRenderer;
    public AudioSource sfx;

    public bool invincible = false;
    public bool playerInputFrozen = false;

    private void Start()
    {
        PlayerController existingPlayer = FindObjectOfType<PlayerController>();
        if (existingPlayer != null && existingPlayer != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        spriteRenderer = GameObject.Find("ENTITY/Player/Forms/Sword/Sprite").GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX/SFX_Player").GetComponent<AudioSource>();

        playerModeController = FindObjectOfType<PlayerModeController>();
        resourceUIController = FindObjectOfType<ResourceUIController>();

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
}
