using UnityEngine;

/// <summary>
/// Base class for various types of attacks performed by player and enemies.
/// </summary>
public abstract class WeaponAttack : MonoBehaviour
{
    /// <summary>
    /// The aim of the attack.
    /// </summary>
    public GameObject Aim { get; set; }
    /// <summary>
    /// Starts the attack. It is intended to be called when the attack animation starts.
    /// </summary>
    public abstract void Attack();
    /// <summary>
    /// Tells if the script considers Aim to be within object's attack range.
    /// </summary>
    /// <returns>true if the aim is considered to be within attack range.</returns>
    public abstract bool IsAimInAttackRange();
}
