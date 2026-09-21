using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States.Dynamics
{
	public class DefNPCHowlState : DefNPCState
	{
		private NPCDynamicData_Move _dynamicData;

		public DefNPCHowlState(Entity entity, int animationHash) : base(entity, animationHash)
		{
			_dynamicData = _dataComp.GetDataSet<NPCDynamicData_Move>();
		}

		public override void Enter()
		{
			base.Enter();

			_dynamicData.destination = _defNPC.InvokedSituationPosition;
			_defNPC.ChangeBehaviour(NPCBehaviourEnum.S_Sprint, false);
		}
	}
}
