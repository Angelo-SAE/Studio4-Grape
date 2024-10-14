using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;

    [Header("UI")]
    [SerializeField] private TMP_Text[] keyText;
    [SerializeField] private GameObject setKeyPanel;

    private int keyToChange;
    private bool isChangingKey;
    private Event keyEvent;

    private void Start()
    {
        for(int a = 0; a < keyText.Length; a++)
        {
            keyText[a].text = keyBindings.keyArray[a].ToString();
        }
    }

    private void OnGUI()
    {
        if(isChangingKey)
        {
            keyEvent = Event.current;
            if(keyEvent is not null)
            {
                CheckForValidEvent();
            }
        }
    }

    private void CheckForValidEvent()
    {
        if(keyEvent.isKey)
        {
            if(keyEvent.keyCode == KeyCode.Escape)
            {
                isChangingKey = false;
                setKeyPanel.SetActive(false);
                return;
            }
            keyBindings.keyArray[keyToChange] = keyEvent.keyCode;
            keyText[keyToChange].text = keyBindings.keyArray[keyToChange].ToString();
            isChangingKey = false;
            keyBindings.UpdateKeyCodes();
            StartCoroutine(RemovePanel());
        } else {
            if(keyEvent.isMouse)
            {
                if(keyEvent.button == 0)
                {
                    keyBindings.keyArray[keyToChange] = KeyCode.Mouse0;
                } else if(keyEvent.button == 1)
                {
                    keyBindings.keyArray[keyToChange] = KeyCode.Mouse1;
                } else if(keyEvent.button == 2)
                {
                    keyBindings.keyArray[keyToChange] = KeyCode.Mouse2;
                } else if(keyEvent.button == 3)
                {
                    keyBindings.keyArray[keyToChange] = KeyCode.Mouse3;
                } else if(keyEvent.button == 4)
                {
                    keyBindings.keyArray[keyToChange] = KeyCode.Mouse4;
                } else {
                    return;
                }
                keyText[keyToChange].text = keyBindings.keyArray[keyToChange].ToString();
                isChangingKey = false;
                keyBindings.UpdateKeyCodes();
                StartCoroutine(RemovePanel());
            }
        }
    }

    private IEnumerator RemovePanel()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        setKeyPanel.SetActive(false);
    }

    public void ChangeKey(int keyNumber)
    {
        isChangingKey = true;
        keyToChange = keyNumber;
        setKeyPanel.SetActive(true);
    }
}
