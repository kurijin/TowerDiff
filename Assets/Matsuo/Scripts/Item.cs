using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] int mp;
    [SerializeField,Header("�A�C�e�����E�������̉�")] AudioClip _itemGetSE;



    ItemSpawner itemSpawner;
    public void Init(ItemSpawner spawner)
    {
        this.itemSpawner = spawner;
    }

    void Start()
    {
        // �X�P�[�����Ȃ���o�ꂷ�鉉�o
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
            Debug.Log("Item��Player�ɓ�����܂���", gameObject);
            SoundManager.Instance.PlaySE(_itemGetSE);
            playerBom.AddMP(mp);
            itemSpawner.OnItemPickedup(this);
            Destroy(gameObject);
        }
    }
}
