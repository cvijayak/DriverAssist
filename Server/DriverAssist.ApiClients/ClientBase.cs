using System;

namespace DriverAssist.ApiClients
{
    public abstract class ClientBase
    {
        protected readonly Uri _baseUri;

        protected ClientBase(Uri baseUri)
        {
            _baseUri = baseUri;
        }
    }
}
