using System;

namespace Patterns.BehaviourTree
{
	//[Serializable]
	public class Action : Task
	{
		//public Action() : base() { }

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
