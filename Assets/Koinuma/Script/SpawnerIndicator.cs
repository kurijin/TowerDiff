using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemySpawner))]
public class SpawnerIndicator : MonoBehaviour
{
    [SerializeField] RectTransform _indicatorRect;
    [SerializeField] Image _indicatorImage;
    [SerializeField] float _displayWidth;
    [SerializeField] float _angleGap;
    [SerializeField] Color _alertColor;
    [SerializeField] float _flashingInterval;
    [SerializeField] bool _alertActive;

    EnemySpawner _enemySpawner;
    Sequence _alertSequence;

    public bool AlertActive { get => _alertActive; set => _alertActive = value; }

    private void Start()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
    }

    void Update()
    {
        DisplayIndicator();
    }

    void DisplayIndicator()
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2);
        Vector2 screenPosPivotCenter = Camera.main.WorldToScreenPoint(transform.position) - center;

        float d = Mathf.Max(
            Mathf.Abs(screenPosPivotCenter.x / (center.x - _displayWidth)),
            Mathf.Abs(screenPosPivotCenter.y / (center.y - _displayWidth))
        );

        Vector2 anchoredPos = screenPosPivotCenter;

        if (d > 1f)
        {
            anchoredPos.x /= d;
            anchoredPos.y /= d;
        }
        else
        {
            _indicatorRect.gameObject.SetActive(false);
            return;
        }

        // 位置の設定
        _indicatorRect.anchoredPosition = anchoredPos;
        _indicatorRect.eulerAngles = 
            new Vector3(0, 0, Mathf.Atan2(screenPosPivotCenter.y, screenPosPivotCenter.x) * Mathf.Rad2Deg + _angleGap);

        if (_alertActive) // todo アラート判定
        {
            if (!_alertSequence.IsActive())
            {
                _alertSequence = DOTween.Sequence();
                _alertSequence.Append(_indicatorImage.DOColor(_alertColor, 0));
                _alertSequence.AppendInterval(_flashingInterval);
                _alertSequence.Append(_indicatorImage.DOColor(Color.white, 0));
                _alertSequence.AppendInterval(_flashingInterval);
                _alertSequence.SetLoops(-1);
                _alertSequence.Play();
            }
        }
        else
        {
            _alertSequence?.Kill();
            _indicatorImage.color = Color.white;
        }

        _indicatorRect.gameObject.SetActive(true);
    }
}
