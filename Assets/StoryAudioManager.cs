using UnityEngine;
using System.Collections;

public class StoryAudioManager : MonoBehaviour
{
    public AudioSource bgMusic;
    public AudioClip storyMusic;

    public float fadeSpeed = 1.5f;

    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        bgMusic.clip = storyMusic;
        bgMusic.volume = 0;
        bgMusic.Play();

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (bgMusic.volume < 1)
        {
            bgMusic.volume += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (bgMusic.volume > 0)
        {
            bgMusic.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        bgMusic.Stop();
    }
}
