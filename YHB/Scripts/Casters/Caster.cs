using Alchemy.Inspector;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Casters
{
	public abstract class Caster : MonoBehaviour
	{
		public abstract bool Cast();

#if UNITY_EDITOR
		[Button]
		private void TestCast()
		{
			bool success = Cast();
			Debug.Log($"Cast success : {success}");
		}
#endif
	}
}
