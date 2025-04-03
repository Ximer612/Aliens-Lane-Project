using UnityEngine;

public class Turret : MonoBehaviour
{
    public int TurretsTickets = 0;

    public void AddTicket()
    {
        TurretsTickets++;
    }

    public void RemoveTicket(TurretAI myTurret)
    {
        if (!myTurret.enabled && TurretsTickets > 0)
        {
            TurretsTickets--;
            myTurret.enabled = true;
            return;
        }

        print("no tickets!");

    }
}
