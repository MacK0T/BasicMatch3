using UnityEngine;

public abstract class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
{
    private static T m_Instance = null;
    public static T instance
    {
        get
        {
            // Instance requiered for the first time, we look for it
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                // Object not found, we create a temporary one
                if (m_Instance == null)
                {
                    Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");
                    m_Instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    // Problem during the creation, this should not happen
                    if (m_Instance == null)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                    }
                }
                m_Instance.Init();
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
            m_Instance.Init();
        }
    }
    
    public virtual void Init() { }
    
    private void OnDestroy()
    {
        m_Instance = null;
    }
}
