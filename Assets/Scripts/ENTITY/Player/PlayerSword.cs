using System.Collections;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{

    private AudioSource sfx;
    public AudioClip swingSFX;

    private Animator animator;

    private bool thisDoesSFX = true;
    private bool attacking = false;

    public void Start()
    {
        sfx = GameObject.Find("SFX/SFX_Player").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    
    public void Attack()
    {
        animator.Play("Attack_Clip");

        if (thisDoesSFX)
        {
            sfx.PlayOneShot(swingSFX);
        }

        attacking = true;
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        attacking = false;
        thisDoesSFX = true;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (attacking)
        {
            if (collider.gameObject.CompareTag("sign") || collider.gameObject.CompareTag("destructible"))
            {
                SimpleDestructible destructible = collider.gameObject.GetComponent<SimpleDestructible>();
                if (destructible != null)
                {
                    destructible.GetDestroyed();
                }
            }
            else if (collider.gameObject.CompareTag("entity"))
            {
                Entity entity = collider.gameObject.GetComponent<Entity>();
                if (entity != null)
                {
                    entity.TakeDamage(1);
                }
            }
            thisDoesSFX = false;
        }
    }
}
