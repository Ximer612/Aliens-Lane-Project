using System.Collections.Generic;
using UnityEngine;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] Fence[] Fences;
    [SerializeField] List<NormalAlien> Aliens;
    [SerializeField] Transform AliensParent;

    void Start()
    {
        for (int i = 0; i < Fences.Length; i++)
        {
            Fences[i].OnChangeState += UpdateAliensPath;
        }

        GetAllAliens();
    }

    public void GetAllAliens()
    {
        Aliens.Clear();

        for (int i = 0; i < AliensParent.childCount; i++)
        {
            Aliens.Add(AliensParent.GetChild(i).GetComponent<NormalAlien>());
        }
    }

    void UpdateAliensPath()
    {
        for (int i = 0; i < Aliens.Count; i++)
        {
            Aliens[i].RefreshDestination();
        }
    }
}
