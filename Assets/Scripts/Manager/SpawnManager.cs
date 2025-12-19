using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;
    [SerializeField] int maxEnemyCount = 30;

    int currentEnemyCount = 0;
    float timer;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && currentEnemyCount < maxEnemyCount)
        {
            SpawnEnemy();
            ResetTimer();
        }
    }

    void SpawnEnemy()
    {
        // 랜덤 스폰 포인트 선택
        int index = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[index];

        // 프리팹 생성
        PooledObject enemy = PoolManager.Instance.SpawnFromPool("Enemy", spawnPoint.position, spawnPoint.rotation);
        IncreaseCount();
    }

    public void IncreaseCount()
    {
        currentEnemyCount++;
    }

    public void DecreaseCount()
    {
        if (currentEnemyCount > 0)
            currentEnemyCount--;
    }

    void ResetTimer()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    // TODO: 맵을 타일로 분할하고 소환 가능한 타일을 구분한 뒤 타일들 중에서 랜덤으로 스폰 지점을 만들어서 리스트에 넣는 로직?
}
