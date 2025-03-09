using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform baseLocation;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minimumDistanceFromBase = 3f;
    [SerializeField] private float spawnInterval = 2f;

    void Start()
    {
        // Belirli bir aral�kta d��man spawn'lama i�lemini ba�lat
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {

        if (!PhotonNetwork.IsMasterClient) return;//master clientte �al��t�r.
        // Rastgele bir pozisyon bul
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Rastgele bir d��man tipi se�
        GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // D��man� olu�tur
        PhotonNetwork.Instantiate(randomEnemyPrefab.name, spawnPosition, Quaternion.identity);

    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 basePosition = baseLocation.position;

        Vector3 randomPosition;
        float distanceFromBase;

        do
        {
            // �ember i�inde rastgele bir pozisyon olu�tur
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minimumDistanceFromBase, spawnRadius);
            randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + basePosition;

            // Oyuncuya olan mesafeyi kontrol et
            distanceFromBase = Vector3.Distance(basePosition, randomPosition);
        } while (distanceFromBase < minimumDistanceFromBase);

        return randomPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (baseLocation != null)
        {
            // Spawn alan�n� �iz
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(baseLocation.position, spawnRadius);

            // Minimum mesafeyi �iz
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(baseLocation.position, minimumDistanceFromBase);
        }
    }
}
