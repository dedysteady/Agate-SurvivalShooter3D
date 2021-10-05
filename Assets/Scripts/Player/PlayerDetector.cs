using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public GameOverManager gameOverManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.isTrigger)
        {
            float enemyDistance = Vector3.Distance(transform.position,other.transform.position);
            gameOverManager.ShowWarning(enemyDistance);
        }
    }
}
