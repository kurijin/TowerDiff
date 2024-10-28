using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �^���[�ɂ��ẴX�N���v�g
/// HP,
/// </summary>
public class Tower: MonoBehaviour
{

    [SerializeField, Header("�^���[��MaxHP")] private int _towerMaxHp=100 ;
    private int _currentTowerHP;
    [SerializeField, Header("�^���[HP�X���C�_�[")] private Slider _towerHpSlider;
    [SerializeField, Header("�_���[�W�󂯂鉹")] AudioClip _TowerSE;


    [SerializeField, Header("�^���[�̎q�I�u�W�F�N�g")] private GameObject _guideUI;
    [SerializeField, Header("�^���[�̔j��v���n�u")] private GameObject _defeatEffect;
    [SerializeField, Header("TowerSE")] private AudioClip _DefeatSE;

    [SerializeField] SpriteRenderer spriteRenderer;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        _guideUI.SetActive(true);
        _currentTowerHP = _towerMaxHp;
        _towerHpSlider.maxValue = _towerMaxHp;
        _towerHpSlider.value = _towerMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        _towerHpSlider.value = _currentTowerHP;
    }


    /// <summary>
    /// �G����_���[�W���󂯂ă^���[��HP�����炷����
    /// </summary>
    /// <param name="damage">�G�̍U����</param>
    public void TakeDamage(int damage)
    {
        if(isDead) return;

        SoundManager.Instance.PlaySE(_TowerSE);
        _currentTowerHP -= damage;

        if (_currentTowerHP <= 0)
        {
            isDead = true;
            _towerHpSlider.value = 0f;
            _guideUI.SetActive(false);
            Instantiate(_defeatEffect, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySE(_DefeatSE);
            GameManager.Instance.GameEnd();
            spriteRenderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        // �_���[�W���o
        float duration = 0.3f;
        Sequence seq = DOTween.Sequence();
        seq.Append(spriteRenderer.DOColor(Color.white, 0));
        seq.Append(
            spriteRenderer.DOColor(Color.red, duration)
                .SetEase(Ease.Linear)
                .SetLoops(2, LoopType.Yoyo)
            );
        seq.Join(transform.DOShakePosition(duration, 0.4f));
    }
}
