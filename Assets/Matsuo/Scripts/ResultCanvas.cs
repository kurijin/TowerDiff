using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ���U���g��ʂł̃X�R�A
/// </summary>
public class ResultCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField, Header("�n�C�X�R�A�e�L�X�g")] TextMeshProUGUI highScoreText;
    [SerializeField, Header("�n�C�X�R�A���̕\���e�L�X�g")] TextMeshProUGUI newRecordText;
    [SerializeField, Header("�^�C���}�l�[�W���[�̎擾")] TimeManager _timeManager;

    [SerializeField, Header("�n�C�X�R�ASE")] private AudioClip _highScoreSE;
    [SerializeField, Header("ScoreGetSE")] private AudioClip _ScoreSE;


    [SerializeField] string titleSceneName;
    [SerializeField] string battleSceneName;

    private const string HighScoreKey = "HighScore";
    private Coroutine blinkCoroutine;

    /// <summary>
    /// �^�C����float�ŕb���Ŏ擾����
    /// �n�C�X�R�A�ƍ���̃X�R�A���r����
    /// </summary>
    void OnEnable()
    {
        SoundManager.Instance.StopBGM();
        newRecordText.gameObject.SetActive(false);
        float currentTimeInSeconds = GetCurrentTimeInSeconds();
        ShowScore(currentTimeInSeconds);

        float bestTimeInSeconds = PlayerPrefs.GetFloat(HighScoreKey, 0);  // �f�t�H���g��0
        ShowHighScore(bestTimeInSeconds);

        CheckHighScore(currentTimeInSeconds);
    }

    /// <summary>
    /// ����̃X�R�A��b���ɒ������\�b�h
    /// </summary>
    /// <returns></returns>
    private float GetCurrentTimeInSeconds()
    {
        return _timeManager.minute * 60 + _timeManager.time;
    }

    /// <summary>
    /// �X�R�A�̕b�������Ԍ`���ŕ\��
    /// </summary>
    /// <param name="scoreInSeconds">����̃X�R�A�̕b��</param>
    public void ShowScore(float scoreInSeconds)
    {
        int minutes = Mathf.FloorToInt(scoreInSeconds / 60);
        float seconds = scoreInSeconds % 60;
        scoreText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// �n�C�X�R�A��\���������
    /// </summary>
    /// <param name="scoreInSeconds">�n�C�X�R�A�̕b</param>
    public void ShowHighScore(float scoreInSeconds)
    {
        int minutes = Mathf.FloorToInt(scoreInSeconds / 60);
        float seconds = scoreInSeconds % 60;
        highScoreText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// �n�C�X�R�A���ǂ������m�F����
    /// </summary>
    /// <param name="currentScoreInSeconds"></param>
    private void CheckHighScore(float currentScoreInSeconds)
    {
        float highScore = PlayerPrefs.GetFloat(HighScoreKey, 0);  // �f�t�H���g��0

        if (currentScoreInSeconds > highScore)  // ��蒷���^�C���Ȃ�n�C�X�R�A�X�V
        {
            // �n�C�X�R�ASE���Đ�
            SoundManager.Instance.PlaySE(_highScoreSE);

            // �V�L�^�Ȃ�n�C�X�R�A���X�V���ۑ�
            PlayerPrefs.SetFloat(HighScoreKey, currentScoreInSeconds);
            PlayerPrefs.Save();

            // newRecordText��\�����A�_�ł�����
            newRecordText.gameObject.SetActive(true);
            blinkCoroutine = StartCoroutine(BlinkNewRecordText());

            // �n�C�X�R�A�e�L�X�g���X�V
            ShowHighScore(currentScoreInSeconds);
        }
        else
        {
            // �n�C�X�R�A�łȂ��ꍇ�͒ʏ�̃X�R�ASE���Đ�
            SoundManager.Instance.PlaySE(_ScoreSE);
        }
    }

    /// <summary>
    /// newRecordText��_�ł�����R���[�`��
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

    // �^�C�g���ɖ߂�
    public void ReturnToTitle()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        SceneManager.LoadScene(titleSceneName);
    }

    // ������x�v���C
    public void PlayAgain()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        SceneManager.LoadScene(battleSceneName);
    }
}
