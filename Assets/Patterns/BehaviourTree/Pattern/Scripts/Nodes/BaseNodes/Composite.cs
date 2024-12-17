using System;

namespace Patterns.BehaviourTree
{
	//[Serializable]
	public abstract class Composite : Node
	{
		public Composite(params Node[] nodes) : base()
		{
			this.nodes = nodes;
		}

		/*[SerializeReference, SubclassSelector]*/
		public Node[] nodes = Array.Empty<Node>();
	}
}
