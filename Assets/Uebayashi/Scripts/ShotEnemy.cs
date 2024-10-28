using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 射撃攻撃を行う敵クラス
/// </summary>
public class ShotEnemy : EnemyBase
{
    [Header("敵の移動速度")]
    [SerializeField] private float _speed;

    [Header("タワーに攻撃する際の距離")]
    [SerializeField] private float _distance;

    [Header("攻撃のインターバル(ミリ秒)")]
    [SerializeField] float _shotInterval = 700;

    [Header("弾のプレハブ参照")]
    [SerializeField] private GameObject _bullet;

    /// <summary> 敵の移動速度 </summary>
    public float Speed { get { return _speed; } set { _speed = value; } }

    /// <summary> 攻撃中フラグ </summary>
    private bool _isAttack = false;

    // Update is called once per frame
    void Update()
    {
        
        if (!_isAttack && !isDead)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, _speed * Time.deltaTime);
            if (Vector3.Distance(this.transform.position, target.position) < _distance)
            {
                _isAttack = true;
                _ = Attack();
            }
        }
    }

    /// <summary>
    /// 攻撃を開始する
    /// </summary>
    private async UniTask Attack()
    {
        animator.Play("Idle-CHW1");
        while (true)
        {
            //LookAt(target.position);
            for (int i = 0; i < 3; i++)
            {
                var obj = Instantiate(_bullet, this.transform.position, this.transform.rotation);
                LookAt(obj.transform, target.position);
                await UniTask.Delay(TimeSpan.FromMilliseconds(_shotInterval), cancellationToken: destroyCancellationToken);
            }
            await UniTask.Delay(TimeSpan.FromMilliseconds(_shotInterval * 2), cancellationToken: destroyCancellationToken);
        }
    }

    /// <summary>
    /// 指定した方向を向かせる
    /// </summary>
    /// <param name="target"></param>
    private void LookAt(Transform target, Vector3 direction)
    {
        Vector3 dir = (direction - target.position);
        target.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
}
