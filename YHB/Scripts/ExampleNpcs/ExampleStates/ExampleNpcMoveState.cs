using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.ExampleNpcs.ExampleStates
{
	public class ExampleNpcMoveState : NPCState
	{
		public ExampleNpcMoveState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}

		public override void Update()
		{
			Vector3 pos = _entity.transform.position;
			pos += _entity.transform.forward * _npc.DeltaTime;

			//if (pos.magnitude > 10)
			//	_npc.ChangeBehaviour(NPCBehaviourEnum.Flee);
			//else
			//	_entity.transform.position = pos;

			_entity.transform.position = pos;
		}
	}
}
