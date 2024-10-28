using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public bool canSpawn = false;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] float minX, maxX, minY, maxY, width;


    [SerializeField] int itemsInScene = 0;
    [SerializeField] int itemLimit;
    public void OnItemPickedup(Item item)
    {
        itemsInScene--;
    }


    float timer = 0;
    [SerializeField] float maxSpawnTime, minSpawnTime;
    float spawnTime;
    float GetRandomSpawnTime() => spawnTime = Random.Range(minSpawnTime, maxSpawnTime);



    void Start()
    {
        spawnTime = GetRandomSpawnTime();
    }



    void Update()
    {
        if (GameManager.Instance._isFinish)
        {
            return;
        }

        if (canSpawn)
        {
            if (itemsInScene >= itemLimit)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= spawnTime)
            {
                Debug.Log("Itemをスポーンします");
                timer = 0;
                spawnTime = GetRandomSpawnTime();
                SpawnSpawner();
            }
        }
    }



    void SpawnSpawner()
    {
        itemsInScene++;
        Vector2 spawnPos = GetRandomPosInRect();
        GameObject item = Instantiate(itemPrefab, spawnPos, Quaternion.identity);
        item.GetComponent<Item>().Init(this);
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
