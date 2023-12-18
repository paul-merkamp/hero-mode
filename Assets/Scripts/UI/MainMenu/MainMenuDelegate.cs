using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDelegate : MonoBehaviour
{
    MainMenuController mainMenuController;

    private void Start()
    {
        mainMenuController = FindObjectOfType<MainMenuController>();
    }

    public void Activate()
    {
        mainMenuController.Activate();
    }

    public void Deactivate()
    {
        mainMenuController.Deactivate();
    }
}
