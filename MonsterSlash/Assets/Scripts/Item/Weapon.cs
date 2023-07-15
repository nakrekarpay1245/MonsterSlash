using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item, IActivetable
{
    /// <summary>
    /// This function reduces the object's health by the specified damage amount.
    /// </summary>
    /// <param name="damage"></param>
    public override void Interact()
    {
        Activate();
    }

    public void Activate()
    {
        StartCoroutine(ActivateRoutine());
    }

    /// <summary>
    /// This coroutine handles the process of killing the object, including triggering the death
    /// animation, playing kill effects, and deactivating the object after a specified delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateRoutine()
    {
        PlayActivateEffects();

        yield return new WaitForSeconds(GameSettings.singleton.TIME_1);

        AttackToAttackPoint();

        gameObject.SetActive(false);
    }

    private void AttackToAttackPoint()
    {
        Tile currentTile = TileManager.singleton.GetNearestTile(transform.position);
        Tile attackTile = TileManager.singleton.GetNearestTile(transform.position, currentTile);
        attackTile.Interact();
    }

    /// <summary>
    /// This function plays the kill effects, such as a splash particle and a sound effect.
    /// </summary>
    private void PlayActivateEffects()
    {
        ParticleManager.singleton.PlayParticleAtPoint((gameObject.name + "ActivateParticle"),
            transform.position);

        AudioManager.singleton.PlaySound("Activate");
    }

    public override void Move(Vector2 targetPosition)
    {
        base.Move(targetPosition);
    }
}
