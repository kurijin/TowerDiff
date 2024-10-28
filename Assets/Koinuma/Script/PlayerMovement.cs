using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed;
 
    Rigidbody2D _rb;
    Vector2 _input2d;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }

    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        if (GameManager.Instance && !GameManager.Instance.IsInGame)
        {
            _input2d = Vector2.zero;
            return;
        }

        _input2d.x = Input.GetAxisRaw("Horizontal");
        _input2d.y = Input.GetAxisRaw("Vertical");
        _input2d.Normalize();
    }

    void Move()
    {
        _rb.velocity = _input2d * _speed;
    }
}
