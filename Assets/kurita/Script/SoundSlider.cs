using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの設定画面での音量の変更
/// </summary>
public class SoundSlider : MonoBehaviour
{
    [SerializeField, Header("BGMのスライダー")] private Slider _bgmSlider;
    [SerializeField, Header("SEのスライダー")] private Slider _seSlider;

    void OnEnable()
    {
        // スライダーの初期値をSoundManagerから取得して設定
        _bgmSlider.value = SoundManager.Instance.BGMVolume;
        _seSlider.value = SoundManager.Instance.SEVolume;
    }

    void Update()
    {
        // BGMの音量をスライダーに応じて変更
        SoundManager.Instance.BGMVolume = _bgmSlider.value;

        // SEの音量をスライダーに応じて一括変更
        SoundManager.Instance.SEVolume = _seSlider.value;
    }
}
