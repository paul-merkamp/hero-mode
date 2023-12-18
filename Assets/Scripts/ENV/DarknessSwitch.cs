using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessSwitch : MonoBehaviour
{
    public GameObject toggledObject;
    private SpriteRenderer sprite;

    private AudioSource sfx;
    private DialogController dialogController;
    public AudioClip switchClip;

    public Sprite offSprite;
    public Sprite onSprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sfx = GameObject.Find("SFX/SFX_Environment").GetComponent<AudioSource>();
    }

    public void Toggle()
    {
        toggledObject.SetActive(!toggledObject.activeSelf);
        sprite.sprite = toggledObject.activeSelf ? onSprite : offSprite;
        sfx.PlayOneShot(switchClip);
    }
}
