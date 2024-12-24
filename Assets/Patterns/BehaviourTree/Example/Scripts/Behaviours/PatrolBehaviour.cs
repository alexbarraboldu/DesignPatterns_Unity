using System;

using UnityEngine;
using UnityEngine.AI;

namespace Patterns.BehaviourTree.Example
{
	[RequireComponent(typeof(BehaviourTreeContext))]
	public class PatrolBehaviour : MonoBehaviour, IAction
	{
		private NavMeshAgent _agent;

		private NodeStatus isPatroling = NodeStatus.RUNNING;

		[SerializeField] private Transform[] patrolPoints = Array.Empty<Transform>();
		private int _currentPatrolPoint = 0;

		private void Awake()
		{
			_agent = GetComponentInParent<NavMeshAgent>();
		}

		public NodeStatus Action()
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
	}
}
