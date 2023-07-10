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

        int randomIndex = Random.Range(0, monsterPrefabList.Count);

        Monster randomMonster = monsterPrefabList[randomIndex];

        Monster generatedMonster = Instantiate(randomMonster, generatePosition,
            Quaternion.identity, transform);

        generatedMonster.name = randomMonster.name;

        generatedMonster.Move(parentTilePosition);

        parentTile.Monster = generatedMonster;
        parentTile.TileState = TileState.Monster;

        return generatedMonster;
    }
}