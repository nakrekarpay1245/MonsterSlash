using UnityEngine;

public class Tile : MonoBehaviour, IDamagable, ISelectable, IMoveable
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
    private Sprite _deSelectedTile;
    [SerializeField]
    private Sprite _selectedTile;

    private SpriteRenderer _spriteRendererComponent;
    [SerializeField]
    private SpriteRenderer _goToDisplayer;

    private void Awake()
    {
        _spriteRendererComponent = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 targetPosition)
    {
        _monster.Move(targetPosition);
    }

    public void TakeDamage(int damage)
    {
        _monster.TakeDamage(damage);
    }

    public void DeSelect()
    {
        _spriteRendererComponent.sprite = _deSelectedTile;
        _goToDisplayer.gameObject.SetActive(false);
    }

    public void Select()
    {
        _spriteRendererComponent.sprite = _selectedTile;
        _goToDisplayer.gameObject.SetActive(false);
    }

    public void GoTo(Vector3 goToPosition)
    {
        _goToDisplayer.gameObject.SetActive(true);
        Vector3 rotationVector = goToPosition - transform.position;
        float angle = Mathf.Atan2(rotationVector.y, rotationVector.x) * Mathf.Rad2Deg;
        _goToDisplayer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

public enum TileState
{
    Player,
    Monster,
    Obstacle,
    Empty
}

