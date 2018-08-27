using Prism.Commands;

namespace Itm.Module.UserManagement.Commands
{
    public interface IModuleCommands
    {
        CompositeCommand NewCommand { get; }
        CompositeCommand UpdateCommand { get; }
        CompositeCommand DeleteCommand { get; }
        CompositeCommand SaveCommand { get; }
    }
}
