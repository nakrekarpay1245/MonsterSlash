using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoSingleton<ParticleManager>
{
    [System.Serializable]
    public class ParticleInfo
    {
        public GameObject particlePrefab;
        [HideInInspector]
        public ParticleSystem particleSystem;
    }

    public List<ParticleInfo> particleList = new List<ParticleInfo>();

    private Dictionary<string, ParticleSystem> particleDictionary =
        new Dictionary<string, ParticleSystem>();

    private void Awake()
    {
        foreach (ParticleInfo particleInfo in particleList)
        {
            if (particleInfo.particlePrefab != null)
            {
                GameObject particleObject = Instantiate(particleInfo.particlePrefab, transform);
                particleInfo.particleSystem = particleObject.GetComponent<ParticleSystem>();
                particleDictionary.Add(particleInfo.particlePrefab.name, particleInfo.particleSystem);
                particleInfo.particleSystem.Stop();
            }
        }
    }

    public void PlayParticleAtPoint(string particleName, Vector3 position)
    {
        if (particleDictionary.ContainsKey(particleName))
        {
            ParticleSystem particleSystem = particleDictionary[particleName];
            particleSystem.transform.position = position;
            particleSystem.Play();
        }
        else
        {
            Debug.LogWarning("Particle with name " + particleName + " does not exist!");
        }
    }

    public void StopAllParticles()
    {
        foreach (ParticleSystem particleSystem in particleDictionary.Values)
        {
            particleSystem.Stop();
        }
    }
}
