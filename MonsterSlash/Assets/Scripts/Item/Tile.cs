using UnityEngine;

public class Tile : MonoBehaviour, IInteractable, ISelectable, IMoveable
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

    private Item _item;
    public Item Item
    {
        get { return _item; }
        set { _item = value; }
    }

    public ItemType ItemType
    {
        get { return _item.ItemType; }
        private set { }
    }

    [Header("Tile visualization reference")]
    [Tooltip("Image that will appear when tile is not selected")]
    [SerializeField]
    private Sprite _deSelectedTile;
    [Tooltip("Image that will appear when tile is selected")]
    [SerializeField]
    private Sprite _selectedTile;

    private SpriteRenderer _spriteRendererComponent;

    [Tooltip("Object showing Tile selected after Tile")]
    [SerializeField]
    private SpriteRenderer _goToDisplayer;

    private void Awake()
    {
        _spriteRendererComponent = GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// Moves the associated monster to the specified target position.
    /// </summary>
    /// <param name="targetPosition">The position to which the monster should move.</param>
    public void Move(Vector2 targetPosition)
    {
        _item.Move(targetPosition);
    }

    /// <summary>
    /// Applies the specified amount of damage to the associated monster.
    /// </summary>
    /// <param name="damage">The amount of damage to be applied.</param>
    public void Interact()
    {
        if (_item)
        {
            _item.Interact();
            _tileState = TileState.Empty;
            _item = null;
        }
    }

    /// <summary>
    /// Deselects the tile by changing its sprite to the deselected tile sprite and hides the 
    /// GoToDisplayer object.
    /// </summary>
    public void DeSelect()
    {
        _spriteRendererComponent.sprite = _deSelectedTile;
        _goToDisplayer.gameObject.SetActive(false);
    }

    /// <summary>
    /// Selects the tile by changing its sprite to the selected tile sprite and hides the 
    /// GoToDisplayer object.
    /// </summary>
    public void Select()
    {
        _spriteRendererComponent.sprite = _selectedTile;
        _goToDisplayer.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the GoToDisplayer object's position and rotation to face the given goToPosition.
    /// </summary>
    /// <param name="goToPosition">The target position to go to.</param>
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
    Item,
    Obstacle,
    Empty
}

