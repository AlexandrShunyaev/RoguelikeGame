using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : Weapon
{
    void Update()
    {
        var rotation = AimPosition - transform.position;
        var rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
