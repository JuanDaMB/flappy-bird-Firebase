using System;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLoggedController : MonoBehaviour
{
    [SerializeField] private string LoggedScene, LogginScene;

    private void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChanged;
        CheckUser();
    }

    private void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChanged;
    }

    private void HandleAuthStateChanged(object sender, EventArgs e)
    {
        CheckUser();
    }
    
    private void CheckUser()
    {
        SceneManager.LoadScene(FirebaseAuth.DefaultInstance.CurrentUser != null ? LoggedScene : LogginScene);
    }
}
