using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour
{
    public TeamName Team = TeamName.blue;

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Robot")
        {
            IPlayer player = other.GetComponent<IPlayer>();
            if(player.Team == Team)
                player.Health += 10 * Time.deltaTime;
        }
    }
}
