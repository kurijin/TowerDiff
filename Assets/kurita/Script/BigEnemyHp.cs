using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 大きい敵のHPバー表示
/// </summary>

public class BigEnemyHp : MonoBehaviour
{
    [SerializeField, Header("BIGエネミーのHPスライダー")] private Slider _hpSlider;
    private int _maxhp;
    private int _currentHp;
    private BombEnemy _bombEnemy;
    // Start is called before the first frame update
    void Start()
    {
        _bombEnemy = GetComponent<BombEnemy>();
        _maxhp = _bombEnemy.Hp;
        _currentHp = _bombEnemy.Hp;
        _hpSlider.maxValue = _currentHp;
        _hpSlider.value = _currentHp;

    }

    // Update is called once per frame
    void Update()
    {
        _currentHp = _bombEnemy.Hp;
        _hpSlider.value = _currentHp;
    }
}
