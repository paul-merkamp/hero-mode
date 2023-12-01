using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public static GameMode selectedMode = GameMode.None;

    public enum GameMode
    {
        None,
        Baby,
        Normal
    } 
}
