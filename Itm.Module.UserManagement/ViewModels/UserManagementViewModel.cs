using System.Linq;
using Itm.Database.Services;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Itm.Models;
using System;
using Prism.Commands;
using Prism.Regions;
using Itm.Definitions;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserManagementViewModel : BindableBase
	{
		IUserService _userService;
		IEventAggregator _eventAggregator;
		IRegionManager _regionManager;
		public Task Initialization { get; private set; }

		public DelegateCommand OnFirstCommand { get; private set; }
		public DelegateCommand OnPreviousCommand { get; private set; }
		public DelegateCommand OnNextCommand { get; private set; }
		public DelegateCommand OnLastCommand { get; private set; }
		public DelegateCommand OnAddCommand { get; private set; }
		public DelegateCommand OnUpdateCommand { get; private set; }
		public DelegateCommand<UserModel> OnDeleteCommand { get; private set; }
		public DelegateCommand OnCancelCommand { get; private set; }

		private string _title;

		public string Title
		{
			get { return _title; }
			set { SetProperty (ref _title, value); }
		}

		public ICollectionView _userListView;
		public ICollectionView UserListView
		{
			get { return _userListView; }
			private set { SetProperty (ref _userListView, value); }
		}

		public UserManagementViewModel (IUserService userService, IEventAggregator eventAggregator, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_userService = userService;
			_eventAggregator = eventAggregator;

			Initialization = InitializeAsync();

			_title = "-User Management Title-";

			OnFirstCommand = new DelegateCommand (FirstCommandHandler);
			OnPreviousCommand = new DelegateCommand (PreviousCommandHandler);
			OnNextCommand = new DelegateCommand (NextCommandHandler);
			OnLastCommand = new DelegateCommand (LastCommandHandler);
			OnAddCommand = new DelegateCommand (AddCommandHandler);
			OnUpdateCommand = new DelegateCommand (UpdateCommandHandler);
			OnDeleteCommand = new DelegateCommand<UserModel> (DeleteCommandHandler);
			OnCancelCommand = new DelegateCommand (CancelCommandHandler);
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
		}

		private void NextCommandHandler ()
		{
			UserListView.MoveCurrentToNext ();
		}

		private void FirstCommandHandler ()
		{
			UserListView.MoveCurrentToFirst ();
		}

		private void AddCommandHandler ()
		{
		}

		private void UpdateCommandHandler ()
		{

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

		}
	}
}
