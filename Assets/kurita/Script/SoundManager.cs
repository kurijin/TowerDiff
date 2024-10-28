using UnityEngine;
using System.Linq;

/// <summary>
/// 音を流すスクリプト
/// BGMはループPlayBGMで再生
/// SEはPlaySEで再生
/// それぞれストップも実装
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField,Header("SEのAUdioSorceの生成数")] private int seSourceCount = 5; 
    private AudioSource bgmSource;
    private AudioSource[] seSources;  
    private float _bgmVolume = 0.1f;
    private float _seVolume = 1f;
    [SerializeField,Header("タイトルBGM")] private AudioClip _bgmClip;
  

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
    /// AudioSourseの生成
    /// </summary>
    private void SetupAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = _bgmVolume;

        // SE用AudioSourceを指定数だけ作成
        seSources = new AudioSource[seSourceCount];
        for (int i = 0; i < seSourceCount; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
            seSources[i].volume = _seVolume;
        }
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="clip">BGMのクリップ名</param>
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource != null)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    // BGMを停止
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();  
        }
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="clip">SEのクリップ名</param>
    public void PlaySE(AudioClip clip)
    {
        AudioSource availableSource = GetAvailableSESource();
        if (availableSource != null)
        {
            availableSource.PlayOneShot(clip);
        }
    }

    // SEを停止
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
    /// 音が流れていないAudioSorceを取得
    /// </summary>
    /// <returns></returns>
    private AudioSource GetAvailableSESource()
    {
        return seSources.FirstOrDefault(source => !source.isPlaying);
    }

    // BGM音量のプロパティ
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

    // SE音量のプロパティ
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
