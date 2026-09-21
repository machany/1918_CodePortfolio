using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.DataSets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCDatasetPresetComponent : MonoBehaviour, IEntityComponent
	{
		[SerializeReference] public INPCDataSets[] dataSets;

		private Dictionary<Type, INPCDataSets> _dataSetDict;

		public void Initialize(Entity entity)
		{
			_dataSetDict = new Dictionary<Type, INPCDataSets>();

			foreach (var item in dataSets)
			{
				if (entity is DefNPC defNPC)
					item.Initialize(defNPC.randomKey);
				_dataSetDict.TryAdd(item.GetType(), item);
			}
		}

		public T GetDataSet<T>() where T : INPCDataSets, new()
		{
			if (!_dataSetDict.ContainsKey(typeof(T)))
			{
				_dataSetDict[typeof(T)] = new T();
				Debug.Log($"Create new {typeof(T).Name}");
			}
			return (T)_dataSetDict[typeof(T)];
		}

		private void OnDrawGizmosSelected()
		{
            foreach (var item in dataSets)
				if (item is not null)
					item.DrawGizmos();
		}
	}
}
