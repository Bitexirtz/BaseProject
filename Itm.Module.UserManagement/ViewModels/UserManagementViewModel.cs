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

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserManagementViewModel : BindableBase
	{

		IUserService _userService;
		IEventAggregator _eventAggregator;
		IRegionManager _regionManager;

		public DelegateCommand OnFirstCommand { get; private set; }
		public DelegateCommand OnPreviousCommand { get; private set; }
		public DelegateCommand OnNextCommand { get; private set; }
		public DelegateCommand OnLastCommand { get; private set; }
		public DelegateCommand OnAddCommand { get; private set; }
		public DelegateCommand OnUpdateCommand { get; private set; }
		public DelegateCommand OnDeleteCommand { get; private set; }
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

		private IList<UserModel> _users;
		public IList<UserModel> Users
		{
			get { return _users; }
			set { SetProperty (ref _users, value); }
		}

		public UserManagementViewModel (IUserService userService, IEventAggregator eventAggregator, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_userService = userService;
			_eventAggregator = eventAggregator;

			_userListView = CollectionViewSource.GetDefaultView (_userService.GetUsersAsync ().Result.Model.ToList ());
	
			_title = "-User Management Title-";

			OnFirstCommand = new DelegateCommand (FirstCommandHandler);
			OnPreviousCommand = new DelegateCommand (PreviousCommandHandler);
			OnNextCommand = new DelegateCommand (NextCommandHandler);
			OnLastCommand = new DelegateCommand (LastCommandHandler);
			OnAddCommand = new DelegateCommand (AddCommandHandler);
			OnUpdateCommand = new DelegateCommand (UpdateCommandHandler);
			OnDeleteCommand = new DelegateCommand (DeleteCommandHandler);
			OnCancelCommand = new DelegateCommand (CancelCommandHandler);
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
			//existingCustomerGrid.Visibility = Visibility.Collapsed;
			//newOrderGrid.Visibility = Visibility.Collapsed;
			//newCustomerGrid.Visibility = Visibility.Visible;

			//// Clear all the text boxes before adding a new customer.  
			//foreach (var child in newCustomerGrid.Children) {
			//	var tb = child as TextBox;
			//	if (tb != null) {
			//		tb.Text = "";
			//	}
			//}
		}

		private void UpdateCommandHandler ()
		{

		}

		private void DeleteCommandHandler ()
		{
			var cur = _userListView.CurrentItem as UserModel;


			_userListView.Refresh ();
		}

		private void CancelCommandHandler ()
		{

		}
	}
}
