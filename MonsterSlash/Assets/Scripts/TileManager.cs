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
                //generatedTile.SetIndex(index);
                index++;

                _tileGrid[x, y] = generatedTile;
                _activeTileList.Add(generatedTile);

                if (playerCharacterPosition.x == x && playerCharacterPosition.y == y)
                {
                    //PlayerCharacter.singleton.transform.position = generatedTile.transform.position;
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
        Tile nearestTile = _activeTileList.OrderBy(tile =>
            Vector3.Distance(tile.transform.position, position)).FirstOrDefault();

        return nearestTile;
    }
}
