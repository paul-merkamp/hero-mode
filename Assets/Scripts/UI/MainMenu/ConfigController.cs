using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfigController : MonoBehaviour
{
    private int originalScreenWidth = 320;
    private int originalScreenHeight = 180;

    public TMP_Text screenSizeText;
    public TMP_Text fullscreenText;

    public GameObject music;
    public List<GameObject> sfx;
    
    public void Start()
    {
        music = GameObject.Find("SFX/SFX_Music");
        GameObject sfxParent = GameObject.Find("SFX");

        for (int i=0; i<sfxParent.transform.childCount; i++)
        {
            if (sfxParent.transform.GetChild(i).gameObject != music)
            {
                sfx.Add(sfxParent.transform.GetChild(i).gameObject);
            }
        }

        UpdateScreenSizeText();
        UpdateFullscreenText();
        UpdateAudioText();

        UpdateAudioSources();
    }

    public void ToggleFullscreen()
    {
        Settings.isFullscreen = !Settings.isFullscreen;
        Screen.fullScreen = Settings.isFullscreen;
        
        UpdateFullscreenText();
    }

    public void SetScreenSizeMultiplier(float mult)
    {
        float newWidth = originalScreenWidth * mult;
        float newHeight = originalScreenHeight * mult;
        Screen.SetResolution((int)newWidth, (int)newHeight, Settings.isFullscreen);
    }

    public void SwitchScreenSizeMultiplier()
    {
        Settings.screenMultiplier += 2f;
        if (Settings.screenMultiplier > 8f)
        {
            Settings.screenMultiplier = 2f;
        }
        SetScreenSizeMultiplier(Settings.screenMultiplier);

        UpdateScreenSizeText();
    }

    public void UpdateScreenSizeText()
    {
        screenSizeText.text = "Screen Size: " + Settings.screenMultiplier + "x";
    }

    public void UpdateFullscreenText()
    {
        if (Settings.isFullscreen)
        {
            fullscreenText.text = "Fullscreen: On";
        }
        else
        {
            fullscreenText.text = "Fullscreen: Off";
        }
    }

    public TMP_Text musicText;
    public TMP_Text sfxText;

    public void ToggleMusic()
    {
        Settings.musicOn = !Settings.musicOn;
        UpdateAudioText();

        UpdateAudioSources();
    }

    public void ToggleSFX()
    {
        Settings.sfxOn = !Settings.sfxOn;
        UpdateAudioText();

        UpdateAudioSources();
    }

    public void UpdateAudioSources()
    {
        if (music == null)
        {
            Debug.LogError("No music source found!");
        }

        if (Settings.musicOn)
        {
            music.SetActive(true);
        }
        else
        {
            music.SetActive(false);
        }

        foreach (GameObject sfx in sfx)
        {
            if (sfx == null)
            {
                Debug.LogError("No sfx source found!");
            }

            if (Settings.sfxOn)
            {
                sfx.SetActive(true);
            }
            else
            {
                sfx.SetActive(false);
            }
        }
    }

    public void UpdateAudioText()
    {
        if (Settings.musicOn)
        {
            musicText.text = "Music: On";
        }
        else
        {
            musicText.text = "Music: Off";
        }
        
        if (Settings.sfxOn)
        {
            sfxText.text = "SFX: On";
        }
        else
        {
            sfxText.text = "SFX: Off";
        }
    }
}