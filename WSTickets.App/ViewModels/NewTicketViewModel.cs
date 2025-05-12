using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;

namespace WSTickets.App.ViewModels
{
    public partial class NewTicketViewModel : ObservableObject
    {
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

        public IAsyncRelayCommand CreateTicketCommand { get; }

        public NewTicketViewModel()
        {
            SelectedPriority = Priorities[1];

            CreateTicketCommand = new AsyncRelayCommand(
                ExecuteCreateAsync,
                () => !IsBusy && !string.IsNullOrWhiteSpace(Title)
            );

            PropertyChanged += (_, __) => CreateTicketCommand.NotifyCanExecuteChanged();
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
                await Shell.Current.GoToAsync(
                    $"{nameof(Views.TicketDetailPage)}?id={created.Id}"
                );
            }

            IsBusy = false;
        }

    }
}
