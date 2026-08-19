using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    GameObject singletonObject = new();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";

                    LogService.Log("Created new singleton instance of " + typeof(T).ToString());
                }
            }
            return _instance;
        }
    }

    public static bool HasInstance => _instance != null;

    public bool TryGetInstance(out T instance)
    {
        instance = Instance;
        return instance != null;
    }

    protected virtual void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this as T;
        }
    }
}
