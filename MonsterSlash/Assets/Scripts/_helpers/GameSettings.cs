using UnityEngine;

public class GameSettings : MonoSingleton<GameSettings>
{
    public float MINIMUM_DISTANCE_BETWEEN_TILES = 1.5f;
    public float MINIMUM_SELECTION_DISTANCE = 0.4f;
    public float TIME_1 = 0.2f;
    public float TIME_2 = 0.05f;
}