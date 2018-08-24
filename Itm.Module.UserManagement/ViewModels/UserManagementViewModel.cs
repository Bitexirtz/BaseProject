using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Itm.Database.Services;
using Itm.Models;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserManagementViewModel : BindableBase
	{
		#region Fields
		IUserService _userService;
		IEventAggregator _eventAggregator;
		IRegionManager _regionManager;
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
		#endregion Command Property

		#region "Bindable Property"
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
			set { SetProperty(ref _newUserFormVisibility, value); }
		}

		#region User Bindable Property
		private string _newUserFirstName;
		public string NewUserFirstName
		{
			get { return _newUserFirstName; }
			private set { SetProperty (ref _newUserFirstName, value); }
		}

		private string _newUserMiddleName;
		public string NewUserMiddleName
		{
			get { return _newUserMiddleName; }
			private set { SetProperty (ref _newUserMiddleName, value); }
		}

		private string _newUserLastName;
		public string NewUserLastName
		{
			get { return _newUserLastName; }
			private set { SetProperty (ref _newUserLastName, value); }
		}

		private string _newUserUserName;
		public string NewUserUserName
		{
			get { return _newUserUserName; }
			private set { SetProperty (ref _newUserUserName, value); }
		}

		private string _newUserPassword;
		public string NewUserPassword
		{
			get { return _newUserPassword; }
			private set { SetProperty (ref _newUserPassword, value); }
		}
		#endregion User Bindable Property

		public ICollectionView _userListView;
		public ICollectionView UserListView
		{
			get { return _userListView; }
			private set { SetProperty (ref _userListView, value); }
		}
		#endregion "Bindable Property"

		public UserManagementViewModel (IUnityContainer container)
		{
			_regionManager = container.Resolve<IRegionManager>();
			_userService = container.Resolve<IUserService>();
			_eventAggregator = container.Resolve <IEventAggregator>();

			_title = "-User Management Title-";
			NewUserFormVisibility = false;

			SetValidationRules();

			OnFirstCommand = new DelegateCommand (FirstCommandHandler);
			OnPreviousCommand = new DelegateCommand (PreviousCommandHandler);
			OnNextCommand = new DelegateCommand (NextCommandHandler);
			OnLastCommand = new DelegateCommand (LastCommandHandler);
			OnAddCommand = new DelegateCommand (AddCommandHandler);
			//OnSaveCommand = new DelegateCommand (SaveCommandHandler, CanSaveNewUser);
			OnSaveCommand = new DelegateCommand(SaveCommandHandler);
			OnDeleteCommand = new DelegateCommand<UserModel> (DeleteCommandHandler);
			OnCancelCommand = new DelegateCommand (CancelCommandHandler);

			Initialization = InitializeAsync();
			UserListView.MoveCurrentToFirst();
		}

		private void SetValidationRules()
		{
			//base.AddRule(() => NewUserModel, () => NewUserModel.FirstName.Length < 5, "First Name is less than 5");
			//AddRule(() => Aid, () =>
			//	Aid.Length >= (5 * 2) &&
			//	Aid.Length <= (16 * 2) &&
			//	Aid.Length % 2 == 0, "Invalid AID.");
		}

		private bool CanSaveNewUser()
		{

			return true;
		}

		private async Task InitializeAsync()
		{
			var result = await _userService.GetUsersAsync();
			if(result.DidError == false)
			{
				UserListView = CollectionViewSource.GetDefaultView(result.Model.ToList());
			}
		}

		private void LastCommandHandler ()
		{
			UserListView.MoveCurrentToLast ();
		}

		private void PreviousCommandHandler ()
		{
			UserListView.MoveCurrentToPrevious ();

			if (UserListView.IsCurrentBeforeFirst == true)
			{
				UserListView.MoveCurrentToFirst();
			}
		}

		private void NextCommandHandler ()
		{
			
			UserListView.MoveCurrentToNext ();

			if (UserListView.IsCurrentAfterLast == true)
			{
				UserListView.MoveCurrentToLast();
			}
		}

		private void FirstCommandHandler ()
		{
			UserListView.MoveCurrentToFirst ();
		}

		private void AddCommandHandler ()
		{
			NewUserFormVisibility = true;
			InitializeNewUserModel ();
		}

		private async void SaveCommandHandler ()
		{
			NewUserFormVisibility = false;
			var newUserModel = new UserModel {
				UserName = NewUserUserName,
				Password = NewUserPassword,
				FirstName = NewUserFirstName,
				MiddleName = NewUserMiddleName,
				LastName = NewUserLastName
			};

			var result = await _userService.AddUserAsync(newUserModel);
			await InitializeAsync();
		}

		private void InitializeNewUserModel()
		{
			NewUserUserName = string.Empty;
			NewUserPassword = string.Empty;
			NewUserFirstName = string.Empty;
			NewUserMiddleName = string.Empty;
			NewUserLastName = string.Empty;
		}

		private async void DeleteCommandHandler(UserModel user)
		{
			if(user != null)
			{
				await _userService.RemoveUserAsync(user.ID);
				await InitializeAsync();
			}
		}

		private void CancelCommandHandler ()
		{
			NewUserFormVisibility = false;
		}
	}
}
