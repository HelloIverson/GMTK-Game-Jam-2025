using UnityEngine;
using UnityEngine.UI;

public class FlashingPanel : MonoBehaviour
{
    private Image panelImage;

    void Awake()
    {
        panelImage = GetComponent<Image>();
        if (panelImage == null)
        {
            Debug.LogError("FlashingPanel requires an Image component on the same GameObject.");
        }
    }

    void Update()
    {
        // Generate a random value between 0 (black) and 0.2 (dark gray)
        float shade = Random.Range(0f, 0.2f);
        panelImage.color = new Color(shade, shade, shade, 1f);
    }
}