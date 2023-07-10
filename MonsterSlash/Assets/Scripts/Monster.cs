using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IKillable, IDamagable, IMoveable
{
    [SerializeField]
    private MonsterType _monsterType;
    public MonsterType MonsterType { get => _monsterType; private set { } }

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

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Kill();
            return;
        }
        PlayHitEffects();
    }

    private void PlayHitEffects()
    {
        //ParticleManager.singleton.PlayParticleAtPoint("BloodParticle", transform.position);
        AudioManager.singleton.PlaySound("Splat");
    }

    public void Kill()
    {
        StartCoroutine(KillRoutine());
    }

    private IEnumerator KillRoutine()
    {
        _animator.SetTrigger(isDeadHashCode);
        PlayKillEffects();

        yield return new WaitForSeconds(Constants.TIME_2);

        //LevelManager.singleton.IncreaseMonsterCount();
        gameObject.SetActive(false);
    }

    private void PlayKillEffects()
    {
        ParticleManager.singleton.PlayParticleAtPoint("BloodParticle", transform.position);
        AudioManager.singleton.PlaySound("Splat");
    }

    public void Move(Vector2 targetPosition)
    {
        StopCoroutine(MoveRoutine(targetPosition));
        StartCoroutine(MoveRoutine(targetPosition));
    }

    public IEnumerator MoveRoutine(Vector2 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float distance = Mathf.Abs(startPosition.y - targetPosition.y);

        float elapsedTime = 0f;
        float fallTime = Constants.TIME_1 * distance;

        while (elapsedTime < fallTime)
        {
            float t = elapsedTime / fallTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

public enum MonsterType
{
    Demon,
    Troll,
}
