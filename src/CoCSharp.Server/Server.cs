using System;
using CoCSharp.Data;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;

namespace CoCSharp.Server
{
    public class Server : IServer
    {
        public AssetManager AssetManager
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbManager DbManager
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
