using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BakoAI : Entity
{
    private GameEventController gameEventController;

    public List<BakoHandAI> hands;

    public List<GameObject> plates;

    public List<GameObject> phase2FinalObstacles;

    public List<GameObject> phase3DisableObstacles;

    public List<GameObject> phase3EnableObstacles;

    private int phase = 1;

    public int handsKilled = 0;

    public MusicController music;

    public AudioClip phase1;
    public AudioClip phase1Pass2;
    public AudioClip phase2;
    public AudioClip phase2Pass2;
    public AudioClip phase3;
    public AudioClip phase3Pass2;

    private GameObject playerFollower;

    private ParticleSystem deathParticles;
    
    public GameObject normalFace;
    public GameObject scaryFace;

    public new void Start()
    {
        gameEventController = FindAnyObjectByType<GameEventController>();

        sprite = GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX/SFX_Entity").GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        rb2d = GetComponent<Rigidbody2D>();

        playerFollower = GameObject.Find("ENTITY/PlayerFollower");

        music = GameObject.Find("SYS/SYS_MusicController").GetComponent<MusicController>();

        deathParticles = GetComponentInChildren<ParticleSystem>();
    }

    public bool skipPhase1;

    public void StartPhase1()
    {
        if (skipPhase1)
        {
            foreach (BakoHandAI hand in hands)
            {
                Destroy(hand.gameObject);
            }

            StartPhase2();
            return;
        }

        music.PlaySequentially(phase1, phase1Pass2, true);

        foreach (BakoHandAI hand in hands)
        {
            hand.StartAttacking();
        }
    }

    public bool skipPhase2;

    public void StartPhase2()
    {
        if (skipPhase2)
        {
            foreach (BakoHandAI hand in hands)
            {
                Destroy(hand.gameObject);
            }

            StartPhase3();
            return;
        }

        plates[0].SetActive(true);

        music.PlaySequentially(phase2, phase2Pass2, true);

        StartCoroutine(DelaySecondPlate(6f));
    }

    public IEnumerator DelaySecondPlate(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        plates[1].SetActive(true);
    }

    public GameObject phase2Part2GameObject;

    public void EnablePhase2Part2()
    {
        phase2Part2GameObject.SetActive(true);
    }

    public GameObject phase2Part3GameObject;

    public void EnablePhase2Part3()
    {
        phase2Part3GameObject.SetActive(true);
    }

    public void StartPhase3()
    {
        normalFace.SetActive(false);
        scaryFace.SetActive(true);

        phase = 3;

        health = 3;

        foreach (GameObject obstacle in phase3DisableObstacles)
        {
            obstacle.SetActive(false);
        }

        foreach (GameObject obstacle in phase3EnableObstacles)
        {
            obstacle.SetActive(true);
        }

        music.PlaySequentially(phase3, phase3Pass2, true);

        StartCoroutine(Phase3());

        Camera.main.GetComponent<Volume>().enabled = true;
    }
    
    public IEnumerator Phase3()
    {
        while (true)
        {
            yield return StartCoroutine(Phase3Dash());
            yield return StartCoroutine(Phase3Bombing());
        }
    }

    public GameObject bakoBigFireballPrefab;
    private float phase3FireballDelay = 0.3f;
    public float dashSpeed = 1.5f;

    public IEnumerator Phase3Dash()
    {
        // Determine the target position (up, down, left, right) at random
        Vector2[] targetPositions = GetRandomPositions();

        foreach (Vector2 targetPosition in targetPositions)
        {
            // Lerp to the target position
            float lerpDuration = dashSpeed;
            float lerpTime = 0.0f;
            Vector2 startPosition = transform.position;

            while (lerpTime < lerpDuration)
            {
                lerpTime += Time.deltaTime;
                float t = Mathf.Clamp01(lerpTime / lerpDuration);
                transform.position = Vector2.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            // Fire fireballs at the player
            for (int i = 0; i < 5; i++)
            {
                GameObject fireball = Instantiate(bakoBigFireballPrefab, transform.position, Quaternion.identity);
                fireball.GetComponent<BakoBigFireball>().SetTarget(playerFollower.transform.position);
                yield return new WaitForSeconds(phase3FireballDelay);
            }
        }

        yield return new WaitForSeconds(3.0f);
    }

    public GameObject bakoBombPrefab;

    public IEnumerator Phase3Bombing()
    {
        Vector2 targetPosition = new Vector2(-3.505f, -0.75f);

        // Lerp to the target position
        float lerpDuration = dashSpeed;
        float lerpTime = 0.0f;
        Vector2 startPosition = transform.position;

        while (lerpTime < lerpDuration)
        {
            lerpTime += Time.deltaTime;
            float t = Mathf.Clamp01(lerpTime / lerpDuration);
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        float radius1 = 0.3f;
        float radius2 = 0.6f;
        float radius3 = 0.9f;
        int numBombs = 5;
        
        float angleIncrement = 360.0f / numBombs;

        for (int i = 0; i < numBombs; i++)
        {
            float angle = i * angleIncrement;
            Vector2 spawnPosition = targetPosition + (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right * radius1);
            Instantiate(bakoBombPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < numBombs; i++)
        {
            float angle = i * angleIncrement;
            Vector2 spawnPosition = targetPosition + (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right * radius2);
            Instantiate(bakoBombPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < numBombs; i++)
        {
            float angle = i * angleIncrement;
            Vector2 spawnPosition = targetPosition + (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right * radius3);
            Instantiate(bakoBombPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);
    }

    private Vector2[] GetRandomPositions()
    {
        // Define the possible target positions (up, down, left, right)
        Vector2 phase3UpPosition = new Vector2(-3.505f, 0.65f);
        Vector2 phase3DownPosition = new Vector2(-3.505f, -2.19f);
        Vector2 phase3LeftPosition = new Vector2(-4.835f, -0.75f);
        Vector2 phase3RightPosition = new Vector2(-2.155f, -0.75f);

        // Create a list of possible target positions
        List<Vector2> possiblePositions = new List<Vector2> { phase3UpPosition, phase3DownPosition, phase3LeftPosition, phase3RightPosition };

        // Shuffle the list using Fisher-Yates algorithm
        System.Random random = new System.Random();
        int n = possiblePositions.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Vector2 value = possiblePositions[k];
            possiblePositions[k] = possiblePositions[n];
            possiblePositions[n] = value;
        }

        // Convert the list to an array and return it
        return possiblePositions.ToArray();
    }

    public void IncrementDeadHands()
    {
        handsKilled++;

        if (handsKilled == 4)
        {
            gameEventController.TriggerEvent_13();
        }
    }

    public override void TakeDamage(int damage, GameObject collision = null)
    {
        if (!invincible)
        {
            health -= damage;
            StartCoroutine(DoIFrames());

            if (health == 5)
            {
                EnablePhase2Part2();
            }
            
            if (health == 4)
            {
                EnablePhase2Part3();
            }
            if (health == 3)
            {
                gameEventController.TriggerEvent_14();
            }

            if (health == 0)
            {
                Die();
            }
        }
    }

    public override void Die()
    {
        StopAllCoroutines();

        normalFace.SetActive(true);
        scaryFace.SetActive(false);

        music.FadeOut();

        Camera.main.GetComponent<Volume>().enabled = false;
        gameEventController.TriggerEvent_4();
    }

    public Animator endAnimator;

    public IEnumerator DeathAnimation()
    {
        deathParticles.Play();
        sprite.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false); // disable shadow
        transform.GetChild(1).gameObject.SetActive(false); // disable face

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        endAnimator.SetTrigger("Ending");
    }
}
