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

    /// <summary>
    /// Generates monsters on empty tiles within the active tile list.
    /// </summary>
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

    /// <summary>
    /// Generates a random monster on the specified parent tile.
    /// </summary>
    /// <param name="parentTile">The parent tile on which the monster will be generated.</param>
    /// <returns>The generated monster.</returns>
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

    /// <summary>
    /// Retrieves a random monster prefab from the list of available monster prefabs.
    /// </summary>
    /// <returns>A random monster prefab.</returns>
    private Monster GetRandomMonsterPrefab()
    {
        int randomIndex = Random.Range(0, monsterPrefabList.Count);
        return monsterPrefabList[randomIndex];
    }

    /// <summary>
    /// Instantiates a monster prefab at the specified position.
    /// </summary>
    /// <param name="prefab">The monster prefab to instantiate.</param>
    /// <param name="position">The position at which to instantiate the monster.</param>
    /// <returns>The instantiated monster.</returns>
    private Monster InstantiateMonster(Monster prefab, Vector3 position)
    {
        Monster generatedMonster = Instantiate(prefab, position, Quaternion.identity, transform);
        return generatedMonster;
    }
}