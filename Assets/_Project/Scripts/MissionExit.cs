using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MissionExit : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;
    [SerializeField] private IntObject currentSector;
    [SerializeField] private IntObject sectorCount;

    [Header("Events")]
    [SerializeField] private UnityEvent PauseGameEvent;
    [SerializeField] private UnityEvent EndEvent;
    [SerializeField] private UnityEvent ExitEvent;
    [SerializeField] private UnityEvent AfterFadeOutEvent;

    [Header("UI")]
    [SerializeField] private GameObject fadePanelObject;
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeSpeed;

    private void Awake()
    {
        currentSector.value = 0;
    }

    public void Exit()
    {
        PauseGameEvent.Invoke();
        if(currentSector.value < sectorCount.value)
        {
            currentSector.value++;
            StartCoroutine(ExitCoroutine());
        } else {
            StartCoroutine(EndCoroutine());
        }
    }

    private IEnumerator ExitCoroutine()
    {
        fadePanelObject.SetActive(true);
        for(int a = 0; a <= 100; a++)
        {
            yield return new WaitForSeconds(fadeSpeed/100f);
            fadePanel.color = new Color(0f, 0f, 0f, a/100f);
        }
        yield return new WaitForSeconds(0.01f);
        ExitEvent.Invoke();
        StartCoroutine(FadeOut());
    }

    private IEnumerator EndCoroutine()
    {
        for(int a = 0; a <= 100; a++)
        {
            yield return new WaitForSeconds(fadeSpeed/100f);
            fadePanel.color = new Color(0f, 0f, 0f, a/100f);
        }
        yield return new WaitForSeconds(0.01f);
        EndEvent.Invoke();
    }

    private IEnumerator FadeOut()
    {
        for(int a = 100; a >= 0; a--)
        {
            yield return new WaitForSeconds(fadeSpeed/100f);
            fadePanel.color = new Color(0f, 0f, 0f, a/100f);
        }
        fadePanelObject.SetActive(false);
        AfterFadeOutEvent.Invoke();
    }
}
