using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinematicStoryUltra : MonoBehaviour
{
    [Header("UI")]
    public Image background;
    public Image fadePanel;
    public TextMeshProUGUI storyText;

    [Header("Slides")]
    public Sprite[] backgrounds;

    string[,] storyLines =
    {
        { "It was just another peaceful \nday.", "Nothing felt out of place." },
        { "Then something appeared \nin the sky.", "Silent... yet impossible \nto ignore." },
        { "In a single moment, \nthe world stopped.", "Time itself seemed frozen." },
        { "Everyone was trapped \nin stillness.", "Except you." }
    };

    void Start()
    {
        StartCoroutine(PlayCinematic());
    }

    IEnumerator PlayCinematic()
    {
        yield return Fade(1, 0, 2f);

        for (int i = 0; i < storyLines.GetLength(0); i++)
        {
            background.sprite = backgrounds[i];

            yield return TypeSentence(storyLines[i, 0]);
            yield return new WaitForSeconds(1f);

            yield return TypeSentence(storyLines[i, 1]);
            yield return new WaitForSeconds(2f);
        }

        // 🔥 FINAL RUN MOMENT
        yield return TypeSentence("RUN", 150);


        yield return new WaitForSeconds(2.5f);

        yield return Fade(0, 1, 1.5f);

        SceneManager.LoadScene("GameScene");
    }

    // TYPEWRITER EFFECT
    IEnumerator TypeSentence(string sentence, float fontSize = 70)
    {
        storyText.fontSize = fontSize;

        storyText.text = "";

        float typingSpeed = 0.03f; // smaller = faster

        foreach (char letter in sentence)
        {
            storyText.text += letter;

            if (letter == '.' || letter == ',' || letter == '!' || letter == '?')
                yield return new WaitForSeconds(0.35f); // longer pause
            else
                yield return new WaitForSeconds(0.03f);
        }


        // Smart reading time
        yield return new WaitForSeconds(GetReadingTime(sentence));
    }

    float GetReadingTime(string sentence)
    {
        int words = sentence.Split(' ').Length;

        float baseTime = 2.5f;
        float extra = words * 0.3f;

        return Mathf.Max(baseTime, extra);
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

    void Update()
    {
        // TAP TO SKIP
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            StopAllCoroutines();
            SceneManager.LoadScene("GameScene");
        }
    }
}
