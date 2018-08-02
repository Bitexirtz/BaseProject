using Itm.Database.Core.Contracts;
using Itm.Database.Core.Services;
using Itm.Database.DataLayer;
using Itm.Database.DataLayer.Contracts;
using Itm.Database.DataLayer.Repositories;
using Microsoft.Extensions.Logging;

namespace Itm.Database.BusinessLayer.Services
{
	public abstract class ServiceBase : IService
	{
		protected ILogger Logger;
		protected IUserInfo UserInfo;
		protected bool Disposed;
		protected readonly AppDbContext DbContext;

		protected IUserRepository m_userRepository;

		public ServiceBase (ILogger logger, IUserInfo userInfo, AppDbContext dbContext)
		{
			Logger = logger;
			UserInfo = userInfo;
			DbContext = dbContext;
		}

		public void Dispose ()
		{
			if (!Disposed) {
				DbContext?.Dispose ();

				Disposed = true;
			}
		}

		protected string CreateInvokedMethodLog (string methodName)
		{
			return string.Format("{0} has been invoked", methodName);
		}

		protected IUserRepository UserRepository => m_userRepository ?? (m_userRepository = new UserRepository (UserInfo, DbContext));
	}
}
