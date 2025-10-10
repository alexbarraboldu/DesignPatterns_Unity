using System;

using UnityEngine;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public abstract class Composite : Node
	{
		public Composite(params Node[] nodes) : base()
		{
			this.nodes = nodes;
		}

		[SerializeReference]
		public Node[] nodes = Array.Empty<Node>();
	}
}
