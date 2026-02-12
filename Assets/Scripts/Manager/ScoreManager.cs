using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    public int rhythmGameScore;
    public int currentCombo;

    public int scorePerfect = 100;
    public int scoreCool = 80;
    public int scoreGood = 50;
    public int scoreBad = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        rhythmGameScore = 0;
    }

    // Update is called once per frame
    public void ProcessJudgement(string judgementType)
    {
        // 1. 콤보 처리
        if (judgementType == "Miss" || judgementType == "Bad")
        {
            currentCombo = 0; // 콤보 끊김
        }
        else
        {
            currentCombo++; // 콤보 증가
        }

        // 2. 점수 합산 (콤보 보너스 로직 추가 가능)
        int addScore = 0;

        switch (judgementType)
        {
            case "Perfect": addScore = scorePerfect; break;
            case "Cool":    addScore = scoreCool;    break;
            case "Good":    addScore = scoreGood;    break;
            case "Bad":     addScore = scoreBad;     break;
            case "Miss":    addScore = 0;            break;
        }

        // (선택사항) 콤보가 높으면 점수 더 주기
        // if (currentCombo > 10) addScore = (int)(addScore * 1.1f);

        rhythmGameScore += addScore;

        // 3. UI 갱신
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = string.Format("{0:n0}", rhythmGameScore); // 1,000 단위 쉼표
        if (comboText != null)
        {
            if (currentCombo > 1) comboText.text = currentCombo.ToString() + " COMBO";
            else comboText.text = ""; // 콤보 없으면 안 보이게
        }
    }
}
