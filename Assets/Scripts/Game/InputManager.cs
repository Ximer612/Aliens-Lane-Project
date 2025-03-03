using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerInputSubscription GetInput {  get; private set; }

    private void Awake()
    {
        GetInput = GetComponent<PlayerInputSubscription>();
    }
}
