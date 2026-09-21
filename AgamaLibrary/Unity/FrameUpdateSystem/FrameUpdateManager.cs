using AgamaLibrary.Unity.MonoBehaviourSingletons;
using System.Collections.Generic;
using UnityEngine;

namespace AgamaLibrary.Unity.FrameUpdateSystem
{
	// Mono의 업데이트 연산처리를 줄여 최적화하기 위함.
	public sealed class FrameUpdateManager : MonoBehaviourSingleton<FrameUpdateManager>
	{
		protected override bool IsDestroyOnLoad => true;

		// 값 복사 줄이기위해 struct말고 class사용
		private class FrameUpdateObjectData
		{
			public IFrameUpdateable frameUpdateable;
			public FrameUpdateData updateData;
		}

		private PriorityQueue<FrameUpdateObjectData, float> _frameUpdateables;
		private HashSet<IFrameUpdateable> _archivedStand;
		private HashSet<IFrameUpdateable> _removeStand;

		protected override void Internally_Initialize()
		{
			_frameUpdateables = new PriorityQueue<FrameUpdateObjectData, float>();
			_archivedStand = new HashSet<IFrameUpdateable>();
			_removeStand = new HashSet<IFrameUpdateable>();
		}

		private float CalculateDelay(byte frame)
		{
			float delay = 1f / frame;
			return delay;
		}

		private void Update()
		{
			UpdateFrameUpdateables();
		}

		private void UpdateFrameUpdateables()
		{
			while (true)
			{
				if (_frameUpdateables.Count <= 0)
					return;

				FrameUpdateObjectData data = _frameUpdateables.Peek();

				if (data.updateData.nextUpdateTime > Time.time)
					break;

				_frameUpdateables.Dequeue();
				bool continueFlag = true;

				if (_removeStand.Contains(data.frameUpdateable)
					|| data.frameUpdateable == null)
				{
					_removeStand.Remove(data.frameUpdateable);
				}
				else if (data.frameUpdateable.UpdateFrame <= 0)
				{
					data.frameUpdateable.SetEnable(false);
					RemoveFrameUpdateable(data.frameUpdateable);
				}
				else
					continueFlag = false;

				if (continueFlag)
				{
					_archivedStand.Remove(data.frameUpdateable);
					continue;
				}

				data.frameUpdateable.FrameUpdate(Time.time - data.updateData.lastUpdateTime);

				data.updateData.lastUpdateTime = Time.time;
				data.updateData.nextUpdateTime = Time.time + CalculateDelay(data.frameUpdateable.UpdateFrame);

				_frameUpdateables.Enqueue(data, data.updateData.nextUpdateTime);
			}
		}

		public void AddFrameUpdateable(IFrameUpdateable frameUpdateable, bool immediateUpdate = false)
		{
			if (!_archivedStand.Add(frameUpdateable))
				return;
			else if (frameUpdateable.UpdateFrame <= 0)
				return;

			FrameUpdateObjectData data = new FrameUpdateObjectData();
			data.frameUpdateable = frameUpdateable;
			data.updateData.lastUpdateTime = Time.time;
			if (immediateUpdate)
				data.updateData.nextUpdateTime = 0f;
			else
				data.updateData.nextUpdateTime = Time.time + CalculateDelay(frameUpdateable.UpdateFrame);

			_frameUpdateables.Enqueue(data, data.updateData.nextUpdateTime);
		}

		public void RemoveFrameUpdateable(IFrameUpdateable frameUpdateable)
		{
			_removeStand.Add(frameUpdateable);
		}
	}
}
