using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUpgradeHoverUI : MonoBehaviour
{
    [Header("Upgrade Script")]
    [SerializeField] private Upgrade upgrade;

    [Header("UI")]
    [SerializeField] private GameObject displayObject;
    [SerializeField] private TMP_Text upgradeNameUI;
    [SerializeField] private TMP_Text upgradeDescriptionUI;
    [SerializeField] private RectTransform uiToDetect;
    [SerializeField] private RectTransform uiDisplay;

    private bool displaying;
    private Vector2 mousePosition, bounds;


    private void Start()
    {
        bounds.x = uiToDetect.sizeDelta.x / 2;
        bounds.y = uiToDetect.sizeDelta.y / 2;
    }

    private void Update()
    {
        GetMousePosition();
        CheckForHover();
    }

    private void GetMousePosition()
    {
        mousePosition = Input.mousePosition;
    }

    private void CheckForHover()
    {
        if(mousePosition.x > uiToDetect.position.x - bounds.x && mousePosition.x < uiToDetect.position.x + bounds.x && mousePosition.y > uiToDetect.position.y - bounds.y && mousePosition.y < uiToDetect.position.y + bounds.y)
        {

            if(!displaying)
            {
                DisplayUpgradeInformation();
                displaying = true;
                displayObject.SetActive(true);
            }
            PositionDisplay();
        } else if(displaying)
        {
            displaying = false;
            displayObject.SetActive(false);
        }
    }

    private void PositionDisplay()
    {
      uiDisplay.position = new Vector2(mousePosition.x - (uiDisplay.sizeDelta.x / 2), mousePosition.y - (uiDisplay.sizeDelta.y / 2));
    }

    private void DisplayUpgradeInformation()
    {
        upgradeNameUI.text = upgrade.upgradeName;
        upgradeDescriptionUI.text = upgrade.description;
    }
}
