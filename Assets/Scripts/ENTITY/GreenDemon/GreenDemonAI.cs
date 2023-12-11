using System.Collections;
using UnityEngine;

public class GreenDemonAI : Entity
{
    private GameEvent phase2Event;
    private GameEvent defeatedEvent;

    public GameObject disabledOnDefeat;

    public GameObject greenFirePrefab;
    public GameObject greenFireHorizontalPrefab;
    public Vector2 verticalSpawnPositionMin;
    public Vector2 verticalSpawnPositionMax;

    public Vector2 horizontalSpawnPositionMin1;
    public Vector2 horizontalSpawnPositionMax1;
    public Vector2 horizontalSpawnPositionMin2;
    public Vector2 horizontalSpawnPositionMax2;

    public float verticalSpawnInterval = 2f;
    public float horizontalSpawnInterval = 5f;

    private bool activePhase1;
    private bool activePhase2;

    private ParticleSystem deathParticles;

    public new void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX/SFX_Entity").GetComponent<AudioSource>();

        defeatedEvent = GetComponent<GameEvent>();
        phase2Event = GameObject.Find("EVENT/Event_6").GetComponent<GameEvent>();

        TryGetComponent<Animator>(out animator);

        deathParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void StartAttacking()
    {
        activePhase1 = true;
        StartCoroutine(SpawnGreenFireVertical());
    }

    private IEnumerator SpawnGreenFireVertical()
    {
        while (activePhase1)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(verticalSpawnPositionMin.x, verticalSpawnPositionMax.x), Random.Range(verticalSpawnPositionMin.y, verticalSpawnPositionMax.y));
            Instantiate(greenFirePrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(verticalSpawnInterval);
        }
    }

    public void StartPhase2()
    {
        phase2Event.Trigger();
        activePhase2 = true;
        StartCoroutine(SpawnGreenFireHorizontal());
    }

    private IEnumerator SpawnGreenFireHorizontal()
    {
        while (activePhase2)
        {
            Vector2 spawnPosition1 = new Vector2(Random.Range(horizontalSpawnPositionMin1.x, horizontalSpawnPositionMax1.x), Random.Range(horizontalSpawnPositionMin1.y, horizontalSpawnPositionMax1.y));
            Vector2 spawnPosition2 = new Vector2(Random.Range(horizontalSpawnPositionMin2.x, horizontalSpawnPositionMax2.x), Random.Range(horizontalSpawnPositionMin2.y, horizontalSpawnPositionMax2.y));
            Instantiate(greenFireHorizontalPrefab, spawnPosition1, Quaternion.identity);
            Instantiate(greenFireHorizontalPrefab, spawnPosition2, Quaternion.identity);
            yield return new WaitForSeconds(horizontalSpawnInterval);
        }
    }

    // Die starts the process
    public override void Die()
    {
        activePhase1 = false;
        activePhase2 = false;
        defeatedEvent.Trigger();
    }

    // DeathAnimation is called when the demon dies
    public void DeathAnimation()
    {
        StartCoroutine(DeathAnimationCoroutine());
    }

    IEnumerator DeathAnimationCoroutine()
    {
        deathParticles.Play();
        sprite.enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    // This is called later in the event
    public void DisableOnDefeat()
    {
        disabledOnDefeat.SetActive(false);
    }

    public override void TakeDamage(int damage, GameObject collision = null)
    {
        if (!invincible)
        {
            health -= damage;
            StartCoroutine(DoIFrames());

            if (health == 1)
            {
                StartPhase2();
            }
        }
    }
}