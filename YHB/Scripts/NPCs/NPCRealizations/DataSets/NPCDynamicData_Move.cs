using System;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets
{
	// dynamic한 이동 값만 저장함.
	[Serializable]
	public sealed class NPCDynamicData_Move : INPCDataSets
	{
		public Vector3 destination;

		public bool isRun;

		public bool isOnChargeStage = false;
		public bool isRunToChargeReady = false;
	}
}
