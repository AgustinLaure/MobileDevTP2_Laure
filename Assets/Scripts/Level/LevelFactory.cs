using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelFactory
{
    private readonly LevelData[] levelsData;

    private AsyncOperationHandle<GameObject> currentLevelHandle;
    private GameObject currentLevelInstance;

    public LevelFactory(LevelData[] levelsData)
    {
        this.levelsData = levelsData;
    }

    public IEnumerator CreateLevel(int index)
    {
        UnloadCurrentLevel();

        currentLevelHandle = levelsData[index].LevelPrefab.LoadAssetAsync<GameObject>();
        yield return currentLevelHandle;

        GameObject tempHolder = new GameObject("Temp");
        tempHolder.SetActive(false);

        currentLevelInstance = Object.Instantiate(currentLevelHandle.Result, tempHolder.transform);

        Coin[] coins = currentLevelInstance.GetComponentsInChildren<Coin>(true);

        foreach (Coin coin in coins)
        {
            coin.Init(levelsData[index]);
        }

        DeathArea[] deathAreas = currentLevelInstance.GetComponentsInChildren<DeathArea>(true);

        foreach (DeathArea deathArea in deathAreas)
        {
            deathArea.Init(levelsData[index]);
        }

        IRestartable[] entities = currentLevelInstance.GetComponentsInChildren<IRestartable>(true);

        currentLevelInstance.GetComponentInChildren<LevelManager>(true).Init(levelsData[index], entities);

        currentLevelInstance.transform.SetParent(null);
        Object.Destroy(tempHolder);
    }

    public void UnloadCurrentLevel()
    {
        if (currentLevelInstance != null)
        {
            Object.Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }

        if (currentLevelHandle.IsValid())
        {
            Addressables.Release(currentLevelHandle);
            currentLevelHandle = default;
        }
    }
}
