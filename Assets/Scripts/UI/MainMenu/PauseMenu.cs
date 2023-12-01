using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject canvas;

    public void Pause()
    {
        canvas.SetActive(true);
    }

    public void Resume()
    {
        canvas.SetActive(false);
    }
}
