using DG.Tweening;
using UnityEngine;

public class ArrowAnim : MonoBehaviour
{
    [SerializeField] float _moveRnageY;
    [SerializeField] float _duration;

    float _initPosY;

    void Start()
    {
        Vector3 Pos1 = transform.position;
        Vector3 Pos2 = Pos1;
        Pos2.y += _moveRnageY;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(Pos2, _duration).SetEase(Ease.InOutCirc));
        sequence.Append(transform.DOMove(Pos1, _duration).SetEase(Ease.InOutCirc));
        sequence.SetLoops(-1);
        sequence.Play();
    }
}
