using System.Collections;
using UnityEngine;

public class PlayerForm : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private PlayerController player;

    private PlayerModeController playerModeController;

    // accessed from CrushAsBigMode
    public GameObject currentCollision;
    public GameObject interactIndicator;
    
    private PlayerSword sword;
    private Rigidbody2D rb2d;
    
    private bool jumping = false;

    private bool lavaImmune = false;
    
    // Form-specific entity values
    public float moveSpeed;

    public void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        player = FindAnyObjectByType<PlayerController>();
        playerModeController = FindAnyObjectByType<PlayerModeController>();
        interactIndicator = transform.GetChild(1).gameObject;
        
        sword = FindAnyObjectByType<PlayerSword>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    [System.Obsolete]
    public void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (!player.playerInputFrozen)
            Move(horizontalInput, verticalInput);

        if (!player.playerInputFrozen && Input.GetKeyDown(KeyCode.Space))
        {
            Interact();

            interactIndicator.SetActive(false);
            animator.SetBool("Walking", false);
        }
    }

    [System.Obsolete]
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

                if (door != null)
                {
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
                            PlayerData.bossDoorUnlocked = true;
                        }
                    }
                }
            }
        }
        else if (currentCollision != null && currentCollision.CompareTag("interact_event"))
        {
            currentCollision.GetComponent<GameEvent>().Trigger();
        }
        else if (currentCollision != null && currentCollision.CompareTag("frog_man"))
        {
            currentCollision.GetComponent<FrogMan>().Interact();
        }
        else if (currentCollision != null && currentCollision.CompareTag("loading_zone"))
        {
            if (currentCollision.GetComponent<LoadingZone>().requiresInteract)
                currentCollision.GetComponent<LoadingZone>().Trigger();
        }
        else if (playerModeController.currentMode == PlayerModeController.PlayerMode.Sword)
        {
            sword.Attack();
        }
        else if (playerModeController.currentMode == PlayerModeController.PlayerMode.Big)
        {   
            Jump();
        }
        else if (playerModeController.currentMode == PlayerModeController.PlayerMode.Stealth && !onStealthCooldown)
        {
            SetStealthed(true);
        }
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

    public void Jump()
    {
        if (!jumping)
        {
            jumping = true;
            player.TogglePlayerControl();
            animator.SetTrigger("Jumping");
            StartCoroutine(JumpCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        SpawnShockwave();

        yield return new WaitForSeconds(0.2f);
        jumping = false;
        player.TogglePlayerControl();
    }

    public void SpawnShockwave()
    {
        GameObject shockwave = Instantiate(Resources.Load<GameObject>("Prefabs/ENTITY/Player/Shockwave"), transform.position, Quaternion.identity);
    }

    private bool stealthed = true;
    public bool onStealthCooldown = false;

    public void SetStealthed(bool stealthed)
    {
        this.stealthed = stealthed;

        if (stealthed)
        {
            onStealthCooldown = true;
            player.modeSwitchFrozen = true;

            spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            gameObject.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
            StartCoroutine(TurnOffStealthAfterDelay(3f));
        }
        else
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            gameObject.layer = LayerMask.NameToLayer("Default");

            player.modeSwitchFrozen = false;
        }
    }

    private IEnumerator TurnOffStealthAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SetStealthed(false);

        yield return new WaitForSeconds(delay);

        onStealthCooldown = false;
    }

    [System.Obsolete]
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

            player.UpdateHeartsUI();
        }
    }

    [System.Obsolete]
    IEnumerator DoIFrames()
    {
        if (!player.dead) player.sfx.PlayOneShot(player.takeDamageSFX);
        
        player.invincible = true;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(player.iFrameLength);
        spriteRenderer.color = Color.white;

        player.invincible = false;

        if (player.health <= 0)
        {
            player.Die();
        }
    }

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("damaging_entity"))
        {
            TakeDamage(1, collision.gameObject);
        }
    }

    [System.Obsolete]
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("damaging_surface") && !lavaImmune)
        {
            TakeDamage(1, collision.gameObject);
        }
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("damaging_entity"))
        {
            TakeDamage(1, collision.gameObject);
        }

        else if (collision.CompareTag("event"))
        {
            GameEvent gameEvent = collision.gameObject.GetComponent<GameEvent>();
            gameEvent.Trigger();
        }

        else if (collision.CompareTag("sign") || collision.CompareTag("door") || collision.CompareTag("switch") ||
                collision.CompareTag("frog_man") || collision.CompareTag("loading_zone"))
        {
            SimpleDestructible destructible = collision.GetComponent<SimpleDestructible>();
            
            if (destructible == null || !destructible.destroyed)
            {
                currentCollision = collision.gameObject;
                interactIndicator.SetActive(true);
            }
        }

        else if (collision.CompareTag("interact_event"))
        {
            if (!collision.GetComponent<GameEvent>().triggered)
            {
                currentCollision = collision.gameObject;
                interactIndicator.SetActive(true);
            }
        }

        else if (collision.CompareTag("collectible"))
        {
            Collectible collectible = collision.gameObject.GetComponent<Collectible>();
            if (collectible.type == Collectible.CollectibleType.Coin)
            {
                player.GainToken();
                player.sfx.PlayOneShot(collectible.collectSound);
                Destroy(collision.gameObject);
            }
            else if (collectible.type == Collectible.CollectibleType.BossKey)
            {
                player.GainBossKey();
                player.sfx.PlayOneShot(collectible.collectSound);
                Destroy(collision.gameObject);
            }
            else if (collectible.type == Collectible.CollectibleType.ModeUnlock)
            {
                playerModeController.UnlockMode(collectible.modeUnlock);
                if (collectible.pickupEvent != null)
                    collectible.pickupEvent.Trigger();

                player.sfx.PlayOneShot(collectible.collectSound);

                Destroy(collision.gameObject);
            }
            else if (collectible.type == Collectible.CollectibleType.Health)
            {
                if (player.health < player.maxHealth)
                {
                    player.health += collectible.healthAmount;
                    player.UpdateHeartsUI();
                    Destroy(collision.gameObject);
                    player.sfx.PlayOneShot(collectible.collectSound);
                }
            }
            else if (collectible.type == Collectible.CollectibleType.MaxHealth)
            {
                MusicController musicController = FindObjectOfType<MusicController>();
                musicController.PlayHPUp();

                player.maxHealth += collectible.healthAmount;
                player.health = player.maxHealth;
                player.UpdateHeartsUI();
                Destroy(collision.gameObject);
            }

            player.temporarilyCollectedItems.Add(GetGameObjectPath(collectible.gameObject));
        }
        else if (collision.CompareTag("lava_immunity"))
        {
            lavaImmune = true;
        }
        else if (collision.CompareTag("checkpoint"))
        {
            player.SaveData(collision.transform.position);
            Debug.Log("Saved data at " + collision.transform.position);
        }
    }

    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("sign") || collision.CompareTag("door") || collision.CompareTag("switch") || collision.CompareTag("interact_event") || collision.CompareTag("frog_man"))
        {
            currentCollision = null;
            interactIndicator.SetActive(false);
        }
        else if (collision.CompareTag("lava_immunity"))
        {
            lavaImmune = false;
        }
    }

    public void MakeSlippery()
    {
        rb2d.drag /= 32;
        rb2d.mass *= 32;
    }

    public void MakeNotSlippery()
    {
        rb2d.drag *= 32;
        rb2d.mass /= 32;
    }
}
