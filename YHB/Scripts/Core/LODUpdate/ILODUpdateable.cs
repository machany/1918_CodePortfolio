using AgamaLibrary.Unity.FrameUpdateSystem;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Core.LODUpdate
{
	public interface ILODUpdateable : IFrameUpdateable
	{
		GameObject GameObject { get; }
		bool Enabled { get; }
	}
}
