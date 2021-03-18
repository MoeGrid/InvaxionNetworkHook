using System;
using System.Runtime.InteropServices;

namespace InvaxionNetworkHook
{
    class EagleTcpCapture
    {
        [DllImport("EagleTcpClient")]
        private static extern bool contectServer(string pszServerIP, int nServerPort, int tag);

        [DllImport("EagleTcpClient")]
        private static extern void disconnectServer(int tag);

        [DllImport("EagleTcpClient")]
        private static extern bool isConnected(int tag);

        [DllImport("EagleTcpClient")]
        private static extern int sendCmd(int tag, uint mainCmd, uint paraCmd, byte[] msgContent, int size);

        [DllImport("EagleTcpClient", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private static extern int parseCmd(int tag, ref int main_cmd, ref int para_cmd, byte[] json_out);

        public static bool ContectServer(string pszServerIP, int nServerPort, int tag)
        {
            var ret = contectServer(pszServerIP, nServerPort, tag);
            MyLogger.Log(string.Format("连接服务器 TAG {0} IP {1} PORT {2} RET {3}", tag, pszServerIP, nServerPort, ret));
            return ret;
        }

        public static bool IsConnected(int tag)
        {
            return isConnected(tag);
        }

        public static void DisconnectServer(int tag)
        {
            disconnectServer(tag);
            MyLogger.Log(string.Format("断开服务器 TAG {0}", tag));
        }

        public static int SendCmd(int tag, uint mainCmd, uint paraCmd, byte[] msgContent, int size)
        {
            var ret = sendCmd(tag, mainCmd, paraCmd, msgContent, size);
            var hex = BitConverter.ToString(msgContent, 0).Replace("-", " ");
            var proto = GetProtoName((int)mainCmd, (int)paraCmd);
            MyLogger.Log(string.Format("发送消息 TAG {0} MAIN_CMD {1} PARA_CMD {2} PROTO {3} DATA \n{4}", tag, mainCmd, paraCmd, proto, hex));
            return ret;
        }

        public static int ParseCmd(int tag, ref int mainCmd, ref int paraCmd, byte[] msgContent)
        {
            var ret = parseCmd(tag, ref mainCmd, ref paraCmd, msgContent);
            if ((mainCmd != 0 && paraCmd != 0) || ret > 0)
            {
                var hex = BitConverter.ToString(msgContent, 0, ret).Replace("-", " ");
                var proto = GetProtoName((int)mainCmd, (int)paraCmd);
                MyLogger.Log(string.Format("接收消息 TAG {0} MAIN_CMD {1} PARA_CMD {2} PROTO {3} DATA \n{4}", tag, mainCmd, paraCmd, proto, hex));
            }
            return ret;
        }

        private static string GetProtoName(int mainCmd, int paraCmd)
        {
            var ret = "";
            if (mainCmd == 1 || mainCmd == 3)
            {
                ret = ((cometGate.ParaCmd)paraCmd).ToString();
            }
            else if (mainCmd == 2)
            {
                ret = ((cometLogin.ParaCmd)paraCmd).ToString();
            }
            else if (mainCmd == 5)
            {
                ret = ((cometScene.ParaCmd)paraCmd).ToString();
            }
            return ret.Replace("ParaCmd_", "");
        }
    }
}
