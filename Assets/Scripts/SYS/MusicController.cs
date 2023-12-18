using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource source;

    public float fadeDuration = 1f;

    private float maxVolume;

    public AudioClip deathSong;
    

    private void Start()
    {
        maxVolume = source.volume;
    }

    public void CutToSong(AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void PlaySong(AudioClip clip)
    {
        StartCoroutine(PlaySongCoroutine(clip));
    }

    public IEnumerator PlaySongCoroutine(AudioClip clip)
    {
        yield return FadeOutCoroutine();

        source.clip = clip;
        source.Play();
    }
    
    public void PlaySequentially(AudioClip clip, AudioClip nextClip, bool hardCut = false)
    {
        StopAllCoroutines();
        StartCoroutine(PlaySequentiallyCoroutine(clip, nextClip, hardCut));
    }

    public IEnumerator PlaySequentiallyCoroutine(AudioClip clip, AudioClip nextClip, bool hardCut)
    {
        if (!hardCut)
            yield return FadeOutCoroutine();
        else    
            source.Stop();

        source.volume = maxVolume;

        source.clip = clip;
        source.loop = false;
        source.Play();

        while (source.isPlaying)
        {
            yield return null;
        }

        source.clip = nextClip;
        source.loop = true;
        source.Play();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (source.isPlaying)
        {
            float startVolume = source.volume;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }

            source.Stop();
        }

        yield return null;
    }

    public AudioSource sfx;
    public AudioClip hpUp;

    public void PlayHPUp()
    {
        StartCoroutine(PlayHPUpCoroutine());
    }

    private IEnumerator PlayHPUpCoroutine()
    {
        float originalVolume = source.volume;
        source.volume = 0;
        sfx.PlayOneShot(hpUp);

        yield return new WaitForSeconds(hpUp.length);

        source.volume = maxVolume;
    }
}