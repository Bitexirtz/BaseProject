using Itm.Database.Services;
using Itm.DataValidation;
using Itm.Module.UserManagement.Commands;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;

namespace Itm.Module.UserManagement.ViewModels
{
    public class UserManagementViewModel : ViewModelValidationBase
    {
        private string _title = "User Management";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private IModuleCommands _moduleCommands;
        public IModuleCommands ModuleCommands
        {
            get { return _moduleCommands; }
            set { SetProperty(ref _moduleCommands, value); }
        }

        public UserManagementViewModel(IUnityContainer container)
        {
            ModuleCommands = container.Resolve<IModuleCommands>();
        }
    }
}
