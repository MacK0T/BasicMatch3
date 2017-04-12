using UnityEngine;

public class TestSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public bool isDestroy = false;
    //private static object _lock = new object();
    


    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            //lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();

                    }
                        var _anotherTemp = _instance as TestSingleton<T>;
                        //var _temp = _instance.gameObject.GetComponent<TestSingleton<T>>();
                        _instance.gameObject.name = "(singleton) " + typeof(T).ToString();
                        if(_anotherTemp.isDestroy)
                            DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}