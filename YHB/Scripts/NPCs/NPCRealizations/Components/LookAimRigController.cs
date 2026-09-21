using _01_Work.LCM._01.Scripts.Entities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class LookAimRigController : MonoBehaviour, IEntityComponent
	{
		[SerializeField] private Rig rig;
		[SerializeField] private Transform rigTarget;
		[SerializeField] private Vector3 offsetTargetLook;
		[SerializeField] private float lookDuration;
		[SerializeField] private float lookTime;

		private Transform _target;
		private Vector3 _targetPosition;
		private float _lookEndTime;

		private bool _isLooking;

		private NPC _npc;

		public void Initialize(Entity entity)
		{
			_npc = entity as NPC;
			_npc.OnUpdate += UpdateCore;
		}

		private void UpdateCore()
		{
			if (_lookEndTime < Time.time)
			{
				DeActiveRig();
				// 중복 호출 장지
				_lookEndTime = float.MaxValue;
				return;
			}

			if (_isLooking)
			{
				if (_target != null)
					_targetPosition = _target.position + offsetTargetLook;
				rigTarget.position = _targetPosition;
			}
		}

		public void SetLookPosition(Vector3 position)
		{
			_targetPosition = position;
			_target = null;
		}

		public void SetLookTarget(Transform trm)
		{
			_target = trm;
		}

		public void ActiveRig()
		{
			float duration = lookDuration * (1 - rig.weight);
			_isLooking = false;
			Sequence seq = DOTween.Sequence();
			seq.Append(DOTween.To(() => rig.weight, x => rig.weight = x, 1f, duration));
			seq.AppendCallback(() => _isLooking = true);

			_lookEndTime = Time.time + lookTime;
		}

		public void DeActiveRig()
		{
			float duration = lookDuration * rig.weight;
			_isLooking = false;
			Sequence seq = DOTween.Sequence();
			seq.Append(DOTween.To(() => rig.weight, x => rig.weight = x, 0f, duration));

			_lookEndTime = Time.time;
		}

		private void OnDestroy()
		{
			_npc.OnUpdate -= UpdateCore;
		}
	}
}
