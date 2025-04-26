using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;

namespace WSTickets.App.PageModels;

public interface IProjectTaskPageModel
{
	IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
	bool IsBusy { get; }
}