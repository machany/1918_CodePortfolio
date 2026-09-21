using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	// Dynamic State처럼 취급되나 실제 로직은 Static State와 다름 없음.
	public class DefNPCRestState : DefNPCPriorityState
	{
		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.D_Rest;

		private NPCStaticData_AnimationInfo _animationStaticData;
		private NPCDynamicData_AnimationInfo _animationDynamicData;

		public DefNPCRestState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_animationStaticData = _dataComp.GetDataSet<NPCStaticData_AnimationInfo>();
			_animationDynamicData = _dataComp.GetDataSet<NPCDynamicData_AnimationInfo>();
		}

		public override void Enter()
		{
			base.Enter();

			_navMovement.SetStop(true);
			_navMovement.SetLockRotation(true);
			_animationStaticData.lookOtherObject.SetActive(true);

			_animationDynamicData.chargeReadyAnim = NPCAnimationEnum.REST;
		}

		public override void Exit()
		{
			_animationStaticData.lookOtherObject.SetActive(false);
			_navMovement.SetStop(false);
			_navMovement.SetLockRotation(false);

			base.Exit();
		}
	}
}
