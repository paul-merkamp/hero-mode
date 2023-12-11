using UnityEngine;
using TMPro;

public class SurvivalSectionController : MonoBehaviour
{
    public int timeLimit;
    public TMP_Text timerFGText;
    public TMP_Text timerBGText;
    public TMP_Text scoreText;
    public GameEventController gameEventController;

    public int score;
    public int prizeScoreMin;

    private float timer;

    public bool started;
    private bool triggered = false;

    public void Start()
    {
        gameEventController = FindAnyObjectByType<GameEventController>();
        timer = timeLimit;
    }

    public void Update()
    {
        if (started && !triggered)
        {
            timer -= Time.deltaTime;
            timerFGText.text = timer.ToString("F0");
            timerBGText.text = timer.ToString("F0");

            if (score < prizeScoreMin)
                scoreText.text = "Score: " + score.ToString() + " / " + prizeScoreMin.ToString();
            else
                scoreText.text = "Score: <color=#25bb22>" + score.ToString() + " / " + prizeScoreMin.ToString() + "</color>";

            if (timer <= 10)
            {
                timerFGText.color = Color.red;
            }

            if (timer <= 0)
            {
                StopAllVampireSpawners();
                
                if (score == 0)
                    gameEventController.TriggerEvent_8();
                else if (score < prizeScoreMin)
                    gameEventController.TriggerEvent_9();
                else
                    gameEventController.TriggerEvent_10();

                triggered = true;

                GameObject.Find("UI/UI_SurvivalDisplay").transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void StopAllVampireSpawners()
    {
        GameObject vampireSpawners = GameObject.Find("ENTITY/VampireSpawners");
        if (vampireSpawners != null)
        {
            Spawner[] spawners = vampireSpawners.GetComponentsInChildren<Spawner>();
            foreach (Spawner spawner in spawners)
            {
                spawner.DestroyAllSpawnedObjects();
            }
        }
    }

    public void StartSection()
    {
        if (!started)
        {
            started = true;
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
    }
}