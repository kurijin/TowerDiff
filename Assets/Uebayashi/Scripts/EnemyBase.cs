using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵種別
/// </summary>
public enum EnemyType
{
    CHW1, //ニワトリ
    SD2,  //ヒツジ
    BW1   //ウシ
}

/// <summary>
/// 敵基底クラス
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("敵の体力")]
    [SerializeField] protected int hp;

    [Header("敵の種別 (CHW1:ニワトリ, SD2:ヒツジ, BW1:ウシ)")]
    [SerializeField] protected EnemyType type;

    [Header("ヒットエフェクトの色")]
    [SerializeField] Color hitEffectSpriteColor = Color.red;

    [Header("ヒットエフェクトの発色時間")]
    [SerializeField] float effectDuration = .1f;

    [Header("被弾時の音")]
    [SerializeField] AudioClip _hitSE;

    [Header("撃破時の煙生成クラス")]
    [SerializeField] protected SmokeManager sm;

    /// <summary> 敵の体力 </summary>
    public int Hp {  get { return hp; } }

    /// <summary> タワー（攻撃対象） </summary>
    protected Transform target;

    /// <summary> 死亡フラグ </summary>
    protected bool isDead = false;

    /// <summary> 子オブジェクト </summary>
    protected Transform body;

    /// <summary> 子オブジェクトのアニメーター </summary>
    protected Animator animator;

    SpriteRenderer[] sprites;

    protected virtual void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        body = this.transform.Find("Body");
        animator = body.GetChild(0).GetComponent<Animator>();
        animator.Play("Walk-" + type.ToString());
        target = GameObject.Find("Tower").transform;
        var distance = this.transform.position - target.position;
        //ターゲットより右にいた場合画像反転
        if(distance.x > 0)
        {
            var scale = body.transform.localScale;
            scale.x *= -1;
            body.transform.localScale = scale;
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage"></param>
    public async void TakeDamage(int damage)
    {
        SoundManager.Instance.PlaySE(_hitSE);
        hp -= damage;
        //Debug.Log($"Get Damage. HP:{hp}");
        if(!isDead && hp <= 0)
        {
            isDead = true;
            this.GetComponent<CircleCollider2D>().enabled = false;
            animator.Play("Die-" + type.ToString());
            sm.GenerateSmoke();
            body.DOShakePosition(0.5f, 0.8f, 10);
            foreach(Transform child in body.GetChild(0))
            {
                var renderer = child.GetComponent<SpriteRenderer>();
                renderer.DOFade(0, 0.3f).SetDelay(0.2f);
            }
            await UniTask.Delay(TimeSpan.FromMilliseconds(1000));
            Destroy(this.gameObject);
        }
        else
        {
            body.DOShakePosition(0.1f, 0.5f, 2);
            PlayHitEffect();
        }
    }

    void PlayHitEffect()
    {
        foreach(var sprite in sprites)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(sprite.DOColor(hitEffectSpriteColor, effectDuration));
            sequence.Append(sprite.DOColor(Color.white, effectDuration));
            sequence.Play();
        }
    }
}
