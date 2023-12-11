using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float moveSpeed;
    public int health;
    public float iFrameLength;
    public AudioClip takeDamageSFX;
    public float knockbackForce = 0.1f;
    public Color hurtColor = new(255, 121, 121);

    protected SpriteRenderer sprite;
    protected AudioSource sfx;
    protected Animator animator;

    public bool invincible = false;

    public float heartDropChance = 0f;
    public GameObject heartPrefab;

    public int survivalScoreValue = 0;

    private Rigidbody2D rb2d;
    private float originalDrag;

    public void Start()
    {
        transform.Find("Sprite").TryGetComponent<SpriteRenderer>(out sprite);
        sfx = GameObject.Find("SFX/SFX_Entity").GetComponent<AudioSource>();

        TryGetComponent<Animator>(out animator);

        rb2d = GetComponent<Rigidbody2D>();
        originalDrag = rb2d.drag;
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

    public virtual void TakeDamage(int damage, GameObject collision = null)
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

    public IEnumerator DoIFrames()
    {
        if (animator != null)
        {
            animator.SetBool("Hurt", true);
        }

        sfx.PlayOneShot(takeDamageSFX);
        
        invincible = true;

        sprite.color = hurtColor;
        yield return new WaitForSeconds(iFrameLength);
        sprite.color = Color.white;

        invincible = false;

        if (health <= 0)
        {
            Die();
        }
        else if (animator != null)
        {
            animator.SetBool("Hurt", false);
        }
    }

    public virtual void Die()
    {
        if (heartDropChance > 0 && Random.Range(0f, 1f) <= heartDropChance)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }

        if (survivalScoreValue != 0)
        {
            GameObject.Find("SYS/SYS_SurvivalSectionController").GetComponent<SurvivalSectionController>().AddScore(survivalScoreValue);
        }

        Destroy(gameObject);
    }

    public void MakeSlippery()
    {
        rb2d.drag = originalDrag/2;
    }

    public void MakeNotSlippery()
    {
        rb2d.drag = originalDrag;
    }
}
