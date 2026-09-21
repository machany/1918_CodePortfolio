using _01_Work.LCM._01.Scripts.Entities;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	public class DefNPCSprintState : DefNPCWalkState
	{
		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.S_Run;
		protected override NPCBehaviourEnum ArrivedBehaviour => NPCBehaviourEnum.S_Idle;
		protected override bool LookOtherTransform => false;

		public DefNPCSprintState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_navMovement.SetSpeed(_moveStaticData.RunSpeed);
			_moveDynamicData.isRun = true;
		}

		public override void Exit()
		{
			_moveDynamicData.isRun = false;

			base.Exit();
		}
	}
}
