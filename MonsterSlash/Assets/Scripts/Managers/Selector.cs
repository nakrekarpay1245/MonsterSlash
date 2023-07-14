using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public List<Tile> selectedTiles;

    private void Awake()
    {
        selectedTiles = new List<Tile>();
    }

    private void Update()
    {
        HandleMouseInput();
    }

    /// <summary>
    /// Handles the mouse input for tile interaction.
    /// </summary>
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseButtonDown();
        }
        else if (Input.GetMouseButton(0))
        {
            HandleMouseButton();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseButtonUp();
        }
    }

    /// <summary>
    /// Handles the mouse button down input for tile selection.
    /// </summary>
    private void HandleMouseButtonDown()
    {
        ClearSelectedTiles();
        ClearLineBetweenTiles();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tile clickedTile = TileManager.singleton.GetNearestTile(mousePosition);

        if (clickedTile != null && clickedTile.TileState == TileState.Player)
        {
            SelectTile(clickedTile);
        }
    }

    /// <summary>
    /// Handles the mouse button input during tile selection.
    /// </summary>
    private void HandleMouseButton()
    {
        if (selectedTiles.Count > 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile draggedTile = TileManager.singleton.GetNearestTile(mousePosition);

            if (draggedTile != null && IsWithinMinimumDistance(draggedTile))
            {
                HandleValidDraggedTile(draggedTile);
            }
        }
    }

    /// <summary>
    /// Checks if the dragged tile is within the minimum distance from the last selected tile.
    /// </summary>
    /// <param name="draggedTile">The tile being dragged.</param>
    /// <returns>True if the dragged tile is within the minimum distance, false otherwise.</returns>
    private bool IsWithinMinimumDistance(Tile draggedTile)
    {
        Tile lastTile = selectedTiles[selectedTiles.Count - 1];
        Vector2 lastTilePosition = lastTile.transform.position;

        Vector2 draggedTilePosition = draggedTile.transform.position;

        float distanceBetweenTiles = Vector2.Distance(lastTilePosition, draggedTilePosition);

        return distanceBetweenTiles <= GameSettings.singleton.MINIMUM_DISTANCE_BETWEEN_TILES;
    }

    /// <summary>
    /// Handles the logic when a valid tile is being dragged during the selection process.
    /// </summary>
    /// <param name="draggedTile">The tile being dragged.</param>
    private void HandleValidDraggedTile(Tile draggedTile)
    {
        if (selectedTiles.Count > 1)
        {
            Tile lastTile = selectedTiles[selectedTiles.Count - 1];

            if (IsAdjacentTile(draggedTile))
            {
                lastTile.DeSelect();
                LineBetweenTiles.singleton.RemovePointFromLine(lastTile.transform.position);
                selectedTiles.Remove(lastTile);
            }
            else if (draggedTile.TileState == TileState.Item &&
                        draggedTile.ItemType == lastTile.ItemType &&
                            !selectedTiles.Contains(draggedTile))
            {
                lastTile.GoTo(draggedTile.transform.position);
                SelectTile(draggedTile);
            }
        }
        else if (!selectedTiles.Contains(draggedTile))
        {
            SelectTile(draggedTile);
        }
    }

    /// <summary>
    /// Checks if the given tiles are adjacent to the first or second-to-last tile in the selected 
    /// tiles list
    /// </summary>
    /// <param name="tile1">The first tile to check.</param>
    /// <param name="tile2">The second tile to check.</param>
    /// <returns>True if the tiles are adjacent; otherwise, false.</returns>
    private bool IsAdjacentTile(Tile tile1)
    {
        return tile1 == selectedTiles[0] || tile1 == selectedTiles[selectedTiles.Count - 2];
    }

    /// <summary>
    /// Handles the mouse button up event. If multiple tiles are selected, it removes the first tile from the line between tiles, updates its state and visual representation, removes it from the selected tiles list, and initiates the smooth movement of the player character to the selected tiles.
    /// </summary>
    private void HandleMouseButtonUp()
    {
        if (selectedTiles.Count > 1)
        {
            LineBetweenTiles.singleton.RemovePointFromLine(selectedTiles[0].transform.position);

            selectedTiles[0].TileState = TileState.Empty;
            selectedTiles[0].DeSelect();
            selectedTiles.Remove(selectedTiles[0]);

            PlayerCharacter.singleton.MoveToPositionsSmoothly(selectedTiles);
        }
    }

    /// <summary>
    /// Selects a tile and performs the necessary actions, such as adding it to the line between tiles, 
    /// adding it to the selected tiles list, and updating its visual state.
    /// </summary>
    /// <param name="tile">The tile to select.</param>
    private void SelectTile(Tile tile)
    {
        LineBetweenTiles.singleton.AddPointToLine(tile.transform.position);
        selectedTiles.Add(tile);
        tile.Select();
    }

    private void ClearSelectedTiles()
    {
        selectedTiles.Clear();
    }

    private void ClearLineBetweenTiles()
    {
        LineBetweenTiles.singleton.ClearLine();
    }
}
