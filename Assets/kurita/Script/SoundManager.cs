using UnityEngine;
using System.Linq;

/// <summary>
/// ���𗬂��X�N���v�g
/// BGM�̓��[�vPlayBGM�ōĐ�
/// SE��PlaySE�ōĐ�
/// ���ꂼ��X�g�b�v������
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField,Header("SE��AUdioSorce�̐�����")] private int seSourceCount = 5; 
    private AudioSource bgmSource;
    private AudioSource[] seSources;  
    private float _bgmVolume = 0.1f;
    private float _seVolume = 1f;
    [SerializeField,Header("�^�C�g��BGM")] private AudioClip _bgmClip;
  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SetupAudioSources();
            PlayBGM(_bgmClip);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// AudioSourse�̐���
    /// </summary>
    private void SetupAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = _bgmVolume;

        // SE�pAudioSource���w�萔�����쐬
        seSources = new AudioSource[seSourceCount];
        for (int i = 0; i < seSourceCount; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
            seSources[i].volume = _seVolume;
        }
    }

    /// <summary>
    /// BGM�̍Đ�
    /// </summary>
    /// <param name="clip">BGM�̃N���b�v��</param>
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource != null)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    // BGM���~
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();  
        }
    }

    /// <summary>
    /// SE�̍Đ�
    /// </summary>
    /// <param name="clip">SE�̃N���b�v��</param>
    public void PlaySE(AudioClip clip)
    {
        AudioSource availableSource = GetAvailableSESource();
        if (availableSource != null)
        {
            availableSource.PlayOneShot(clip);
        }
    }

    // SE���~
    public void StopAllSE()
    {
        foreach (var source in seSources)
        {
            if (source.isPlaying)
            {
                source.Stop();  
            }
        }
    }

    /// <summary>
    /// ��������Ă��Ȃ�AudioSorce���擾
    /// </summary>
    /// <returns></returns>
    private AudioSource GetAvailableSESource()
    {
        return seSources.FirstOrDefault(source => !source.isPlaying);
    }

    // BGM���ʂ̃v���p�e�B
    public float BGMVolume
    {
        get { return _bgmVolume; }
        set
        {
            _bgmVolume = value;
            if (bgmSource != null)
            {
                bgmSource.volume = _bgmVolume;
            }
        }
    }

    // SE���ʂ̃v���p�e�B
    public float SEVolume
    {
        get { return _seVolume; }
        set
        {
            _seVolume = value;
            foreach (var source in seSources)
            {
                source.volume = _seVolume;  
            }
        }
    }
}
