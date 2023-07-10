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

    private void HandleMouseButton()
    {
        if (selectedTiles.Count == 0)
        {
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tile draggedTile = TileManager.singleton.GetNearestTile(mousePosition);

        if (draggedTile != null && IsWithinMinimumDistance(draggedTile))
        {
            HandleValidDraggedTile(draggedTile);
        }
    }

    private bool IsWithinMinimumDistance(Tile draggedTile)
    {
        float distanceBetweenTiles = Vector2.Distance(selectedTiles[selectedTiles.Count - 1].transform.position, draggedTile.transform.position);
        return distanceBetweenTiles <= Constants.MINIMUM_DISTANCE_BETWEEN_TILES;
    }

    private void HandleValidDraggedTile(Tile draggedTile)
    {
        if (selectedTiles.Count > 1)
        {
            Tile previousTile = selectedTiles[selectedTiles.Count - 1];

            if (IsAdjacentTile(draggedTile, previousTile))
            {
                previousTile.DeSelect();
                LineBetweenTiles.singleton.RemovePointToLine(previousTile.transform.position);
                selectedTiles.Remove(previousTile);
            }
            else if (draggedTile.TileState == TileState.Monster && draggedTile.MonsterType == selectedTiles[1].MonsterType && !selectedTiles.Contains(draggedTile))
            {
                previousTile.GoTo(draggedTile.transform.position);
                SelectTile(draggedTile);
            }
        }
        else if (!selectedTiles.Contains(draggedTile))
        {
            SelectTile(draggedTile);
        }
    }

    private bool IsAdjacentTile(Tile tile1, Tile tile2)
    {
        return tile1 == selectedTiles[0] || tile1 == selectedTiles[selectedTiles.Count - 2];
    }

    private void HandleMouseButtonUp()
    {
        if (selectedTiles.Count > 1)
        {
            LineBetweenTiles.singleton.RemovePointToLine(selectedTiles[0].transform.position);
            selectedTiles[0].TileState = TileState.Empty;
            selectedTiles[0].DeSelect();
            selectedTiles.Remove(selectedTiles[0]);
            PlayerCharacter.singleton.MoveToPositionsSmoothly(selectedTiles);
        }
    }

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
