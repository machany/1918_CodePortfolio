using _01_Work.LCM._01.Scripts.Entities;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Statics
{
	public class DefNPCHowlingState : DefNPCPriorityState
	{
		protected override NPCBehaviourEnum Behaviour => NPCBehaviourEnum.S_Howling;

		public DefNPCHowlingState(Entity entity, int animationHash) : base(entity, animationHash)
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
