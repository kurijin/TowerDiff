using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ìGåÇîjéûÇÃâåê∂ê¨ÉNÉâÉX
/// </summary>
public class SmokeManager : MonoBehaviour
{
    [SerializeField] Image bigSmoke;
    [SerializeField] Image smallSmoke;
    Vector3 defaultBS_Pos;
    Vector3 defaultBS_Scale;
    Vector3 defaultSS_Pos;
    Vector3 defaultSS_Scale;

    void Start()
    {
        defaultBS_Pos = bigSmoke.rectTransform.localPosition;
        defaultBS_Scale = bigSmoke.rectTransform.localScale;
        defaultSS_Pos = smallSmoke.rectTransform.localPosition;
        defaultSS_Scale = smallSmoke.rectTransform.localScale;
        bigSmoke.rectTransform.DOLocalMove(Vector2.zero, 0);
        bigSmoke.rectTransform.DOScale(0, 0);
        smallSmoke.rectTransform.DOLocalMove(Vector2.zero, 0);
        smallSmoke.rectTransform.DOScale(0, 0);
        this.gameObject.SetActive(false);
    }

    public void GenerateSmoke()
    {
        this.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(bigSmoke.rectTransform.DOLocalMove(defaultBS_Pos, 0.5f))
            .Join(bigSmoke.rectTransform.DOScale(defaultBS_Scale, 0.5f).SetEase(Ease.OutBack))
            .Append(bigSmoke.DOFade(0, 0.5f));

        DOTween.Sequence()
            .Append(smallSmoke.rectTransform.DOLocalMove(defaultSS_Pos, 0.5f))
            .Join(smallSmoke.rectTransform.DOScale(defaultSS_Scale, 0.5f).SetEase(Ease.OutBack))
            .Append(smallSmoke.DOFade(0, 0.5f));
    }
}
