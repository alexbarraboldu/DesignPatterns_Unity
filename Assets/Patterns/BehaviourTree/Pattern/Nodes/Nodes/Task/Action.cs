using System;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public class Action : Task
	{
		public Action() : base() { }

		public Action(IAction iAction)
		{
			action = iAction;
		}

		public IAction action;

		public override NodeStatus RunNode()
		{
			return status = action.Action();
		}
	}
}
