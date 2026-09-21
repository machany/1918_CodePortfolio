using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States
{
	public class DefNPCChargeState : DefNPCState
	{
		private bool _changeAnimationFlag;

		public DefNPCChargeState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}

		public override void Enter()
		{
			base.Enter();

			// charge ready에서 charge로 넘어왔다는 건 isOnOffMeshLink가 보장된 상황임.
			// chage되는 참호를 올라가고 (애니마션 출력) (RootMotion 등 trm처리), link끝점으로 이동
			// OffMeshLinkData linkData = _navMovement.agent.currentOffMeshLinkData;
			// Vector3 linkEnd = linkData.endPos;
			// linkEnd쪽으로 이동되게 애니메이션 처리 // 오타 고치지않았나

			// link끝점으로 이동
			// _navMovement.WarpToPosition(linkEnd);
			// _navMovement.agent.CompleteOffMeshLink();

			// link연결 지점을 지나지 않음.
			// _navMovement.agent.autoTraverseOffMeshLink = false;

			SetChargeDestination();

			// 현재 CREEP animation이면
			if (_dataComp.GetDataSet<NPCDynamicData_AnimationInfo>().chargeReadyAnim == NPCAnimationEnum.CREEP)
			{
				_npcAnimator.SetParam(_npc.GetAnimationHash(NPCAnimationEnum.CREEP_TO_STAND));
				_changeAnimationFlag = false;
			}
			else
				_changeAnimationFlag = true;
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
				_changeAnimationFlag = true;

			if (_changeAnimationFlag)
			{
				// 돌격 상태가 되었으니 연결된 메쉬를 지날 수 있게 처리
				// _navMovement.agent.autoTraverseOffMeshLink = true;
				NPCDynamicData_Move data = _dataComp.GetDataSet<NPCDynamicData_Move>();
				_navMovement.LookAtTarget(data.destination, false);
				_npc.ChangeBehaviour(NPCBehaviourEnum.S_Sprint, false);
				return;
			}
		}
	}
}
