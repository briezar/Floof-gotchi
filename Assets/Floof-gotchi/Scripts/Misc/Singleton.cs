using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T _singleton = new T();

    public static T Instance => _singleton;

}

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : UnityEngine.Component
{
    public void Release()
    {
        _singleton = null;
    }

    private static T _singleton;

    public static T Instance
    {
        get
        {
            if (_singleton == null)
            {
                var allInstances = FindObjectsOfType<T>();
                if (allInstances.Length == 0)
                {
                    var gameObject = new GameObject();
                    _singleton = gameObject.AddComponent<T>();
                    gameObject.name = typeof(T).Name;
                }
                else if (allInstances.Length == 1)
                {
                    _singleton = allInstances[0];
                }
                else
                {
                    Debug.LogError($"{typeof(T).Name} has more than 1 instance!");
                }
            }
            return _singleton;
        }
    }

}