using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class NetworkUtil
    {
        public static bool TestTcpConnection(IPAddress address, int port)
        {
            TcpClient client = new();
            try
            {
                client.Connect(address, port);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                client.Dispose();
            }
        }

        public static async Task<bool> TestTcpConnectionAsync(IPAddress address, int port)
        {
            TcpClient client = new();
            try
            {
                await client.ConnectAsync(address, port);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
