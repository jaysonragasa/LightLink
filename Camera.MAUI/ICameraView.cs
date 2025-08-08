using Microsoft.Maui.Controls;

namespace Camera.MAUI; 
public interface ICameraView 
{
    public event EventHandler<byte[]> FrameReceived;
    public FlashMode FlashMode { get; set; }
}
