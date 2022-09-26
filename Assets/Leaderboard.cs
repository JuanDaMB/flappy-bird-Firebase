using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public List<GameObject> hgscores;
    private List<TextMeshProUGUI> hgscoresText;
    private DatabaseReference _reference;

    public void Start()
    {
        hgscoresText = new List<TextMeshProUGUI>();
        foreach (GameObject o in hgscores)
        {
            hgscoresText.Add(o.GetComponentInChildren<TextMeshProUGUI>());
        }
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoadScores()
    {
        StartCoroutine(loadScores());
    }

    private IEnumerator loadScores()
    {
        var DBTask = _reference.Child("PLAYERS").OrderByChild("highScore").GetValueAsync();
        yield return new WaitUntil(() => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogError(message:$"Failed to register task with: {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log(DBTask.Result);
            Debug.Log(DBTask.Result.Children);
            foreach (GameObject hgscore in hgscores)
            {
                hgscore.SetActive(false);
            }

            int i = 0;
            foreach (DataSnapshot child in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (i < hgscores.Count)
                {
                    string username = child.Child("name").Value.ToString();
                    string highscore = child.Child("highScore").Value.ToString();
                    hgscores[i].SetActive(true); 
                    hgscoresText[i].text = username + ": "+highscore;
                }
                i++;
            }
        }
    }
}
