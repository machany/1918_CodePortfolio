using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using System;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	// navmesh는 놀랍게도 link를 사용하면 점유함.
	public class NPCCheckChargeStage : MonoBehaviour, IEntityComponent
	{
		[SerializeField] private LayerMask chargeStateLayer;

		private NPCDatasetPresetComponent _dataComp;

		public void Initialize(Entity entity)
		{
			_dataComp = entity.GetComp<NPCDatasetPresetComponent>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (IsEqualsLayer(chargeStateLayer, other.gameObject.layer))
				_dataComp.GetDataSet<NPCDynamicData_Move>().isOnChargeStage = true;
		}

		private void OnTriggerExit(Collider other)
		{
			if (IsEqualsLayer(chargeStateLayer, other.gameObject.layer))
				_dataComp.GetDataSet<NPCDynamicData_Move>().isOnChargeStage = false;
		}

		private bool IsEqualsLayer(LayerMask layerMask, int gameobjectLayer)
			=> (layerMask & (1 << gameobjectLayer)) != 0;
	}
}
