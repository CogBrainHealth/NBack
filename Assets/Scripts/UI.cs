using System.Collections;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [Header("Scripts References")]
    public GameManager gameManager;
    public ScoreManager scoreManager;
    
    [Header("Text References")]
    public TextMeshProUGUI problemNum;
    public TextMeshProUGUI correctPopUp;
    public TextMeshProUGUI scoreText;

    public void ProblemNumUpdate()
    {
        problemNum.text = $"{gameManager.currentProblem} / 10";
    }

    public IEnumerator CorrectPopUp(bool isCorrect)
    {
        correctPopUp.text = isCorrect ? "O" : "X";
        yield return new WaitForSeconds(1f);
        correctPopUp.text = "";
    }

    public void ScoreText()
    {
        float acc = scoreManager.Accuracy();
        float rt = scoreManager.AverageReactTime();
        float dPrime = scoreManager.CalculateDPrime();
        float finalScore = scoreManager.TotalScore();

        scoreText.text = $"최종 점수: {finalScore:F1}점\n\n정확도: {acc:F1}%\n반응 시간: {rt:F2}초\n민감도 d': {dPrime:F2}";
    }
}
