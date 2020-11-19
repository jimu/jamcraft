using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region HelperClasses

[Serializable]
public class Result
{
    public List<Highscore> result;
}


[Serializable]
public class Highscore
{
    public int rank;
    public string name;
    public int score;

    public Highscore(int rank, string name, int score)
    {
        this.rank = rank;
        this.name = name;
        this.score = score;
    }
}
#endregion




public class Leaderboard : MonoSingleton<Leaderboard>
{
    static public Action<List<Highscore>> onChange;
    
    private List<Highscore> highscores = new List<Highscore>();
    bool fetching = false;
    bool dirty = true;
    float nextFetchTime = float.MinValue;
    const string URL_GETSCORES = "https://www.tripleplusungood.com/jamcraft_rank.php";

    protected override void Init()
    {
        Fetch();
    }

    public void ForceFetch()
    {
        Debug.Log("ForceFetch()");
        dirty = true;
        Fetch();
    }

    public void Fetch()
    {
        GetHighscores();
        //StartCoroutine(FetchComplete());
    }


    public List<Highscore> Get()
    {
        return highscores;
    }

    private IEnumerator Debug_FetchComplete()
    {
        yield return new WaitForSeconds(2f);
        highscores.Clear(); // Todo: simulation
        highscores.Add(new Highscore(1, "Scarmez", 101));
        highscores.Add(new Highscore(1, "BunnyViking", 101));
        highscores.Add(new Highscore(1, "Dakineya", 101));
        highscores.Add(new Highscore(4, "Flibbertigibbet", 90));
        highscores.Add(new Highscore(5, "Snickersnee", 80));
        highscores.Add(new Highscore(6, "Widdershins", 11));
        highscores.Add(new Highscore(7, "Snollygoster", 5));
        highscores.Add(new Highscore(7, "Nudiustertian", 5));
        highscores.Add(new Highscore(9, "Yarborough", 1));

        fetching = false;
        nextFetchTime = Time.realtimeSinceStartup + 15f;

        onChange?.Invoke(highscores);
    }


    // This is a generic HTTP[S]-GET call with a callback
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }

    /**
     * This makes a leaderboard API call to get the highscore list
     * The call is only made if
     *  a) we are not already fetching, and
     *   1) enough time has passed since the last fetch, or
     *   2) the table has been marked dirty (usually because of a save-score operation)
     *  
     */

    public void GetHighscores(bool force = false)
    {
        if (!fetching && (nextFetchTime < Time.realtimeSinceStartup || dirty))
        {
            fetching = true;
            StartCoroutine(GetRequest(URL_GETSCORES, (UnityWebRequest req) =>
            {
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                }
                else
                {
                    Result result = JsonUtility.FromJson<Result>(req.downloadHandler.text);
                    this.highscores = result.result;

                    fetching = false;
                    nextFetchTime = Time.realtimeSinceStartup + 15f;

                    onChange?.Invoke(highscores);
                }
            }));
        }
    }
}
