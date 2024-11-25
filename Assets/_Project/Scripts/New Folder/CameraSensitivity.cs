using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSensitivity : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatObject cameraSensitivity;

    [Header("UI")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityText;

    private void Start()
    {
        sensitivitySlider.value = cameraSensitivity.value;
        sensitivityText.text = cameraSensitivity.value.ToString();
    }

    public void SetSensitivity()
    {
        cameraSensitivity.value = Mathf.Round(sensitivitySlider.value * 100f) / 100f;
        sensitivityText.text = cameraSensitivity.value.ToString();
    }
}
