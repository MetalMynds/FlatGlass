using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.DirectoryServices.AccountManagement;
using System.Management;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MetalMynds.Utilities
{
    public class NetworkHelper
    {

        protected class CachedUser
        {
            public String UserName;
            public String DisplayName;
            public String EmailAddress;
        }

        protected Dictionary<String, CachedUser> BaseUserCache = new Dictionary<String, CachedUser>();
        protected PrincipalContext BaseContext = null;
        protected String BaseDomain = String.Empty;
        protected Boolean BaseForeignDomain = false;
        protected Boolean BaseDomainLookupFailed = false;

        public NetworkHelper()
        {
            try
            {
                BaseDomain = Environment.GetEnvironmentVariable("USERDOMAIN");
                BaseContext = new PrincipalContext(ContextType.Domain, Domain);
            }
            catch
            {
            }
        }

        public NetworkHelper(String Domain)
        {
            InitialiseDomain(Domain, true);
        }

        public NetworkHelper(String Domain, Boolean OnlyIfSameDomain)
        {
            InitialiseDomain(Domain, OnlyIfSameDomain);
        }

        protected virtual void InitialiseDomain(String Domain, Boolean OnlyIfUserDomain)
        {
            try
            {
                BaseDomain = Domain;

                if ((UserDomain.ToUpper() == BaseDomain.ToUpper()) && OnlyIfUserDomain)
                {
                    BaseContext = new PrincipalContext(ContextType.Domain, Domain);
                }
                else
                {
                    BaseForeignDomain = true;
                }
            }
            catch
            {
                BaseDomainLookupFailed = true;
            }
        }

        public Boolean IsForeignDomain { get { return BaseForeignDomain; } }
        public Boolean DomainLookupFailed { get { return BaseDomainLookupFailed; } }
        public String UserDomain { get { return Environment.GetEnvironmentVariable("USERDOMAIN"); } }

        public static Boolean Ping(String HostName)
        {
            System.Net.NetworkInformation.Ping ping = new Ping();
            PingReply reply = ping.Send(HostName);
            return (reply.Status == IPStatus.Success);
        }

        public static Boolean Ping(Uri Address)
        {
            return Ping(Address.Host);
        }

        public static bool IsPortFree(int Port)
        {
            try
            {
                using (new System.Net.Sockets.TcpClient("127.0.0.1", Port))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }

        }

        public static int GetFreePort(int StartPort= 49152, int LastPort = 65535)
        {
          
            List<int> usedPorts = new List<int>();

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endpoint in tcpConnInfoArray)
            {
                if (endpoint.Port >= StartPort && endpoint.Port <= LastPort )
                {
                    usedPorts.Add(endpoint.Port);
                }
            }

            int freePort = StartPort;

            if (usedPorts.Count > 0)
            {
                freePort = usedPorts.Max() + 1;
            }

            return freePort;

        }

        public static String GetNetworkCardMacAddress()
        {

            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (netInterface.GetIPProperties().GatewayAddresses.Count > 0)
                {
                    PhysicalAddress address = netInterface.GetPhysicalAddress();

                    return address.ToString();
                }

            }

            return String.Empty;

            //ManagementClass oMClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            //ManagementObjectCollection colMObj = oMClass.GetInstances();

            //foreach (ManagementObject objMO in colMObj)
            //{

            //    Console.WriteLine(objMO["MacAddress"].ToString());
            //}
        }

        public static Process GetPortProcess(int Port) 
        {
            TcpTable table = GetExtendedTcpTable(false);

            foreach (TcpRow row in table)
            {
                if (row.LocalEndPoint.Port == Port)
                {
                    try
                    {
                        return Process.GetProcessById(row.ProcessId);
                    }
                    catch
                    {
                        return null;
                    }

                }
            }

            return null;
        }

        public static void GetNetworkCardMacAddress(out Byte[] Address)
        {

            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (netInterface.GetIPProperties().GatewayAddresses.Count > 0)
                {
                    PhysicalAddress address = netInterface.GetPhysicalAddress();

                    Address = address.GetAddressBytes();
                    return;
                }

            }

            Address = null;

            //ManagementClass oMClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            //ManagementObjectCollection colMObj = oMClass.GetInstances();

            //foreach (ManagementObject objMO in colMObj)
            //{

            //    Console.WriteLine(objMO["MacAddress"].ToString());
            //}
        }

        public String GetDisplayName(String UserName)
        {
            String displayName = String.Empty;
            String emailAddress = String.Empty;

            if (ResolveUser(UserName, out displayName, out emailAddress))
            {
                return displayName;
            }
            else
            {
                return UserName;
            }

        }        

        public Boolean ResolveUser(String UserName, out String DisplayName, out String EmailAddress)
        {
            try
            {
                if (!BaseUserCache.ContainsKey(UserName.ToUpper()))
                {
                    if (!BaseForeignDomain)
                    {
                        if (BaseContext != null)
                        {
                            UserPrincipal foundUserPrincipal = UserPrincipal.FindByIdentity(BaseContext, UserName);

                            String displayName = foundUserPrincipal.DisplayName;
                            String emailAddress = foundUserPrincipal.EmailAddress;
                            String userName = UserName.ToUpper();

                            CachedUser user = new CachedUser();

                            user.UserName = userName;
                            user.DisplayName = displayName;
                            user.EmailAddress = emailAddress;

                            BaseUserCache.Add(user.UserName, user);

                            DisplayName = displayName;
                            EmailAddress = emailAddress;

                            return true;
                        }
                        else
                        {
                            throw new InvalidOperationException("Network Helper: Cannot Resolve User Without a Domain Name!");
                        }
                    }
                    else
                    {
                        DisplayName = UserName;
                        EmailAddress = String.Empty;
                        return false;
                    }

                }
                else
                {
                    String userName = UserName.ToUpper();

                    DisplayName = BaseUserCache[userName].DisplayName;
                    EmailAddress = BaseUserCache[userName].EmailAddress;
                    return true;
                }
            }
            catch
            {
                DisplayName = UserName;
                EmailAddress = String.Empty;
                return false;
            }

        }

        //public static void SendMessage(String RecipentUserName, String Message)
        //{


        //}

        public String Domain { get { return BaseDomain; } }

        public PrincipalContext Context { get { return BaseContext; } }

        public static TcpTable GetExtendedTcpTable(bool sorted)
        {
            List<TcpRow> tcpRows = new List<TcpRow>();

            IntPtr tcpTable = IntPtr.Zero;
            int tcpTableLength = 0;

            if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, sorted, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) != 0)
            {
                try
                {
                    tcpTable = Marshal.AllocHGlobal(tcpTableLength);
                    if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, true, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) == 0)
                    {
                        IpHelper.TcpTable table = (IpHelper.TcpTable)Marshal.PtrToStructure(tcpTable, typeof(IpHelper.TcpTable));

                        IntPtr rowPtr = (IntPtr)((long)tcpTable + Marshal.SizeOf(table.length));
                        for (int i = 0; i < table.length; ++i)
                        {
                            tcpRows.Add(new TcpRow((IpHelper.TcpRow)Marshal.PtrToStructure(rowPtr, typeof(IpHelper.TcpRow))));
                            rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(typeof(IpHelper.TcpRow)));
                        }
                    }
                }
                finally
                {
                    if (tcpTable != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(tcpTable);
                    }
                }
            }

            return new TcpTable(tcpRows);
        }


    }

    #region Managed IP Helper API

    public class TcpTable : IEnumerable<TcpRow>
    {
        #region Private Fields

        private IEnumerable<TcpRow> tcpRows;

        #endregion

        #region Constructors

        public TcpTable(IEnumerable<TcpRow> tcpRows)
        {
            this.tcpRows = tcpRows;
        }

        #endregion

        #region Public Properties

        public IEnumerable<TcpRow> Rows
        {
            get { return this.tcpRows; }
        }

        #endregion

        #region IEnumerable<TcpRow> Members

        public IEnumerator<TcpRow> GetEnumerator()
        {
            return this.tcpRows.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.tcpRows.GetEnumerator();
        }

        #endregion
    }

    public class TcpRow
    {
        #region Private Fields

        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;
        private TcpState state;
        private int processId;

        #endregion

        #region Constructors

        public TcpRow(IpHelper.TcpRow tcpRow)
        {
            this.state = tcpRow.state;
            this.processId = tcpRow.owningPid;

            int localPort = (tcpRow.localPort1 << 8) + (tcpRow.localPort2) + (tcpRow.localPort3 << 24) + (tcpRow.localPort4 << 16);
            long localAddress = tcpRow.localAddr;
            this.localEndPoint = new IPEndPoint(localAddress, localPort);

            int remotePort = (tcpRow.remotePort1 << 8) + (tcpRow.remotePort2) + (tcpRow.remotePort3 << 24) + (tcpRow.remotePort4 << 16);
            long remoteAddress = tcpRow.remoteAddr;
            this.remoteEndPoint = new IPEndPoint(remoteAddress, remotePort);
        }

        #endregion

        #region Public Properties

        public IPEndPoint LocalEndPoint
        {
            get { return this.localEndPoint; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return this.remoteEndPoint; }
        }

        public TcpState State
        {
            get { return this.state; }
        }

        public int ProcessId
        {
            get { return this.processId; }
        }

        #endregion
    }
    
    #region P/Invoke IP Helper API

    /// <summary>
    /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366073.aspx"/>
    /// </summary>
    public static class IpHelper
    {
        #region Public Fields

        public const string DllName = "iphlpapi.dll";
        public const int AfInet = 2;

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa365928.aspx"/>
        /// </summary>
        [DllImport(IpHelper.DllName, SetLastError = true)]
        public static extern uint GetExtendedTcpTable(IntPtr tcpTable, ref int tcpTableLength, bool sort, int ipVersion, TcpTableType tcpTableType, int reserved);

        #endregion

        #region Public Enums

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366386.aspx"/>
        /// </summary>
        public enum TcpTableType
        {
            BasicListener,
            BasicConnections,
            BasicAll,
            OwnerPidListener,
            OwnerPidConnections,
            OwnerPidAll,
            OwnerModuleListener,
            OwnerModuleConnections,
            OwnerModuleAll,
        }

        #endregion

        #region Public Structs

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366921.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TcpTable
        {
            public uint length;
            public TcpRow row;
        }

        /// <summary>
        /// <see cref="http://msdn2.microsoft.com/en-us/library/aa366913.aspx"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TcpRow
        {
            public TcpState state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public uint remoteAddr;
            public byte remotePort1;
            public byte remotePort2;
            public byte remotePort3;
            public byte remotePort4;
            public int owningPid;
        }

        #endregion
    }

    #endregion

}

#endregion