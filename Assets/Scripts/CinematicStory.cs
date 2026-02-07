using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CinematicSlidesPro_Fixed : MonoBehaviour
{
    [Header("UI")]
    public Image background;
    public Image fadePanel;
    public TextMeshProUGUI runText;

    [Header("Slides")]
    public Sprite[] backgrounds;
    public float slideDuration = 5f;
    public float zoomSpeed = 0.02f;

    [Header("Audio")]
    public AudioSource storyAudio;
    public AudioSource bgMusic;
    public float musicFadeSpeed = 1.5f;

    bool skipping = false;
    bool runShown = false;

    void Start()
    {
        runText.gameObject.SetActive(false);

        storyAudio.Play();

        StartCoroutine(FadeMusicIn());
        StartCoroutine(PlaySlides());
        StartCoroutine(AudioWatcher()); // ⭐ THIS FIXES EVERYTHING
    }

    // ⭐ Slides run independently
    IEnumerator PlaySlides()
    {
        yield return Fade(1, 0, 2f);

        int i = 0;

        while (!skipping)
        {
            background.sprite = backgrounds[i];
            background.rectTransform.localScale = Vector3.one;

            float timer = 0;

            while (timer < slideDuration)
            {
                timer += Time.deltaTime;

                background.rectTransform.localScale +=
                    Vector3.one * zoomSpeed * Time.deltaTime;

                yield return null;
            }

            i = (i + 1) % backgrounds.Length; // loop safely
        }
    }

    // ⭐ WATCH AUDIO IN PARALLEL (PRO METHOD)
    IEnumerator AudioWatcher()
    {
        // Wait until near the end
        while (storyAudio.isPlaying && storyAudio.time < storyAudio.clip.length - 0.4f)
        {
            yield return null;
        }

        if (!runShown)
        {
            runShown = true;
            ShowRunImpact();
        }

        yield return new WaitForSeconds(3f);

        yield return FadeMusicOut();
        yield return Fade(0, 1, 1.5f);

        SceneManager.LoadScene("GameScene");
    }

    // 🔥 RUN IMPACT
    void ShowRunImpact()
    {
        runText.gameObject.SetActive(true);
        runText.alpha = 1;

        StartCoroutine(RunImpactAnimation());
    }

    IEnumerator RunImpactAnimation()
    {
        RectTransform rect = runText.rectTransform;

        Vector3 start = Vector3.one * 0.5f;
        Vector3 overshoot = Vector3.one * 1.3f;
        Vector3 normal = Vector3.one;

        rect.localScale = start;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 6f;
            rect.localScale = Vector3.Lerp(start, overshoot, t);
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 4f;
            rect.localScale = Vector3.Lerp(overshoot, normal, t);
            yield return null;
        }
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float t = 0;

        while (t < duration)
        {
            float a = Mathf.Lerp(start, end, t / duration);
            fadePanel.color = new Color(0, 0, 0, a);

            t += Time.deltaTime;
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, end);
    }

    IEnumerator FadeMusicIn()
    {
        bgMusic.volume = 0;
        bgMusic.Play();

        while (bgMusic.volume < 0.5f)
        {
            bgMusic.volume += Time.deltaTime * musicFadeSpeed;
            yield return null;
        }
    }

    IEnumerator FadeMusicOut()
    {
        while (bgMusic.volume > 0)
        {
            bgMusic.volume -= Time.deltaTime * musicFadeSpeed;
            yield return null;
        }

        bgMusic.Stop();
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !skipping)
        {
            skipping = true;
            SceneManager.LoadScene("GameScene");
        }
    }
}
