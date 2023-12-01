using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    private PlayerModeController playerModeController;

    private readonly List<GameObject> modeGOs = new List<GameObject>();
    private readonly List<Animator> modeAnimators = new List<Animator>();
    private readonly List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private GameObject interactIndicator;
    
    private PlayerSword sword;

    private void Start()
    {
        playerModeController = FindObjectOfType<PlayerModeController>();

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

        interactIndicator = transform.GetChild(1).gameObject;

        sword = formsTransform.GetChild(0).GetChild(2).GetComponent<PlayerSword>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Move(horizontalInput, verticalInput);

        // Set animation parameters
        foreach (Animator a in modeAnimators)
        {
            a.SetBool("Walking", horizontalInput != 0f || verticalInput != 0f);
        }

        // Cycle through modes
        if (Input.GetKeyDown(KeyCode.N))
        {
            playerModeController.SwitchModes(-1);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            playerModeController.SwitchModes(1);            
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerModeController.currentMode == PlayerModeController.PlayerMode.Sword)
            {
                sword.Attack();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("sign"))
        {
            interactIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("sign"))
        {
            interactIndicator.SetActive(false);
        }
    }
}
