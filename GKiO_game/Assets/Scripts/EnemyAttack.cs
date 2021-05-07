using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public GameObject Aim { get; set; }
    public abstract void Attack();
}
