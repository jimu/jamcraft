using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


// Do not use this component on GameObjects (Buttons or Panels) that may be deactivated.
// Deactivating a GameObject will kill the coroutine

public class SubmitScore : MonoBehaviour
{
    const string BASE_URL = "https://www.tripleplusungood.com/savescore.php";

    public string MakeGetUrl(string name, int score)
    {
        return $"{BASE_URL}?name={name}&score={score}";
    }


    public void Submit(string name, int score)
    {
        string url = MakeGetUrl(name, score);
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
        Debug.Log($"Post: {url}");
        StartCoroutine(GetRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Debug.Log(req.downloadHandler.text);
                Leaderboard.Instance.ForceFetch();
            }
        }));
    }

}
