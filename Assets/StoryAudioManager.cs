using UnityEngine;
using TMPro;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI storyText;

    [TextArea(3, 8)]
    public string[] lines;

    public float typingSpeed = 0.04f;

    private int index;
    private bool isTyping;
    private Coroutine typingCoroutine;

    [Header("Typing Sound")]
    public AudioSource typingSource;
    public AudioClip typingClip;
    [Range(0f, 1f)]
    public float typingVolume = 0.2f;

    [Header("Background Music")]
    public AudioSource bgMusic;
    public AudioClip storyMusic;
    public float fadeSpeed = 1.5f;

    Coroutine fadeRoutine;

    void Start()
    {
        PlayMusic();
        typingCoroutine = StartCoroutine(TypeLine());
    }

    // ================= MUSIC =================

    void PlayMusic()
    {
        bgMusic.clip = storyMusic;
        bgMusic.volume = 0;
        bgMusic.Play();

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (bgMusic.volume < 1)
        {
            bgMusic.volume += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        bgMusic.volume = 1;
    }

    void StopMusic()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOut());
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

    // ================= TYPEWRITER =================

    IEnumerator TypeLine()
    {
        isTyping = true;
        storyText.text = "";

        foreach (char letter in lines[index])
        {
            storyText.text += letter;

            // Natural typing sound (not robotic)
            if (Random.value > 0.5f)
                typingSource.PlayOneShot(typingClip, typingVolume);

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void Update()
    {
        // Works on PC + Mobile
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                storyText.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            StoryFinished();
        }
    }

    // ================= END =================

    void StoryFinished()
    {
        StopMusic();

        Debug.Log("Story Finished");

        // OPTIONAL:
        // SceneManager.LoadScene("GameScene");
    }
}
