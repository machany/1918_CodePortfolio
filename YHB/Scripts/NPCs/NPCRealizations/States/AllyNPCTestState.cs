using _01_Work.KJY.Code.NpcCodes;
using _01_Work.LCM._01.Scripts.Entities;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States
{
	public class AllyNPCTestState : NPCState
	{
		DefNPC allyNpc;

		public AllyNPCTestState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			allyNpc = _npc as DefNPC;
		}

		public override void Enter()
		{
			base.Enter();

			var navComp = _npc.GetCompo<NavMovement>();
			navComp.SetStop(false);
			//navComp.SetDestination(allyNpc.target.position);
		}

		public override void Update()
		{
			var navComp = _npc.GetCompo<NavMovement>();
			//navComp.SetDestination(allyNpc.target.position);
		}

		public override void Exit()
		{
			var navComp = _npc.GetCompo<NavMovement>();
			navComp.SetDestination(_npc.transform.position);
			navComp.SetStop(true);

			base.Exit();
		}
	}
}
