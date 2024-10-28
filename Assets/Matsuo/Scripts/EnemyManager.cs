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
    int waveCnt = 0; // ����܂ŉ���X�|�[����������(��Wave�ڂ��H)

    // [SerializeField] int maxSpawnCnt, minSpawnCnt;
    // [SerializeField] TimeManager timeManager;



    void Start()
    {
        spawnTime = GetRandomSpawnTime();
    }



    void Update()
    {
        // ===== �ǂ̂悤�ȕp�x�œG���X�|�[�������邩�͂����Ŏ��� ===== //
        if(canSpawn)
        {
            timer += Time.deltaTime;
            if(timer >= spawnTime)
            {
                Debug.Log("�G���X�|�[�����܂�");
                waveCnt++;
                timer = 0;
                spawnTime = GetRandomSpawnTime();

                // Wave�ɉ����ēG�̐��𑝂₷
                // ===== �G�P ===== //
                int enemyNum1 = 3 * waveCnt;
                // enemyNum1 = Mathf.Clamp(enemyNum1, minSpawnCnt, maxSpawnCnt);
                SpawnEnemiesRandom(enemyNum1, 1);

                // ===== �G�Q ===== //
                int enemyNum2 = waveCnt - 1;
                // enemyNum2 = Mathf.Clamp(enemyNum2, minSpawnCnt, maxSpawnCnt);
                SpawnEnemiesRandom(enemyNum2, 2);
            }
        }
    }




    /// <summary>
    /// �w�肵����ނ̓G���w�肵�������������_���Ȉʒu�ɃX�|�[��������
    /// </summary>
    /// <param name="enemyCnt">��������G�̐�</param>
    /// <param name="enemyType">��������G�̎��</param>
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
                Debug.LogWarning("SpawnEnemiesRandom�̑�Q�����ɂ�1��2�����Ă�������");
                spawnEnemy = enemyPrefab1;
                break;
        }

        for(int k = 0; k < enemyCnt; k++)
        {
            Vector2 spawnPos = GetRandomPosInRect();
            Instantiate(spawnEnemy, spawnPos, Quaternion.identity);
        }
    }



    // ����l�p�`�̈�̃����_���Ȉʒu��Ԃ�
    Vector2 GetRandomPosInRect()
    {
        float spawnX, spawnY;
        int edgeNum = Random.Range(0, 4);   // �S�̕ӂ̂ǂ̕ӂɃX�|�[�����邩�H
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
