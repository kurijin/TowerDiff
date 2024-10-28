using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ˌ��U�����s���G�N���X
/// </summary>
public class ShotEnemy : EnemyBase
{
    [Header("�G�̈ړ����x")]
    [SerializeField] private float _speed;

    [Header("�^���[�ɍU������ۂ̋���")]
    [SerializeField] private float _distance;

    [Header("�U���̃C���^�[�o��(�~���b)")]
    [SerializeField] float _shotInterval = 700;

    [Header("�e�̃v���n�u�Q��")]
    [SerializeField] private GameObject _bullet;

    /// <summary> �G�̈ړ����x </summary>
    public float Speed { get { return _speed; } set { _speed = value; } }

    /// <summary> �U�����t���O </summary>
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
    /// �U�����J�n����
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
    /// �w�肵����������������
    /// </summary>
    /// <param name="target"></param>
    private void LookAt(Transform target, Vector3 direction)
    {
        Vector3 dir = (direction - target.position);
        target.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
}
