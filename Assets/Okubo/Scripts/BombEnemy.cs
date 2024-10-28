using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : EnemyBase
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _damage;

    Rigidbody2D _rb;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Vector3 current = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Tower tower))
        {
            tower.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
