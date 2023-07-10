
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string Name;
    public AudioClip Clip;
    public float Volume = 1f;
    public float Pitch = 1f;
    public bool Loop = false;
}