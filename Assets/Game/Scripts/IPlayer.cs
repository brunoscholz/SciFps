using UnityEngine;
using System.Collections;

public interface IPlayer
{
    Transform Body { get; }
    int teamID { get; set; }
    TeamName Team { get; set; }
    float Health { get; set; }
    float Damage { get; set; }
    float Accuracy { get; set; }
    bool CanAttack { get; }
    bool CanTakeDamage { get; }
    bool IsAlive { get; }

    void SetUpPlayer(TeamName t, int id);
    void SetColors();
    void SetColors(Hud h);
    void Respawn();
    void SetCommander(GameObject go);
    void SetCommand(string action);
    void SetAction(string name, string value);
    void SetAction(string name, float value);
    void SetAction(string name, bool value);
    void SetAction(string name, Vector3 value);
    void SetAction(string name, GameObject value);

    bool Attack(IPlayer target);
    bool TakeDamage(Hit hit);

    void Hit(Hit hit);
    void Eliminate();
}
