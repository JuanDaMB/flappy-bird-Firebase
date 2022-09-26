using System;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseController : MonoBehaviour
{
    private static FirebaseController _instance;

    public static FirebaseController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = InitFirebaseManager();
            }

            return _instance;
        }
    }


    private FirebaseAuth _auth;

    public FirebaseAuth Auth
    {
        get
        {
            if (_auth == null)
            {
                _auth = FirebaseAuth.GetAuth(App);
            }

            return _auth;
        }
    }

    private FirebaseApp _app;

    public FirebaseApp App
    {
        get
        {
            if (_app == null)
            {
                _app = GetAppSynchronous();
            }

            return _app;
        }
    }

    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    
    private async void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
            var dependencyResult = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyResult == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                OnFirebaseInitialized.Invoke();
            }
            else
            {
                Debug.LogError("Error initializing Firebase");
            }
        }
        else
        {
            Debug.LogError("There is already an instance of firebase manager");
        }
    }

    private void OnDestroy()
    {
        
    }
    private static FirebaseController InitFirebaseManager()
    {
        throw new NotImplementedException();
    }

    private FirebaseApp GetAppSynchronous()
    {
        throw new NotImplementedException();
    }
}
