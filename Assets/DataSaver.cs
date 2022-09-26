using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    private const string PLAYER_KEY = "PLAYERS/";
    private FirebaseDatabase _database;

    private void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;
    }

    public void SavePlayerData(PlayerData player)
    {
        PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(player));
        _database.GetReference(PLAYER_KEY + player.id).SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    public async Task<PlayerData?> LoadPlayer()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            return null;
        }
        var user = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var dataSnapshot = await _database.GetReference(PLAYER_KEY+user).GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return null;
        }
        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<bool> SaveExists()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            return false;
        }
        var user = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var dataSnapshot = await _database.GetReference(PLAYER_KEY+user).GetValueAsync();
        return dataSnapshot.Exists;
    }
}
[Serializable]
public class PlayerData
{
    public string name;
    public string id;
    public int highScore;
}
