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

    /// <summary>
    /// Generates the tiles of the game board based on the grid width and height.
    /// </summary>
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

    /// <summary>
    /// Retrieves the nearest tile to the specified position based on minimum selection distance.
    /// </summary>
    /// <param name="position">The position to compare with.</param>
    /// <returns>The nearest tile to the position within the minimum selection distance, or null 
    /// if no tile is found.</returns>    
    public Tile GetNearestTile(Vector3 position)
    {
        Tile nearestTile = _activeTileList
                .OrderBy(tile => Vector3.Distance(tile.transform.position, position))
                    .FirstOrDefault(tile => Vector2.Distance(tile.transform.position, position) <
                        GameSettings.singleton.MINIMUM_SELECTION_DISTANCE);

        return nearestTile;
    }

    /// <summary>
    /// Retrieves the nearest tile to the specified position based on minimum selection distance.
    /// </summary>
    /// <param name="position">The position to compare with.</param>
    /// <returns>The nearest tile to the position within the minimum selection distance, or null 
    /// if no tile is found.</returns>    
    public Tile GetNearestTile(Vector3 position, Tile exceptTile)
    {
        List<Tile> tileList = new List<Tile>(_activeTileList);

        tileList.Remove(exceptTile);

        Tile nearestTile = tileList
                .OrderBy(tile => Vector3.Distance(tile.transform.position, position))
                    .FirstOrDefault(tile => Vector2.Distance(tile.transform.position, position) <
                        GameSettings.singleton.MINIMUM_DISTANCE_BETWEEN_TILES);

        return nearestTile;
    }

    /// <summary>
    /// Returns a list of the nearest tiles to the given position, excluding the currentTile.
    /// </summary>
    /// <param name="position">The position from which to find the nearest tiles.</param>
    /// <param name="count">The maximum number of nearest tiles to return.</param>
    /// <param name="exceptTile">The tile to exclude from the nearest tiles.</param>
    /// <returns>A list of the nearest tiles to the given position.</returns>
    public List<Tile> GetNearestTiles(Vector2 position, int count, Tile exceptTile)
    {
        List<Tile> tileList = new List<Tile>(_activeTileList);

        tileList.Remove(exceptTile);

        tileList.Sort((a, b) => Vector2.Distance(a.transform.position,
            position).CompareTo(Vector2.Distance(b.transform.position, position)));

        return tileList.GetRange(0, Mathf.Min(count, tileList.Count));
    }


    /// <summary>
    /// Sorts the grid by moving monsters down to fill empty spaces.
    /// </summary>
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

                    if (upTile.TileState == TileState.Item)
                    {
                        if (downTile.TileState == TileState.Empty)
                        {
                            upTile.Move(downTile.transform.position);
                            downTile.Item = upTile.Item;
                            downTile.TileState = TileState.Item;
                            upTile.Item = null;
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
                                    downTile.Item = upTile.Item;
                                    downTile.TileState = TileState.Item;
                                    upTile.Item = null;
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

    /// <summary>
    /// Fills the empty tiles in the grid with randomly generated monsters.
    /// </summary>
    private void FillTheGrid()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Tile tile = _tileGrid[x, y];

                if (tile.TileState == TileState.Empty)
                {
                    ItemManager.singleton.GenerateRandomItem(tile);
                    tile.TileState = TileState.Item;
                }
            }
        }
    }
}
