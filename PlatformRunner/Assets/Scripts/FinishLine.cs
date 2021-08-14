using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.StopGame();
            
        }
        if (other.CompareTag("Girl"))
        {
            other.GetComponent<Enemy>().finishEvents();
        }
    }
}
