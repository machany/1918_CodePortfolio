using _01_Work.LCM._01.Scripts.Entities;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.NPCs
{
	public class NPCAnimator : EntityAnimator
	{
		private int _beforeAnimHash;
		private int _currentAnimHash;

		public void ChangeAnimToBefore()
		{
			ChangeAnimation(_beforeAnimHash);
		}

		public void ChangeAnimation(int newHash)
		{
			if (_currentAnimHash != 0)
			{
				_beforeAnimHash = _currentAnimHash;
				SetParam(_currentAnimHash, false);
			}
			_currentAnimHash = newHash;
			if (_currentAnimHash != 0)
			{
				SetParam(_currentAnimHash, true);
			}
		}
	}
}
