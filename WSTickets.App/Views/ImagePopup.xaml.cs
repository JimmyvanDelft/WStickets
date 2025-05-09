using CommunityToolkit.Maui.Views;

namespace WSTickets.App.Views;

public partial class ImagePopup : Popup
{
    public ImagePopup(string imageUrl)
    {
        InitializeComponent();
        PopupImage.Source = imageUrl;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }
}