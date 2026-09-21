using AgamaLibrary.Unity.FrameUpdateSystem;
using AgamaLibrary.Unity.MonoBehaviourSingletons;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Core.LODUpdate
{
	public sealed class LODUpdateManager : MonoBehaviourSingleton<LODUpdateManager>
	{
		protected override bool IsDestroyOnLoad => true;

		// 해당 오브젝트를 기준으로 LOD가 적용됨.
		[SerializeField] private GameObject _lodPivot;
		private HashSet<ILODUpdateable> _lodUpdateableList;

		[Range(1, 255)]
		[SerializeField] private byte minFrame = 1;
		[Range(1, 255)]
		[SerializeField] private byte maxFrame = 80;
		[Tooltip("해당 거리부터 프레임이 감소되기 시작합니다.")]
		[SerializeField] private float frameDecreaseThreshold = 2f;
		[Tooltip("해당 수치마다 거리에 따라 프레임이 1씩 감소합니다.")]
		[SerializeField] private float frameAccordingToDistance = 0.8f;

		protected override void Internally_Initialize()
		{
			_lodUpdateableList = new HashSet<ILODUpdateable>();

			Debug.Assert(frameDecreaseThreshold > 0
				|| frameAccordingToDistance > 0, "[LODUpdateManager] frame setting value error. errors : " +
				$"{(frameDecreaseThreshold <= 0 ? "frameDecreaseThreshold, " : "")}{(frameAccordingToDistance <= 0 ? "frameAccordingToDistance, " : "")}");
		}

		// LOD 중심 설정
		public void SetLODPivot(GameObject lodPivot)
		{
			_lodPivot = lodPivot;
			Debug.Log("lod pivot is changed");
		}

		/// <summary>
		/// 제거의 책임은 이 Manager가 가짐.
		/// </summary>
		/// <remarks>
		/// 다만, 사망시 제거 등은 등록한 오브젝트에서 제거해야함.
		/// </remarks>
		public void Register(ILODUpdateable lodUpdateable, bool immediateUpdate)
		{
			_lodUpdateableList.Add(lodUpdateable);
			FrameUpdateManager.Instance.AddFrameUpdateable(lodUpdateable, immediateUpdate);
		}

		public void Deregister(ILODUpdateable lodUpdateable)
		{
			_lodUpdateableList.Remove(lodUpdateable);
			FrameUpdateManager.Instance?.RemoveFrameUpdateable(lodUpdateable);
		}

		private void Update()
		{
			if (_lodPivot == null)
			{
				return;
			}

			ApplyLODUpdateFrame(_lodPivot.transform.position);
		}

		private void ApplyLODUpdateFrame(Vector3 pivotPosition)
		{
			foreach (var lodUpdateable in _lodUpdateableList)
			{
				if (lodUpdateable == null
					|| lodUpdateable.GameObject == null)
					continue;

				float distance = Vector3.Distance(pivotPosition, lodUpdateable.GameObject.transform.position);
				lodUpdateable.UpdateFrame = CalculateFrame(distance);
			}
		}

		private byte CalculateFrame(float distance)
		{
			distance -= frameDecreaseThreshold;

			if (distance < 0)
				return maxFrame;

			float frameToReduce = distance / frameAccordingToDistance;
			float frame = Mathf.Max(minFrame, maxFrame - frameToReduce);
			return (byte)frame;
		}

		private void OnValidate()
		{
			if (minFrame > maxFrame)
				maxFrame = minFrame;

			if (frameDecreaseThreshold < 0)
				frameDecreaseThreshold = 0;

			if (frameAccordingToDistance < 0)
				frameAccordingToDistance = 0;
		}
	}
}
