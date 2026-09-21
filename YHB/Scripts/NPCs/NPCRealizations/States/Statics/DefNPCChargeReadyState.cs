using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using KHG.Utilities.RandomSystem;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	// Dynamic State처럼 취급되나 실제 로직은 Static State와 다름 없음.
	public class DefNPCChargeReadyState : DefNPCPriorityState
	{
		//private Vector3 _destination;
		private NPCDynamicData_Move _moveDynamicData;
		private NPCDynamicData_AnimationInfo _animationDynamicData;

		private bool _creepAnim = false;
		private bool _restAnim = false;

		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.D_ChargeReady;

		public DefNPCChargeReadyState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_moveDynamicData = _dataComp.GetDataSet<NPCDynamicData_Move>();
			_animationDynamicData = _dataComp.GetDataSet<NPCDynamicData_AnimationInfo>();

			_moveDynamicData.isRunToChargeReady = false;
		}

		public override void Enter()
		{
			base.Enter();

			_navMovement.SetStop(true);
			_navMovement.SetLockRotation(true);

			if (_moveDynamicData.isRunToChargeReady)
			{
				_moveDynamicData.isRunToChargeReady = false;

				// 여기서 자동으로 CREEP상태로 넘어가지 않음.
				_creepAnim = DeterministicRandomManager.Instance.Get(GetRandKey())
					.Get(DeterministicRandomManager.Instance.GetOffsetIndex(GetRandKey()));
				NPCAnimationEnum animation = _creepAnim ? NPCAnimationEnum.STAND_TO_CREEP : NPCAnimationEnum.CROUCH;

				if (!_creepAnim)
					_dataComp.GetDataSet<NPCDynamicData_AnimationInfo>().chargeReadyAnim = animation;


				SetChargeDestination();
				_npcAnimator.ChangeAnimation(_npc.GetAnimationHash(animation));

				// trigger call호출보다 이게 더 이득임. (스테이트 전황을 안 함)
				//_animatorTrigger.OnAnimationEndTrigger += HandleAnimationEnd;
				// 정정. 애니메이션 재생 중 로테이션 필요함.

				return;
			}
			else
			{
				_restAnim = _animationDynamicData.chargeReadyAnim == NPCAnimationEnum.REST;

				if (_restAnim)
					_npcAnimator.ChangeAnimation(_npc.GetAnimationHash(NPCAnimationEnum.REST_TO_STAND));
				else
					Charging();

				return;
			}
		}

		private void Charging()
		{
			_moveDynamicData.isRunToChargeReady = true;

			// 목표 설정
			// navmesh는 비동기.
			//_navMovement.SetDestination(_npc.transform.position);
			SetChargeDestination();

			_npc.ChangeBehaviour(NPCBehaviourEnum.S_Run, false);
		}

		private void SetChargeDestination()
		{
			Vector3 destination = _dataComp.GetDataSet<NPCStaticData_Point>()
				.GetChargeDestination(_npc.transform.position);
			_dataComp.GetDataSet<NPCDynamicData_Move>().destination = destination;
		}

		public override void Update()
		{
			if (_isTriggerCall)
			{
				_isTriggerCall = false;

				if (_creepAnim)
				{
					_dataComp.GetDataSet<NPCDynamicData_AnimationInfo>().chargeReadyAnim = NPCAnimationEnum.CREEP;
					_npcAnimator.ChangeAnimation(_npc.GetAnimationHash(NPCAnimationEnum.CREEP));
				}
				else if (_restAnim)
					Charging();
			}
		}

		public override void Exit()
		{
			//if (!_dataComp.GetDataSet<NPCStaticData_Move>().isChargeReady)
			//{
			//	// 돌격 상태가 되었으니 연결된 메쉬를 지날 수 있게 처리
			//	// '돌격'은 다이나믹적으로 변환됨
			//}
			_navMovement.SetStop(false);
			_navMovement.SetLockRotation(false);

			base.Exit();
		}
	}
}
