using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakoHandAI : Entity
{
    private BakoAI bako;

    private int layerMask;
    private Transform player;
    public bool active = false;

    public Sprite handSprite_normal;
    public Sprite handSprite_punch;
    public Sprite handSprite_rock;
    public Sprite handSprite_bomb;

    public GameObject rockPrefab;
    public GameObject bombPrefab;

    private new void Start()
    {
        bako = transform.parent.GetComponent<BakoAI>();

        player = GameObject.Find("ENTITY/PlayerFollower").transform;
        TryGetComponent<Animator>(out animator);

        int enemyMask = 1 << LayerMask.NameToLayer("damaging_entity");
        int coverMask = 1 << LayerMask.NameToLayer("vision_cover");
        int ignoreRaycastMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        int lavaMask = 1 << LayerMask.NameToLayer("damaging_surface");

        layerMask = ~(enemyMask | coverMask | ignoreRaycastMask | lavaMask);

        sfx = GameObject.Find("SFX/SFX_Entity").GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        rockPrefab = Resources.Load<GameObject>("Prefabs/ENV/Dungeon1/Rock");
        bombPrefab = Resources.Load<GameObject>("Prefabs/ENTITY/Bomb");
    }

    private Vector3 initialPosition;

    public void StartAttacking()
    {
        active = true;
        StartCoroutine(AttackRoutine());
    }

    private enum AttackType
    {
        None,
        Punch,
        Rock,
        Bomb
    }

    private bool attackCancelled = false;

    private IEnumerator AttackRoutine()
    {
        while (active)
        {
            attackCancelled = false;
            initialPosition = transform.position;

            AttackType attack = (AttackType)Random.Range(1, 4);
            SetSprite(attack);
            
            Vector2 targetPosition = new Vector2();

            if (attack == AttackType.Punch)
            {
                targetPosition = FindPlayer();
                if (targetPosition == Vector2.zero)
                {
                    float randomAngle = Random.Range(0f, 2f * Mathf.PI);
                    targetPosition = new Vector2(
                        bako.transform.position.x + 1.5f * Mathf.Cos(randomAngle),
                        bako.transform.position.y + 1.5f * Mathf.Sin(randomAngle)
                    );
                }

                moveTowardsCoroutine = StartCoroutine(MoveTowardsCoroutine(targetPosition, 1.4f));
                yield return new WaitUntil(() => moveTowardsCoroutine == null);
                yield return new WaitForSeconds(1f);
                SetSprite(AttackType.None);
            }
            if (attack == AttackType.Rock)
            {
                float randomAngle = Random.Range(0f, 2f * Mathf.PI);
                targetPosition = new Vector2(
                    bako.transform.position.x + 1.5f * Mathf.Cos(randomAngle),
                    bako.transform.position.y + 1.5f * Mathf.Sin(randomAngle)
                );

                moveTowardsCoroutine = StartCoroutine(MoveTowardsCoroutine(targetPosition, 1.0f));
                yield return new WaitUntil(() => moveTowardsCoroutine == null);
                yield return new WaitForSeconds(0.5f);

                if (!attackCancelled)
                {
                    GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
                    SetSprite(AttackType.None);

                    yield return new WaitForSeconds(0.5f);
                }
            }

            else if (attack == AttackType.Bomb)
            {
                targetPosition = FindPlayer();
                if (targetPosition == Vector2.zero)
                {
                    float randomAngle = Random.Range(0f, 15f) * Mathf.Deg2Rad;
                    targetPosition = new Vector2(
                        transform.position.x + 1.4f * Mathf.Cos(randomAngle),
                        transform.position.y + 1.4f * Mathf.Sin(randomAngle)
                    );
                }
                
                moveTowardsCoroutine = StartCoroutine(MoveTowardsCoroutine(targetPosition, 1.0f));
                yield return new WaitUntil(() => moveTowardsCoroutine == null);
                yield return new WaitForSeconds(0.5f);

                if (!attackCancelled)
                {
                    GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
                    SetSprite(AttackType.None);
                    yield return new WaitForSeconds(0.5f);
                }
            }

            moveBackCoroutine = StartCoroutine(MoveBackCoroutine(initialPosition));
            yield return new WaitUntil(() => moveBackCoroutine == null);

            float delay = Random.Range(1f, 3f);
            yield return new WaitForSeconds(delay);
        }
    }

    private Coroutine moveTowardsCoroutine;
    private Coroutine moveBackCoroutine;

    private IEnumerator MoveTowardsCoroutine(Vector2 targetPosition, float speedMultiplier = 1f)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime * speedMultiplier);
            yield return null;
        }

        moveTowardsCoroutine = null;
    }

    private IEnumerator MoveBackCoroutine(Vector3 startPosition, float speedMultiplier = 1f)
    {
        while (Vector3.Distance(transform.position, startPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime * speedMultiplier);
            yield return null;
        }

        moveBackCoroutine = null;
    }

    public void CancelMoveTowards()
    {
        StopCoroutine(moveTowardsCoroutine);
        moveTowardsCoroutine = null;
        StartMoveBack(2f);
    }

    public void StartMoveBack(float speedMultiplier = 1f)
    {
        moveBackCoroutine = StartCoroutine(MoveBackCoroutine(initialPosition, speedMultiplier));
    }

    public override void TakeDamage(int damage, GameObject collision = null)
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

            CancelMoveTowards();
            attackCancelled = true;
        }
    }

    public new IEnumerator DoIFrames()
    {
        if (animator != null)
        {
            animator.SetBool("Hurt", true);
        }

        if (sfx != null)
        {
            sfx.PlayOneShot(takeDamageSFX);
        }
        
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

    public override void Die()
    {
        bako.IncrementDeadHands();

        Destroy(gameObject);
    }

    private Vector2 FindPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y += 0.03f;
        direction.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);
        Debug.DrawLine(transform.position, hit.point, Color.blue);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return player.position;
        }

        return Vector2.zero;
    }

    private void SetSprite(AttackType attack)
    {
        switch (attack)
        {
            case AttackType.None:
                sprite.sprite = handSprite_normal;
                break;
            case AttackType.Punch:
                sprite.sprite = handSprite_punch;
                break;
            case AttackType.Rock:
                sprite.sprite = handSprite_rock;
                break;
            case AttackType.Bomb:
                sprite.sprite = handSprite_bomb;
                break;
            default:
                sprite.sprite = handSprite_normal;
                break;
        }
    }
}
