using System;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public abstract class Task : Node
	{
		public string SelectedBehaviourId;

		public abstract void ResolveBehaviour(BehaviourTreeContext treeContext);
		//public Task() : base() { }
	}
}
