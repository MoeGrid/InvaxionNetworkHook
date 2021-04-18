using Aquatrax;
using Harmony;
using System.Collections.Generic;

namespace InvaxionNetworkHook.Hook
{
    class HookGlobalConfig
    {
        public static void InitPostfix(GlobalConfig __instance)
        {
            MyLogger.Log("GlobalConfig Hook OK!");

            // 在这里修改配置
            var config = Traverse.Create(__instance).Field<JsonConfig>("jsonConfig").Value;

            // 登陆服务器 prod-steam-invaxion.bilibiligame.net:20017
            config.Login.HostList = new List<HostPortData>()
            {
                new HostPortData()
                {
                    host="42.193.125.167",
                    port=60311
                }
            };
        }

    }
}
