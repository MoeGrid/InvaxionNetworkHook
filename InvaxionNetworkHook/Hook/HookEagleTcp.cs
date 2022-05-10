using System;
using Harmony;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InvaxionNetworkHook.Hook
{
    class HookEagleTcp
    {

        private static readonly Type HookTargetType = typeof(EagleTcp);
        private static readonly Type HookType = typeof(EagleTcpClient);


        private static IEnumerable<CodeInstruction> GeneralTranspiler(IEnumerable<CodeInstruction> instructions, MethodInfo method, MethodInfo method2)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand == method)
                    yield return new CodeInstruction(OpCodes.Call, method2);
                else
                    yield return instruction;
            }
        }

        public static IEnumerable<CodeInstruction> DotCtorTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo method = AccessTools.Method(HookEagleTcp.HookTargetType, "contectServer");
            MethodInfo method2 = AccessTools.Method(HookEagleTcp.HookType, "ContectServer");

            return GeneralTranspiler(instructions, method, method2);
        }

        public static IEnumerable<CodeInstruction> IsConnectedTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo method = AccessTools.Method(HookEagleTcp.HookTargetType, "isConnected");
            MethodInfo method2 = AccessTools.Method(HookEagleTcp.HookType, "IsConnected");

            return GeneralTranspiler(instructions, method, method2);
        }

        public static IEnumerable<CodeInstruction> DisconnectTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo method = AccessTools.Method(HookEagleTcp.HookTargetType, "disconnectServer");
            MethodInfo method2 = AccessTools.Method(HookEagleTcp.HookType, "DisconnectServer");

            return GeneralTranspiler(instructions, method, method2);
        }

        public static IEnumerable<CodeInstruction> SendCmdTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo method = AccessTools.Method(HookEagleTcp.HookTargetType, "sendCmd");
            MethodInfo method2 = AccessTools.Method(HookEagleTcp.HookType, "SendCmd");

            return GeneralTranspiler(instructions, method, method2);
        }

        public static IEnumerable<CodeInstruction> ParseCmdTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo method = AccessTools.Method(HookEagleTcp.HookTargetType, "parseCmd");
            MethodInfo method2 = AccessTools.Method(HookEagleTcp.HookType, "ParseCmd");

            return GeneralTranspiler(instructions, method, method2);
        }
    }
}
