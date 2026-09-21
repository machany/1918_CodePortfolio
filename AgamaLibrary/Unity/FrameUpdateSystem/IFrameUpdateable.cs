namespace AgamaLibrary.Unity.FrameUpdateSystem
{
	public interface IFrameUpdateable
	{
		byte UpdateFrame { get; set; }
		void SetEnable(bool enableValue);
		void FrameUpdate(float deltaTime);
	}
}
