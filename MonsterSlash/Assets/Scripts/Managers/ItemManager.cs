using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
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

        int randomIndex = (int)Random.Range(0, 4);

        Item generatedItem = ObjectPoolManager.singleton.GetPooledObject(randomIndex,
            generatePosition, parentTile.transform).GetComponent<Item>();

        generatedItem.Move(parentTilePosition);

        parentTile.Item = generatedItem;
        parentTile.TileState = TileState.Item;

        return generatedItem;
    }
}