using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.Situations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCSurpriseAnimationController : MonoBehaviour, IEntityComponent
	{
		[SerializeField] private Situation[] surpriseableSituationList;
		private HashSet<Situation> _surpriseableSituationSet;

		[SerializeField] private NPCBehaviourSetSO surpriseBehaviourSetSO;

		private DefNPC _defNpc;

		public UnityEvent<Vector3> OnSurpriseEvent;

		public void Initialize(Entity entity)
		{
			_defNpc = entity as DefNPC;

			_surpriseableSituationSet = new HashSet<Situation>(surpriseableSituationList);
		}

		public void HandleSituation(Situation situation)
		{
			// 놀랄 수 있다면
			if (_surpriseableSituationSet.Contains(situation)
				&& surpriseBehaviourSetSO.Contains(_defNpc.CurrentState))
			{
				// 놀라기
				OnSurpriseEvent?.Invoke(_defNpc.InvokedSituationPosition);
			}
		}
	}
}
