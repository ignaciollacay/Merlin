using UnityEngine;

/// <summary>
/// Inherit to get a single, global-accesible instance of a class, available at all times.
/// It will create a new instance if a single instance does not exist in the scene.
/// </summary>
/// <typeparam name="T">Class name which inherits from singleton</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        Instance = GetSingleInstance();

        //DontDestroyOnLoad(Instance.gameObject);
    }


    private static T GetSingleInstance()
    {
        T[] instances = FindObjectsOfType<T>();

        if (instances.Length == 1)
        {
            return GetExistingInstance(instances);
        }
        else if (instances.Length == 0)
        {
            return CreateNewInstance();
        }
        else // instances.Length > 1)
        {
            DestroyAllInstances(instances);
            return CreateNewInstance();
        }
    }

    private static T GetExistingInstance(T[] instances)
    {
        //Debug.Log("A Singleton Instance was found in the scene");
        return instances[0];
    }

    private static T CreateNewInstance()
    {
        Debug.LogWarning("Creating a new Singleton Instance");
        var singletonObj = new GameObject();
        singletonObj.name = typeof(T).ToString();
        T instance = singletonObj.AddComponent<T>();
        return instance;
    }

    private static void DestroyAllInstances(T[] instances)
    {
        Debug.LogError("Multiple instances were found and will be destroyed.");
        foreach (var instance in instances)
        {
            Destroy(instance);
        }
    }
}
