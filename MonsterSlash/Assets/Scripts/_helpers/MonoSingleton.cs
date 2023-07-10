using UnityEngine;

/// <summary>
/// MonoSingleton is a generic base class for creating singleton instances of MonoBehaviours.
/// It ensures that only one instance of the derived class can exist in the scene at a time.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T singleton;

    protected virtual void OnEnable()
    {
        // If no instance of the singleton exists, set this instance as the singleton.
        if (!singleton)
        {
            singleton = (T)this;
        }
        // If an instance already exists and it's not this instance, destroy this gameObject.
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        // If the instance being destroyed is the current singleton, set it to null.
        if (singleton == this)
        {
            singleton = null;
        }
    }
}

