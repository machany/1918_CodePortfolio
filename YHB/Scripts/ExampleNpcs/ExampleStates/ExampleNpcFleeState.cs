using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.ExampleNpcs.ExampleStates
{
	public class ExampleNpcFleeState : NPCState
	{
		private float _enterTime;

		public ExampleNpcFleeState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_enterTime = 5;
		}

		public override void Update()
		{
			if (_enterTime > 0)
				_enterTime -= Time.deltaTime;
			else
				_npc.ChangeBehaviour(NPCBehaviourEnum.Aiming);
		}
	}
}
