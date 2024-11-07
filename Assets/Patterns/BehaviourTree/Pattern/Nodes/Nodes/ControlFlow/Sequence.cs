namespace Patterns.BehaviourTree
{
	public class Sequence : Composite
	{
		public Sequence(params Node[] nodes) : base(nodes)
		{

		}

		public override NodeStatus RunNode()
		{
			NodeStatus _status = NodeStatus.FAILURE;

			bool exitLoop = false;
			for (int i = 0; i < nodes.Length && !exitLoop; i++)
			{
				_status = nodes[i].RunNode();

				switch (_status)
				{
					case NodeStatus.FAILURE:
						_status = NodeStatus.FAILURE;
						exitLoop = true;
						break;
					case NodeStatus.SUCCESS:
						_status = NodeStatus.SUCCESS;
						continue;
					case NodeStatus.RUNNING:
						_status = NodeStatus.RUNNING;
						exitLoop = true;
						break;
				}
			}

			status = _status;
			return _status;
		}
	}
}
