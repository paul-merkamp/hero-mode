using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndCutsceneController : MonoBehaviour
{
    public TMP_Text deathsText;

    public GameObject prize;

    public void Start()
    {
        deathsText.text = "Player deaths: " + PlayerData.deathCount.ToString();

        if (PlayerData.deathCount == 0)
        {
            deathsText.text += "\n<color=#d7d51e>You're no baby... you're a true hero!</color>";
            prize.SetActive(true);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
