using System;
using System.Collections;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginFlow : MonoBehaviour
{
    [SerializeField] private Button LoginButton, recoverButton;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    private Coroutine loginCoroutine;

    public State CurrentState
    {
        get;
        private set;
    }

    public UnityEvent<FirebaseUser> OnLoginSucceded;
    public UnityEvent<Exception> OnLoginFailed;

    public string Email => emailField.text;
    public string Password => passwordField.text;

    private void Start()
    {
        emailField.onValueChanged.AddListener(HandleValueChanged);
        passwordField.onValueChanged.AddListener(HandleValueChanged);
        LoginButton.onClick.AddListener(HandleRegistrationButtonClicked);
        recoverButton.onClick.AddListener(RecoverPassword);

        ComputeState();
    }

    private void OnDestroy()
    {
        emailField.onValueChanged.RemoveListener(HandleValueChanged);
        passwordField.onValueChanged.RemoveListener(HandleValueChanged);
        LoginButton.onClick.RemoveListener(HandleRegistrationButtonClicked);
        recoverButton.onClick.RemoveListener(RecoverPassword);
    }

    private void ComputeState()
    {
        if (string.IsNullOrEmpty(emailField.text))
        {
            SetState(State.EnterEmail);
        }
        else if (string.IsNullOrEmpty(passwordField.text))
        {
            SetState(State.EnterPassword);
        }
        else
        {
            SetState(State.OK);
        }
        UpdateInteractable();
    }

    private void SetState(State state)
    {
        CurrentState = state;
    }
    
    public enum State
    {
        EnterEmail,
        EnterPassword,
        OK
    }

    private void HandleValueChanged(string _)
    {
        ComputeState();
    }
    

    private void UpdateInteractable()
    {
        recoverButton.interactable = CurrentState != State.EnterEmail;
        LoginButton.interactable = 
            CurrentState == State.OK && loginCoroutine == null;
    }

    private void HandleRegistrationButtonClicked()
    {
        loginCoroutine = StartCoroutine(RegisterUser(Email, Password));
        UpdateInteractable();
    }

    private IEnumerator RegisterUser(string loginFlowEmail, string loginFlowPassword)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(loginFlowEmail, loginFlowPassword);
        yield return new WaitUntil(()=>loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Login Failed with {loginTask.Exception}");
            OnLoginFailed.Invoke(loginTask.Exception);
        }
        else
        {
            Debug.Log($"Succesfully logged in user {loginTask.Result.Email}");
            OnLoginSucceded.Invoke(loginTask.Result);
        }

        loginCoroutine = null;
        UpdateInteractable();
    }

    public void RecoverPassword()
    {
        var auth = FirebaseAuth.DefaultInstance;
        if (Email != null) {
            auth.SendPasswordResetEmailAsync(Email).ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("Password reset email sent successfully.");
            });
        }
    }
}
