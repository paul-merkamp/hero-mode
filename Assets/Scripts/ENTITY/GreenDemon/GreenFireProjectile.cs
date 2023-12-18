using UnityEngine;

public class GreenFireProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public int lifetimeInSeconds = 20;
    private float lifetimeTimer;

    public enum Direction
    {
        down,
        right,
        up
    }

    public Direction direction;

    private void Start()
    {
        lifetimeTimer = lifetimeInSeconds;
    }

    private void Update()
    {
        // Move the projectile forward
        if (direction == Direction.right)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else if (direction == Direction.down)
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        else if (direction == Direction.up)
            transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Decrease the lifetime timer
        lifetimeTimer -= Time.deltaTime;

        // Destroy the projectile if lifetime timer expires
        if (lifetimeTimer <= 0)
        {
            Destroy(gameObject);
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
