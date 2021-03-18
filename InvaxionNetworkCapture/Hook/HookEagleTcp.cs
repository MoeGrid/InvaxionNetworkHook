using System;
using Harmony;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InvaxionNetworkHook.Hook
{
    class HookEagleTcp
    {
        private static readonly Type HookTargetType = typeof(EagleTcp);
        private static readonly Type HookType = typeof(EagleTcpCapture);

        public static IEnumerable<CodeInstruction> DotCtorTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var method = AccessTools.Method(HookTargetType, "contectServer");
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
            var method = AccessTools.Method(HookTargetType, "isConnected");
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
            var method = AccessTools.Method(HookTargetType, "disconnectServer");
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
            var method = AccessTools.Method(HookTargetType, "sendCmd");
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
            var method = AccessTools.Method(HookTargetType, "parseCmd");
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
