using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool canSpawn = false;
    [SerializeField] GameObject enemyPrefab1;
    [SerializeField] GameObject enemyPrefab2;
    [SerializeField] float minX, maxX, minY, maxY, width;

    float timer = 0;
    [SerializeField] float maxSpawnTime, minSpawnTime;
    float spawnTime;
    float GetRandomSpawnTime() => spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    int waveCnt = 0; // これまで何回スポーンさせたか(何Wave目か？)

    // [SerializeField] int maxSpawnCnt, minSpawnCnt;
    // [SerializeField] TimeManager timeManager;



    void Start()
    {
        spawnTime = GetRandomSpawnTime();
    }



    void Update()
    {
        // ===== どのような頻度で敵をスポーンさせるかはここで実装 ===== //
        if(canSpawn)
        {
            timer += Time.deltaTime;
            if(timer >= spawnTime)
            {
                Debug.Log("敵をスポーンします");
                waveCnt++;
                timer = 0;
                spawnTime = GetRandomSpawnTime();

                // Waveに応じて敵の数を増やす
                // ===== 敵１ ===== //
                int enemyNum1 = 3 * waveCnt;
                // enemyNum1 = Mathf.Clamp(enemyNum1, minSpawnCnt, maxSpawnCnt);
                SpawnEnemiesRandom(enemyNum1, 1);

                // ===== 敵２ ===== //
                int enemyNum2 = waveCnt - 1;
                // enemyNum2 = Mathf.Clamp(enemyNum2, minSpawnCnt, maxSpawnCnt);
                SpawnEnemiesRandom(enemyNum2, 2);
            }
        }
    }




    /// <summary>
    /// 指定した種類の敵を指定した数だけランダムな位置にスポーンさせる
    /// </summary>
    /// <param name="enemyCnt">生成する敵の数</param>
    /// <param name="enemyType">生成する敵の種類</param>
    public void SpawnEnemiesRandom(int enemyCnt, int enemyType)
    {
        GameObject spawnEnemy;
        switch(enemyType)
        {
            case 1:
                spawnEnemy = enemyPrefab1;
                break;

            case 2:
                spawnEnemy = enemyPrefab2;
                break;

            default:
                Debug.LogWarning("SpawnEnemiesRandomの第２引数には1か2を入れてください");
                spawnEnemy = enemyPrefab1;
                break;
        }

        for(int k = 0; k < enemyCnt; k++)
        {
            Vector2 spawnPos = GetRandomPosInRect();
            Instantiate(spawnEnemy, spawnPos, Quaternion.identity);
        }
    }



    // 中空四角形領域のランダムな位置を返す
    Vector2 GetRandomPosInRect()
    {
        float spawnX, spawnY;
        int edgeNum = Random.Range(0, 4);   // ４つの辺のどの辺にスポーンするか？
        switch (edgeNum)
        {
            case 0:
                spawnX = Random.Range(minX - width, minX);
                spawnY = Random.Range(minY, maxY);
                break;

            case 1:
                spawnX = Random.Range(maxX, maxX + width);
                spawnY = Random.Range(minY, maxY);
                break;

            case 2:
                spawnX = Random.Range(minX, maxX);
                spawnY = Random.Range(minY - width, minY);
                break;

            case 3:
                spawnX = Random.Range(minX, maxX);
                spawnY = Random.Range(maxY, maxY + width);
                break;
            default:
                spawnX = Random.Range(minX, maxX);
                spawnY = Random.Range(maxY, maxY + width);
                break;
        }
        Vector2 spawnPos = new Vector2(spawnX, spawnY);
        return spawnPos;
    }
}
