using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [field: Header("Level")]
    [field: SerializeField] public float LevelMaxTime { get; private set; }

    [field: Header("DeathAreas")]
    [field: SerializeField] public int DeathAreaDamage { get; private set; }
    [field: SerializeField] public float DeathAreaBlinkCooldown { get; private set; }
    [field: SerializeField] public float DeathAreaBlinkTime { get; private set; }
    [field: SerializeField] public float DeathAreaFadeInTime { get; private set; }

    [field: Header("Coins")]
    [field: SerializeField] public int CoinValue { get; private set; }
    [field: SerializeField] public AudioClip CoinPickAudio { get; private set; }
}