using _01_Work.LCM._01.Scripts.Entities;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Dynamics
{
	// Dynamic State처럼 취급되나 실제 로직은 Static State와 다름 없음.
	public class DefNPCPanicState : DefNPCPriorityState
	{
		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.D_Panic;

		public DefNPCPanicState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_navMovement.SetStop(true);
			_navMovement.SetLockRotation(true);
		}

		public override void Exit()
		{
			_navMovement.SetStop(false);
			_navMovement.SetLockRotation(false);

			base.Exit();
		}
	}
}
