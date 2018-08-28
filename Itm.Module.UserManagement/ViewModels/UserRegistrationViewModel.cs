using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Itm.Database.Services;
using Itm.DataValidation;
using Itm.Models;
using Itm.Module.UserManagement.Commands;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using static Itm.Module.UserManagement.Definitions.Enumeration;

namespace Itm.Module.UserManagement.ViewModels
{

	public class UserRegistrationViewModel : ViewModelValidationBase, IActiveAware
	{
		#region Fields
		private IUserService _userService;
		private IEventAggregator _eventAggregator;
		private IRegionManager _regionManager;
		private IModuleCommands _moduleCommands;

		private int _userListViewCount;
		#endregion Fields

		#region Property
		public Task Initialization { get; private set; }
		#endregion Property

		#region Command Property
		public DelegateCommand OnAddCommand { get; private set; }
		public DelegateCommand OnEditCommand { get; private set; }
		public DelegateCommand OnSaveCommand { get; private set; }
		public DelegateCommand<UserModel> OnDeleteCommand { get; private set; }
		public DelegateCommand OnCancelCommand { get; private set; }

		public DelegateCommand OnFirstNavCommand { get; private set; }
		public DelegateCommand OnPreviousNavCommand { get; private set; }
		public DelegateCommand OnNextNavCommand { get; private set; }
		public DelegateCommand OnLastNavCommand { get; private set; }

		public DelegateCommand OnValidationErrorCommand { get; private set; }
		#endregion Command Property

		#region Bindable Property
		private string _title;
		public string Title
		{
			get { return _title; }
			set { SetProperty (ref _title, value); }
		}

		private bool _newUserFormVisibility;
		public bool NewUserFormVisibility
		{
			get { return _newUserFormVisibility; }
			set
			{
				SetProperty (ref _newUserFormVisibility, value);
			}
		}

		private bool _isUserListEnabled;
		public bool IsUserListEnabled
		{
			get { return _isUserListEnabled; }
			set
			{
				SetProperty (ref _isUserListEnabled, value);
			}
		}

		private ICollectionView _userListView;
		public ICollectionView UserListView
		{
			get { return _userListView; }
			private set {
				SetProperty (ref _userListView, value);

				if (_userListView != null) {
					_userListViewCount = _userListView.Cast<object> ().Count ();
				}

				SetNavigationCommandEnableStatus ();
			}
		}

		private int _selectedUserIndex;
		public int SelectedUserIndex
		{
			get { return _selectedUserIndex; }
			set {
				SetProperty (ref _selectedUserIndex, value);
				SetCommandEnableStatus (ExecutionTypes.None);
				SetNavigationCommandEnableStatus ();
			}
		}

		private bool _isActive;
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				_isActive = value;
				OnIsActiveChanged ();
			}
		}


		#region Enable/disable Command Property
		private bool _canAddNewUser;
		public bool CanAddNewUser
		{
			get { return _canAddNewUser; }
			set { SetProperty (ref _canAddNewUser, value); }
		}

		private bool _canEditUser;
		public bool CanEditUser
		{
			get { return _canEditUser; }
			set { SetProperty (ref _canEditUser, value); }
		}

		private bool _canDeleteUser;
		public bool CanDeleteUser
		{
			get { return _canDeleteUser; }
			set { SetProperty (ref _canDeleteUser, value); }
		}

		private bool _canSaveUser;
		public bool CanSaveUser
		{
			get { return _canSaveUser; }
			set { SetProperty (ref _canSaveUser, value); }
		}

		private bool _canCancelUser;
		public bool CanCancelUser
		{
			get { return _canCancelUser; }
			set { SetProperty (ref _canCancelUser, value); }
		}

		private bool _canNavToFirst;
		public bool CanNavToFirst
		{
			get { return _canNavToFirst; }
			set { SetProperty (ref _canNavToFirst, value); }
		}

		private bool _canNavToLast;
		public bool CanNavToLast
		{
			get { return _canNavToLast; }
			set { SetProperty (ref _canNavToLast, value); }
		}

		private bool _canNavToPrevious;
		public bool CanNavToPrevious
		{
			get { return _canNavToPrevious; }
			set { SetProperty (ref _canNavToPrevious, value); }
		}

		private bool _canNavToNext;
		public bool CanNavToNext
		{
			get { return _canNavToNext; }
			set { SetProperty (ref _canNavToNext, value); }
		}
		#endregion Enable/disable Command Property

		#region User Bindable Property
		private string _newUserFirstName;
		public string NewUserFirstName
		{
			get { return _newUserFirstName; }
			set
			{
				SetProperty (ref _newUserFirstName, value);
			}
		}

		private string _newUserMiddleName;
		public string NewUserMiddleName
		{
			get { return _newUserMiddleName; }
			set { SetProperty (ref _newUserMiddleName, value); }
		}

		private string _newUserLastName;
		public string NewUserLastName
		{
			get { return _newUserLastName; }
			set { SetProperty (ref _newUserLastName, value); }
		}

		private string _newUserUserName;
		public string NewUserUserName
		{
			get { return _newUserUserName; }
			set { SetProperty (ref _newUserUserName, value); }
		}

		private string _newUserPassword;
		public string NewUserPassword
		{
			get { return _newUserPassword; }
			set { SetProperty (ref _newUserPassword, value); }
		}
		#endregion User Bindable Property

		#endregion Bindable Property

		#region Event
		public event EventHandler IsActiveChanged;
		#endregion Event

		#region Constructor
		public UserRegistrationViewModel (IUnityContainer container)
		{
			_regionManager = container.Resolve<IRegionManager> ();
			_userService = container.Resolve<IUserService> ();
			_eventAggregator = container.Resolve<IEventAggregator> ();
			_moduleCommands = container.Resolve<IModuleCommands> ();

			InitializeFields ();
			SetValidationRules ();
			InitializeCommandHandler ();
			SetCommandEnableStatus ();

			Initialization = InitializeAsync ().ContinueWith (
				t =>
				{
					UserListView.MoveCurrentToFirst ();
				}
			);

			Title = "User";
		}

		#endregion Constructor

		#region Method

		#region Initialization Method
		private void InitializeFields ()
		{
			_userListViewCount = 0;
		}

		private async Task InitializeAsync ()
		{
			var result = await _userService.GetUsersWithCredentialsAsync ();
			if (result.DidError == false) {
				UserListView = CollectionViewSource.GetDefaultView (result.Model.ToList ());
			}
		}

		private void InitializeCommandHandler ()
		{
			// User operations
			OnAddCommand = new DelegateCommand (AddCommandHandler).ObservesCanExecute (() => CanAddNewUser);
			OnEditCommand = new DelegateCommand (EditCommandHandler).ObservesCanExecute (() => CanEditUser);
			OnDeleteCommand = new DelegateCommand<UserModel> (DeleteCommandHandler).ObservesCanExecute (() => CanDeleteUser);
			OnSaveCommand = new DelegateCommand (SaveCommandHandler).ObservesCanExecute (() => CanSaveUser);
			OnCancelCommand = new DelegateCommand (CancelCommandHandler).ObservesCanExecute (() => CanCancelUser);

			// Navigation
			OnFirstNavCommand = new DelegateCommand (FirstNavCommandHandler).ObservesCanExecute (() => CanNavToFirst);
			OnPreviousNavCommand = new DelegateCommand (PreviousNavCommandHandler).ObservesCanExecute (() => CanNavToPrevious);
			OnNextNavCommand = new DelegateCommand (NextNavCommandHandler).ObservesCanExecute (() => CanNavToNext);
			OnLastNavCommand = new DelegateCommand (LastNavCommandHandler).ObservesCanExecute (() => CanNavToLast);

			OnValidationErrorCommand = new DelegateCommand (ValidationErrorCommandHandler);

			_moduleCommands.NewCommand.RegisterCommand (OnAddCommand);
			_moduleCommands.EditCommand.RegisterCommand (OnEditCommand);
			_moduleCommands.DeleteCommand.RegisterCommand (OnDeleteCommand);
			_moduleCommands.SaveCommand.RegisterCommand (OnSaveCommand);
			_moduleCommands.CancelCommand.RegisterCommand (OnCancelCommand);

			_moduleCommands.FirstNavCommand.RegisterCommand (OnFirstNavCommand);
			_moduleCommands.PreviousNavCommand.RegisterCommand (OnPreviousNavCommand);
			_moduleCommands.NextNavCommand.RegisterCommand (OnNextNavCommand);
			_moduleCommands.LastNavCommand.RegisterCommand (OnLastNavCommand);
		}

		private void InitializeNewUserModel ()
		{
			NewUserUserName = string.Empty;
			NewUserPassword = string.Empty;
			NewUserFirstName = string.Empty;
			NewUserMiddleName = string.Empty;
			NewUserLastName = string.Empty;
		}

		private void SetValidationRules ()
		{
			AddRule (() => NewUserFirstName, () => (string.IsNullOrEmpty (NewUserFirstName)), "First Name is required.");
			AddRule (() => NewUserLastName, () => (string.IsNullOrEmpty (NewUserLastName)), "Last Name is required.");
			AddRule (() => NewUserUserName, () => (string.IsNullOrEmpty (NewUserUserName)), "Username is required.");
			AddRule (() => NewUserPassword, () => (string.IsNullOrEmpty (NewUserPassword)), "Password is required.");
		}

		#endregion Initialization Method

		#region Command Handler
		private void OnIsActiveChanged ()
		{
			OnCancelCommand.IsActive = IsActive;
			OnAddCommand.IsActive = IsActive;
			OnEditCommand.IsActive = IsActive;
			OnDeleteCommand.IsActive = IsActive;
			OnSaveCommand.IsActive = IsActive;

			OnFirstNavCommand.IsActive = IsActive;
			OnPreviousNavCommand.IsActive = IsActive;
			OnNextNavCommand.IsActive = IsActive;
			OnLastNavCommand.IsActive = IsActive;

			IsActiveChanged?.Invoke (this, new EventArgs ());
		}

		private void ValidationErrorCommandHandler ()
		{
			OnSaveCommand.RaiseCanExecuteChanged ();
		}

		private void LastNavCommandHandler ()
		{
			UserListView.MoveCurrentToLast ();
			SetNavigationCommandEnableStatus ();
		}

		private void PreviousNavCommandHandler ()
		{
			UserListView.MoveCurrentToPrevious ();

			if (UserListView.IsCurrentBeforeFirst == true) {
				UserListView.MoveCurrentToFirst ();
			}

			SetNavigationCommandEnableStatus ();
		}

		private void NextNavCommandHandler ()
		{
			UserListView.MoveCurrentToNext ();

			if (UserListView.IsCurrentAfterLast == true) {
				UserListView.MoveCurrentToLast ();
			}

			SetNavigationCommandEnableStatus ();
		}

		private void FirstNavCommandHandler ()
		{
			UserListView.MoveCurrentToFirst ();

			SetNavigationCommandEnableStatus ();
		}

		private void CancelCommandHandler ()
		{
			SetCommandEnableStatus (ExecutionTypes.Cancel);
		}

		private void AddCommandHandler ()
		{
			SetCommandEnableStatus (ExecutionTypes.Add);

			InitializeNewUserModel ();
		}

		private void EditCommandHandler ()
		{
			SetCommandEnableStatus (ExecutionTypes.Edit);
		}

		private async void DeleteCommandHandler (UserModel user)
		{
			SetCommandEnableStatus (ExecutionTypes.Delete);
			if (user != null) {
				await _userService.RemoveUserAsync (user.ID);
				await InitializeAsync ();
			}
		}

		private async void SaveCommandHandler ()
		{
			var newUserModel = new UserModel
			{
				UserName = NewUserUserName,
				Password = NewUserPassword,
				FirstName = NewUserFirstName,
				MiddleName = NewUserMiddleName,
				LastName = NewUserLastName
			};

			var result = await _userService.AddUserAsync (newUserModel);
			await InitializeAsync ();

			SetCommandEnableStatus (ExecutionTypes.Save);
		}
		#endregion Command Handler

		#region Helper Method
		private void SetCommandEnableStatus (ExecutionTypes activeType = ExecutionTypes.None)
		{
			switch (activeType) {
				case ExecutionTypes.Add:
				case ExecutionTypes.Edit:
				case ExecutionTypes.Delete: {
						CanCancelUser = true;
						CanAddNewUser = false;
						CanEditUser = false;
						CanDeleteUser = false;
						CanSaveUser = true;

						IsUserListEnabled = false;
					}
					break;

				default: {
						CanCancelUser = false;
						CanAddNewUser = true;
						CanEditUser = false;
						CanDeleteUser = false;
						CanSaveUser = false;

						if (SelectedUserIndex != -1) {
							CanEditUser = true;
							CanDeleteUser = true;
						}

						IsUserListEnabled = true;
					}
					break;
			}

			NewUserFormVisibility = activeType == ExecutionTypes.Add ? true : false;
		}

		private void SetNavigationCommandEnableStatus ()
		{
			CanNavToFirst = false;
			CanNavToPrevious = false;
			CanNavToNext = false;
			CanNavToLast = false;

			if(SelectedUserIndex < _userListViewCount - 1) {
				CanNavToLast = true;
				CanNavToNext = true;
			}

			if (0 < SelectedUserIndex) {
				CanNavToFirst = true;
				CanNavToPrevious = true;
			}
		}

		#endregion Helper Method
		#endregion Method
	}
}