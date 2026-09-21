using _01_Work.LCM._01.Scripts.Entities;
using Assets._01_Work.YHB.Scripts.Casters;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class LookNearOtherEntity : MonoBehaviour, IEntityComponent
	{
		[SerializeField] private float castingTimeRange;
		[SerializeField] private OverlapCaster otherEntityCaster;

		private NPC _npc;
		private LookAimRigController _lookAimController;

		private float _nextCastingTime;

		public void Initialize(Entity entity)
		{
			_lookAimController = entity.GetComp<LookAimRigController>();
			this.enabled = false;
			_nextCastingTime = 0;

			_npc = entity as NPC;
			_npc.OnUpdate += UpdateCore;
		}

		private void OnDestroy()
		{
			_npc.OnUpdate -= UpdateCore;
		}

		private void UpdateCore()
		{
			if (_nextCastingTime < Time.time)
			{
				_nextCastingTime = Time.time + castingTimeRange;
				bool success = otherEntityCaster.Check(out int cnt, out var colliders);

				if (success)
				{
					for (int i = 0; i < cnt; i++)
						if (colliders[i].transform != _npc.transform)
						{
							_lookAimController.SetLookTarget(colliders[i].transform);
							_lookAimController.ActiveRig();
						}
				}
				else
				{
							_lookAimController.SetLookTarget(_npc.transform);
					_lookAimController.DeActiveRig();
				}
			}
		}
	}
}
