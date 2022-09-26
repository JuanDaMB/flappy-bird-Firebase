using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RegistrationFlow : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField verifyField;
    [SerializeField] private TMP_InputField usernameField;

    public State CurrentState
    {
        get;
        private set;
    }

    public UnityEvent<State> OnStateChanged;

    public string Email => emailField.text;
    public string Password => passwordField.text;
    public string Username => usernameField.text;

    private void Start()
    {
        emailField.onValueChanged.AddListener(HandleValueChanged);
        passwordField.onValueChanged.AddListener(HandleValueChanged);
        verifyField.onValueChanged.AddListener(HandleValueChanged);
        usernameField.onValueChanged.AddListener(HandleValueChanged);
        ComputeState();
    }

    private void OnDestroy()
    {
        emailField.onValueChanged.RemoveListener(HandleValueChanged);
        passwordField.onValueChanged.RemoveListener(HandleValueChanged);
        verifyField.onValueChanged.RemoveListener(HandleValueChanged);
        usernameField.onValueChanged.RemoveListener(HandleValueChanged);
    }

    private void ComputeState()
    {
        if (string.IsNullOrEmpty(emailField.text))
        {
            SetState(State.EnterEmail);
        }
        else if (string.IsNullOrEmpty(usernameField.text))
        {
            SetState(State.Username);
        }
        else if (string.IsNullOrEmpty(passwordField.text))
        {
            SetState(State.EnterPassword);
        }
        else if (passwordField.text != verifyField.text)
        {
            SetState(State.PasswordsDontMatch);
        }
        else
        {
            SetState(State.OK);
        }
    }

    private void SetState(State state)
    {
        CurrentState = state;
        OnStateChanged.Invoke(state);
    }
    
    public enum State
    {
        EnterEmail,
        EnterPassword,
        PasswordsDontMatch,
        Username,
        OK
    }

    private void HandleValueChanged(string _)
    {
        ComputeState();
    }
}
