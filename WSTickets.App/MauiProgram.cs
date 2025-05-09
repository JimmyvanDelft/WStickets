using Microsoft.Extensions.Logging;
using UraniumUI;
using CommunityToolkit.Maui;
using WSTickets.App.Views;
using WSTickets.App.ViewModels;

namespace WSTickets.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddMaterialSymbolsFonts();
            });

        builder.Services.AddTransient<TicketListViewModel>();
        builder.Services.AddTransient<TicketDetailViewModel>();
        builder.Services.AddTransient<TicketListPage>();
        builder.Services.AddTransient<TicketDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
