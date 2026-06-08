using System;

using UnityEngine;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public class Condition : Task
	{
		[NonSerialized] public ICondition condition;

		public Condition() { }

		public Condition(ICondition iCondition)
		{
			condition = iCondition;
		}

		public override void ResolveBehaviour(BehaviourTreeContext treeContext)
		{
			condition = treeContext.GetBehaviour<ICondition>(SelectedBehaviourId);
		}

		public override NodeStatus RunNode()
		{
			if (condition == null)
			{
				Debug.LogError($"Condition behaviour not resolved. SelectedBehaviourId: {SelectedBehaviourId}");
				return status = NodeStatus.FAILURE;
			}

			return status = condition.Condition() ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
		}
	}
}
