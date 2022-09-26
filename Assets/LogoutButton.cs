using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoutButton : MonoBehaviour
{
    public string LoggedOutScene;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    public void Click()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene(LoggedOutScene);
    }
}
