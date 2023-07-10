using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : MonoSingleton<PlayerCharacter>
{
    [Header("Soldier Movement")]
    [Tooltip("Soldier moveSpeed")]
    [SerializeField]
    public float _walkSpeedBetweenTiles;

    [Tooltip("The walkpoints that the soldier will advance through in sequence")]
    public List<Tile> walkTileList;

    /// <summary>
    /// The coroutine responsible for movement.
    /// </summary>
    private Coroutine moveCoroutine;

    private Tile playerTile;

    /// <summary>
    /// Initiates smooth movement to a list of target tiles. Checks if there is an ongoing movement 
    /// coroutine and stops it if necessary. Sets the target tile list, current position index, and
    /// starts the movement coroutine.
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
    /// Performs the movement coroutine, smoothly moving the entity between the target tiles. It
    /// iterates through the target tile list, moving the entity towards each target position.
    /// Once the entity reaches a target position, it updates the occupancy status of the current
    /// and next tiles, sets the entity's parent to the current tile, and updates the list of tiles 
    /// in the entity. The coroutine continues until all target positions have been reached. Finally,
    /// it stops the movement coroutine when the movement is complete.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCoroutine()
    {
        float time_1 = Constants.TIME_1;
        playerTile = walkTileList[walkTileList.Count - 1];
        int currentPositionIndex = 0;
        int targetTileCount = walkTileList.Count;

        while (currentPositionIndex < targetTileCount)
        {
            Vector3 targetPosition = walkTileList[currentPositionIndex].transform.position;
            Vector3 currentPosition = transform.position;

            Vector3 newPosition = Vector3.MoveTowards(currentPosition,
                targetPosition, _walkSpeedBetweenTiles * Time.deltaTime);

            transform.position = newPosition;

            if (newPosition == targetPosition)
            {
                walkTileList[currentPositionIndex]?.EmptyTile();
                walkTileList[currentPositionIndex]?.TakeDamage();
                walkTileList[currentPositionIndex].TileState = TileState.Empty;
                walkTileList[currentPositionIndex].Monster = null;
                currentPositionIndex++;

                yield return new WaitForSeconds(time_1);
                yield return new WaitForSeconds(time_1);

                time_1 -= time_1 / 65f;
                LineBetweenTiles.singleton.RemovePointToLine(walkTileList[currentPositionIndex - 1].transform.position);
            }
        }

        playerTile.TileState = TileState.Player;
        StopMoving();

        yield return null;
    }

    /// <summary>
    /// Stops the movement of the entity by clearing the moveCoroutine, resetting the currentPositionIndex, 
    /// and clearing the walkTileList.
    /// </summary>
    private void StopMoving()
    {
        moveCoroutine = null;
        walkTileList.Clear();
        TileManager.singleton.SortGrid();
        LineBetweenTiles.singleton.ClearLine();
    }
}
