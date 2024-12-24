using Patterns.BehaviourTree;

using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : MonoBehaviour, IAction, ICondition
{
	private NavMeshAgent _agent;

	public bool canChase = false;
	public NodeStatus isChasing = NodeStatus.RUNNING;

	Transform chaseTransform;

	[SerializeField] public LayerMask filterLayer;

	private void Awake()
	{
		_agent = GetComponentInParent<NavMeshAgent>();
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, 5f);
	}


	public bool Condition()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, filterLayer);

		canChase = colliders.Length != 0;

		chaseTransform = canChase ? colliders[0].transform : null;

		Debug.LogWarning("Can chase: " + canChase);
		return canChase;
	}

	public NodeStatus Action()
	{
		Debug.LogWarning("Chasing: "/* + Time.time*/);

		_agent.SetDestination(chaseTransform.position);

		if (transform.position == chaseTransform.position) isChasing = NodeStatus.SUCCESS;
		else isChasing = NodeStatus.RUNNING;

		return isChasing;
	}
}
