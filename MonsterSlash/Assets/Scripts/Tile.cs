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


    [SerializeField]
    private MonsterType _monsterType;
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

    //public void SetMonster(Monster newMonster)
    //{
    //    if (newMonster != null)
    //    {
    //        EmptyTile();
    //        monster = newMonster;
    //    }
    //}

    //public void FallMonster()
    //{
    //    monster.Fall();
    //}

    //public Monster GetMonster()
    //{
    //    return monster;
    //}

    //public MonsterType GetMonsterType()
    //{
    //    return monster.GetMonsterType();
    //}

    //public void TakeDamage()
    //{
    //    monster.TakeDamage();
    //}

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

