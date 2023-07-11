using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Create Game Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    public static GameSettings singleton;

    public float MINIMUM_DISTANCE_BETWEEN_TILES = 1.5f;
    public float MINIMUM_SELECTION_DISTANCE = 0.25f;
    public float TIME_1 = 0.2f;
    public float TIME_2 = 0.2f;

    private void OnEnable()
    {
        singleton = this;
    }
}