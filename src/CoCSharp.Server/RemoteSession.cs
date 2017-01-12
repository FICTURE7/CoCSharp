using System;
using CoCSharp.Logic;
using CoCSharp.Server.Api;

namespace CoCSharp.Server
{
    public class RemoteSession : Session
    {
        #region Constructors
        public RemoteSession(IServer server) : base()
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
        }
        #endregion

        #region Fields & Properties
        private readonly IServer _server;

        public IServer Server => _server;
        #endregion

        #region Methods
        public override Level SearchOpponent()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
