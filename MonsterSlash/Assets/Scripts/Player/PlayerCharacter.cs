using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : MonoSingleton<PlayerCharacter>, IDamage
{
    [Header("Soldier Movement")]
    [Tooltip("Soldier moveSpeed")]
    [SerializeField]
    public float _walkSpeedBetweenTiles;

    [Tooltip("The walkpoints that the soldier will advance through in sequence")]
    public List<Tile> walkTileList;

    [Header("Soldier Attack")]
    [Tooltip("Soldier attackPoint")]
    [SerializeField]
    public int _damagePoint;

    /// <summary>
    /// The coroutine responsible for movement.
    /// </summary>
    private Coroutine moveCoroutine;

    private Tile playerTile;

    /// <summary>
    /// This function initiates the smooth movement of the character to the specified targetTiles.
    /// If there is an ongoing moveCoroutine, it stops it before starting a new one.
    /// </summary>
    /// <param name="targetTiles"></param>
    public void MoveToPositionsSmoothly(List<Tile> targetTiles)
    {
        if (moveCoroutine != null)
        {
            StopMoving();
        }

        walkTileList = targetTiles.ToList<Tile>();
        moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    /// <summary>
    /// This coroutine moves the character along the defined walkTileList, updating its position
    /// and performing actions on each tile it reaches. It also handles tile selection, damage,
    /// clearing the tile, and updating the time_1 value.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCoroutine()
    {
        float time_1 = GameSettings.singleton.TIME_1;
        playerTile = walkTileList[walkTileList.Count - 1];
        int currentPositionIndex = 0;
        int targetTileCount = walkTileList.Count;

        while (currentPositionIndex < targetTileCount)
        {
            Vector3 targetPosition = walkTileList[currentPositionIndex].transform.position;
            Vector3 currentPosition = transform.position;

            float step = _walkSpeedBetweenTiles;
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);

            if (transform.position == targetPosition)
            {
                Tile currentTile = walkTileList[currentPositionIndex];
                currentTile.DeSelect();

                Damage(currentTile);

                currentTile.TileState = TileState.Empty;
                currentTile.Monster = null;

                currentPositionIndex++;
                time_1 -= time_1 / 65f;
                yield return new WaitForSeconds(time_1);

                Tile previousTile = walkTileList[currentPositionIndex - 1];
                Vector2 previousTilePosition = previousTile.transform.position;
                LineBetweenTiles.singleton.RemovePointFromLine(previousTilePosition);
            }

            yield return null;
        }

        playerTile.TileState = TileState.Player;
        StopMoving();
    }

    /// <summary>
    /// This function applies damage to an object that implements the IDamagable interface.
    /// It calls the TakeDamage function of the damagable object with the specified damage points.
    /// </summary>
    /// <param name="damagable"></param>
    public void Damage(IDamagable damagable)
    {
        damagable.TakeDamage(_damagePoint);
    }

    /// <summary>
    /// Stops the movement of the player by clearing the moveCoroutine and clearing the walkTileList
    /// </summary>
    private void StopMoving()
    {
        moveCoroutine = null;
        walkTileList.Clear();
        TileManager.singleton.SortGrid();
        LineBetweenTiles.singleton.ClearLine();
    }
}
