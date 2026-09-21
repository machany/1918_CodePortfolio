using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.Core;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCCollisionableComponent : MonoBehaviour, IEntityComponent, IAfterInitialize, ICollisionable
	{
		public bool IsRun => _moveDynamicData.isRun;

		private NPC _npc;
		private NPCDynamicData_Move _moveDynamicData;

		public void Initialize(Entity entity)
		{
			_npc = entity as NPC;
		}

		public void AfterInitialize()
		{
			_moveDynamicData = _npc.GetComp<NPCDatasetPresetComponent>().GetDataSet<NPCDynamicData_Move>();
		}
	}
}
