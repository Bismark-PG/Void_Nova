using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Intro : MonoBehaviour
{
    public Text NameText;
    public Text UniText;
    public float FadeDuration = 1.5f;
    public float DisplayDuration = 2.0f;
    public string NextSceneName = "Title Scene";

    private void Start()
    {
        StartCoroutine(FadeInAndShow());
    }

    private IEnumerator FadeInAndShow()
    {
        float Timer = 0f;
        Color NameC = NameText.color;
        Color UniC = UniText.color;

        while (Timer < FadeDuration)
        {
            float Alpha = 1 - (Timer / FadeDuration);
            NameText.color = new Color(NameC.r, NameC.g, NameC.b, 1 - Alpha);
            UniText.color = new Color(UniC.r, UniC.g, UniC.b, 1 - Alpha);

            Timer += Time.deltaTime;
            yield return null;
        }

        NameText.color = new Color(NameC.r, NameC.g, NameC.b, 1f);
        UniText.color = new Color(UniC.r, UniC.g, UniC.b, 1f);

        yield return new WaitForSeconds(DisplayDuration);

        SceneManager.LoadScene(NextSceneName);
    }
}