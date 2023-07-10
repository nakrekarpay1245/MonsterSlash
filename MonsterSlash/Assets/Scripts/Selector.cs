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
        if (Input.GetMouseButtonDown(0))
        {
            ClearSelectedTiles();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile clickedTile = TileManager.singleton.GetNearestTile(mousePosition);

            if (clickedTile != null)
            {
                if (clickedTile.TileState != TileState.Player)
                {
                    return;
                }
                SelectTile(clickedTile);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (selectedTiles.Count > 0)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Tile draggedTile = TileManager.singleton.GetNearestTile(mousePosition);

                if (draggedTile != null)
                {
                    float distanceBetweenTiles =
                        Vector2.Distance(selectedTiles[selectedTiles.Count - 1].transform.position,
                            draggedTile.transform.position);

                    if (distanceBetweenTiles > Constants.MINIMUM_DISTANCE_BETWEEN_TILES)
                    {
                        return;
                    }

                    if (selectedTiles.Count > 1)
                    {
                        Tile previousTile = selectedTiles[selectedTiles.Count - 1];

                        if (draggedTile == selectedTiles[0] ||
                            draggedTile == selectedTiles[selectedTiles.Count - 2])
                        {
                            previousTile.EmptyTile();
                            LineBetweenTiles.singleton.RemovePointToLine(previousTile.transform.position);
                            selectedTiles.Remove(previousTile);
                        }
                        else if (draggedTile.TileState == TileState.Monster)
                        {
                            if (draggedTile.MonsterType == selectedTiles[1].MonsterType)
                            {
                                if (!selectedTiles.Contains(draggedTile))
                                {
                                    SelectTile(draggedTile);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!selectedTiles.Contains(draggedTile))
                        {
                            SelectTile(draggedTile);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedTiles.Count > 1)
            {
                LineBetweenTiles.singleton.RemovePointToLine(selectedTiles[0].transform.position);
                selectedTiles[0].TileState = TileState.Empty;
                selectedTiles[0].EmptyTile();
                selectedTiles.Remove(selectedTiles[0]);
                PlayerCharacter.singleton.MoveToPositionsSmoothly(selectedTiles);
            }
            ClearSelectedTiles();
        }
    }

    private void SelectTile(Tile tile)
    {
        LineBetweenTiles.singleton.AddPointToLine(tile.transform.position);
        selectedTiles.Add(tile);
        tile.SelectTile();
    }

    private void ClearSelectedTiles()
    {
        selectedTiles.Clear();
    }
}
