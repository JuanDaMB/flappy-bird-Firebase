using System;
using System.Collections;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RegistrationButton : MonoBehaviour
{
    public RegistrationFlow registrationFlow;

    public Button registrationButton;
    private Coroutine registrationCoroutine;

    public UnityEvent<FirebaseUser> OnUserRegistered;
    public UnityEvent<Exception> OnUserRegistrationFailed;

    private void Reset()
    {
        registrationFlow = FindObjectOfType<RegistrationFlow>();
        registrationButton = GetComponent<Button>();
    }

    private void Start()
    {
        registrationFlow.OnStateChanged.AddListener(HandleRegistrationStateChanged);
        registrationButton.onClick.AddListener(HandleRegistrationButtonClicked);

        UpdateInteractable();
    }

    private void UpdateInteractable()
    {
        registrationButton.interactable = 
            registrationFlow.CurrentState == RegistrationFlow.State.OK && registrationCoroutine == null;
    }

    private void HandleRegistrationButtonClicked()
    {
        registrationCoroutine = StartCoroutine(RegisterUser(registrationFlow.Email, registrationFlow.Password, registrationFlow.Username));
        UpdateInteractable();
    }

    private IEnumerator RegisterUser(string registrationFlowEmail, string registrationFlowPassword, string username)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(registrationFlowEmail, registrationFlowPassword);
        yield return new WaitUntil(()=>registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
            OnUserRegistrationFailed.Invoke(registerTask.Exception);
        }
        else
        {
            Debug.Log($"Succesfully registered user {registerTask.Result.Email}");
            PlayerData playerData = new PlayerData();
            playerData.id = registerTask.Result.UserId;
            playerData.name = username;
            playerData.highScore = 0;
            FindObjectOfType<DataSaver>().SavePlayerData(playerData);
            OnUserRegistered.Invoke(registerTask.Result);
        }

        registrationCoroutine = null;
        UpdateInteractable();
    }

    private void HandleRegistrationStateChanged(RegistrationFlow.State arg0)
    {
        UpdateInteractable();
    }

    private void OnDestroy()
    {
        registrationFlow.OnStateChanged.RemoveListener(HandleRegistrationStateChanged);
        registrationButton.onClick.RemoveListener(HandleRegistrationButtonClicked);
    }
}
