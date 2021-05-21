using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for various types of attacks performed by player and enemies.
/// </summary>
public abstract class WeaponAttack : MonoBehaviour
{
    public GameObject Aim { get; set; }
    public abstract void Attack();
    public abstract bool IsAimInAttackRange();
}