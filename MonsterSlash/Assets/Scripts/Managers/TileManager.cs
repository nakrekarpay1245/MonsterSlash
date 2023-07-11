using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoSingleton<TileManager>
{
    [Header("Tile Manager Parameters")]
    [Header("References")]
    [Tooltip("The tile object that will create the grid")]
    [SerializeField]
    private Tile _tilePrefab;

    [Header("Grid Parameters")]
    [Header("Grid Scale")]
    [Tooltip("Grid horizontal scale")]
    [SerializeField]
    private int _gridWidth;

    [Tooltip("Grid vertical scale")]
    [SerializeField]
    private int _gridHeight;

    [SerializeField]
    private Vector2Int playerCharacterPosition;

    private Tile[,] _tileGrid;
    public Tile[,] TileGrid
    {
        get { return _tileGrid; }
        private set { }
    }

    private List<Tile> _activeTileList;
    public List<Tile> ActiveTileList
    {
        get { return _activeTileList; }
        private set { }
    }

    private void Awake()
    {
        _tileGrid = new Tile[_gridWidth, _gridHeight];
        _activeTileList = new List<Tile>();
        GenerateTiles();
    }

    private void GenerateTiles()
    {
        int index = 0;
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Vector2 gridOffset = new Vector2(_gridWidth / 2f, _gridHeight / 2f) * -1f +
                    new Vector2(0.5f, 0.5f);

                Vector2 tilePosition = (new Vector2(x, y) + gridOffset);

                Tile generatedTile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity,
                    transform);

                Vector2Int tileGridPosition = new Vector2Int(x, y);

                generatedTile.TileGridPosition = tileGridPosition;

                generatedTile.name = "Tile " + index;
                index++;

                _tileGrid[x, y] = generatedTile;
                _activeTileList.Add(generatedTile);

                if (playerCharacterPosition.x == x && playerCharacterPosition.y == y)
                {
                    PlayerCharacter.singleton.transform.position = generatedTile.transform.position;
                    generatedTile.TileState = TileState.Player;
                }
                else
                {
                    generatedTile.TileState = TileState.Empty;
                }
            }
        }
    }

    public Tile GetNearestTile(Vector3 position)
    {
        Tile nearestTile = _activeTileList
                .OrderBy(tile => Vector3.Distance(tile.transform.position, position))
                    .FirstOrDefault(tile => Vector2.Distance(tile.transform.position, position) <
                        GameSettings.singleton.MINIMUM_SELECTION_DISTANCE);

        return nearestTile;
    }

    public void SortGrid()
    {
        for (int a = 0; a < _gridHeight - 1; a++)
        {
            for (int i = _gridWidth - 1; i >= 0; i--)
            {
                for (int j = _gridHeight - 1; j > 0; j--)
                {
                    Tile upTile = _tileGrid[i, j];
                    Tile downTile = _tileGrid[i, j - 1];

                    if (upTile.TileState == TileState.Monster)
                    {
                        if (downTile.TileState == TileState.Empty)
                        {
                            upTile.Move(downTile.transform.position);
                            downTile.Monster = upTile.Monster;
                            downTile.TileState = TileState.Monster;
                            upTile.Monster = null;
                            upTile.TileState = TileState.Empty;
                        }
                        else if (downTile.TileState == TileState.Player ||
                            downTile.TileState == TileState.Obstacle)
                        {
                            if (j - 2 >= 0)
                            {
                                downTile = _tileGrid[i, j - 2];

                                if (downTile.TileState == TileState.Empty)
                                {
                                    upTile.Move(downTile.transform.position);
                                    downTile.Monster = upTile.Monster;
                                    downTile.TileState = TileState.Monster;
                                    upTile.Monster = null;
                                    upTile.TileState = TileState.Empty;
                                }
                            }
                        }
                    }
                }
            }
        }

        FillTheGrid();
    }

    private void FillTheGrid()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Tile tile = _tileGrid[x, y];

                if (tile.TileState == TileState.Empty)
                {
                    MonsterManager.singleton.GenerateRandomMonster(tile);
                    tile.TileState = TileState.Monster;
                }
            }
        }
    }
}
