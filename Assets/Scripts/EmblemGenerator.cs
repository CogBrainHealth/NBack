using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EmblemGenerator : MonoBehaviour
{
    [Header("Scripts References")]
    public GameManager gameManager;
    public UI ui;
    
    [Header("UI")]
    public GameObject gameCanvas;
    
    [Header("Diagram Flags")]
    public Diagram[] diagrams;
    public Image diagramPoint;
    public List<Diagram> history = new List<Diagram>();
    private bool isShowingDiagram = false; // 중복 실행 방지

    [Header("Text")] 
    public GameObject ment;
    public GameObject question;
    public TextMeshProUGUI emblemCount;
    
    public IEnumerator StartDiagram()
    {
        diagramPoint.enabled = false;
        yield return new WaitForSeconds(1f);
        gameCanvas.SetActive(true);

        history.Add(diagrams[Random.Range(0, diagrams.Length)]);
        do
        {
            if (history.Count > 1) history.RemoveAt(1);
            history.Add(diagrams[Random.Range(0, diagrams.Length)]);
        }
        while (history[1] == history[0]);

        for (int i = 0; i < 2; i++)
        {
            diagramPoint.enabled = true;
            diagramPoint.sprite = history[i].sprite;
            
            // 카운트다운 3초
            for (int sec = 3; sec > 0; sec--)
            {
                emblemCount.text = sec.ToString();
                yield return new WaitForSeconds(1f);
            }
            emblemCount.text = "";
            
            diagramPoint.enabled = false;
            yield return new WaitForSeconds(1f);
        }

        ment.SetActive(false);
        StartCoroutine(ShowNextDiagram());
    }
    
    public IEnumerator ShowNextDiagram()
    {
        if (isShowingDiagram) yield break; // 중복 실행 방지
        isShowingDiagram = true;
        
        diagramPoint.enabled = false;
        question.SetActive(false);
        yield return new WaitForSeconds(1f);
        ui.ProblemNumUpdate();

        Diagram questDiagram;
        float roll = Random.value;

        if (roll < 0.25f) questDiagram = history[history.Count - 2];
        else if (roll < 0.50f) questDiagram = history[history.Count - 1];
        else
        {
            do {
                questDiagram = diagrams[Random.Range(0, diagrams.Length)];
            } while (questDiagram == history[history.Count - 2] || questDiagram == history[history.Count - 1]);
        }
        history.Add(questDiagram);

        diagramPoint.enabled = true;
        question.SetActive(true);
        diagramPoint.sprite = questDiagram.sprite;
        gameManager.ShowButtons();
        
        isShowingDiagram = false;
    }
}

[System.Serializable]
public class Diagram
{
    public Sprite sprite;
}