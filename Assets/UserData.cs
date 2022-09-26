using System.Collections;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserData : MonoBehaviour
{
    [SerializeField] private string LoggedScene, LogginScene;
    [SerializeField] private DataSaver _playerData;
    private Coroutine _coroutine;
    
    public void Trigger()
    {
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(LoadSceneCoroutine());
        }
    }

    private IEnumerator LoadSceneCoroutine()
    {
        var saveExistsTask = _playerData.SaveExists();
        yield return new WaitUntil(() => saveExistsTask.IsCompleted);
        SceneManager.LoadScene(FirebaseAuth.DefaultInstance.CurrentUser != null && saveExistsTask.Result ? LoggedScene : LogginScene);
        _coroutine = null;
    }
}
