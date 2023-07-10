using UnityEngine;

public class Tile : MonoBehaviour
{
    private TileState _tileState;
    public TileState TileState
    {
        get { return _tileState; }
        set { _tileState = value; }
    }

    private Vector2Int _tileGridPosition;
    public Vector2Int TileGridPosition
    {
        get { return _tileGridPosition; }
        set { _tileGridPosition = value; }
    }

    [SerializeField]
    private Monster _monster;
    public Monster Monster
    {
        get { return _monster; }
        set { _monster = value; }
    }

    public MonsterType MonsterType
    {
        get { return _monster.MonsterType; }
        private set { }
    }

    [SerializeField]
    private Sprite _emptyTile;
    [SerializeField]
    private Sprite _selectedTile;

    private SpriteRenderer _spriteRendererComponent;

    private void Awake()
    {
        _spriteRendererComponent = GetComponent<SpriteRenderer>();
    }

    public void Deactivate()
    {
        Destroy(gameObject);
    }

    public void FallMonster()
    {
        _monster.Fall();
    }

    public void TakeDamage()
    {
        _monster.TakeDamage();
    }

    public void EmptyTile()
    {
        _spriteRendererComponent.sprite = _emptyTile;
    }

    public void SelectTile()
    {
        _spriteRendererComponent.sprite = _selectedTile;
    }
}

public enum TileState
{
    Player,
    Monster,
    Obstacle,
    Empty
}

