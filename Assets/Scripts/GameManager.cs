using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scripts References")]
    public EmblemGenerator emblemGenerator;
    public UI ui;
    public ScoreManager scoreManager;

    [Header("UI")] 
    public GameObject start;
    public GameObject game;
    public GameObject end;
    public GameObject button1;
    public GameObject button2;
    public GameObject buttonX;
    
    [Header("Problem Flags")]
    public int currentProblem = 1;
    public int totalProblems = 10;

    void Awake()
    {
        start.SetActive(true);
        game.SetActive(false);
        end.SetActive(false);
    }
    
    public void GameStart()
    {
        ui.ProblemNumUpdate();
        StartCoroutine(emblemGenerator.StartDiagram());
    }
    
    public void OnUserAnswerr(int selected)
    {
        Diagram curr = emblemGenerator.history[emblemGenerator.history.Count - 1];
        Diagram oneBack = emblemGenerator.history[emblemGenerator.history.Count - 2];
        Diagram twoBack = emblemGenerator.history[emblemGenerator.history.Count - 3];

        bool match2Back = curr == twoBack;
        bool match1Back = curr == oneBack;

        // 다중 정답 가능
        bool isCorrect =
            (selected == 2 && match2Back) ||
            (selected == 1 && match1Back) ||
            (selected == 0 && !match2Back && !match1Back);

        // 기록 저장
        int correctAnswer;
        if (match2Back && match1Back) correctAnswer = selected;
        else if (match2Back) correctAnswer = 2;
        else if (match1Back) correctAnswer = 1;
        else correctAnswer = 0;

        scoreManager.RecordAnswer(selected, correctAnswer);
    
        Debug.Log(isCorrect ? "정답" : "오답");
        StartCoroutine(ui.CorrectPopUp(isCorrect));

        currentProblem++;
        if (currentProblem <= totalProblems)
        {
            HideButtons();
            StartCoroutine(emblemGenerator.ShowNextDiagram());
        }
        else EndGame();
    }

    
    public void ShowButtons()
    {
        button1.SetActive(true);
        button2.SetActive(true);
        buttonX.SetActive(true);
    }

    public void HideButtons()
    {
        button1.SetActive(false);
        button2.SetActive(false);
        buttonX.SetActive(false);
    }

    public void EndGame()
    {
        emblemGenerator.diagramPoint.enabled = false;
        HideButtons();
        game.SetActive(false);
        end.SetActive(true);
        ui.ScoreText();
        Debug.Log("게임 종료");
    }
}
