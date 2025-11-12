using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LoadingPanel : MonoBehaviour
{

    [SerializeField] private CanvasGroup loadingPanelCanvasGroup;
    public float fadeDuration = 1;


    Coroutine loadingRoutine;


    public void ShowLoadingPanel(UnityAction onLoading, UnityAction afterLoading)
    {
        if (loadingRoutine != null)
            StopCoroutine(StartLoadingPanel(onLoading, afterLoading));

        loadingRoutine = StartCoroutine(StartLoadingPanel(onLoading, afterLoading));
    }

    IEnumerator StartLoadingPanel(UnityAction onLoading, UnityAction afterLoading) 
    {
        loadingPanelCanvasGroup.interactable = true;
        loadingPanelCanvasGroup.blocksRaycasts = true;

        float timer = 0;

        while ((timer / fadeDuration) < 1)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, (timer / fadeDuration));
            loadingPanelCanvasGroup.alpha = alpha;
            yield return null;
        }
        onLoading?.Invoke();

        yield return new WaitForSeconds(0.5f);

        timer = 0;
        while ((timer / fadeDuration) < 1)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, (timer / fadeDuration));
            loadingPanelCanvasGroup.alpha = alpha;
            yield return null;
        }
        loadingPanelCanvasGroup.interactable = false;
        loadingPanelCanvasGroup.blocksRaycasts = false;
        afterLoading?.Invoke();
    }
}
