using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerForm : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private PlayerController player;

    private PlayerModeController playerModeController;
    private GameObject currentCollision;
    private GameObject interactIndicator;
    
    private PlayerSword sword;
    private Rigidbody2D rb2d;
    
    // Form-specific entity values
    public float moveSpeed;

    public void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        player = FindAnyObjectByType<PlayerController>();
        playerModeController = FindAnyObjectByType<PlayerModeController>();
        interactIndicator = transform.GetChild(1).gameObject;
        
        sword = FindAnyObjectByType<PlayerSword>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        PushValuesToMaster();
    }

    public void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (!player.playerInputFrozen)
            Move(horizontalInput, verticalInput);

        if (!player.playerInputFrozen && Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (currentCollision != null && currentCollision.CompareTag("sign"))
        {
            currentCollision.GetComponent<Sign>().Interact();
        }
        else if (currentCollision != null && currentCollision.CompareTag("switch"))
        {
            currentCollision.GetComponent<DarknessSwitch>().Toggle();
        }
        else if (currentCollision != null && currentCollision.CompareTag("door"))
        {
            if (currentCollision.CompareTag("door")){

                Door door = currentCollision.GetComponent<Door>();

                if (!door.locked)
                {
                    door.ToggleOpen();
                }

                else if (door.locked && door.doorType == Door.DoorType.Boss)
                {
                    if (player.bossKeys > 0)
                    {
                        player.bossKeys--;
                        currentCollision.GetComponent<Door>().locked = false;
                        currentCollision.GetComponent<Door>().ToggleOpen();
                    }
                }
            }
        }
        else if (playerModeController.currentMode == PlayerModeController.PlayerMode.Sword)
        {
            sword.Attack();
        }
    }

    public void PushValuesToMaster()
    {
        player.spriteRenderer = spriteRenderer;
    }

    public void Move(float horizontalInput, float verticalInput)
    {
        Vector2 movement = new(horizontalInput, verticalInput);
        rb2d.AddForce(movement * moveSpeed);
        
        // Flip
        if (horizontalInput < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (horizontalInput > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // Set animation parameters
        animator.SetBool("Walking", horizontalInput != 0f || verticalInput != 0f);
    }

    public void TakeDamage(int damage, GameObject collision = null)
    {
        if (!player.invincible)
        {
            player.health -= damage;
            StartCoroutine(DoIFrames());

            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            if (rb2d != null && collision != null)
            {
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                rb2d.AddForce(knockbackDirection * player.knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator DoIFrames()
    {
        player.sfx.PlayOneShot(player.takeDamageSFX);
        
        player.invincible = true;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(player.iFrameLength);
        spriteRenderer.color = Color.white;

        player.invincible = false;

        if (player.health <= 0)
        {
            // TODO: Game over
            Destroy(player.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("damaging_surface") ||
            collision.gameObject.layer == LayerMask.NameToLayer("damaging_entity"))
        {
            TakeDamage(1, collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("event"))
        {
            GameEvent gameEvent = collision.gameObject.GetComponent<GameEvent>();
            gameEvent.Trigger();
        }

        else if (collision.CompareTag("sign") || collision.CompareTag("door") || collision.CompareTag("switch"))
        {
            currentCollision = collision.gameObject;
            interactIndicator.SetActive(true);
        }

        else if (collision.CompareTag("collectible"))
        {
            Collectible collectible = collision.gameObject.GetComponent<Collectible>();
            if (collectible.type == Collectible.CollectibleType.Coin)
            {
                player.GainToken();
                Destroy(collision.gameObject);
            }
            else if (collectible.type == Collectible.CollectibleType.BossKey)
            {
                player.GainBossKey();
                Destroy(collision.gameObject);
            }
            else if (collectible.type == Collectible.CollectibleType.ModeUnlock)
            {
                playerModeController.UnlockMode(collectible.modeUnlock);
                if (collectible.pickupEvent != null)
                    collectible.pickupEvent.Trigger();

                Destroy(collision.gameObject);
            }

            player.sfx.PlayOneShot(collectible.collectSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("sign") || collision.CompareTag("door") || collision.CompareTag("switch"))
        {
            currentCollision = null;
            interactIndicator.SetActive(false);
        }
    }
}
