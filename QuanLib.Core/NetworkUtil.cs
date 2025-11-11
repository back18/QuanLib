using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class NetworkUtil
    {
        public static bool TcpListenerIsActive(int port)
        {
            ThrowHelper.ArgumentOutOfRange(IPEndPoint.MinPort, IPEndPoint.MaxPort, port, nameof(port));

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] listeners = properties.GetActiveTcpListeners();
            return listeners.Any(s => s.Port == port);
        }

        public static bool TcpConnectionIsActive(int port)
        {
            ThrowHelper.ArgumentOutOfRange(IPEndPoint.MinPort, IPEndPoint.MaxPort, port, nameof(port));

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            return connections.Any(s => s.LocalEndPoint.Port == port);
        }

        public static bool TestTcpConnectivity(IPAddress address, int port)
        {
            ArgumentNullException.ThrowIfNull(address, nameof(address));
            ThrowHelper.ArgumentOutOfRange(IPEndPoint.MinPort, IPEndPoint.MaxPort, port, nameof(port));

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

        public static async Task<bool> TestTcpConnectivityAsync(IPAddress address, int port)
        {
            ArgumentNullException.ThrowIfNull(address, nameof(address));
            ThrowHelper.ArgumentOutOfRange(IPEndPoint.MinPort, IPEndPoint.MaxPort, port, nameof(port));

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
