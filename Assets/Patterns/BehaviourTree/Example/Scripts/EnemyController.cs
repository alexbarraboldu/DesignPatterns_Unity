using UnityEngine;

public static class Helpers
{
	public static bool IsApproximately(this Vector3 a, Vector3 b, float difference)
	{
		bool x = a.x >= (b.x - difference) && a.x <= (b.x + +difference);
		bool y = a.y >= (b.y - difference) && a.y <= (b.y + +difference);
		bool z = a.z >= (b.z - difference) && a.z <= (b.z + +difference);

		return x && y && z;
	}
}

namespace Patterns.BehaviourTree.Example
{
	public class EnemyController : MonoBehaviour
	{
		[SerializeField] private BehaviourTreeContext _tree;

		private PatrolBehaviour patrolBehaviour;
		private ChaseBehaviour chaseBehaviour;
		private AttackBehaviour attackBehaviour;

		private void Awake()
		{
			_tree = GetComponentInChildren<BehaviourTreeContext>();

			patrolBehaviour = GetComponentInChildren<PatrolBehaviour>();
			chaseBehaviour = GetComponentInChildren<ChaseBehaviour>();
			attackBehaviour = GetComponentInChildren<AttackBehaviour>();

			SetBehaviourTree();
		}


		private void SetBehaviourTree()
		{
			//Condition aCon = new Condition(attackBehaviour.GetComponent<ICondition>());
			//Action aAct = new Action(attackBehaviour.GetComponent<IAction>());

			//Condition cCon = new Condition(chaseBehaviour.GetComponent<ICondition>());
			//Action cAct = new Action(chaseBehaviour.GetComponent<IAction>());

			//Action pAct = new Action(patrolBehaviour.GetComponent<IAction>());


			//Sequence s1 = new Sequence(aCon, aAct);
			//Sequence s2 = new Sequence(cCon, cAct);

			//Selector sel = new Selector(s1, s2, pAct);

			//_tree.Node = sel;


			//_tree.Node =
			//	new Selector(
			//		new Sequence(
			//			new Condition(attackBehaviour.GetComponent<ICondition>()),
			//			new Action(attackBehaviour.GetComponent<IAction>())
			//		),
			//		new Sequence(
			//			new Condition(chaseBehaviour.GetComponent<ICondition>()),
			//			new Action(chaseBehaviour.GetComponent<IAction>())
			//		),
			//		new Action(patrolBehaviour.GetComponent<IAction>())
			//	);

			_tree.TimerRate = 0f;
		}

		///	Not needed in theory
		//public BehaviourTreeContext GetBehvaiourTreeContext() => _tree;
	}
}
