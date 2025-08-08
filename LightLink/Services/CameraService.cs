namespace LightLink.Services;

public interface ICameraService
{
	event EventHandler<byte[]>? FrameReady;

	void StartPreview();
	void StopPreview();

	bool IsRunning { get; }
}

//public class CameraFrameEventArgs : EventArgs
//{
//	public byte[] GrayscaleData { get; }
//	public int Width { get; }
//	public int Height { get; }

//	public CameraFrameEventArgs(byte[] data, int width, int height)
//	{
//		GrayscaleData = data;
//		Width = width;
//		Height = height;
//	}
//}