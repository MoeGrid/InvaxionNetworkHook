using Harmony;
using System;

namespace InvaxionNetworkHook.Hook
{
    class HookManager
    {
        private static HookManager _instance;

        public static HookManager getInstance()
        {
            if (_instance == null)
                _instance = new HookManager();
            return _instance;
        }

        public void Create()
        {
            var instance = HarmonyInstance.Create("hook");

            // .ctor
            var EagleTcpDotCtor = AccessTools.Constructor(typeof(EagleTcp), new Type[] {
                typeof(EagleTcp.CSocketType),
                typeof(string),
                typeof(uint),
            });
            var EagleTcpDotCtorTranspiler = AccessTools.Method(typeof(HookEagleTcp), "DotCtorTranspiler");
            instance.Patch(EagleTcpDotCtor, null, null, new HarmonyMethod(EagleTcpDotCtorTranspiler));

            // IsConnected
            var EagleTcpIsConnected = AccessTools.Method(typeof(EagleTcp), "IsConnected");
            var EagleTcpIsConnectedTranspiler = AccessTools.Method(typeof(HookEagleTcp), "IsConnectedTranspiler");
            instance.Patch(EagleTcpIsConnected, null, null, new HarmonyMethod(EagleTcpIsConnectedTranspiler));

            // Disconnect
            var EagleTcpDisconnect = AccessTools.Method(typeof(EagleTcp), "Disconnect");
            var EagleTcpDisconnectTranspiler = AccessTools.Method(typeof(HookEagleTcp), "DisconnectTranspiler");
            instance.Patch(EagleTcpDisconnect, null, null, new HarmonyMethod(EagleTcpDisconnectTranspiler));

            // SendCmd
            var EagleTcpSendCmd = AccessTools.Method(typeof(EagleTcp), "SendCmd");
            var EagleTcpSendCmdTranspiler = AccessTools.Method(typeof(HookEagleTcp), "SendCmdTranspiler");
            instance.Patch(EagleTcpSendCmd, null, null, new HarmonyMethod(EagleTcpSendCmdTranspiler));

            // ParseCmd
            var EagleTcpParseCmd = AccessTools.Method(typeof(EagleTcp), "ParseCmd");
            var EagleTcpParseCmdTranspiler = AccessTools.Method(typeof(HookEagleTcp), "ParseCmdTranspiler");
            instance.Patch(EagleTcpParseCmd, null, null, new HarmonyMethod(EagleTcpParseCmdTranspiler));

            MyLogger.Log("All OK!");
        }
    }
}
