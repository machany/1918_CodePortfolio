using System;

namespace Assets.AgamaLibrary.DataStructures
{
	public class FlagInvoker
	{
		public Action OnFlagActived;
		public Action<bool> OnFlagChanged;
		public bool Flag { get; private set; } = false;

		public void SetFlag(bool value, bool withChangeEvent = false, bool withActiveEvent = false)
		{
			if (Flag == value)
				return;

			Flag = value;

			if (withChangeEvent)
				OnFlagChanged?.Invoke(Flag);

			if (withActiveEvent && Flag)
				OnFlagActived?.Invoke();
		}

		public void ToggleFlag(bool withChangeEvent = true, bool withActiveEvent = true)
		{
			Flag = !Flag;
			if (withChangeEvent)
				OnFlagChanged?.Invoke(Flag);

			if (withActiveEvent && Flag)
				OnFlagActived?.Invoke();
		}
	}
}
