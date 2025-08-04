using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<bool> correctList = new List<bool>();
    public List<float> reactTimeList = new List<float>();
    public List<int> userAnswerList = new List<int>();
    public List<int> correctAnswerList = new List<int>();
    
    private float answerStartTime;
    
    public void StartTiming()
    {
        answerStartTime = Time.time;
    }
    
    public void RecordAnswer(int userAnswer, int correctAnswer)
    {
        float rt = Time.time - answerStartTime;
        reactTimeList.Add(rt);
        userAnswerList.Add(userAnswer);
        correctAnswerList.Add(correctAnswer);
        correctList.Add(userAnswer == correctAnswer);
    }
    
    public float Accuracy() // 정확도
    {
        int correctCount = 0;
        foreach (bool isCorrect in correctList)
            if (isCorrect) correctCount++;

        return (float)correctCount / correctList.Count * 100f;
    }

    public float AverageReactTime() // 정답 반응 평균 시간
    {
        float sum = 0f;
        int count = 0;

        for (int i = 0; i < correctList.Count; i++)
        {
            if (correctList[i])
            {
                sum += reactTimeList[i];
                count++;
            }
        }

        return count > 0 ? sum / count : 0f;
    }

    // public float CalculateConsistency()
    // {
    //     float avg = CalculateAverageReactionTime();
    //     float sumSq = 0f;
    //     int count = 0;
    //
    //     for (int i = 0; i < correctList.Count; i++)
    //     {
    //         if (correctList[i])
    //         {
    //             float diff = reactTimeList[i] - avg;
    //             sumSq += diff * diff;
    //             count++;
    //         }
    //     }
    //
    //     return count > 1 ? Mathf.Sqrt(sumSq / (count - 1)) : 0f;
    // }
    
    public float CalculateDPrime() // 민감도
    {
        int hits = 0;
        int misses = 0;
        int falseAlarms = 0;
        int correctRejections = 0;

        for (int i = 0; i < userAnswerList.Count; i++)
        {
            int user = userAnswerList[i];
            int correct = correctAnswerList[i];

            if (correct != 0) // 타깃 있음
            {
                if (user == correct) hits++;
                else misses++;
            }
            else // 타깃 없음
            {
                if (user != 0) falseAlarms++;
                else correctRejections++;
            }
        }

        float hitRate = Mathf.Clamp01((float)hits / (hits + misses));
        float falseAlarmRate = Mathf.Clamp01((float)falseAlarms / (falseAlarms + correctRejections));

        float zHit = ZScore(hitRate);
        float zFA = ZScore(falseAlarmRate);

        return zHit - zFA;
    }

    private float ZScore(float p) // Z 변환용 함수 GPT 짱
    {
        if (p == 0f) p = 0.0001f;
        if (p == 1f) p = 0.9999f;

        return Mathf.Sqrt(2f) * InverseErf(2f * p - 1f);
    }
    
    private float InverseErf(float x) // 역 에러 함수 근사값 (Z-score 계산용)
    {
        float a = 0.147f;
        float ln = Mathf.Log(1 - x * x);
        float firstPart = 2 / (Mathf.PI * a) + ln / 2f;
        float secondPart = ln / a;

        return Mathf.Sign(x) * Mathf.Sqrt(Mathf.Sqrt(firstPart * firstPart - secondPart) - firstPart);
    }
    
    float NormalizeReactionTime(float rt)
    {
        float min = 0.8f; // 빠른 반응
        float max = 2.0f; // 느린 반응
        float clamped = Mathf.Clamp(rt, min, max);
        return (1f - ((clamped - min) / (max - min))) * 100f; // 빠르면 100점
    }
    
    float NormalizeDPrime(float dPrime)
    {
        float min = -1f;
        float max = 3f;
        float clamped = Mathf.Clamp(dPrime, min, max);
        return ((clamped - min) / (max - min)) * 100f; // -1 → 0점, 3 → 100점
    }
    
    public float TotalScore()
    {
        float acc = Accuracy(); // 0~100
        float rtAvg = AverageReactTime(); // 초 단위
        float dPrime = CalculateDPrime();

        float normRT = NormalizeReactionTime(rtAvg); // 0~100
        float normDP = NormalizeDPrime(dPrime);      // 0~100

        float score = acc * 0.6f + normRT * 0.2f + normDP * 0.2f;
        return score;
    }
}
