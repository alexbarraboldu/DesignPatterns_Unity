using System;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public class Condition : Task
	{
		public Condition() { }

		public Condition(ICondition iCondition)
		{
			condition = iCondition;
		}

		public ICondition condition;

		public override NodeStatus RunNode()
		{
			return status = condition.Condition() ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
		}
	}
}
