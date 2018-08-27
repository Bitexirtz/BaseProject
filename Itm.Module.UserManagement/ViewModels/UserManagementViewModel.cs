using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Itm.Database.Services;
using Itm.DataValidation;
using Itm.Models;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserManagementViewModel : ViewModelValidationBase
	{
		#region Fields
		private IUserService _userService;
		private IEventAggregator _eventAggregator;
		private IRegionManager _regionManager;
		#endregion Fields

		#region Property
		public Task Initialization { get; private set; }
		#endregion Property

		#region Command Property
		public DelegateCommand OnFirstCommand { get; private set; }
		public DelegateCommand OnPreviousCommand { get; private set; }
		public DelegateCommand OnNextCommand { get; private set; }
		public DelegateCommand OnLastCommand { get; private set; }
		public DelegateCommand OnAddCommand { get; private set; }
		public DelegateCommand OnSaveCommand { get; private set; }
		public DelegateCommand<UserModel> OnDeleteCommand { get; private set; }
		public DelegateCommand OnCancelCommand { get; private set; }
		public DelegateCommand OnValidationErrorCommand { get; private set; }
		#endregion Command Property

		#region "Bindable Property"
		private string _title;

		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		private bool _newUserFormVisibility;
		public bool NewUserFormVisibility
		{
			get { return _newUserFormVisibility; }
			set {
				SetProperty(ref _newUserFormVisibility, value);
				OnAddCommand.RaiseCanExecuteChanged();
			}
		}

		#region User Bindable Property
		private string _newUserFirstName;
		public string NewUserFirstName
		{
			get { return _newUserFirstName; }
			set
			{
				SetProperty(ref _newUserFirstName, value);
			}
		}

		private string _newUserMiddleName;
		public string NewUserMiddleName
		{
			get { return _newUserMiddleName; }
			set { SetProperty(ref _newUserMiddleName, value); }
		}

		private string _newUserLastName;
		public string NewUserLastName
		{
			get { return _newUserLastName; }
			set { SetProperty(ref _newUserLastName, value); }
		}

		private string _newUserUserName;
		public string NewUserUserName
		{
			get { return _newUserUserName; }
			set { SetProperty(ref _newUserUserName, value); }
		}

		private string _newUserPassword;
		public string NewUserPassword
		{
			get { return _newUserPassword; }
			set { SetProperty(ref _newUserPassword, value); }
		}
		#endregion User Bindable Property

		public ICollectionView _userListView;
		public ICollectionView UserListView
		{
			get { return _userListView; }
			private set { SetProperty(ref _userListView, value); }
		}
		#endregion "Bindable Property"

		public UserManagementViewModel(IUnityContainer container)
		{
			_regionManager = container.Resolve<IRegionManager>();
			_userService = container.Resolve<IUserService>();
			_eventAggregator = container.Resolve<IEventAggregator>();

			_title = "-User Management Title-";

			InitializeCommandHandler();

			NewUserFormVisibility = false;
			SetValidationRules();

			Initialization = InitializeAsync();
			UserListView.MoveCurrentToFirst();
		}

		private void InitializeCommandHandler ()
		{
			OnFirstCommand = new DelegateCommand(FirstCommandHandler);
			OnPreviousCommand = new DelegateCommand(PreviousCommandHandler);
			OnNextCommand = new DelegateCommand(NextCommandHandler);
			OnLastCommand = new DelegateCommand(LastCommandHandler);
			OnAddCommand = new DelegateCommand(AddCommandHandler, CanAddNewUser);
			OnSaveCommand = new DelegateCommand(SaveCommandHandler, CanSaveNewUser);
			OnDeleteCommand = new DelegateCommand<UserModel>(DeleteCommandHandler);
			OnCancelCommand = new DelegateCommand(CancelCommandHandler);
			OnValidationErrorCommand = new DelegateCommand(ValidationErrorCommandHandler);
		}

		private void InitializeNewUserModel()
		{
			NewUserUserName = string.Empty;
			NewUserPassword = string.Empty;
			NewUserFirstName = string.Empty;
			NewUserMiddleName = string.Empty;
			NewUserLastName = string.Empty;
		}

		private void SetValidationRules()
		{
			AddRule(() => NewUserFirstName, () => (string.IsNullOrEmpty(NewUserFirstName)), "First Name is required.");
			AddRule(() => NewUserLastName, () => (string.IsNullOrEmpty(NewUserLastName)), "Last Name is required.");
			AddRule(() => NewUserUserName, () => (string.IsNullOrEmpty(NewUserUserName)), "Username is required.");
			AddRule(() => NewUserPassword, () => (string.IsNullOrEmpty(NewUserPassword)), "Password is required.");
		}

		private void ValidationErrorCommandHandler()
		{
			OnSaveCommand.RaiseCanExecuteChanged();
		}

		private bool CanAddNewUser()
		{
			return NewUserFormVisibility == false;
		}

		private bool CanSaveNewUser()
		{
			return NewUserFormVisibility == true && HasErrors == false;
		}

		private async Task InitializeAsync()
		{
			var result = await _userService.GetUsersWithCredentialsAsync();
			if (result.DidError == false)
			{
				UserListView = CollectionViewSource.GetDefaultView(result.Model.ToList());
			}
		}

		private void LastCommandHandler()
		{
			UserListView.MoveCurrentToLast();
		}

		private void PreviousCommandHandler()
		{
			UserListView.MoveCurrentToPrevious();

			if (UserListView.IsCurrentBeforeFirst == true)
			{
				UserListView.MoveCurrentToFirst();
			}
		}

		private void NextCommandHandler()
		{

			UserListView.MoveCurrentToNext();

			if (UserListView.IsCurrentAfterLast == true)
			{
				UserListView.MoveCurrentToLast();
			}
		}

		private void FirstCommandHandler()
		{
			UserListView.MoveCurrentToFirst();
		}

		private void AddCommandHandler()
		{
			NewUserFormVisibility = true;
			InitializeNewUserModel();
		}

		private async void SaveCommandHandler()
		{
			NewUserFormVisibility = false;
			var newUserModel = new UserModel
			{
				UserName = NewUserUserName,
				Password = NewUserPassword,
				FirstName = NewUserFirstName,
				MiddleName = NewUserMiddleName,
				LastName = NewUserLastName
			};

			var result = await _userService.AddUserAsync(newUserModel);
			await InitializeAsync();
		}

		private async void DeleteCommandHandler(UserModel user)
		{
			if (user != null)
			{
				await _userService.RemoveUserAsync(user.ID);
				await InitializeAsync();
			}
		}

		private void CancelCommandHandler()
		{
			NewUserFormVisibility = false;
		}
	}
}
