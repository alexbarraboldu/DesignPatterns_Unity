using UnityEngine;

namespace Patterns.BehaviourTree.Example
{
	public class Character : MonoBehaviour
	{
		[SerializeField] private CharacterBehaviourTree _tree;

		private void Start()
		{
			_tree = new CharacterBehaviourTree(this);
		}

		private void Update()
		{
			_tree.RunBehaviourTree(Time.deltaTime);
		}

		public bool CheckAttack()
		{
			return true;
		}

		public bool CheckChase()
		{
			return true;
		}

		public NodeStatus Patrol()
		{
			return NodeStatus.SUCCESS;
		}

		public NodeStatus Attack()
		{
			return NodeStatus.READY;
		}

		public NodeStatus Chase()
		{
			return NodeStatus.READY;
		}
	}
}
