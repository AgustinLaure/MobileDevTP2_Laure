using UnityEngine;

public class LevelFactory
{
    private readonly LevelData[] levelsData;

    public LevelFactory(LevelData[] levelsData)
    {
        this.levelsData = levelsData;
    }

    public GameObject CreateLevel(int index)
    {
        GameObject levelObject = Object.Instantiate(levelsData[index].LevelPrefab);

        Coin[] coins = levelObject.GetComponentsInChildren<Coin>(true);

        foreach (Coin coin in coins)
        {
            coin.Init(levelsData[index]);
        }

        DeathArea[] deathAreas = levelObject.GetComponentsInChildren<DeathArea>(true);

        foreach (DeathArea deathArea in deathAreas)
        {
            deathArea.Init(levelsData[index]);
        }

        IRestartable[] entities = levelObject.GetComponentsInChildren<IRestartable>();

        levelObject.GetComponentInChildren<LevelManager>().Init(levelsData[index], entities);

        return levelObject;
    }
}
