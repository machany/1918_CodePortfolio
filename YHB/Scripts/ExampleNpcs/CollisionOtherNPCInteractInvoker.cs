using Assets._01_Work.YHB.Scripts.NPCs;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._01_Work.YHB.Scripts.ExampleNpcs
{
	public class CollisionOtherNPCInteractInvoker : MonoBehaviour
	{
		[SerializeField] private bool isStrongCollision;

		public UnityEvent OnCollision;
		public UnityEvent OnCollisionStrong;

		[SerializeField] private LayerMask npcLayer;

		private void OnCollisionEnter(Collision collision)
		{
			if (((1 << collision.gameObject.layer) & npcLayer) != 0)
			{
				if (isStrongCollision)
					OnCollisionStrong?.Invoke();
				else
					OnCollision?.Invoke();
			}
		}
	}
}
