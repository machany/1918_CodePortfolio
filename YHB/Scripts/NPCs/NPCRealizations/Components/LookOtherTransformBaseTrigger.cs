using _01_Work.LCM._01.Scripts.Entities;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class LookOtherTransformBaseTrigger : MonoBehaviour, IEntityComponent
	{
		private LookAimRigController _lookAimController;
		private Transform _lastLookTarget;

		public void Initialize(Entity entity)
		{
			_lookAimController = entity.GetComp<LookAimRigController>();
			gameObject.SetActive(false);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (_lastLookTarget != null)
				return;

			_lastLookTarget = other.transform;
			_lookAimController.SetLookTarget(_lastLookTarget);
			_lookAimController.ActiveRig();
		}

		private void OnTriggerExit(Collider other)
		{
			if (_lastLookTarget == null)
				return;

			_lastLookTarget = null;
			_lookAimController.SetLookTarget(null);
			_lookAimController.DeActiveRig();
		}
	}
}
