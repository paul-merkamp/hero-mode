using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public List<GameObject> activatedObjects;
    public List<GameObject> deactivatedObjects;

    public AudioClip music;
    public AudioClip music2;

    public MusicController musicController;

    public static GameMode selectedMode = GameMode.None;

    public enum GameMode
    {
        None,
        Baby,
        Normal
    }

    public void Activate()
    {
        foreach (GameObject obj in activatedObjects)
        {
            obj.SetActive(true);
        }

        musicController.PlaySequentially(music, music2);
    }

    public void Deactivate()
    {
        foreach (GameObject obj in deactivatedObjects)
        {
            obj.SetActive(false);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
