using System.Collections;
using UnityEngine;

public class CardAcquiredDisplay : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    private float fadeTime = 1f;

    private UIcardDisplayScript cardDisplayScript;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cardDisplayScript = GetComponentInChildren<UIcardDisplayScript>();
        canvasGroup.alpha = 0;
    }

    public void Display(int _cardIndex)
    {
        cardDisplayScript._cardIndex = _cardIndex;
        cardDisplayScript.gameObject.SetActive(true);
        StartCoroutine(ShowAndFade());
    }

    private IEnumerator ShowAndFade()
    {
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(3f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}