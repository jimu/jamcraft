using UnityEngine;

/**
 * Abstract base class implementing the Singleton pattern
 * 
 * Example implementation:
 *   public class SpawnManager : MonoSingleton<SpawnManager>
 *   
 * Optionally, implement Init() for setup code:
 *   public override void Init()
 *   
 * Example usage:
 *   SpawnManager.Instance.MyFunction()
 */


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            Debug.Assert(instance != null, $"Error: {typeof(T)} Instance is null");
            return instance;
        }
    }
    public void Awake()
    {
        instance = this as T;

        if (instance != null && instance != this)
            Destroy(this);
        else
            Init();
    }

    protected virtual void Init()
    {
        // optional
    }
}

