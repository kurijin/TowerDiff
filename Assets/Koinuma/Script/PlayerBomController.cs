using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBomController : MonoBehaviour
{
    [SerializeField] GameObject _bomPrefab;
    [SerializeField] int _initialMP;
    [Space(10)]
    [SerializeField] Sprite _bomIcon;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;

    // mp1Ç≈1Ç¬bomÇÇ®ÇØÇÈÇ∆Ç∑ÇÈ
    int _currentMP;
    Queue<GameObject> _iconQueue = new Queue<GameObject>();

    private void Start()
    {
        _currentMP = _initialMP;
    }

    private void Update()
    {
        if (_currentMP > 0 && Input.GetButtonDown("Fire1")) GenerateBom();
    }

    void GenerateBom()
    {
        GameObject.Instantiate(_bomPrefab, transform.position, Quaternion.identity);
        _currentMP--;
        Destroy(_iconQueue.Dequeue());
    }

    public void AddMP(int mp)
    {
        _currentMP += mp;

        // uiçXêV
        for (int i = 0; i < mp; i++)
        {
            GameObject newIcon = new GameObject("Bom Icon");
            newIcon.transform.SetParent(_gridLayoutGroup.transform);
            newIcon.AddComponent<Image>().sprite = _bomIcon;
            newIcon.AddComponent<Outline>().effectColor = Color.black;
            _iconQueue.Enqueue(newIcon);
        }
    }
}
