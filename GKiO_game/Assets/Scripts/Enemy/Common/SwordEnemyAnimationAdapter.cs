using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class for animation adapters for the enemies.
/// Different enemies such as skeleton or goblin use the SwordEnemyController script for patrolling and attacking but they have different animations.
/// The adapter provides an interface for controlling different animations from the SwordEnemyController scripts.
/// Each enemy has its own implementation of this class.
/// </summary>
public abstract class SwordEnemyAnimationAdapter : MonoBehaviour
{
    /// <summary>
    /// Perform the animation of attacking.
    /// </summary>
    public abstract void Attack();
    /// <summary>
    /// Perform the animation of dying.
    /// </summary>
    public abstract void Die();
}

