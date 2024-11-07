namespace Patterns.BehaviourTree
{
	public class Selector : Composite
	{
		public Selector(params Node[] nodes) : base(nodes)
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
						continue;
					case NodeStatus.SUCCESS:
						_status = NodeStatus.SUCCESS;
						exitLoop = true;
						break;
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
