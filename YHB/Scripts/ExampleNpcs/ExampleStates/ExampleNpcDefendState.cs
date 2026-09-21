using _01_Work.LCM._01.Scripts.Entities;
using Assets.AgamaLibrary.DataStructures;
using Assets._01_Work.YHB.Scripts.NPCs;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.ExampleNpcs.ExampleStates
{
	public class ExampleNpcDefendState : NPCState
	{
		private float _enterTime;
		// bt의 wait for second 같은 용도로 사용하면 편함.
		private FlagInvoker _isTimeoutOne;

		public ExampleNpcDefendState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_isTimeoutOne = new FlagInvoker();
		}

		public override void Enter()
		{
			base.Enter();

			_enterTime = Time.time;
			_isTimeoutOne.SetFlag(false);
			_isTimeoutOne.OnFlagActived += HandleFlagActived;
		}

		private void HandleFlagActived()
		{
			_npcAnimator.ChangeAnimation(_npc.GetAnimationHash(NPCAnimationEnum.WALK));
		}

		public override void Update()
		{
			if (Mathf.Abs(Time.time - _enterTime) < 1 && !_isTimeoutOne.Flag)
			{
				_isTimeoutOne.SetFlag(true, false, true);
				return;
			}

			Vector3 dir = Vector3.zero - _npc.transform.position;
			dir.Normalize();

			_npc.transform.position += dir * _npc.DeltaTime;

			if (_npc.transform.position.magnitude < Mathf.Epsilon
				|| Mathf.Abs(Time.time - _enterTime) > 5)
				_npc.ChangeBehaviour(NPCBehaviourEnum.S_Idle);
		}

		public override void Exit()
		{
			_isTimeoutOne.OnFlagActived -= HandleFlagActived;

			base.Exit();
		}
	}
}
