using UnityEngine;

public class ItemShower : MonoBehaviour
{
    [SerializeField] int _currentWave;
    [SerializeField] GameObject[] weapons;

    private void Awake()
    {
        WaveSpawner.OnChangeState += ShouldShowItems;
    }

    public void ShouldShowItems(SpawnerState newState, int waveIndex)
    {
        if(newState == SpawnerState.CanShop)
        {
            for (int i = 0; i < waveIndex+1; i++)
            {
                if(i >= weapons.Length)
                {
                    break;
                }

                weapons[i].SetActive(true);
            }
        }
        else if (newState == SpawnerState.Idle)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (i >= weapons.Length)
                {
                    break;
                }

                weapons[i].SetActive(false);
            }
        }
    }
}
