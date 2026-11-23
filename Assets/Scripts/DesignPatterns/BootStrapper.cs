using UnityEngine;
using UnityEngine.AddressableAssets;

public static class BootStrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;

        serviceLocator.AddService(new CompositePool());
        serviceLocator.AddService(new EventBus());
        serviceLocator.AddService(new InputSystemController());

        GameObject gmObj = Addressables.InstantiateAsync("GameManager").WaitForCompletion();
        Object.DontDestroyOnLoad(gmObj);

        serviceLocator.AddService<GameManager>(gmObj.GetComponent<GameManager>());

        GameObject loggerTestPrefab = Resources.Load<GameObject>("LoggerTest");
        serviceLocator.AddServiceFromPrefab<LoggerTest>(loggerTestPrefab, true);
    }

    public static T CreateMonoSingletonGameObject<T>() where T : MonoBehaviour
    {
        T existingInstance = UnityEngine.Object.FindFirstObjectByType<T>();
        if (existingInstance != null)
        {
            Debug.Log($"{typeof(T)} already exists in the scene. Skipping bootstrapper creation.");

            return existingInstance;
        }

        Debug.Log("Add MonoSingleton");

        GameObject gameObjectHandle = new GameObject();
        gameObjectHandle.name = typeof(T).ToString();
        UnityEngine.Object.DontDestroyOnLoad(gameObjectHandle);

        T component = gameObjectHandle.AddComponent<T>();

        return component;
    }

    public static void OnReset()
    {
        
    }
}
