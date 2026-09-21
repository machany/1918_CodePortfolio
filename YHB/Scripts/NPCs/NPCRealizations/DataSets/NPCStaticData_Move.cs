using AgamaLibrary.Unity.DataStructures;
using Alchemy.Inspector;
using KHG.Utilities.RandomSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets
{
	[Serializable]
	public sealed class NPCStaticData_Move : INPCDataSets
	{
		[SerializeField] private float idleTime = 5f;
		[SerializeField] private float idleTimeDeviation = 1f;

		[Range(0f, .1f)]
		[field: SerializeField] public float RunFrequencyDuringPatrol { get; private set; } = 0.001f;
		[field: SerializeField] public float AccelerationToRunSpeedTime { get; private set; } = 1f;
		[field: SerializeField] public float WalkSpeed { get; private set; } = 1f;
		[field: SerializeField] public float RunSpeed { get; private set; } = 5f;

		[SerializeField] public SerializedDictionary<NPCBehaviourEnum, BehaviourPrioritySet> priorityValue;

		public float GetIdleTime(string key)
		{
			return DeterministicRandomManager.Instance.Get(key)
				.Get(idleTime - idleTimeDeviation, idleTime + idleTimeDeviation);
		}
	}

	[Serializable]
	public struct BehaviourPrioritySet
	{
		public int priority;
		public int priorityRange;
	}
}
