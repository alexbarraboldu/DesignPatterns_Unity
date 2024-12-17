namespace Patterns.BehaviourTree.Example
{
	//[Serializable]
	public class CharacterBehaviourTree : BehaviourTreeContext
	{
		public CharacterBehaviourTree(Character character)
		{
			node = new Selector(
				new Sequence(
					new Condition(character.CheckAttack),
					new Action(character.Attack)
				),
				new Sequence(
					new Condition(character.CheckChase),
					new Action(character.Chase)
				),
				new Action(character.Patrol)
			);

			SetNodesArray();
		}
	}
}
