using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * R�ni przeciwnicy, np. szkielet i goblin korzystaj� z tego samego skryptu do patrolowania terenu i atakowania,
 * ale maj� r�ne animacje.
 * Klasa abstrakcyjna adaptera s�u�y do tego, aby mo�na by�o kontrolowa� animacje r�nych przeciwnik�w z tego samego skryptu.
 * Z ka�dym przeciwnikiem zwi�zana jest implementacja tej klasy.
 */
public abstract class SwordEnemyAnimationAdapter : MonoBehaviour
{
    public abstract void Attack();
    public abstract void Die();
}
