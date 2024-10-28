using DG.Tweening;
using System;
using UnityEngine;

public class PlayerBom : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] CircleCollider2D _damageArea;
    [SerializeField] float _timeToExplosion;

    [SerializeField] SpriteRenderer _spriteStay;
    [SerializeField] SpriteRenderer _spriteBark;

    [SerializeField] float barkScale;
    [SerializeField] float barkDuration;
    [SerializeField, Header("アイテムを置く音")] AudioClip _useItemSE;

    [SerializeField] float explosionScale;
    [SerializeField] float explosionDuration;
    [SerializeField] SpriteRenderer explosion1, explosion2;

    bool exploded = false;


    float _timer = 0;

    private void Start()
    {
        _spriteStay.enabled = true;
        _spriteBark.enabled = false;
        explosion1.transform.DOScale(0, 0);
        explosion2.transform.DOScale(0, 0);
    }

    private void Update()
    {
        if (_timer >= _timeToExplosion)
        {
            if (exploded)
                return;
            _spriteStay.enabled = false;
            _spriteBark.enabled = true;
            exploded = true;
            Explosion();
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    void Explosion()
    {
        Collider2D[] hitCols = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), _damageArea.radius);

        foreach (Collider2D collider in hitCols)
        {
            if (collider.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDamage(_damage);
            }

            if (collider.TryGetComponent(out EnemySpawner spawner))
            {
                spawner.GetDamage(_damage);
            }
        }

        // 吠える演出
        SoundManager.Instance.PlaySE(_useItemSE);
        Vector3 scaleVec = new Vector3(barkScale, barkScale, barkScale);
        Sequence barkSeq = DOTween.Sequence();
        barkSeq.Append(
            _spriteBark.transform.DOScale(scaleVec, barkDuration)
            .SetEase(Ease.Linear)
            .SetLoops(2, LoopType.Yoyo)
            );
        barkSeq.AppendInterval(0.5f);
        barkSeq.AppendCallback(() => Destroy(gameObject));

        Vector3 explosionScaleVec = new Vector3(explosionScale, explosionScale, explosionScale);
        Sequence explosionSeq = DOTween.Sequence();
        explosionSeq.Append(explosion1.transform.DOScale(explosionScale, explosionDuration).SetEase(Ease.OutExpo));
        explosionSeq.Join(explosion1.DOFade(0.0f, explosionDuration).SetEase(Ease.OutExpo));
        explosionSeq.AppendInterval(0.1f);
        explosionSeq.Append(explosion2.transform.DOScale(explosionScale, explosionDuration).SetEase(Ease.OutExpo));
        explosionSeq.Join(explosion2.DOFade(0, explosionDuration).SetEase(Ease.OutExpo));
    }
}
