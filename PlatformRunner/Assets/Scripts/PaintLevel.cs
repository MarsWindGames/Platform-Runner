using UnityEngine;

public class PaintLevel : MonoBehaviour
{

    [Header("Instances")]
    public Camera paintCamera;
    public Paint paint;
    public GameObject colorAmountShower;
    public GameObject gameUI;
    public static PaintLevel instance;
    public RenderTexture renderTexture;

    Camera mainCam;
    GameObject player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        mainCam = Camera.main;
        player = FindObjectOfType<Player>().gameObject;
    }

    public void PaintLevelEnable()
    {
        renderTexture.Release();
        paintCamera.enabled = true;
        paint.enabled = true;

        colorAmountShower.SetActive(true);

        gameUI.SetActive(false);

        mainCam.enabled = false;
        gameObject.SetActive(false);
    }
}
