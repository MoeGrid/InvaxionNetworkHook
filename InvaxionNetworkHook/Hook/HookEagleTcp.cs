using System;
using Harmony;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InvaxionNetworkHook.Hook
{
    class HookEagleTcp
    {
        // 选择Hook类
        // EagleTcpClient 明文协议
        // EagleTcpCapture 抓包器
        private static readonly Type HookType = typeof(EagleTcpClient);

        public static IEnumerable<CodeInstruction> DotCtorTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var method = AccessTools.Method(typeof(EagleTcp), "contectServer");
            var method2 = AccessTools.Method(HookType, "ContectServer");

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand == method)
                    yield return new CodeInstruction(OpCodes.Call, method2);
                else
                    yield return instruction;
            }
        }

        public static IEnumerable<CodeInstruction> IsConnectedTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var method = AccessTools.Method(typeof(EagleTcp), "isConnected");
            var method2 = AccessTools.Method(HookType, "IsConnected");

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand == method)
                    yield return new CodeInstruction(OpCodes.Call, method2);
                else
                    yield return instruction;
            }
        }

        public static IEnumerable<CodeInstruction> DisconnectTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var method = AccessTools.Method(typeof(EagleTcp), "disconnectServer");
            var method2 = AccessTools.Method(HookType, "DisconnectServer");

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand == method)
                    yield return new CodeInstruction(OpCodes.Call, method2);
                else
                    yield return instruction;
            }
        }

        public static IEnumerable<CodeInstruction> SendCmdTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var method = AccessTools.Method(typeof(EagleTcp), "sendCmd");
            var method2 = AccessTools.Method(HookType, "SendCmd");

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand == method)
                    yield return new CodeInstruction(OpCodes.Call, method2);
                else
                    yield return instruction;
            }
        }

        public static IEnumerable<CodeInstruction> ParseCmdTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var method = AccessTools.Method(typeof(EagleTcp), "parseCmd");
            var method2 = AccessTools.Method(HookType, "ParseCmd");

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand == method)
                    yield return new CodeInstruction(OpCodes.Call, method2);
                else
                    yield return instruction;
            }
        }

    }
}
