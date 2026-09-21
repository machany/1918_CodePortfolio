using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Situations
{
	public interface ISituationable
	{
		void InvokeSituation(Situation situation, Vector3 invokedPosition);
	}
}
