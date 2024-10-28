using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Q�[���̐ݒ��ʂł̉��ʂ̕ύX
/// </summary>
public class SoundSlider : MonoBehaviour
{
    [SerializeField, Header("BGM�̃X���C�_�[")] private Slider _bgmSlider;
    [SerializeField, Header("SE�̃X���C�_�[")] private Slider _seSlider;

    void OnEnable()
    {
        // �X���C�_�[�̏����l��SoundManager����擾���Đݒ�
        _bgmSlider.value = SoundManager.Instance.BGMVolume;
        _seSlider.value = SoundManager.Instance.SEVolume;
    }

    void Update()
    {
        // BGM�̉��ʂ��X���C�_�[�ɉ����ĕύX
        SoundManager.Instance.BGMVolume = _bgmSlider.value;

        // SE�̉��ʂ��X���C�_�[�ɉ����Ĉꊇ�ύX
        SoundManager.Instance.SEVolume = _seSlider.value;
    }
}
