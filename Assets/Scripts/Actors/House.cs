using UnityEngine;

public class House : Actor
{
    public static House Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

}
