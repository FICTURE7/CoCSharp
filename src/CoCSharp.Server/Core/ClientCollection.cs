using CoCSharp.Server.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CoCSharp.Server.Core
{
    public class ClientCollection : ICollection<IClient>
    {
        public ClientCollection()
        {
            _sync = new object();
            _clients = new IClient[64];
        }

        private int _count;
        private IClient[] _clients;
        private readonly object _sync;

        bool ICollection<IClient>.IsReadOnly => false;
        public int Count => Thread.VolatileRead(ref _count);

        public void Add(IClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            lock (_sync)
            {
                var index = 0;
                var length = _clients.Length;
                for (; index < length; index++)
                {
                    var tmp = _clients[index];
                    if (tmp == null)
                        break;
                }

                if (index >= length)
                    Array.Resize(ref _clients, length * 2);

                _clients[index] = client;
                _count++;
            }
        }

        public bool Remove(IClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            lock (_sync)
            {
                var length = _clients.Length;
                for (int i = 0; i < length; i++)
                {
                    if (_clients[i] == client)
                    {
                        _clients[i] = null;
                        _count--;
                        return true;
                    }
                }
                return false;
            }
        }

        public void Clear()
        {
            lock (_sync)
            {
                var length = _clients.Length;
                for (int i = 0; i < length; i++)
                    _clients[i] = null;
            }
        }

        public bool Contains(IClient client)
        {
            lock (_sync)
            {
                var length = _clients.Length;
                for (int i = 0; i < length; i++)
                {
                    if (_clients[i] == client)
                        return true;
                }
                return false;
            }
        }

        public void CopyTo(IClient[] array, int arrayIndex)
        {
            lock (_sync)
            {
                Array.Copy(_clients, 0, array, arrayIndex, _clients.Length - arrayIndex);
            }
        }

        public IEnumerator<IClient> GetEnumerator()
        {
            var clients = _clients;
            for (int i = 0; i < clients.Length; i++)
            {
                var tmp = clients[i];
                if (tmp != null)
                    yield return tmp;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
