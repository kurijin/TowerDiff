using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// リザルト画面でのスコア
/// </summary>
public class ResultCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField, Header("ハイスコアテキスト")] TextMeshProUGUI highScoreText;
    [SerializeField, Header("ハイスコア時の表示テキスト")] TextMeshProUGUI newRecordText;
    [SerializeField, Header("タイムマネージャーの取得")] TimeManager _timeManager;

    [SerializeField, Header("ハイスコアSE")] private AudioClip _highScoreSE;
    [SerializeField, Header("ScoreGetSE")] private AudioClip _ScoreSE;


    [SerializeField] string titleSceneName;
    [SerializeField] string battleSceneName;

    private const string HighScoreKey = "HighScore";
    private Coroutine blinkCoroutine;

    /// <summary>
    /// タイムをfloatで秒数で取得する
    /// ハイスコアと今回のスコアを比較する
    /// </summary>
    void OnEnable()
    {
        SoundManager.Instance.StopBGM();
        newRecordText.gameObject.SetActive(false);
        float currentTimeInSeconds = GetCurrentTimeInSeconds();
        ShowScore(currentTimeInSeconds);

        float bestTimeInSeconds = PlayerPrefs.GetFloat(HighScoreKey, 0);  // デフォルトは0
        ShowHighScore(bestTimeInSeconds);

        CheckHighScore(currentTimeInSeconds);
    }

    /// <summary>
    /// 今回のスコアを秒数に直すメソッド
    /// </summary>
    /// <returns></returns>
    private float GetCurrentTimeInSeconds()
    {
        return _timeManager.minute * 60 + _timeManager.time;
    }

    /// <summary>
    /// スコアの秒数を時間形式で表示
    /// </summary>
    /// <param name="scoreInSeconds">今回のスコアの秒数</param>
    public void ShowScore(float scoreInSeconds)
    {
        int minutes = Mathf.FloorToInt(scoreInSeconds / 60);
        float seconds = scoreInSeconds % 60;
        scoreText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// ハイスコアを表示するもの
    /// </summary>
    /// <param name="scoreInSeconds">ハイスコアの秒</param>
    public void ShowHighScore(float scoreInSeconds)
    {
        int minutes = Mathf.FloorToInt(scoreInSeconds / 60);
        float seconds = scoreInSeconds % 60;
        highScoreText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// ハイスコアかどうかを確認する
    /// </summary>
    /// <param name="currentScoreInSeconds"></param>
    private void CheckHighScore(float currentScoreInSeconds)
    {
        float highScore = PlayerPrefs.GetFloat(HighScoreKey, 0);  // デフォルトは0

        if (currentScoreInSeconds > highScore)  // より長いタイムならハイスコア更新
        {
            // ハイスコアSEを再生
            SoundManager.Instance.PlaySE(_highScoreSE);

            // 新記録ならハイスコアを更新し保存
            PlayerPrefs.SetFloat(HighScoreKey, currentScoreInSeconds);
            PlayerPrefs.Save();

            // newRecordTextを表示し、点滅させる
            newRecordText.gameObject.SetActive(true);
            blinkCoroutine = StartCoroutine(BlinkNewRecordText());

            // ハイスコアテキストも更新
            ShowHighScore(currentScoreInSeconds);
        }
        else
        {
            // ハイスコアでない場合は通常のスコアSEを再生
            SoundManager.Instance.PlaySE(_ScoreSE);
        }
    }

    /// <summary>
    /// newRecordTextを点滅させるコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlinkNewRecordText()
    {
        while (true)
        {
            newRecordText.enabled = !newRecordText.enabled;  
            yield return new WaitForSeconds(0.7f);  
        }
    }

    // タイトルに戻る
    public void ReturnToTitle()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        SceneManager.LoadScene(titleSceneName);
    }

    // もう一度プレイ
    public void PlayAgain()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        SceneManager.LoadScene(battleSceneName);
    }
}
