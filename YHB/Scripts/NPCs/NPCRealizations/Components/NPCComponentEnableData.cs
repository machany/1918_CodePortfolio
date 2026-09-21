using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCComponentEnableData : MonoBehaviour, IComparable<NPCComponentEnableData>
	{
		public int enableFrame;
		public int enableChangeThresholds;

		public bool enable = true;

		public UnityEvent<bool> OnEnable;

		public int CompareTo(NPCComponentEnableData other)
		{
			if (other == null)
				return 1;

			return this.enableFrame.CompareTo(other.enableFrame);
		}
	}
}
