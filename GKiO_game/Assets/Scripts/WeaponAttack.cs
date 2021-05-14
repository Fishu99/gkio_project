using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAttack : MonoBehaviour
{
    public GameObject Aim { get; set; }
    public abstract void Attack();
    public abstract bool IsAimInAttackRange();
}
