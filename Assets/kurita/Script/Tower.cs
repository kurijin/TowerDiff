using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タワーについてのスクリプト
/// HP,
/// </summary>
public class Tower: MonoBehaviour
{

    [SerializeField, Header("タワーのMaxHP")] private int _towerMaxHp=100 ;
    private int _currentTowerHP;
    [SerializeField, Header("タワーHPスライダー")] private Slider _towerHpSlider;
    [SerializeField, Header("ダメージ受ける音")] AudioClip _TowerSE;


    [SerializeField, Header("タワーの子オブジェクト")] private GameObject _guideUI;
    [SerializeField, Header("タワーの破壊プレハブ")] private GameObject _defeatEffect;
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
    /// 敵からダメージを受けてタワーのHPを減らすもの
    /// </summary>
    /// <param name="damage">敵の攻撃力</param>
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

        // ダメージ演出
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
