using System.Collections;
using UnityEngine;
using TMPro;

public class MenuIntroController : MonoBehaviour
{
    [Header("Typing Text")]
    public TextMeshProUGUI introText;
    [TextArea]
    public string message;

    public float typingSpeed = 0.001f;

    [Header("UI")]
    public GameObject mainMenuUI;

    [Header("Audio")]
    public AudioSource bgMusic;

    void Start()
    {
        mainMenuUI.SetActive(false);
        introText.text = "";

        StartCoroutine(TypeMessage());
    }

    IEnumerator TypeMessage()
    {
        foreach (char letter in message)
        {
            introText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(1.5f);
        bgMusic.Play();
        // Wait after typing finishes
        yield return new WaitForSeconds(2.5f);

        // Hide intro text
        introText.gameObject.SetActive(false);

        // Show menu
        mainMenuUI.SetActive(true);

        // Start background music
        //bgMusic.Play();
    }
}
