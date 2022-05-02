using Mirror;
using Mirror.Examples.Pong;
using TMPro;
using UnityEngine;

public class RightGoal : NetworkBehaviour
{
    [SerializeField] private NetworkManagerPong networkManager;
    
    [SyncVar] public int LeftScore = 0;

    private void OnCollisionEnter2D()
    {
        // CmdServerAddPointRight();
        networkManager.SendScoreMessage(true);
    }

    [Command(requiresAuthority = false)]
    private void CmdServerAddPointRight()
    {
        LeftScore++;
        RpcUpdateScore(LeftScore);
    }

    [ClientRpc]
    public void RpcUpdateScore(int score)
    {
        GameObject.Find("LeftScore").GetComponent<TextMeshProUGUI>().SetText(score.ToString());
    }
}
