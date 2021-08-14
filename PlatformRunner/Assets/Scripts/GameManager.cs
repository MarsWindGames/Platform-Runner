using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Transform[] spawnPoints;
    public bool gameStarted = false;

    //Instances
    Player player;
    public Canvas startCanvas;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        player = FindObjectOfType<Player>();
    }

    public void StopGame()
    {
        PaintLevel.instance.PaintLevelEnable();
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.enabled = false;
    }

    public void StartGame()
    {
        startCanvas.enabled = false;
        gameStarted = true;
    }

    public void restartGame()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));

    }

    IEnumerator LoadScene(int sceneID)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void returnToMenu()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex - 1));
    }


    public void RespawnMe(Transform player)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length - 1);

        player.transform.position = spawnPoints[spawnIndex].position;
    }


}
