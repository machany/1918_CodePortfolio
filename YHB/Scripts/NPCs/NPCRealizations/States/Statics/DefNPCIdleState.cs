using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	public class DefNPCIdleState : DefNPCPriorityState
	{
		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.S_Idle;
		private NPCStaticData_AnimationInfo _animationStaticData;
		private float _leaveTime;

		public DefNPCIdleState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_animationStaticData = _dataComp.GetDataSet<NPCStaticData_AnimationInfo>();
		}

		public override void Enter()
		{
			base.Enter();

			_navMovement.SetStop(true);
			_animationStaticData.lookOtherObject.SetActive(true);
			_leaveTime = Time.time + _moveStaticData.GetIdleTime(GetRandKey());
		}

		public override void Update()
		{
			if (_leaveTime < Time.time)
				// 이전 상태로 되돌리는 거라 굳이 저장할 필요가 없을 듯
				_npc.ChangeBehaviour(_npc.GetCurrentBehaviour(), false);
		}

		public override void Exit()
		{
			_animationStaticData.lookOtherObject.SetActive(false);
			_navMovement.SetStop(false);

			base.Exit();
		}
	}
}
