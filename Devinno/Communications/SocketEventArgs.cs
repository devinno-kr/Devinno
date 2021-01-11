using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications
{
    #region SocketEventArgs
    public class SocketEventArgs : EventArgs
    {
        public Socket Client { get; private set; }
        public SocketEventArgs(Socket Client)
        {
            this.Client = Client;
        }
    }
    #endregion
}
