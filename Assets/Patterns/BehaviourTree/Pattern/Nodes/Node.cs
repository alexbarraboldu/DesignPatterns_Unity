namespace Patterns.BehaviourTree
{
	public enum NodeStatus { FAILURE, SUCCESS, READY, RUNNING };

	public abstract class Node
	{
		public Node()
		{
			status = NodeStatus.READY;
		}

		public NodeStatus status;

		public abstract NodeStatus RunNode();
	}
}
