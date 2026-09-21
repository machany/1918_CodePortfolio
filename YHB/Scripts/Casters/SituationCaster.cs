using Assets._01_Work.YHB.Scripts.Situations;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Casters
{
	public class SituationCaster : OverlapCaster
	{
		[SerializeField] private Situation situation;

		public override void CastOverlaps(int count)
		{
			if(count <= 0) return;
			for (int i = 0; i < count; i++)
			{
				if (_colliders[i].TryGetComponent<ISituationable>(out ISituationable situationable))
				{
					situationable.InvokeSituation(situation, transform.position);
				}
			}
		}

        public void CastO()
        {
			Cast();
        }
    }
}
