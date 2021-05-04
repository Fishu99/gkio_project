using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Ró¿ni przeciwnicy, np. szkielet i goblin korzystaj¹ z tego samego skryptu do patrolowania terenu i atakowania,
 * ale maj¹ ró¿ne animacje.
 * Klasa abstrakcyjna adaptera s³u¿y do tego, aby mo¿na by³o kontrolowaæ animacje ró¿nych przeciwników z tego samego skryptu.
 * Z ka¿dym przeciwnikiem zwi¹zana jest implementacja tej klasy.
 */
public abstract class SwordEnemyAnimationAdapter : MonoBehaviour
{
    public abstract void Attack();
    public abstract void Die();
}
