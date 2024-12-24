using UnityEngine;

public static class Helpers
{
	public static bool IsApproximately(this Vector3 a, Vector3 b, float difference)
	{
		bool x = a.x >= (b.x - difference) && a.x <= (b.x + + difference);
		bool y = a.y >= (b.y - difference) && a.y <= (b.y + + difference);
		bool z = a.z >= (b.z - difference) && a.z <= (b.z + + difference);

		return x && y && z;
	}
}

namespace Patterns.BehaviourTree.Example
{
	public class EnemyController : MonoBehaviour
	{
		private BehaviourTreeContext _tree;

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
			//Condition aCon = new Condition(attackBehaviour.Condition);
			//Action aAct = new Action(attackBehaviour.Action);

			//Condition cCon = new Condition(chaseBehaviour.Condition);
			//Action cAct = new Action(chaseBehaviour.Action);

			//Action pAct = new Action(patrolBehaviour.Action);

			
			//Sequence s1 = new Sequence(aCon, aAct);
			//Sequence s2 = new Sequence(cCon, cAct);

			//Selector sel = new Selector(s1, s2, pAct);

			_tree.Node = //sel;
				new Selector(
					new Sequence(
						new Condition(attackBehaviour.Condition),
						new Action(attackBehaviour.Action)
					),
					new Sequence(
						new Condition(chaseBehaviour.Condition),
						new Action(chaseBehaviour.Action)
					),
					new Action(patrolBehaviour.Action)
				);

			_tree.TimerRate = 0f;
		}
	}
}
