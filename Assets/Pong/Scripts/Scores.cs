// Inspired by Dapper Dinos's video: https://www.youtube.com/watch?v=LnqnO7_KRsU
using Mirror;
using TMPro;
using UnityEngine;

public struct ScoreMessage : NetworkMessage
{
    public bool leftScored;
}

public class Scores : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;

    private int leftPlayerScore;
    private int rightPlayerScore;
    
    public void SetupScoring()
    {
        if (!NetworkClient.active)
        {
            return;
        }
        
        NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
    }

    public void OnScore(ScoreMessage msg)
    {
        if (msg.leftScored)
        {
            leftPlayerScore++;
        }
        else
        {
            rightPlayerScore++;
        }
        
        leftScoreText.SetText(leftPlayerScore.ToString());
        rightScoreText.SetText(rightPlayerScore.ToString());
    }
}
