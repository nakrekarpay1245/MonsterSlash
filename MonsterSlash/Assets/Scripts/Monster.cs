using System.Collections;
using UnityEngine;

public class Monster : AbstractItem, IKillable, IInteractable, IMoveable
{
    [SerializeField]
    private ItemType _monsterType;
    public ItemType MonsterType { get => _monsterType; private set { } }

    [SerializeField]
    private float _health;

    private Animator _animator;
    private int isGetHitHashCode;
    private int isDeadHashCode;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        isGetHitHashCode = Animator.StringToHash("isGetHit");
        isDeadHashCode = Animator.StringToHash("isDead");
    }

    /// <summary>
    /// This function reduces the object's health by the specified damage amount.
    /// </summary>
    /// <param name="damage"></param>
    public void Interact()
    {
        _health--;
        if (_health <= 0)
        {
            Kill();
            return;
        }
        PlayHitEffects();
    }

    /// <summary>
    /// This function plays the hit effects, such as a splash particle and a sound effect.
    /// </summary>
    private void PlayHitEffects()
    {
        ParticleManager.singleton.PlayParticleAtPoint("BloodParticle", transform.position);
        AudioManager.singleton.PlaySound("Splat");
    }

    public void Kill()
    {
        StartCoroutine(KillRoutine());
    }

    /// <summary>
    /// This coroutine handles the process of killing the object, including triggering the death
    /// animation, playing kill effects, and deactivating the object after a specified delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator KillRoutine()
    {
        _animator.SetTrigger(isDeadHashCode);

        PlayKillEffects();
        yield return new WaitForSeconds(GameSettings.singleton.TIME_1);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// This function plays the kill effects, such as a splash particle and a sound effect.
    /// </summary>
    private void PlayKillEffects()
    {
        ParticleManager.singleton.PlayParticleAtPoint((gameObject.name + "SplashParticle"), transform.position);
        AudioManager.singleton.PlaySound("Splat");
    }

    public void Move(Vector2 targetPosition)
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