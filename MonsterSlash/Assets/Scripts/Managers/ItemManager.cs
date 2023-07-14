using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField]
    private List<Item> itemPrefabList;

    private void Start()
    {
        GenerateItems();
    }

    /// <summary>
    /// Generates monsters on empty tiles within the active tile list.
    /// </summary>
    public void GenerateItems()
    {
        List<Tile> tileList = TileManager.singleton.ActiveTileList;
        for (int i = 0; i < tileList.Count; i++)
        {
            Tile currentTile = tileList[i];
            if (currentTile.TileState != TileState.Empty)
            {
                continue;
            }
            GenerateRandomItem(currentTile);
        }
    }

    /// <summary>
    /// Generates a random monster on the specified parent tile.
    /// </summary>
    /// <param name="parentTile">The parent tile on which the monster will be generated.</param>
    /// <returns>The generated monster.</returns>
    public Item GenerateRandomItem(Tile parentTile)
    {
        Vector2 parentTilePosition = parentTile.transform.position;
        Vector3 generatePosition = new Vector3(parentTilePosition.x, 5);

        Item randomItemPrefab = GetRandomItemPrefab();

        Item generatedItem = InstantiateItem(randomItemPrefab, generatePosition);
        generatedItem.Move(parentTilePosition);
        generatedItem.name = randomItemPrefab.name;

        parentTile.Item = generatedItem;
        parentTile.TileState = TileState.Item;

        return generatedItem;
    }

    /// <summary>
    /// Retrieves a random monster prefab from the list of available monster prefabs.
    /// </summary>
    /// <returns>A random monster prefab.</returns>
    private Item GetRandomItemPrefab()
    {
        int randomIndex = Random.Range(0, itemPrefabList.Count);
        return itemPrefabList[randomIndex];
    }

    /// <summary>
    /// Instantiates a monster prefab at the specified position.
    /// </summary>
    /// <param name="prefab">The monster prefab to instantiate.</param>
    /// <param name="position">The position at which to instantiate the monster.</param>
    /// <returns>The instantiated monster.</returns>
    private Item InstantiateItem(Item prefab, Vector3 position)
    {
        Item generatedItem = Instantiate(prefab, position, Quaternion.identity, transform);
        return generatedItem;
    }
}