using System;

using TMPro.EditorUtilities;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;

public static class Helpers
{
	public static bool IsApproximately(this Vector3 a, Vector3 b, float difference)
	{
		bool x = a.x >= (b.x - difference) && a.x <= (b.x + + difference);
		bool y = a.y >= (b.y - difference) && a.y <= (b.y + + difference);
		bool z = a.z >= (b.z - difference) && a.z <= (b.z + + difference);

		return x && y && z;
	}
}

namespace Patterns.BehaviourTree.Example
{
	[RequireComponent(typeof(BehaviourTreeContext), typeof(NavMeshAgent))]
	public class EnemyController : MonoBehaviour
	{
		private BehaviourTreeContext _tree;

		private NavMeshAgent _agent;

		[SerializeField] private LayerMask enemyLayer;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();

			SetBehaviourTree();
		}


		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, 2f);
			Gizmos.DrawWireSphere(transform.position, 5f);
		}


		private void SetBehaviourTree()
		{
			_tree = GetComponent<BehaviourTreeContext>();
			_tree.Node =
				new Selector(
					new Sequence(
						new Condition(CheckAttack),
						new Action(Attack)
					),
					new Sequence(
						new Condition(CheckChase),
						new Action(Chase)
					),
					new Action(Patrol)
				);
			_tree.SetNodesArray();

			_tree.TimerRate = 0f;
		}

		#region CHASE BEHAVIOUR
		public bool canChase = false;
		public NodeStatus isChasing = NodeStatus.RUNNING;

		Transform chaseTransform;
		public bool CheckChase()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, enemyLayer);

			canChase = colliders.Length != 0;

			chaseTransform = canChase ? colliders[0].transform : null;

			Debug.LogWarning("Can chase: " + canChase);
			return canChase;
		}

		public NodeStatus Chase()
		{º
			Debug.LogWarning("Chasing: "/* + Time.time*/);

			_agent.SetDestination(chaseTransform.position);

			if (transform.position == chaseTransform.position) isChasing = NodeStatus.SUCCESS;
			else isChasing = NodeStatus.RUNNING;

			return isChasing;
		}
		#endregion

		#region ATTACK BEHAVIOUR
		public bool canAttack = false;
		public NodeStatus isAttacking = NodeStatus.RUNNING;

		Transform attackTransform;
		public bool CheckAttack()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, enemyLayer);

			canAttack = colliders.Length != 0;

			attackTransform = canAttack ? colliders[0].transform : null;

			Debug.LogWarning("Can attack: " + canAttack);
			return canAttack;
		}

		public NodeStatus Attack()
		{
			Debug.LogWarning("Attacking: "/* + Time.time*/);

			Destroy(attackTransform.gameObject);

			return isAttacking;
		}
		#endregion

		#region PATROL BEHAVIOUR
		private NodeStatus isPatroling = NodeStatus.RUNNING;

		[SerializeField] private Transform[] patrolPoints = Array.Empty<Transform>();
		private int _currentPatrolPoint = 0;

		public NodeStatus Patrol()
		{
			if (_agent.remainingDistance == 0f)
			{
				if (patrolPoints[_currentPatrolPoint].position.IsApproximately(_agent.destination, 1f))
				{
					if (_currentPatrolPoint < patrolPoints.Length - 1) _currentPatrolPoint++;
					else _currentPatrolPoint = 0;
				}

				_agent.SetDestination(patrolPoints[_currentPatrolPoint].position);
			}
			return isPatroling;
		}
		#endregion
	}
}
