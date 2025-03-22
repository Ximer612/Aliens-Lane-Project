using UnityEngine;

[CreateAssetMenu(fileName = "Wave_INDEX", menuName = "ScriptableObjects/Wave")]
public class WaveScriptableObject : ScriptableObject
{
    public WaveManche[] SpawnManche;
}

[System.Serializable]
public struct WaveManche
{
    public float TimeFromLastManche;
    public GameObject[] Aliens;
}