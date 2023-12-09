using System.Collections;
using UnityEngine;

public class GreenDemonAI : Entity
{
    private GameEvent defeatedEvent;

    public GameObject disabledOnDefeat;

    public GameObject greenFirePrefab;
    public Vector2 spawnPositionMin;
    public Vector2 spawnPositionMax;
    public float spawnInterval = 2f;

    private bool active;

    public new void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX/SFX_Entity").GetComponent<AudioSource>();

        defeatedEvent = GetComponent<GameEvent>();
    }

    public void StartAttacking()
    {
        active = true;
        StartCoroutine(SpawnGreenFire());
    }

    private IEnumerator SpawnGreenFire()
    {
        while (active)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(spawnPositionMin.x, spawnPositionMax.x), Random.Range(spawnPositionMin.y, spawnPositionMax.y));
            Instantiate(greenFirePrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Die starts the process
    public override void Die()
    {
        active = false;
        defeatedEvent.Trigger();
    }

    // DeathAnimation is called when the demon dies
    public void DeathAnimation()
    {
        gameObject.SetActive(false);
    }

    // This is called later in the event
    public void DisableOnDefeat()
    {
        disabledOnDefeat.SetActive(false);
    }
}