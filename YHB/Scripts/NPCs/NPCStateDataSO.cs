using System.Collections.Generic;
using _01_Work.LCM._01.Scripts.FSM;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs
{
	[CreateAssetMenu(fileName = "StateData", menuName = "SO/FSM/NPCStateData", order = 0)]
	public class NPCStateDataSO : StateDataSO
	{
		public NPCBehaviourEnum stateEnumType;
		public override int stateEnumTypeValue => (int)stateEnumType;

		public class EqualityNPCStateDataComparer : EqualityComparer<NPCStateDataSO>
		{
			public override bool Equals(NPCStateDataSO x, NPCStateDataSO y)
			{
				return x.stateEnumType == y.stateEnumType;
			}

			public override int GetHashCode(NPCStateDataSO obj)
			{
				return obj.stateEnumType.GetHashCode();
			}
		}
	}
}
