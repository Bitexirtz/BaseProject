using Itm.Models;

namespace Itm.Model.Extensions
{
	public static class UserModelExtension
    {
		public static bool IsComplete(this UserModel userModel)
		{
			if(string.IsNullOrEmpty(userModel.FirstName) == true
				|| string.IsNullOrEmpty(userModel.LastName) == true
				|| string.IsNullOrEmpty(userModel.UserName) == true
				|| string.IsNullOrEmpty(userModel.Password) == true
				)
			{
				return false;
			}

			return true;
		}
    }
}
