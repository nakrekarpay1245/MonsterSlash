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
        Vector2 parentTilePosition = parentTile.transform.position;
        Vector3 generatePosition = new Vector3(parentTilePosition.x, 5);

        Monster randomMonster = GetRandomMonsterPrefab();

        Monster generatedMonster = InstantiateMonster(randomMonster, generatePosition);
        generatedMonster.Move(parentTilePosition);
        generatedMonster.name = randomMonster.name;

        parentTile.Monster = generatedMonster;
        parentTile.TileState = TileState.Monster;

        return generatedMonster;
    }

    private Monster GetRandomMonsterPrefab()
    {
        int randomIndex = Random.Range(0, monsterPrefabList.Count);
        return monsterPrefabList[randomIndex];
    }

    private Monster InstantiateMonster(Monster prefab, Vector3 position)
    {
        Monster generatedMonster = Instantiate(prefab, position, Quaternion.identity, transform);
        return generatedMonster;
    }
}