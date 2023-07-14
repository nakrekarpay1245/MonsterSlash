using UnityEngine;

public abstract class Item : MonoBehaviour, IMoveable, IInteractable
{
    [SerializeField]
    private ItemType _itemType;
    public ItemType ItemType { get => _itemType; private set { } }

    public abstract void Move(Vector2 targetPosition);
    public abstract void Interact();
}

public enum ItemType
{
    Demon,
    Troll,
    Brick,
    Weapon
}
