using System.Collections;
using UnityEngine;

public class Brick : Item, IBreakable
{
    /// <summary>
    /// This function reduces the object's health by the specified damage amount.
    /// </summary>
    /// <param name="damage"></param>
    public override void Interact()
    {
        Break();
    }

    public void Break()
    {
        StartCoroutine(BreakRoutine());
    }

    /// <summary>
    /// This coroutine handles the process of killing the object, including triggering the death
    /// animation, playing kill effects, and deactivating the object after a specified delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BreakRoutine()
    {
        PlayBreakEffects();
        yield return new WaitForSeconds(GameSettings.singleton.TIME_1);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// This function plays the kill effects, such as a splash particle and a sound effect.
    /// </summary>
    private void PlayBreakEffects()
    {
        ParticleManager.singleton.PlayParticleAtPoint((gameObject.name + "BreakParticle"),
            transform.position);

        AudioManager.singleton.PlaySound("Break");
    }

    public override void Move(Vector2 targetPosition)
    {
        base.Move(targetPosition);
    }
}
