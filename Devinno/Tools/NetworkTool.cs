using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Tools
{
    public class NetworkTool
    {
        #region ValidIPv4
        /// <summary>
        /// 유효한 IPv4 주소인가?
        /// </summary>
        /// <param name="address">IP 주소</param>
        /// <returns>유효 여부</returns>
        public static bool ValidIPv4(string address)
        {
            byte n;
            return Uri.CheckHostName(address) == UriHostNameType.IPv4 && address.Split('.').Where(x => byte.TryParse(x, out n)).Count() == 4;
        }
        #endregion
        #region ValidDomain
        /// <summary>
        /// 유효한 도메인인가?
        /// </summary>
        /// <param name="address">도메인 or IP 주소</param>
        /// <returns>유효 여부</returns>
        public static bool ValidDomain(string address)
        {
            bool ret = false;
            try
            {
                if (Uri.CheckHostName(address) == UriHostNameType.IPv4)
                {
                    ret = ValidIPv4(address);
                }
                else
                {
                    var r = Dns.GetHostEntry(address);
                    ret = r.AddressList.Count() > 0;
                }
            }
            catch { ret = false; }
            return ret;
        }
        #endregion
        #region GetLocalIP
        /// <summary>
        /// 현재 IP 주소 획득
        /// </summary>
        /// <returns>IP주소</returns>
        public static string GetLocalIP()
        {
            string localIP = "UNKNOWN";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        #endregion
        #region SetLocalIP
        /// <summary>
        /// 지정한 NIC의 IP, SubnetMask, Gateway 설정
        /// </summary>
        /// <param name="description">NIC Description</param>
        /// <param name="ip">설정할 IP</param>
        /// <param name="subnet">설정할 SubnetMask</param>
        /// <param name="gateway">설정할 Gateway</param>
        /// <returns>설정 결과</returns>
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public static bool SetLocalIP(string description, string ip, string subnet, string gateway)
        {
            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                string _description = managementObject["Description"] as string;

                if (string.Compare(_description, description, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    try
                    {
                        ManagementBaseObject setGatewaysManagementBaseObject = managementObject.GetMethodParameters("SetGateways");
                        setGatewaysManagementBaseObject["DefaultIPGateway"] = new string[] { gateway };
                        setGatewaysManagementBaseObject["GatewayCostMetric"] = new int[] { 1 };

                        ManagementBaseObject enableStaticManagementBaseObject = managementObject.GetMethodParameters("EnableStatic");
                        enableStaticManagementBaseObject["IPAddress"] = new string[] { ip };
                        enableStaticManagementBaseObject["SubnetMask"] = new string[] { subnet };

                        managementObject.InvokeMethod("EnableStatic", enableStaticManagementBaseObject, null);
                        managementObject.InvokeMethod("SetGateways", setGatewaysManagementBaseObject, null);

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
        #region SetDHCP
        /// <summary>
        /// 지정한 NIC의 DHCP 설정
        /// </summary>
        /// <param name="description">NIC Description</param>
        /// <returns>설정 결과</returns>
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public static bool SetDHCP(string description) => SetLocalIP(description, null, null, null);
        #endregion
        #region GetNicDescriptions
        /// <summary>
        /// NIC Description 배열 획득
        /// </summary>
        /// <returns>NIC Description 배열</returns>
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public string[] GetNicDescriptions()
        {
            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            var ls = new List<string>();
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                ls.Add(managementObject["Description"] as string);
            }

            return ls.ToArray();
        }
        #endregion
        #region IsSocketConnected
        /// <summary>
        /// 해당 소켓이 접속중인지 확인
        /// </summary>
        /// <param name="s">소켓</param>
        /// <param name="PollTime">Poll 타임아웃시간</param>
        /// <returns>접속 여부</returns>
        public static bool IsSocketConnected(Socket s, int PollTime = 1000) {

            try
            {
                if (s != null)
                {
                    return !(s.Poll(PollTime, SelectMode.SelectRead) && s.Available == 0);
                }
                else return false;
            }
            catch (SocketException)
            {
                return false;
            }
        }
        #endregion
    }
}
