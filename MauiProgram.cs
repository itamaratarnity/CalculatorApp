using Microsoft.Extensions.Logging;

namespace CalculatorApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                //-- built in fonts
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                // google fonts
                fonts.AddFont("BungeeSpice.ttf", "BungeeSpice");
                fonts.AddFont("Roboto-italic.ttf", "RobotoItalic");
                fonts.AddFont("Roboto-italic.ttf", "RobotoItalic");
                fonts.AddFont("Bitcount.ttf", "Bitcount");
                fonts.AddFont("AlfaSlabr.ttf", "AlfaSlabr");
                fonts.AddFont("Matemasie.ttf", "Matemasie");

                // material icons (avoided, because it is too large, used png icons instead)
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
