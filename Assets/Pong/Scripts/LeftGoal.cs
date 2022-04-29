using Mirror;
using TMPro;
using UnityEngine;

public class LeftGoal : NetworkBehaviour
{
    [SyncVar] public int RightScore = 0;

    private void OnCollisionEnter2D()
    {
        CmdServerAddPointLeft();
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
