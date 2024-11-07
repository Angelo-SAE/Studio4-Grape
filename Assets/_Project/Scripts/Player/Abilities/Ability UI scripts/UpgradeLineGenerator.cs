using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLineGenerator : MonoBehaviour

{

    [SerializeField] int lineWidth = 3;
    private Vector3[] targetBoxPositions;
    private float vLineHeight, vLineWidth, hLineHeight, hLineWidth;

    public Color colour;

    public RectTransform[] targetBoxes;

    void Start()
    {

        targetBoxPositions = new Vector3[targetBoxes.Length];
        for (int i = 0; i < targetBoxes.Length; i++)
        {
            targetBoxPositions[i] = targetBoxes[i].localPosition;
            float targetBoxWidth = targetBoxes[i].rect.width;
            float targetBoxHeight = targetBoxes[i].rect.height;
            GenerateLines(targetBoxWidth, targetBoxHeight, targetBoxPositions[i].x, targetBoxPositions[i].y);

            //Debug.Log($"Target Box {i + 1} - Position: {targetBoxPositions[i]}, Width: {targetBoxWidth}, Height: {targetBoxHeight}");
        }

    }

    private void GenerateLines(float width, float height, float targetPositionX, float targetPositionY)
    {
        float vLineY, vLineX, hLineY, hLineX;

        vLineHeight = Mathf.Abs(targetPositionY) - (height * 0.5f);
        vLineWidth = lineWidth;

        hLineHeight = lineWidth;
        hLineWidth = Mathf.Abs(targetPositionX) - (width * 0.5f) + 1.5f;

        if (targetPositionY < 0)
        {
            vLineY = targetPositionY + (vLineHeight * 0.5f);
        }
        else
        {
            vLineY = targetPositionY - (vLineHeight * 0.5f);
        }

        vLineX = 0;

        if (targetPositionX < 0)
        {
            hLineX = (targetPositionX + (width * 0.5f) + 1.5f) * 0.5f;
        }
        else
        {
            hLineX = (targetPositionX - (width * 0.5f) - 1.5f) * 0.5f;
        }
        
        hLineY = targetPositionY;

        //Debug.Log(vLineX + "," + vLineY);
        //Debug.Log(hLineX + "," + hLineY);

        GameObject vLine = new GameObject("VerticalLine");
        vLine.transform.SetParent(this.transform);
        RectTransform vLineRect = vLine.AddComponent<RectTransform>();
        vLineRect.sizeDelta = new Vector2(vLineWidth, vLineHeight);
        vLineRect.localPosition = new Vector2(vLineX, vLineY);
        vLineRect.localScale = Vector3.one;
        vLine.AddComponent<UnityEngine.UI.Image>().color = colour;

        // Instantiate the horizontal line
        GameObject hLine = new GameObject("HorizontalLine");
        hLine.transform.SetParent(this.transform);
        RectTransform hLineRect = hLine.AddComponent<RectTransform>();
        hLineRect.sizeDelta = new Vector2(hLineWidth, hLineHeight);
        hLineRect.localPosition = new Vector2(hLineX, hLineY);
        hLineRect.localScale = Vector3.one;
        hLine.AddComponent<UnityEngine.UI.Image>().color = colour;

    }

    
}
