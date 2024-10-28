using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイトルシーンのボタン管理
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] string battleSceneName;
    [SerializeField, Header("スタートボタン")] Button _startButton;
    [SerializeField, Header("設定ボタン")] Button _settingsButton;
    [SerializeField, Header("ライセンスボタン")] Button _licenseButton;
    [SerializeField, Header("戻るボタン")] Button _backButton;
    [SerializeField, Header("戻るボタン1")] Button _backButton1;

    [SerializeField, Header("設定UI")] GameObject _settingsUI;
    [SerializeField, Header("背景")] GameObject _backUI;
    [SerializeField, Header("ライセンスUI")] GameObject _licenseUI;
    [SerializeField,Header("ボタンクリック音")] AudioClip _buttonSE;


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
