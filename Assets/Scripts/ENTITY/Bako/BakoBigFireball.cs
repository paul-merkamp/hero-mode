using UnityEngine;

public class BakoBigFireball : MonoBehaviour
{
    public int damage = 1;
    public Vector2 target;
    public float speed = 10f;
    public int lifetimeInSeconds = 20;

    private float lifetimeTimer;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        lifetimeTimer = lifetimeInSeconds;
        spriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MoveTowardsTarget();
        DecreaseLifetimeTimer();
        CheckFlipX();
    }

    private Vector2 direction;

    public void SetTarget(Vector2 target)
    {
        this.target = target;
        direction = target - (Vector2)transform.position;
        direction.Normalize();
    }

    private void MoveTowardsTarget()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void DecreaseLifetimeTimer()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void CheckFlipX()
    {
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile collided with an Entity or Player
        PlayerForm player = other.gameObject.GetComponent<PlayerForm>();

        if (player != null)
        {
            // Deal damage to the player
            player.TakeDamage(damage, gameObject);
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
