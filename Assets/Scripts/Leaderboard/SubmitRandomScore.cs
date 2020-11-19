using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


public class SubmitRandomScore : MonoBehaviour
{
    public string playerName = "Random User";
    const string BASE_URL = "https://www.tripleplusungood.com/savescore.php";

    public string MakeGetUrl(string name, int score)
    {
        return $"{BASE_URL}?name={name}&score={score}";
    }


    public void Submit()
    {
        int score = Random.Range(1, 1000);

        string randomPlayerName = $"{playerName} {Random.Range(1, 100)}";

        string url = MakeGetUrl(randomPlayerName, score);
        Post(url);
    }

    // todo: this is also used for getting scores
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }



    public void Post(string url)
    {
        Debug.Log("Post(" + url + ")");
        StartCoroutine(GetRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Debug.Log(req.downloadHandler.text);
                Leaderboard.Instance.Fetch();
            }
        }));
    }

}
