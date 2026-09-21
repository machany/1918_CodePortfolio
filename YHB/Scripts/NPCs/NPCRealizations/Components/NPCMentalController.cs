using _01_Work.LCM._01.Scripts.Entities;
using AgamaLibrary.Unity.DataStructures;
using Assets._01_Work.YHB.Scripts.Situations;
using KHG.Utilities.RandomSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCMentalController : MonoBehaviour, IEntityComponent
	{
		[SerializeField] private float maxMP;
		[SerializeField] private float maxMPRdomRange;
		[SerializeField] private float mpDamageTolerance;
		[SerializeField] private SerializedDictionary<Situation, float> mpDamageFromSituation;

		[SerializeField] private NPCBehaviourSetSO panicableBehaviourSetSO;

		private float _currentMP;

		private DefNPC _defNPC;

		public void Initialize(Entity entity)
		{
			_defNPC = entity as DefNPC;
			_currentMP = maxMP;
		}

		private void Start()
		{
			maxMP += DeterministicRandomManager.Instance.Get(_defNPC.randomKey).Get(-maxMPRdomRange, maxMPRdomRange);
		}

		public void HandleSituationEvent(Situation situation)
		{
			if (mpDamageFromSituation.GetDict().TryGetValue(situation, out float damage))
			{
				damage = Mathf.Max(
					damage - DeterministicRandomManager.Instance.Get(_defNPC.randomKey).Get(-mpDamageTolerance, mpDamageTolerance)
					, 0);

				if ((_currentMP -= damage) <= 0f)
					if (panicableBehaviourSetSO.Contains(_defNPC.GetCurrentBehaviour()))
						_defNPC.ChangeBehaviour(NPCBehaviourEnum.D_Panic);
			}
		}
	}
}
