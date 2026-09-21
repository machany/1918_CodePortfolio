using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using UnityEngine;
using UnityEngine.AI;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	public class DefNPCWalkState : DefNPCPriorityState
	{
		protected NPCDynamicData_Move _moveDynamicData;
		protected NPCStaticData_AnimationInfo _animationStaticData;

		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.S_Walk;
		protected virtual NPCBehaviourEnum ArrivedBehaviour => NPCBehaviourEnum.S_Idle;
		protected virtual bool LookOtherTransform => true;

		public DefNPCWalkState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_moveDynamicData = _dataComp.GetDataSet<NPCDynamicData_Move>();
			_animationStaticData = _dataComp.GetDataSet<NPCStaticData_AnimationInfo>();
		}

		public override void Enter()
		{
			base.Enter();

			_navMovement.SetDestination(_moveDynamicData.destination);
			_navMovement.SetSpeed(_moveStaticData.WalkSpeed);

			if (LookOtherTransform)
			_animationStaticData.lookOtherObject.SetActive(true);
		}

		public override void Update()
		{
			// 길을 이미 찾았는데 길이 유효하지 않으면
			if (!_navMovement.agent.pathPending
				&& _navMovement.agent.pathStatus == NavMeshPathStatus.PathInvalid)
			{
				Debug.LogError($"[{_npc.gameObject.name}] NavAgent couldn't find path. so, change behaviour to before.");
				_npc.ChangeBehaviour(_npc.GetCurrentBehaviour(), false);
				return;
			}

			// Vector3 agentDirection = _navMovement.agent.velocity.normalized;
			// 굳이? navMovement로 인해 어짜피 진행 방향을 바라봄
			//_npcAnimator.SetParam(_npc.GetAnimationHash(NPCAnimationEnum.XMOVE), agentDirection.x);
			//_npcAnimator.SetParam(_npc.GetAnimationHash(NPCAnimationEnum.ZMOVE), agentDirection.y);

			if (_navMovement.IsArrived)
			{
				_npc.ChangeBehaviour(ArrivedBehaviour, false);
				return;
			}
		}

		public override void Exit()
		{
			_navMovement.SetDestination(_npc.transform.position);

			if (LookOtherTransform)
				_animationStaticData.lookOtherObject.SetActive(false);

			//_npcAnimator.SetParam(_npc.GetAnimationHash(NPCAnimationEnum.XMOVE), .0f);
			//_npcAnimator.SetParam(_npc.GetAnimationHash(NPCAnimationEnum.ZMOVE), .0f);

			base.Exit();
		}
	}
}
