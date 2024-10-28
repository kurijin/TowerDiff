using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G���
/// </summary>
public enum EnemyType
{
    CHW1, //�j���g��
    SD2,  //�q�c�W
    BW1   //�E�V
}

/// <summary>
/// �G���N���X
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("�G�̗̑�")]
    [SerializeField] protected int hp;

    [Header("�G�̎�� (CHW1:�j���g��, SD2:�q�c�W, BW1:�E�V)")]
    [SerializeField] protected EnemyType type;

    [Header("�q�b�g�G�t�F�N�g�̐F")]
    [SerializeField] Color hitEffectSpriteColor = Color.red;

    [Header("�q�b�g�G�t�F�N�g�̔��F����")]
    [SerializeField] float effectDuration = .1f;

    [Header("��e���̉�")]
    [SerializeField] AudioClip _hitSE;

    [Header("���j���̉������N���X")]
    [SerializeField] protected SmokeManager sm;

    /// <summary> �G�̗̑� </summary>
    public int Hp {  get { return hp; } }

    /// <summary> �^���[�i�U���Ώہj </summary>
    protected Transform target;

    /// <summary> ���S�t���O </summary>
    protected bool isDead = false;

    /// <summary> �q�I�u�W�F�N�g </summary>
    protected Transform body;

    /// <summary> �q�I�u�W�F�N�g�̃A�j���[�^�[ </summary>
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
        //�^�[�Q�b�g���E�ɂ����ꍇ�摜���]
        if(distance.x > 0)
        {
            var scale = body.transform.localScale;
            scale.x *= -1;
            body.transform.localScale = scale;
        }
    }

    /// <summary>
    /// �_���[�W���󂯂�
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
