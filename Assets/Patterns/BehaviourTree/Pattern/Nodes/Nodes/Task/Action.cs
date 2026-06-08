using System;

using UnityEngine;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public class Action : Task
	{
		[NonSerialized] public IAction action;

		public Action() : base() { }

		public Action(IAction iAction)
		{
			action = iAction;
		}

		public override void ResolveBehaviour(BehaviourTreeContext treeContext)
		{
			action = treeContext.GetBehaviour<IAction>(SelectedBehaviourId);
		}

		public override NodeStatus RunNode()
		{
			if (action == null)
			{
				Debug.LogError($"Action behaviour not resolved. SelectedBehaviourId: {SelectedBehaviourId}");
				return NodeStatus.FAILURE;
			}

			return status = action.Action();
		}
	}
}
