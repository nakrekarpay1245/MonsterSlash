using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterType _monsterType;
    public MonsterType MonsterType { get => _monsterType; private set { } }

    [SerializeField]
    private float _health;
    public float Health { get => _health; private set { } }

    public void TakeDamage()
    {
        _health--;
        if (_health <= 0)
        {
            Kill();
            return;
        }
        PlayHitEffects();
    }

    private void PlayHitEffects()
    {
        ParticleManager.singleton.PlayParticleAtPoint("BloodParticle", transform.position);
        AudioManager.singleton.PlaySound("Splat");
    }

    private void Kill()
    {
        //LevelManager.singleton.IncreaseMonsterCount();
        PlayKillEffects();
        gameObject.SetActive(false);
    }

    private void PlayKillEffects()
    {
        ParticleManager.singleton.PlayParticleAtPoint("BloodParticle", transform.position);
        AudioManager.singleton.PlaySound("Splat");
    }

    public void Fall()
    {
        transform.localPosition -= Vector3.up;
    }
}

public enum MonsterType
{
    Demon,
    Troll,
}
