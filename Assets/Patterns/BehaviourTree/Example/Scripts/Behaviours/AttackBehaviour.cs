using Patterns.BehaviourTree;

using UnityEngine;

public class AttackBehaviour : MonoBehaviour, IAction, ICondition
{
	public bool canAttack = false;
	public NodeStatus isAttacking = NodeStatus.RUNNING;

	Transform attackTransform;

	[SerializeField] public LayerMask filterLayer;

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, 2f);
	}


	public bool Condition()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, filterLayer);

		canAttack = colliders.Length != 0;

		attackTransform = canAttack ? colliders[0].transform : null;

		Debug.LogWarning("Can attack: " + canAttack);
		return canAttack;
	}

	public NodeStatus Action()
	{
		Debug.LogWarning("Attacking: "/* + Time.time*/);

		Destroy(attackTransform.gameObject);

		return isAttacking;
	}
}
