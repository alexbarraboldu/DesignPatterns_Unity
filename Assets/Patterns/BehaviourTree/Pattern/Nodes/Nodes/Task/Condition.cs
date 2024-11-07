using System;


namespace Patterns.BehaviourTree
{
	public class Condition : Task
	{
		public Condition(Func<bool> methodCondition)
		{
			condition = methodCondition;
		}

		Func<bool> condition;

		public override NodeStatus RunNode()
		{
			return status = condition() ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
		}
	}
}
