using Prism.Commands;

namespace Itm.Module.UserManagement.Commands
{
    public class ModuleCommands : IModuleCommands
    {
        public CompositeCommand NewCommand { get; }
        public CompositeCommand UpdateCommand { get; }
        public CompositeCommand DeleteCommand { get; }
        public CompositeCommand SaveCommand { get; }

        public ModuleCommands()
        {
            SaveCommand = new CompositeCommand(true);
            NewCommand = new CompositeCommand(true);
            DeleteCommand = new CompositeCommand(true);
            SaveCommand = new CompositeCommand(true);
        }
    }
}