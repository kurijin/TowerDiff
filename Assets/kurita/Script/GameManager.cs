using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// ゲームフローについて状態を管理するもの
/// 
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("ステート関連（必要ない？？）")]
    [SerializeField, Header("ゲームスタートフラグ")] public bool _isStart = true;
    [SerializeField, Header("ゲーム中フラグ")] public bool _isInGame = false;
    [SerializeField, Header("ゲーム終了フラグ")] public bool _isFinish = false;

    [Header("UI関連,InGameUIを参照")]
    [SerializeField, Header("IngaUI")] private GameObject _inGameUI;
    [SerializeField, Header("ゲームスタートUI")] private GameObject _startUI;
    [SerializeField, Header("ゲーム終了UI")] private GameObject _gameFinishUI;

    [SerializeField, Header("タイトルシーンの名前")] private　string _titleSceneName;

    [Header("敵＆タイム管理,敵とタイムのマネージャー参照")]
    [SerializeField, Header("敵マネージャー")] private SpawnerSpawner _spawnerSpawner;
    [SerializeField, Header("タイムマネージャー")] private TimeManager _timeManager;

    [SerializeField, Header("DefaultBGM")] private AudioClip _defaultBGM;

    public bool IsInGame => _isInGame;



    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        SoundManager.Instance.PlayBGM(_defaultBGM);
        _inGameUI.SetActive(false);
        _startUI.SetActive(false);
        _gameFinishUI.SetActive(false);

        if (_isStart)
        {
            _startUI.SetActive(true);
        }
    }

    /// <summary>
    /// ゲームスタート時に読み込まれるもの
    /// </summary>
    public void GameStart()
    {
        _startUI.SetActive(false);
        _inGameUI.SetActive(true);

        _isStart = false;
        _isInGame = true;
        _spawnerSpawner.canSpawn = true;
        _timeManager.canCountTime = true;
    }

    /// <summary>
    /// リトライ
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// ゲーム終了時に呼び出される
    /// </summary>
    public async void GameEnd()
    {
        _spawnerSpawner.canSpawn = false;
        _timeManager.canCountTime = false;

        await UniTask.Delay(TimeSpan.FromSeconds(3f));

        _isInGame = false;
        _isFinish = true;
        _gameFinishUI.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        //ゲーム終了処理
        //リザルト表示？



        //SceneManager.LoadScene(_titleSceneName);
    }




}
