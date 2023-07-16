using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IMoveable, IActivetable
{
    public void Activate()
    {
        transform.parent = null;
        gameObject.SetActive(true);
    }

    public void Move(Vector2 targetPosition)
    {
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
        float elapsedTime = 0f;
        float moveTime = GameSettings.singleton.TIME_2;

        while (elapsedTime < moveTime)
        {
            float t = elapsedTime / moveTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
