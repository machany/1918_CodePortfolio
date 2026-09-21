using _01_Work.KJY.Code.Events;
using _01_Work.LCM._01.Scripts.Core;
using _01_Work.LCM._01.Scripts.Interact;
using UnityEngine;

namespace Assets._01_Work.YHB.Scripts.Others
{
	public class InteractSubTitleObjects : InteractionObject
	{
		[SerializeField] protected GameEventChannelSO subtitleChannel;
		[SerializeField] protected InteractSubTitleListSO interactSubTitleList;

		public override void InteractAction()
		{
			subtitleChannel.RaiseEvent(
				SubtitleChannel.AddSubtitleEvent.Initialize(
					interactSubTitleList.talker
					, interactSubTitleList.GetSubTitle()
					, interactSubTitleList.color
					)
				);
		}
	}
}
