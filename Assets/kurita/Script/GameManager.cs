using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// �Q�[���t���[�ɂ��ď�Ԃ��Ǘ��������
/// 
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("�X�e�[�g�֘A�i�K�v�Ȃ��H�H�j")]
    [SerializeField, Header("�Q�[���X�^�[�g�t���O")] public bool _isStart = true;
    [SerializeField, Header("�Q�[�����t���O")] public bool _isInGame = false;
    [SerializeField, Header("�Q�[���I���t���O")] public bool _isFinish = false;

    [Header("UI�֘A,InGameUI���Q��")]
    [SerializeField, Header("IngaUI")] private GameObject _inGameUI;
    [SerializeField, Header("�Q�[���X�^�[�gUI")] private GameObject _startUI;
    [SerializeField, Header("�Q�[���I��UI")] private GameObject _gameFinishUI;

    [SerializeField, Header("�^�C�g���V�[���̖��O")] private�@string _titleSceneName;

    [Header("�G���^�C���Ǘ�,�G�ƃ^�C���̃}�l�[�W���[�Q��")]
    [SerializeField, Header("�G�}�l�[�W���[")] private SpawnerSpawner _spawnerSpawner;
    [SerializeField, Header("�^�C���}�l�[�W���[")] private TimeManager _timeManager;

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
    /// �Q�[���X�^�[�g���ɓǂݍ��܂�����
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
    /// ���g���C
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// �Q�[���I�����ɌĂяo�����
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
        //�Q�[���I������
        //���U���g�\���H



        //SceneManager.LoadScene(_titleSceneName);
    }




}
