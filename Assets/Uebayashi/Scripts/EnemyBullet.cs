using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _damage;
    [SerializeField] GameObject _egg;
    [SerializeField] GameObject _brokenEgg;
    float _lifeTime = 2;
    bool _isHit = false;

    Rigidbody2D _rb;

    private void Start()
    {
        _brokenEgg.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;

        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        if(_isHit) { return; }

        Move();
    }

    void Move()
    {
        _rb.velocity = transform.up * _speed;
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Tower tower))
        {
            tower.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
    */
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Tower tower))
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(100)); //タワーの中心近くにヒットするように少し遅延
            _isHit = true;
            this.GetComponent<CircleCollider2D>().enabled = false;
            _rb.velocity = Vector2.zero;
            tower.TakeDamage(_damage);
            _egg.SetActive(false);
            _brokenEgg.SetActive(true);
            var eggPos = new Vector3(UnityEngine.Random.Range(-3.0f, 3.0f), UnityEngine.Random.Range(-3.0f, 3.0f), 0);
            _brokenEgg.transform.localPosition = eggPos;
            await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            _brokenEgg.GetComponent<SpriteRenderer>().DOFade(0, 0.3f);
            await UniTask.Delay(TimeSpan.FromMilliseconds(300));
            Destroy(gameObject);
        } 
    }
}
