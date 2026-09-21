using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using KHG.Utilities.RandomSystem;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.States
{
	public class DefNPCPatrolState : DefNPCState
	{
		public DefNPCPatrolState(Entity entity, int animationHash) : base(entity, animationHash)
		{
		}

		public override void Enter()
		{
			base.Enter();

			Vector3 destination = _dataComp.GetDataSet<NPCStaticData_Point>().GetDugoutPoints(GetRandKey());
			_dataComp.GetDataSet<NPCDynamicData_Move>().destination = destination;

			float runRandValue = DeterministicRandomManager.Instance.Get(GetRandKey())
				.Get(0f, .1f);

			NPCBehaviourEnum nextState
				= runRandValue <= _dataComp.GetDataSet<NPCStaticData_Move>().RunFrequencyDuringPatrol
				? NPCBehaviourEnum.S_Run : NPCBehaviourEnum.S_Walk;

			_npc.ChangeBehaviour(nextState, false);
		}
	}
}
