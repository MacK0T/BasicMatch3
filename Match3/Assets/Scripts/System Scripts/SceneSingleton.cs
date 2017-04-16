using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
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

                return _instance;
            }
        }
    }

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<T>();
            _instance.gameObject.name = "(singleton) " + typeof(T).ToString();            
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