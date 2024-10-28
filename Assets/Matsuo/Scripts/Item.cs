using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] int mp;
    [SerializeField,Header("アイテムを拾った時の音")] AudioClip _itemGetSE;



    ItemSpawner itemSpawner;
    public void Init(ItemSpawner spawner)
    {
        this.itemSpawner = spawner;
    }

    void Start()
    {
        // スケールしながら登場する演出
        float defaultScale = transform.localScale.x;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0, 0));
        seq.Append(transform.DOScale(defaultScale, 0.2f));
    }


    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.TryGetComponent<PlayerBomController>(out PlayerBomController playerBom))
        {
            Debug.Log("ItemがPlayerに当たりました", gameObject);
            SoundManager.Instance.PlaySE(_itemGetSE);
            playerBom.AddMP(mp);
            itemSpawner.OnItemPickedup(this);
            Destroy(gameObject);
        }
    }
}
