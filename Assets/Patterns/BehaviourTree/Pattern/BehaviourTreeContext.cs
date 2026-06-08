using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public class BehaviourTreeContext : MonoBehaviour
	{
		[SerializeField] private BlackboardSO _blackboardSO;

		[SerializeField] private bool startRunning = false;
		[SerializeReference, Space(5)] private Node node;

		private Node[] _nodes;

		private float timer = 0f;
		private float timerRate = 1f;

		readonly Dictionary<string, MonoBehaviour> behavioursById = new();
		public IReadOnlyDictionary<string, MonoBehaviour> BehavioursById => behavioursById;


		public bool StartRunning
		{
			get => startRunning;
			set
			{
				startRunning = value;
			}
		}

		public Node Node
		{
			get => node;
			set
			{
				Debug.Assert(value != null);
				node = value;
			}
		}

		public float TimerRate
		{
			get => timerRate;
			set => timerRate = value;
		}


		private void Awake()
		{
			CacheBehaviours();
			ResolveBehaviourTree();
		}

		private void Start()
		{
			SetNodesArray();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			CacheBehaviours();
		}
#endif

		private void Update()
		{
			if (StartRunning)
				RunBehaviourTree(Time.deltaTime);
		}


		public void CacheBehaviours()
		{
			behavioursById.Clear();

			MonoBehaviour[] behaviours = GetComponentsInChildren<MonoBehaviour>(true)
				.Where(behaviour =>
					behaviour != this &&
					(behaviour is IAction || behaviour is ICondition))
				.ToArray();

			foreach (MonoBehaviour behaviour in behaviours)
			{
				string id = GetBehaviourId(behaviour);

				if (!behavioursById.TryAdd(id, behaviour))
				{
					Debug.LogWarning($"Duplicate behaviour id found: {id}", behaviour);
				}
			}
		}

		public TInterface GetBehaviour<TInterface>(string behaviourId) where TInterface : class
		{
			if (string.IsNullOrEmpty(behaviourId))
			{
				return null;
			}

			if (!behavioursById.TryGetValue(behaviourId, out MonoBehaviour behaviour))
			{
				Debug.LogError($"Behaviour id not found: {behaviourId}", this);
				return null;
			}

			if (behaviour is TInterface interfaceBehaviour)
			{
				return interfaceBehaviour;
			}

			Debug.LogError($"Behaviour '{behaviour.name}' does not implement {typeof(TInterface).Name}", behaviour);
			return null;
		}

		public static string GetBehaviourId(MonoBehaviour behaviour)
		{
			return $"{GetTransformPath(behaviour.transform)}|{behaviour.GetType().FullName}";
		}

		public static string GetTransformPath(Transform transform)
		{
			var names = new Stack<string>();

			while (transform != null)
			{
				names.Push(transform.name);
				transform = transform.parent;
			}

			return string.Join("/", names);
		}

		public void ResolveBehaviourTree()
		{
			if (node == null)
				return;

			ResolveNode(node);
		}

		private void ResolveNode(Node node)
		{
			if (node == null)
				return;

			if (node is Task task)
			{
				task.ResolveBehaviour(this);
			}

			if (node is Composite composite)
			{
				foreach (Node child in composite.nodes)
				{
					ResolveNode(child);
				}
			}
		}

		private void SetNodesArray()
		{
			List<Node> nodes = new List<Node>();
			TraverseNodeTree(node, ref nodes);
			_nodes = nodes.ToArray();
		}

		protected virtual void RunBehaviourTree(float deltaTime)
		{
			if (timer >= timerRate)
			{
				node.RunNode();
				timer = 0f;

				//PrintAllNodesStatus();
				if (node.status == NodeStatus.RUNNING)
				{
					ResetNodeTree();
					PrintAllNodesStatus();
				}
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
