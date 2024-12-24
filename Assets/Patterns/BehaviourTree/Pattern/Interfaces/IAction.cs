using System.Collections;
using System.Collections.Generic;

using Patterns.BehaviourTree;

using UnityEngine;

public interface IAction
{
	public NodeStatus Action();
}
