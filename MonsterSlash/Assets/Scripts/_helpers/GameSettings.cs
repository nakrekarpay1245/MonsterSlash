using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Create Game Settings", order = 1)]
public class GameSettings : MonoSingleton<GameSettings>
{
    public float MINIMUM_DISTANCE_BETWEEN_TILES = 1.5f;
    public float MINIMUM_SELECTION_DISTANCE = 0.25f;
    public float TIME_1 = 0.2f;
    public float TIME_2 = 0.2f;
}