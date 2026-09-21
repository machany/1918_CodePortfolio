using System;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets
{
	// animation 관련 데이터 저장
	[Serializable]
	public sealed class NPCDynamicData_AnimationInfo : INPCDataSets
	{
		public NPCAnimationEnum chargeReadyAnim;
	}
}
