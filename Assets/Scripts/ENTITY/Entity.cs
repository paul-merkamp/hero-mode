using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float moveSpeed;
    public int health;
    public float iFrameLength;
    public AudioClip takeDamageSFX;
    public float knockbackForce = 0.1f;

    protected SpriteRenderer sprite;
    protected AudioSource sfx;

    private bool invincible = false;

    public void Start()
    {
        transform.Find("Sprite").TryGetComponent<SpriteRenderer>(out sprite);
        sfx = GameObject.Find("SFX/SFX_Entity").GetComponent<AudioSource>();
    }

    public void Move(float horizontalInput, float verticalInput){
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Flip
        if (horizontalInput < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (horizontalInput > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Flip
        if (direction.x < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (direction.x > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void TakeDamage(int damage, GameObject collision = null)
    {
        if (!invincible)
        {
            health -= damage;
            StartCoroutine(DoIFrames());

            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            if (rb2d != null && collision != null)
            {
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                rb2d.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator DoIFrames()
    {
        sfx.PlayOneShot(takeDamageSFX);
        
        invincible = true;

        sprite.color = Color.red;
        yield return new WaitForSeconds(iFrameLength);
        sprite.color = Color.white;

        invincible = false;

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
