using LightLink.Services;
using Camera.MAUI;
using Microsoft.Extensions.Logging;

namespace LightLink
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
				.UseMauiCameraView()
				.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

//#if ANDROID
//            builder.Services.AddSingleton<ICameraService, LightLink.Platforms.Android.CameraService>();
//#elif WINDOWS
//            builder.Services.AddSingleton<ICameraService, LightLink.Platforms.Windows.CameraService>();
//#endif

			return builder.Build();
        }
    }
}
