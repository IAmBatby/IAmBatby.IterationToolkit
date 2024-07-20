using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFiller : MonoBehaviour
{
    public Image image;
    public VerticalLayoutGroup verticalLayoutGroup;

    public void Update()
    {
        float newX = verticalLayoutGroup.preferredWidth;
        float newY = verticalLayoutGroup.preferredHeight;
        image.rectTransform.sizeDelta = new Vector2(newX, newY);
        image.rectTransform.anchoredPosition = new Vector2(newX / 2, -(newY / 2));
    }
}
