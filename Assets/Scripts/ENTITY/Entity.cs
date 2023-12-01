using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float moveSpeed;
    public int health;
    public float iFrameLength;
    public AudioClip takeDamageSFX;

    private SpriteRenderer sprite;
    private AudioSource sfx;

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

    public void TakeDamage(int damage){
        if (!invincible) {
            health -= damage;
            StartCoroutine(DoIFrames());
        }
    }

    IEnumerator DoIFrames()
    {
        if (takeDamageSFX != null)
            sfx.PlayOneShot(takeDamageSFX);
        
        invincible = true;

        sprite.color = Color.red;
        yield return new WaitForSeconds(iFrameLength);
        sprite.color = Color.white;

        invincible = false;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
