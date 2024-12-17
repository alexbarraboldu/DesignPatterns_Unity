using System;


namespace Patterns.BehaviourTree
{
	//[Serializable]
	public class Condition : Task
	{
		//public Condition() { }

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
