using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public interface IShooter
{
    bool IsAiming { get; set; }
    IGun Gun { get; }
    GunType GunType { get; set; }

    void Reload();
    LaserBullet Shoot();
}
