using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �^�C�g���V�[���̃{�^���Ǘ�
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] string battleSceneName;
    [SerializeField, Header("�X�^�[�g�{�^��")] Button _startButton;
    [SerializeField, Header("�ݒ�{�^��")] Button _settingsButton;
    [SerializeField, Header("���C�Z���X�{�^��")] Button _licenseButton;
    [SerializeField, Header("�߂�{�^��")] Button _backButton;
    [SerializeField, Header("�߂�{�^��1")] Button _backButton1;

    [SerializeField, Header("�ݒ�UI")] GameObject _settingsUI;
    [SerializeField, Header("�w�i")] GameObject _backUI;
    [SerializeField, Header("���C�Z���XUI")] GameObject _licenseUI;
    [SerializeField,Header("�{�^���N���b�N��")] AudioClip _buttonSE;


    void Start()
    {
        _startButton.onClick.AddListener(GameStart);
        _settingsButton.onClick.AddListener(Settings);
        _licenseButton.onClick.AddListener(License);
        _backButton.onClick.AddListener(Back);
        _backButton1.onClick.AddListener(Back);
    }


    private void GameStart()
    {
        SoundManager.Instance.PlaySE(_buttonSE);
        SceneManager.LoadScene(battleSceneName);
    }

    private void Settings()
    {
        _backUI.SetActive(false);
        SoundManager.Instance.PlaySE(_buttonSE);
        _settingsUI.SetActive(true);
    }

    private void License()
    {
        SoundManager.Instance.PlaySE(_buttonSE);
        _licenseUI.SetActive(true);
    }

    private void Back()
    {
        SoundManager.Instance.PlaySE(_buttonSE);
        _backUI.SetActive(true);
        _settingsUI.SetActive(false);
        _licenseUI.SetActive(false);
    }


    public void LoadBattleScene()
    {
        SceneManager.LoadScene(battleSceneName);
    }
}
