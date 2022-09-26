using Firebase;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseDatabaseController : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log($"Failed to initialized Firebase with{task.Exception}");
                return;
            }
            OnFirebaseInitialized.Invoke();
        });
    }
}
