using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour, IMoveable, IInteractable
{
    [SerializeField]
    private ItemType _itemType;
    public ItemType ItemType { get => _itemType; private set { } }
    public abstract void Interact();

    public virtual void Move(Vector2 targetPosition)
    {
        StopCoroutine(MoveRoutine(targetPosition));
        StartCoroutine(MoveRoutine(targetPosition));
    }

    /// <summary>
    /// This coroutine smoothly moves the object from its current position to the target position 
    /// over a specified fall time.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public IEnumerator MoveRoutine(Vector2 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float distance = Mathf.Abs(startPosition.y - targetPosition.y);

        float elapsedTime = 0f;
        float fallTime = GameSettings.singleton.TIME_1 * distance;

        while (elapsedTime < fallTime)
        {
            float t = elapsedTime / fallTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

public enum ItemType
{
    Demon,
    Troll,
    Brick,
    WeaponTrigger
}
