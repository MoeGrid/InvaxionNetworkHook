using InvaxionNetworkHook.Hook;
using System;

namespace InvaxionNetworkHook
{
    public class Loader
    {
        public static void Main()
        {
            try
            {
                MyLogger.Log("Invaxion network capture!");

                HookManager.getInstance().Create();
            }
            catch (Exception e)
            {
                MyLogger.Error(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
