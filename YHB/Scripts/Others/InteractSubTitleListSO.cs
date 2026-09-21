using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Others
{
[CreateAssetMenu(fileName = "SubTitle List SO", menuName = "SO/Interact/SubTitle", order = 0)]
	public class InteractSubTitleListSO : ScriptableObject
	{
		public string talker;
		public Color color;
		[TextArea] public List<string> subTitleList;

		private int _index;

		public string GetSubTitle()
		{
			if (_index >= subTitleList.Count)
				_index = 0;

			return subTitleList[_index++];
		}

		public void Suffle()
		{
			for (int i = subTitleList.Count - 1; i > 0; i--)
			{
				int t = Random.Range(0, i + 1);
				(subTitleList[i], subTitleList[t]) = (subTitleList[t], subTitleList[i]);
			}
		}
	}
}
