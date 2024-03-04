using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Object
{
    private static T mInstance;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = FindObjectOfType<T>();
            return mInstance;
        }
    }
}