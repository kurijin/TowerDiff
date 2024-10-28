using System.Collections;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ゲームスタート時のカウントダウン
/// </summary>
public class StartUI : MonoBehaviour
{
    [SerializeField, Header("カウントダウンテキスト")] private Text _countDownText;
    [SerializeField] AudioClip countdown;


    void OnEnable()
    {
        _countDownText.text = "";
        CountdownCoroutine().Forget();
    }

    private async UniTask CountdownCoroutine()
    {
        _countDownText.gameObject.SetActive(true);
        SoundManager.Instance.PlaySE(countdown);

        _countDownText.text = "3";
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        _countDownText.text = "2";
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        _countDownText.text = "1";
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        _countDownText.text = "GO!";
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        _countDownText.text = "";
        _countDownText.gameObject.SetActive(false);

        GameManager.Instance.GameStart();
    }
}