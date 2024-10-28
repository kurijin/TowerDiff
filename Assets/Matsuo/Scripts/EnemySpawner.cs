using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    public bool canSpawn = false;
    [SerializeField] GameObject enemyPrefab1, enemyPrefab2, bossEnemy;
    [SerializeField] float spawnRadius;
    [SerializeField] float enemy1Rate;  // enemy1���X�|�[������銄�� (0 �` 1)


    float timer = 0;
    float interval = 0;
    public float avgSpawnInterval;
    [SerializeField] float spawnIntervalRange;
    [SerializeField] float bossInterval;
    float GetRandomInterval() => Random.Range(avgSpawnInterval - spawnIntervalRange / 2.0f, avgSpawnInterval + spawnIntervalRange / 2.0f);


    [SerializeField] float HP;
    [SerializeField] float defaultHP;
    public void GetDamage(float damage)
    {
        HP -= damage;

        // �_���[�W���o
        float duration = 0.3f;
        Sequence seq = DOTween.Sequence();
        seq.Append(spawnerSprite.DOColor(Color.white, 0));
        seq.Append(
            spawnerSprite.DOColor(Color.red, duration)
                .SetEase(Ease.Linear)
                .SetLoops(2, LoopType.Yoyo)
            );
        seq.Join(transform.DOShakePosition(duration, 0.4f));

        damageSound.Play();
    }

    [SerializeField] SpriteRenderer spawnerSprite;
    [SerializeField] AudioSource damageSound;


    float bossTimer = 0;
    [SerializeField] float bossTime;
    [SerializeField] float alertTime;

    bool isBossPhase = false;
    bool startedAlart = false;


    [SerializeField] GameObject cautionObject;
    [SerializeField] SpriteRenderer cautionColor;
    [SerializeField] AudioSource cautionAudio;

    [SerializeField] SpawnerIndicator spawnerIndicator;

    [SerializeField] Collider2D col;

    SpawnerSpawner spawnerSpawner;

    [SerializeField] protected SmokeManager sm;

    public void Init(SpawnerSpawner spawnerSpawner)
    {
        this.spawnerSpawner = spawnerSpawner;
    }

    [SerializeField] Slider hpSlider;



    void Start()
    {
        HP = defaultHP;
        
        // �X�P�[�����Ȃ���o�ꂷ�鉉�o
        float defaultScale = transform.localScale.x;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0, 0));
        seq.Append(transform.DOScale(defaultScale, 0.1f));
    }



    void Update()
    {
        if (GameManager.Instance._isFinish)
        {
            if(cautionAudio.isPlaying)
            {
                cautionAudio.Stop();
            }
            return;
        }


        // ===== HP ===== //
        if (HP <= 0)
        {
            Death();
        }
        else
        {
            hpSlider.value = HP / defaultHP;
        }


        // ===== �{�X�o���^�C�}�[ ===== //
        bossTimer += Time.deltaTime;
        if (bossTimer > bossTime)
        {
            Debug.Log("�{�X�o��", gameObject);
            isBossPhase = true;
        }
        if (bossTimer > alertTime)
        {
            if(!startedAlart)
            {
                // �x���i��x�������s�j
                startedAlart = true;
                cautionObject.SetActive(true);
                cautionAudio.Play();
                cautionColor.DOColor(Color.red, 0.2f)
                    .OnStart(() => cautionColor.color = Color.white)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo);
                spawnerIndicator.AlertActive = true;
            }
        }


        // ===== �G���� ===== //
        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;

            // �����ʒu���擾
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 spawnPos = myPos + Random.insideUnitCircle * spawnRadius;

            // �ǂ̃G�l�~�[���X�|�[�����邩�H
            GameObject enemyToSpawn;
            if(isBossPhase)
            {
                interval = bossInterval;
                enemyToSpawn = bossEnemy;
                spawnPos = transform.position;
            }
            else
            {
                interval = GetRandomInterval();
                float random = Random.Range(0.0f, 1.0f);
                if (random < enemy1Rate)
                {
                    enemyToSpawn = enemyPrefab1;
                }
                else
                {
                    enemyToSpawn = enemyPrefab2;
                }
            }

            // �G�l�~�[����
            Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
        }
    }


    void Death()
    {
        spawnerSpawner.OnSpawnerDied(this);

        col.enabled = false;

        // �Ԃ�鉉�o
        float defaultScale = transform.GetChild(0).localScale.x;
        float expandScale = defaultScale * 1.5f;
        sm.GenerateSmoke();
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.GetChild(0).DOScaleY(0, 0.5f).SetEase(Ease.OutExpo));
        seq.AppendCallback(() => Destroy(gameObject));
    }
}
