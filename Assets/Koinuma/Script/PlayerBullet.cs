using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _damage;
    [SerializeField] float _lifeTime;

    Rigidbody2D _rb;
    Animator _animator;

    const string HitAnimName = "hit";

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;

        _animator = GetComponentInChildren<Animator>();

        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        _rb.velocity = transform.up * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // “–‚½‚è”»’è‚ð–³Ž‹‚·‚é‚â‚Â
        if (collision.gameObject.tag == "Player") return;

        if (collision.TryGetComponent(out EnemyBase enemyBase))
        {
            enemyBase.TakeDamage(_damage);

            _animator.Play(HitAnimName, 0);
            GetComponent<Collider2D>().enabled = false;
            _speed = 0;
            Destroy(gameObject, .3f);
        }
        else if(collision.TryGetComponent(out EnemySpawner enemySpawner))
        {
            enemySpawner.GetDamage(_damage);

            _animator.Play(HitAnimName, 0);
            GetComponent<Collider2D>().enabled = false;
            _speed = 0;
            Destroy(gameObject, .3f);
        }
    }
}
