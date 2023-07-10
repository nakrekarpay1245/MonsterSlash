using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoSingleton<MonsterManager>
{
    [SerializeField]
    private List<Monster> monsterPrefabList;

    private void Start()
    {
        GenerateMonsters();
    }

    public void GenerateMonsters()
    {
        List<Tile> tileList = TileManager.singleton.ActiveTileList;
        for (int i = 0; i < tileList.Count; i++)
        {
            Tile currentTile = tileList[i];
            if (currentTile.TileState != TileState.Empty)
            {
                continue;
            }
            GenerateRandomMonster(currentTile);
        }
    }

    public Monster GenerateRandomMonster(Tile parentTile)
    {
        Transform parentTileTransform = parentTile.transform;

        Vector3 generatePosition = new Vector3(parentTileTransform.position.x,
            parentTileTransform.position.y);

        int randomIndex = Random.Range(0, monsterPrefabList.Count);

        Monster randomMonster = monsterPrefabList[randomIndex];

        Monster generatedMonster = Instantiate(randomMonster, generatePosition,
            Quaternion.identity, transform);

        parentTile.Monster = generatedMonster;
        parentTile.TileState = TileState.Monster;

        return generatedMonster;
    }
}