using Alchemy.Inspector;
using System.Xml;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Casters
{
	public class OverlapCaster : Caster
	{
		[FoldoutGroup("Caster Setting")]
		[SerializeField] protected int castCount;
		[FoldoutGroup("Caster Setting")]
		[SerializeField] protected Color gizmosColor;
		[FoldoutGroup("Caster Setting")]
		[SerializeField] protected LayerMask castableLayer;
		[FoldoutGroup("Overlap Setting")]
		[SerializeField] protected OverlapCastType castType;
		[FoldoutGroup("Overlap Setting")]
		[SerializeField] protected Vector3 size;

		protected Collider[] _colliders;

		protected virtual void Awake()
		{
			_colliders = new Collider[castCount];
		}

		public override bool Cast()
		{
			bool success = Check(out int cnt, out var _);

			if (success)
				CastOverlaps(cnt);

			return success;
		}

		public bool Check()
		{
			return Check(out int _, out var _);
		}

		public bool Check(out int count, out Collider[] colliders)
		{
			switch (castType)
			{
				case OverlapCastType.Box:
					{
						count = Physics.OverlapBoxNonAlloc(transform.position, size / 2, _colliders, transform.rotation, castableLayer);
						colliders = _colliders;
					}
					break;
				case OverlapCastType.Sphere:
					{
						count = Physics.OverlapSphereNonAlloc(transform.position, size.x, _colliders, castableLayer);
						colliders = _colliders;
					}
					break;
				default:
					{
						count = 0;
						colliders = null;
					}
					break;
			}
			return count > 0;
		}

		public virtual void CastOverlaps(int count) { }

		public void SetSize(float size)
		{
			this.size = new Vector3(size, size, size);

        }

		public void SetSize(Vector3 size)
		{
			this.size = size;
		}

		protected virtual void OnValidate()
		{
			switch (castType)
			{
				case OverlapCastType.Box:
					break;
				case OverlapCastType.Sphere:
					{
						size.y = size.x;
						size.z = size.x;
					}
					break;
				default:
					break;
			}
		}

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = gizmosColor;
			switch (castType)
			{
				case OverlapCastType.Box:
					{
						Gizmos.DrawWireCube(transform.position, size);
					}
					break;
				case OverlapCastType.Sphere:
					{
						Gizmos.DrawWireSphere(transform.position, size.x);
					}
					break;
				default:
					break;
			}
		}
	}
}
