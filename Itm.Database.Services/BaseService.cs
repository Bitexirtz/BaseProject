using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Core.Services;
using Itm.Database.Repositories;
using Itm.Log.Core;

namespace Itm.Database.Services
{
    public abstract class BaseService : IService
    {
        protected bool Disposed;

        protected AppDbContext DbContext { get; }

        protected ILogger Logger { get; }

        protected IMapper Mapper { get; }

        protected IAppUser UserInfo { get; }

        public BaseService (ILogger logger, IMapper mapper, IAppUser userInfo, AppDbContext dbContext)
        {
            Logger = logger;
            Mapper = mapper;
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
            return string.Format ("{0} has been invoked", methodName);
        }
    }
}
