using Harmony;
using System;
using System.Runtime.CompilerServices;

namespace InvaxionNetworkHook
{
    class MyLogger
    {

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Error(string str)
        {
#if DEBUG
            FileLog.Log(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss][ERROR] ") + str);
#else
            Debug.LogError(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss][ERROR] ") + str);
#endif
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Log(string str)
        {
#if DEBUG
            FileLog.Log(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss][INFO] ") + str);
#else
            Debug.LogError(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss][INFO] ") + str);
#endif
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Warning(string str)
        {
#if DEBUG
            FileLog.Log(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss][WARNING] ") + str);
#else
            Debug.LogError(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss][WARNING] ") + str);
#endif
        }
    }
}
