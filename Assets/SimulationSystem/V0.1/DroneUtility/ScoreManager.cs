using UnityEngine;
using System.Collections;
using TMPro;
public class ScoreManager : MonoBehaviour
{

    public int currentScore = 0;
    public int MaxScorePerInstance;

    public float StandardTimeToAvoidAnObstacle;

    public int DeductionAndIncrementEachSecond;

    public bool DonewithObstacle;

    public TextMeshProUGUI ScoreText, FinalScreentext;

    public float TimeElapsed;
    private void Update()
    {
        ScoreText.text =  currentScore.ToString();
       
    }
    public void UpdateFinalScroneonEndScreen()
    {
        FinalScreentext.text =  currentScore.ToString();
    }
    public void StartNewScoringSession()
    {
      
        DonewithObstacle = false;
        StartCoroutine(startCountdown());
    }

    public void EndcurrentScoringSession()
    {
        DonewithObstacle = true;
    }
    public IEnumerator startCountdown()
    {
        float ElapsedTime = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);

            ElapsedTime += 1f;
            TimeElapsed = ElapsedTime;
            if (DonewithObstacle)
            {
                if(ElapsedTime< StandardTimeToAvoidAnObstacle)
                {
                    int Timediff =(int) Mathf.Abs(StandardTimeToAvoidAnObstacle - ElapsedTime);
                    int TempScore = MaxScorePerInstance;
                    for(int i =0; i<Timediff; i++)
                    {
                        Debug.Log("#101 We are Incremnenting the score");
                        TempScore += DeductionAndIncrementEachSecond;
                    }

                    currentScore += TempScore;
                    yield break;

                }
                else if (ElapsedTime >= StandardTimeToAvoidAnObstacle)
                {
                    int Timediff = (int)Mathf.Abs( ElapsedTime - StandardTimeToAvoidAnObstacle);
                    int TempScore = MaxScorePerInstance;

                    for (int i = 0; i < Timediff; i++)
                    {
                        Debug.Log("#101 We are decrementing the score");

                        TempScore -= DeductionAndIncrementEachSecond;
                    }

                    if (TempScore >= 0)
                    {
                    currentScore += TempScore;

                    }
                    yield break;
                }
            }
        }
    }

}
