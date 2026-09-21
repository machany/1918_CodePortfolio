using AgamaLibrary.Unity.DataStructures;
using Alchemy.Inspector;
using Assets._01_Work.YHB.Scripts.NPCs;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Situations
{
	[CreateAssetMenu(fileName = "NPCSituationToBehaviourSO", menuName = "SO/NPC/situationToBehaviour", order = 0)]
	public class SituationToBehaviourSO : ScriptableObject
	{
		public SerializedDictionary<Situation, NPCBehaviourEnum> situationToBehaviour;

		public NPCBehaviourEnum Get(Situation situation)
		{
			if (situationToBehaviour.GetDict().TryGetValue(situation, out NPCBehaviourEnum behaviour))
				return behaviour;
			return NPCBehaviourEnum.None;
		}

#if UNITY_EDITOR
		[Button]
		private void Situation_To_Behaviour_Distinct()
		{
			situationToBehaviour.Distinct();
		}
#endif
	}
}
