using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUIManager : MonoBehaviour
{
    public Image coreIcon;
    public Image stimIcon;
    public Image grenadeIcon;

    public Image coreOverlay, stimOverlay, grenadeOverlay;

    public TextMeshProUGUI coreCooldownText;
    public TextMeshProUGUI stimCooldownText;
    public TextMeshProUGUI grenadeCooldownText;

    public Sprite coreNormal;
    public Sprite coreCooldown;
    public Sprite stimNormal;
    public Sprite stimCooldown;
    public Sprite grenadeNormal;
    public Sprite grenadeCooldown;
    
    public void SetIconOnCooldown(int abilityIndex, bool isReady, float cooldownTime)
    {
        Image selectedIcon;
        TextMeshProUGUI cooldownText;
        Sprite normalIcon, cooldownIcon;

        switch (abilityIndex)
        {
            case 1:
                selectedIcon = coreIcon;
                cooldownText = coreCooldownText;
                normalIcon = coreNormal;
                cooldownIcon = coreCooldown;
                break;
            case 2:
                selectedIcon = stimIcon;
                cooldownText = stimCooldownText;
                normalIcon = stimNormal;
                cooldownIcon = stimCooldown;
                break;
            case 3:
                selectedIcon = grenadeIcon;
                cooldownText = grenadeCooldownText;
                normalIcon = grenadeNormal;
                cooldownIcon = grenadeCooldown;
                break;
            default:
                return;
        }

        selectedIcon.sprite = isReady ? normalIcon : cooldownIcon;

        if (isReady)
        {
            StartCoroutine(FadeOutOverlay(selectedIcon, abilityIndex));
            cooldownText.text = "";
        }
        else
        {
            StartCoroutine(StartCooldownCountdown(cooldownText, cooldownTime));
        }
    }

    private IEnumerator FadeOutOverlay(Image icon, int abilityIndex)
    {
        Image overlay = abilityIndex switch
        {
            1 => coreOverlay,
            2 => stimOverlay,
            3 => grenadeOverlay,
            _ => null
        };

        if (overlay == null) yield break;

        Color overlayColor = overlay.color;
        overlayColor.a = 1f;
        overlay.color = overlayColor;

        //Vector3 initialScale = icon.transform.localScale;
        Vector3 initialScale = Vector3.one;
        Vector3 targetScale = initialScale * 1.1f;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            overlayColor.a = Mathf.Lerp(1f, 0f, progress);
            overlay.color = overlayColor;

            icon.transform.localScale = Vector3.Lerp(targetScale, initialScale, progress);

            yield return null;
        }

        overlayColor.a = 0f;
        overlay.color = overlayColor;
        icon.transform.localScale = initialScale;
    }

    private IEnumerator StartCooldownCountdown(TextMeshProUGUI cooldownText, float cooldownTime)
    {
        float remainingTime = cooldownTime;

        while (remainingTime > 0)
        {
            cooldownText.text = Mathf.Ceil(remainingTime).ToString(); // Display rounded time
            yield return new WaitForSeconds(1f); // Update every second
            remainingTime -= 1f;
        }

        cooldownText.text = ""; // Clear the text when cooldown finishes
    }
}

