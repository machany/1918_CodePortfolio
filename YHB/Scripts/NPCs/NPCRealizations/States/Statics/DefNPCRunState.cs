using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	public class DefNPCRunState : DefNPCWalkState
	{
		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.S_Run;
		protected override bool LookOtherTransform => false;

		private bool _runSpeedReached;

		private float _enterTime;
		private float _accelerationTime;

		private float _walkSpeed;
		private float _runSpeed;

		public DefNPCRunState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}


		public override void Enter()
		{
			base.Enter();

			_moveDynamicData.isRun = true;

			_runSpeedReached = false;

			_enterTime = Time.time;
			_accelerationTime = _moveStaticData.AccelerationToRunSpeedTime;

			_walkSpeed = _moveStaticData.WalkSpeed;
			_runSpeed = _moveStaticData.RunSpeed;
		}

		public override void Update()
		{
			if (!_runSpeedReached)
			{
				float t = (Time.time - _enterTime) / _accelerationTime;
				_navMovement.SetSpeed(Mathf.Lerp(_walkSpeed, _runSpeed, Mathf.Clamp01(t)));

				if (t >= 1f)
				{
					_runSpeedReached = true;
					_navMovement.SetSpeed(_runSpeed);
				}
			}

			if (_moveDynamicData.isOnChargeStage
				&& _moveDynamicData.isRunToChargeReady)
			{
				// Dynamic적인 Behaviour임. 따라서 to 상태 저장 필요
				_npc.ChangeBehaviour(NPCBehaviourEnum.D_ChargeReady, true);
				return;
			}

			base.Update();
		}

		public override void Exit()
		{
			_moveDynamicData.isRun = false;

			base.Exit();
		}
	}
}
