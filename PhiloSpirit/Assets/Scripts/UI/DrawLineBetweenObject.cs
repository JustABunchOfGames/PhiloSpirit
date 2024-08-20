using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DrawLineBetweenObject
    {
        public static void DrawLine(Transform a, Transform b, Color col)
        {
            // Create Link
            GameObject link = new GameObject();
            link.name = "link";
            Image linkImage = link.AddComponent<Image>();

            // Color link
            linkImage.color = col;

            // Get transform in place
            RectTransform imageRect = link.GetComponent<RectTransform>();
            imageRect.SetParent(b);
            imageRect.localScale = Vector3.one;
            imageRect.anchorMin = new Vector2(0, 0.5f);
            imageRect.anchorMax = new Vector2(0, 0.5f);

            // Get RectTransform for size
            RectTransform rb = a.GetComponent<RectTransform>();

            // Set position
            imageRect.position = (a.position + b.position) / 2;

            // Set size
            Vector3 dif = b.localPosition - a.localPosition;
            dif.x -= rb.sizeDelta.x;
            imageRect.sizeDelta = new Vector3(dif.magnitude, 5);

            // Set rotation
            imageRect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }
    }
}