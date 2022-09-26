using TMPro;
using UnityEngine;

public class PasswordShow : MonoBehaviour
{
    private TMP_InputField _tmpInputField;
    void Start()
    {
        _tmpInputField = GetComponent<TMP_InputField>();
    }

    public void ChangeState(bool visible)
    {
        if (visible)
        {
            _tmpInputField.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            _tmpInputField.contentType = TMP_InputField.ContentType.Password;
        }
        _tmpInputField.DeactivateInputField();
        _tmpInputField.ActivateInputField();
    }
}
