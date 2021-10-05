using UnityEngine;

public class EnemyFactory : MonoBehaviour, IFactory
{
    [SerializeField] GameObject[] enemyPrefab;

    public GameObject FactoryMethod(int tag, Transform spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab[tag], spawnPoint);
        return enemy;
    }
}