using System.Collections;
using UnityEngine;

public class StartPopup : MonoBehaviour
{
    public GameObject startMessageUI;
    public float displayDuration = 2f;
    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private void Start()
    {
        if (startMessageUI != null)
        {
            canvasGroup = startMessageUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = startMessageUI.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 1f;
            startMessageUI.SetActive(true);
            StartCoroutine(FadeOutAfterDelay());
        }
        else
        {
            Debug.LogWarning("StartMessageUI is not assigned.");
        }
    }

    private IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        startMessageUI.SetActive(false);
    }
}

