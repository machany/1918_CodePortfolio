using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets._01_Work.YHB.Scripts.NPCs.NPCRealizations.Components
{
	public class SurpriseRigController : MonoBehaviour
	{
		[SerializeField] private Rig rig;
		[SerializeField] private float surpriseTime;

		private void Awake()
		{
			rig.weight = .0f;
		}

		public void ApplySurpriseAnimation()
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(DOTween.To(() => rig.weight, x => rig.weight = x, 1f, surpriseTime));
			seq.Append(DOTween.To(() => rig.weight, x => rig.weight = x, 0f, surpriseTime));
		}
	}
}
