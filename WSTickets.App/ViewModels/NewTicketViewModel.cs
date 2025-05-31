using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace WSTickets.App.ViewModels
{
    public partial class NewTicketViewModel : ObservableObject
    {
        // holds the raw FileResults until after ticket creation
        private readonly List<FileResult> _pendingFiles = new();

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private TicketPriority selectedPriority;

           public ObservableCollection<TicketPriority> Priorities { get; }
               = new ObservableCollection<TicketPriority>(
                   Enum.GetValues<TicketPriority>()
                 );

        public ObservableCollection<ImageSource> AttachmentPreviews { get; }
                = new ObservableCollection<ImageSource>();

        public IAsyncRelayCommand CreateTicketCommand { get; }
        public IAsyncRelayCommand AddScreenshotCommand { get; }

        public NewTicketViewModel()
        {
            SelectedPriority = Priorities[1];

            AddScreenshotCommand = new AsyncRelayCommand(ExecuteAddScreenshotAsync);

            CreateTicketCommand = new AsyncRelayCommand(
                ExecuteCreateAsync,
                () => !IsBusy && !string.IsNullOrWhiteSpace(Title)
            );

            PropertyChanged += (_, __) => CreateTicketCommand.NotifyCanExecuteChanged();
        }

        private async Task ExecuteAddScreenshotAsync()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No Permissions", "OK");
                    return;
                }

                // let the user take a photo (or swap for PickPhotoAsync)
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take screenshot"
                });
                if (photo == null)
                    return;

                _pendingFiles.Add(photo);

                // display a local preview
                var stream = await photo.OpenReadAsync();
                AttachmentPreviews.Add(ImageSource.FromStream(() => stream));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task ExecuteCreateAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            HasError = false;

            var dto = new TicketCreateDto
            {
                Title = Title,
                Description = Description,
                Priority = SelectedPriority,
                CompanyId = 1
            };

            var created = await TicketService.Instance.CreateTicketAsync(dto);

            if (created == null)
            {
                ErrorMessage = "Could not create ticket.";
                HasError = true;
            }
            else
            {
                // upload each pending file
                foreach (var file in _pendingFiles)
                {
                    using var stream = await file.OpenReadAsync();
                    await TicketService.Instance
                        .UploadAttachmentAsync(created.Id, stream, file.FileName, file.ContentType);
                }

                // navigate to detail
                await Shell.Current.GoToAsync(
                    $"{nameof(Views.TicketDetailPage)}?id={created.Id}"
                );
            }

            IsBusy = false;
        }

    }
}
