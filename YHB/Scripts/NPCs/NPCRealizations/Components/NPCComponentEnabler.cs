using _01_Work.LCM._01.Scripts.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class NPCComponentEnabler : MonoBehaviour, IEntityComponent, IAfterInitialize
	{
		[SerializeField] private List<NPCComponentEnableData> enableDataList;

		private NPC _npc;
		private LinkedList<(NPCComponentEnableData data, int priority)> _componentList;
		private PriorityQueue<NPCComponentEnableData, int> _tempQueue;
		private int _beforeFrame;

		public void Initialize(Entity entity)
		{
			_tempQueue = new PriorityQueue<NPCComponentEnableData, int>();
			_componentList = new LinkedList<(NPCComponentEnableData data, int priority)>();

			foreach (var item in enableDataList)
			{
				_componentList.AddLast((item, item.enableFrame));
				item.OnEnable?.Invoke(item.enable);
			}

			_npc = entity as NPC;
			_beforeFrame = _npc.UpdateFrame;
		}

		public void AfterInitialize()
		{
			if (_componentList.Count <= 0)
				return;

			_npc.OnUpdateFrameChanged += HandleUpdateFrameChanged;
			_npc.OnDestroying += OnDestroying;
		}

		private void HandleUpdateFrameChanged(int frame)
		{
			if (_componentList.Count <= 0)
				return;

			_tempQueue.Clear();
			int beforePriority;

			// frame증가 enable true 되는지 check
			if (_beforeFrame < frame)
			{
				beforePriority = _componentList.First.Value.priority;

				while (_componentList.Count > 0
				&& beforePriority > frame)
				{
					beforePriority = _componentList.First.Value.priority;
					var item = _componentList.First.Value.data;

					_componentList.RemoveFirst();

					bool enable = true;
					item.OnEnable?.Invoke(enable);

					int priority = beforePriority + (enable ? -item.enableChangeThresholds : item.enableChangeThresholds);
					_tempQueue.Enqueue(item, priority);
				}
			}
			// frame감소 enable false check
			else
			{
				beforePriority = _componentList.Last.Value.priority;

				while (_componentList.Count > 0
				&& beforePriority < frame)
				{
					beforePriority = _componentList.Last.Value.priority;
					var item = _componentList.Last.Value.data;

					_componentList.RemoveLast();

					bool enable = false;
					item.OnEnable?.Invoke(enable);

					int priority = beforePriority + (enable ? -item.enableChangeThresholds : item.enableChangeThresholds);
					_tempQueue.Enqueue(item, priority);
				}
			}

			_beforeFrame = frame;

			while (_tempQueue.Count > 0)
			{
				int priority = _tempQueue.PeekPriority();
				var data = _tempQueue.Dequeue();
				_componentList.AddLast((data, priority));
			}

		}

		private void OnDestroying()
		{
			_npc.OnDestroying -= OnDestroying;
			_npc.OnUpdateFrameChanged -= HandleUpdateFrameChanged;
		}
	}
}
