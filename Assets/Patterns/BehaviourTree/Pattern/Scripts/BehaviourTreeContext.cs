using System;
using System.Collections.Generic;

using UnityEngine;

namespace Patterns.BehaviourTree
{
	/// <summary>
	/// Parent class for a BehvaiourTree
	/// </summary>
	[Serializable]
	public abstract class BehaviourTreeContext
	{
		[SerializeField] private BlackboardSO _blackboardSO;

		/*[SerializeReference, SubclassSelector]*/
		protected Node node;
		private Node[] _nodes;

		private float timer = 0f;
		private float timerRate = 1f;
		protected float TimerRate
		{
			get => TimerRate;
			set => TimerRate = value;
		}

		protected void SetNodesArray()
		{
			List<Node> nodes = new List<Node>();
			TraverseNodeTree(node, ref nodes);
			_nodes = nodes.ToArray();
		}

		public virtual void RunBehaviourTree(float deltaTime)
		{
			if (timer >= timerRate)
			{
				node.RunNode();
				timer = 0f;

				PrintAllNodesStatus();
				if (node.status == NodeStatus.RUNNING) ResetNodeTree();
				PrintAllNodesStatus();
			}
			else timer += deltaTime;
		}
		private void ResetNodeTree()
		{
			for (int i = 0; i < _nodes.Length; i++)
			{
				if (_nodes[i].status != NodeStatus.RUNNING) _nodes[i].status = NodeStatus.READY;
			}
			_nodes[_nodes.Length - 1].status = NodeStatus.READY;
		}

		private void PrintAllNodesStatus()
		{
			string typeNames = "";
			for (int i = 0; i < _nodes.Length; i++)
			{
				string nodeName = _nodes[i].GetType().ToString();
				nodeName.Substring(nodeName.IndexOf('.') + 1);
				string nodeStatus = _nodes[i].status.ToString();
				string nodeInfo = nodeName + ": " + nodeStatus;

				typeNames += nodeInfo + " <- ";
			}
			Debug.Log(typeNames);
		}

		private bool TraverseNodeTree(Node fromNode, ref List<Node> nodeTree)
		{
			Node[] nodes = fromNode is Composite ?
							((Composite)fromNode).nodes :
							Array.Empty<Node>();

			for (int i = 0; i < nodes.Length; i++)
			{
				TraverseNodeTree(nodes[i], ref nodeTree);
			}

			nodeTree.Add(fromNode);

			return nodes.Length != 0;
		}
	}
}
