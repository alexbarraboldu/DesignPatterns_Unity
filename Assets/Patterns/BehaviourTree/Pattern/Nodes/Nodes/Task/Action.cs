using System;

namespace Patterns.BehaviourTree
{
	public class Action : Task
	{
		public Action(Func<NodeStatus> methodAction)
		{
			action = methodAction;
		}

		Func<NodeStatus> action;

		public override NodeStatus RunNode()
		{
			return status = action();
		}
	}
}
