using WSTickets.App.Models;
using WSTickets.App.PageModels;

namespace WSTickets.App.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}