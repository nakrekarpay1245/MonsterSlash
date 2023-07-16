using System.Collections;
using UnityEngine;

public class WeaponTrigger : Item, IActivetable
{
    [Header("Weapon Parameters")]
    [Tooltip("Weapon to be thrown at the time of attack")]
    [SerializeField]
    private Weapon _weapon;

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

        yield return new WaitForSeconds(GameSettings.singleton.TIME_2);

        AttackToAttackPoint();

        gameObject.SetActive(false);
    }

    private void AttackToAttackPoint()
    {
        Tile currentTile = TileManager.singleton.GetNearestTile(transform.position);
        Tile attackTile = TileManager.singleton.GetNearestTile(transform.position, currentTile);
        attackTile.Interact();
        _weapon.Activate();
        _weapon.Move(attackTile.transform.position);
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
