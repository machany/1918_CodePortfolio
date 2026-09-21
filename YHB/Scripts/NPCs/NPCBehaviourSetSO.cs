using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs
{
	[CreateAssetMenu(fileName = "NPC_BS_", menuName = "SO/NPC/BehaviourSet", order = 0)]
	public class NPCBehaviourSetSO : ScriptableObject
	{
		[SerializeField] private NPCBehaviourEnum[] npcBehaviourEnums;
		private HashSet<NPCBehaviourEnum> _npcBehaviourSet;

		private void OnEnable()
		{
			_npcBehaviourSet = new HashSet<NPCBehaviourEnum>(npcBehaviourEnums);
		}

		public HashSet<NPCBehaviourEnum> Get()
			=> _npcBehaviourSet;

		public bool Contains(NPCBehaviourEnum behaviour)
			=> _npcBehaviourSet.Contains(behaviour);
	}
}
