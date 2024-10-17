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
    [SerializeField] private TMP_Text rebindText;
    [SerializeField] private GameObject setKeyPanel;
    [SerializeField] private GameObject rebindKeyPanel;

    private int keyToChange;
    private int contestingKey;
    private KeyCode currentKeyCode;
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
            currentKeyCode = keyEvent.keyCode;
            isChangingKey = false;
            if(CheckForExistingKey())
            {
                rebindKeyPanel.SetActive(true);
                setKeyPanel.SetActive(false);
            } else {
                BindKey();
            }

        } else {
            if(keyEvent.isMouse)
            {
                switch(keyEvent.button)
                {
                    case(0):
                    currentKeyCode = KeyCode.Mouse0;
                    break;
                    case(1):
                    currentKeyCode = KeyCode.Mouse1;
                    break;
                    case(2):
                    currentKeyCode = KeyCode.Mouse2;
                    break;
                    case(3):
                    currentKeyCode = KeyCode.Mouse3;
                    break;
                    case(4):
                    currentKeyCode = KeyCode.Mouse4;
                    break;
                    case(5):
                    currentKeyCode = KeyCode.Mouse5;
                    break;

                    return;
                }
                isChangingKey = false;
                if(CheckForExistingKey())
                {
                    rebindKeyPanel.SetActive(true);
                    setKeyPanel.SetActive(false);
                } else {
                    BindKey();
                }
            }
        }
    }

    private bool CheckForExistingKey()
    {
        for(int a = 0; a < keyBindings.keyArray.Length; a++)
        {
            if(a != keyToChange)
            {
                if(keyBindings.keyArray[a] == currentKeyCode)
                {
                    rebindText.text = currentKeyCode.ToString() + " Is Already Binded Would You Like To Rebind This Key.";
                    contestingKey = a;
                    return true;
                }
            }
        }
        return false;
    }

    public void RebindKey()
    {
        keyBindings.keyArray[contestingKey] = KeyCode.None;
        keyText[contestingKey].text = keyBindings.keyArray[contestingKey].ToString();
        BindKey();
    }

    private void BindKey()
    {
        keyBindings.keyArray[keyToChange] = currentKeyCode;
        keyText[keyToChange].text = keyBindings.keyArray[keyToChange].ToString();
        keyBindings.UpdateKeyCodes();
        StartCoroutine(RemovePanels());
    }

    private IEnumerator RemovePanels()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        setKeyPanel.SetActive(false);
        rebindKeyPanel.SetActive(false);
    }

    public void ChangeKey(int keyNumber)
    {
        isChangingKey = true;
        keyToChange = keyNumber;
        setKeyPanel.SetActive(true);
    }
}
