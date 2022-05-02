using System.Collections;
using Mirror.Examples.Pong;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetPost : MonoBehaviour
{
    [SerializeField] private NetworkManagerPong networkManager;
    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;
    
    private int leftPlayerScore;
    private int rightPlayerScore;

    public void Score(bool leftScored)
    {
        SendScore(leftScored ? "left" : "right");
        networkManager.SendScoreMessage(leftScored);
    }
    
    public void SendScore(string scores)
    {
        StartCoroutine(SendPost(scores));
    }

    public void GetScores()
    {
        StartCoroutine(SendGet());
    }

    private IEnumerator SendPost(string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("unitypost", message);

        using (UnityWebRequest www = UnityWebRequest.Post("http://ipv4.fiddler/php_program/prog.php", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                print($"Sent score.");
            }
        }
    }

    private IEnumerator SendGet()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://ipv4.fiddler/php_program/prog.php?unityget="))
        {
            yield return www.SendWebRequest();
 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string[] scores = www.downloadHandler.text.Split();
                leftScoreText.SetText(scores[0]);
                rightScoreText.SetText(scores[1]);
                print($"Received score {scores[0]} {scores[1]}");
            }
        }
    }
}
