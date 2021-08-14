using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Paint : MonoBehaviour
{
    public Transform brush;
    public Camera paintCamera;

    public RenderTexture renderTexture;
    public Texture2D renderedTexture;
    Texture2D copiedRenderedTexture;
    public Renderer wallRenderer;

    public Slider colorAmountShower;

    public Canvas finishCanvas;
    public TMP_Text amountText;
    public Transform brushSprite;

    Color32[] pixelArray;
    int redCount = 0;

    private void Start()
    {
        renderTexture.Release();
        copiedRenderedTexture = renderedTexture;
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = paintCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                brushSprite.position = hitPos;
                brush.position = hitPos;
            }
            StartCoroutine(RenderToTexture());
        }
    }

    private IEnumerator RenderToTexture()
    {
        yield return new WaitForEndOfFrame();
        RenderTexture.active = renderTexture;
        copiedRenderedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        pixelArray = copiedRenderedTexture.GetPixels32();

        Invoke("CheckColorDensity", 0.1f);
    }

    public void CheckColorDensity()
    {
        // we get all pixels of the copied texture. And check how many of them are red.
        for (int i = 0; i < pixelArray.Length; i++)
        {
            if (pixelArray[i].r >= 255)
            {
                redCount++;
            }
        }

        int totalCount = pixelArray.Length;

        if (totalCount != 0)
        {
            int totalAmount = Mathf.Clamp(redCount * 100 / totalCount, 0, 100);
            colorAmountShower.value = totalAmount;
            amountText.text = totalAmount.ToString();
            redCount = 0;
        }

        if (colorAmountShower.value >= 100)
        {
            finishCanvas.enabled = true;
        }
    }

}
