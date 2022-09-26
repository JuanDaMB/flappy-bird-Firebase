using System;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserLoginWorkflow : MonoBehaviour
{
    public GameObject alert;
    public TextMeshProUGUI textAlert;
    public string GameScene;

    public void StartAlert(Exception e)
    {
        alert.SetActive(true);
        textAlert.text = e.InnerException.InnerException.Message;

    }

    public void LoadScene(FirebaseUser user)
    {
        SceneManager.LoadScene(GameScene);
    }
}
