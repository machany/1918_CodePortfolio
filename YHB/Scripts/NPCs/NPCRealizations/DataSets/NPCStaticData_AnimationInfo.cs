using System;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets
{
	[Serializable]
	public sealed class NPCStaticData_AnimationInfo : INPCDataSets
	{
		public GameObject lookOtherObject;
		public bool canChangeTalkAnim;
		public int canTalkFrame;
		public float talkAnimDuration;
		public float talkAnimDurationRange;
	}
}
