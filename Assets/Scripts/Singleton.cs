using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_shuttingDown = false;
    private static object m_lock = new object();
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_shuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }
            lock (m_lock)
            {
                if (m_instance == null)
                {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    if (m_instance == null)
                    {
                       var singletonObject = new GameObject();
                        m_instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                       /* Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singletonObject +
                            "' was created with DontDestroyOnLoad.");*/
                    }
                    
                }
                return m_instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        m_shuttingDown = true;
    }

    private void OnDestroy()
    {
        m_shuttingDown = true;
    }

}

