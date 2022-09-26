using System.Collections;
using Firebase.Auth;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private DataSaver _playerDataSaver;
    public string PlayerName;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerHighscoreText;
    public int highScore = 0;
    public PlayerData Data;
    private IEnumerator Start()
    {
        _playerDataSaver = FindObjectOfType<DataSaver>();
        var playerDataTask = _playerDataSaver.LoadPlayer();
        yield return new WaitUntil(() => playerDataTask.IsCompleted);
        var playerData = playerDataTask.Result;
        if (playerData != null && !string.IsNullOrEmpty(playerData.name))
        {
            Data = playerData;
            PlayerName = playerData.name;
            playerNameText.text = PlayerName;
            highScore = playerData.highScore;
            playerHighscoreText.text = "HG: " + highScore;
        }
        else
        {
            PlayerName = "New user";
            playerNameText.text = PlayerName;
            highScore = 0;
            playerHighscoreText.text = "HG: " + highScore;
        }
    }
}
