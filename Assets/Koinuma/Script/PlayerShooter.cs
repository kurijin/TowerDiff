using UnityEngine;

/// <summary>
/// �v���C���[�̎ˌ��U��
/// </summary>
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _lookDirectionArrow;
    [SerializeField] Transform _muzzle;
    [SerializeField] float _interval;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField,Header("������")] AudioClip _attackSE;

    Vector3 _cursorWorldPos;
    float _shootIntervalTimer;
    Animator _animator;

    const string MovementX = "movementX";
    const string MovementY = "movementY";
    const string ShootTriggerName = "shoot";

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        GetInput();
        LookToCursor();
        InputAnimParam();

        if (GameManager.Instance && !GameManager.Instance.IsInGame) return;

        ShootBullet();
    }

    void GetInput()
    {
        _cursorWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _cursorWorldPos.z = 0;
    }

    /// <summary> �J�[�\���̕��������� </summary>
    void LookToCursor()
    {
        Vector3 dir = _cursorWorldPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        _lookDirectionArrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void ShootBullet()
    {
        if (_shootIntervalTimer <= 0)
        {
            GameObject.Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
            _shootIntervalTimer = _interval;

            _animator.SetTrigger(ShootTriggerName);
            SoundManager.Instance.PlaySE(_attackSE);
        }
        else
        {
            _shootIntervalTimer -= Time.deltaTime;
        }
    }

    void InputAnimParam()
    {
        // �v���C���[���J�[�\���Ɍ�����
        Vector3 dir = _cursorWorldPos - transform.position;
        _animator.SetFloat(MovementX, dir.x);
        _animator.SetFloat(MovementY, dir.y);

        // �������Ă�����Flip����
        if (_spriteRenderer) _spriteRenderer.flipX = dir.x < 0;
    }
}
