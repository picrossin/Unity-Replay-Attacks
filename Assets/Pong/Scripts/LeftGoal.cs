using Mirror;
using Mirror.Examples.Pong;
using TMPro;
using UnityEngine;

public class LeftGoal : MonoBehaviour
{
    [SerializeField] private NetworkManagerPong networkManager;
    [SerializeField] private GetPost getPost;
    
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (networkManager.useUDPScores)
        {
            networkManager.SendScoreMessage(false);
        }
        else
        {
            getPost.Score(false);
        }
    }
}
