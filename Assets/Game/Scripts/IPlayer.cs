using UnityEngine;
using System.Collections;

public interface IPlayer
{
    TeamName Team { get; set; }
    float Health { get; set; }
    float Damage { get; set; }
    float Accuracy { get; set; }
    bool CanAttack { get; }
    bool CanTakeDamage { get; }
    bool IsAlive { get; }

    void SetUpPlayer(TeamName t);
    void SetColors();
    bool Attack(IPlayer target);
    bool TakeDamage(Hit hit);

    void Hit(Hit hit);
}
