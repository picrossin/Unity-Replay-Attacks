using Mirror;
using Mirror.Examples.Pong;
using TMPro;
using UnityEngine;

public class LeftGoal : NetworkBehaviour
{
    [SerializeField] private NetworkManagerPong networkManager;
    
    [SyncVar] public int RightScore = 0;

    private void OnCollisionEnter2D()
    {
        // CmdServerAddPointLeft();
        networkManager.SendScoreMessage(false);
    }

    [Command(requiresAuthority = false)]
    private void CmdServerAddPointLeft()
    {
        RightScore++;
        RpcUpdateScore(RightScore);
    }

    [ClientRpc]
    public void RpcUpdateScore(int score)
    {
        GameObject.Find("RightScore").GetComponent<TextMeshProUGUI>().SetText(score.ToString());
    }
}
