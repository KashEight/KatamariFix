using HarmonyLib;
using MyGame;
using System.Collections.Generic;
using UnityEngine;

namespace KatamariFixDebug
{
    [HarmonyPatch(typeof(InputController))]
    class DisableClick
    {
        static bool IsMousePush(int pad_index, bool isMouse, bool isLeft) => pad_index == 0 && isMouse && Input.GetMouseButton(isLeft ? 0 : 1);

        static bool IsMouseDown(int pad_index, bool isMouse, bool isLeft) => pad_index == 0 && isMouse && Input.GetMouseButtonDown(isLeft ? 0 : 1);

        [HarmonyPatch(nameof(InputController.IsSelectDown))]
        [HarmonyPostfix]
        static void DisableIsSelectDown(ref bool __result, int pad_index = 0, bool isMouse = true)
        {
            if (IsMouseDown(pad_index, isMouse, true))
            {
                __result = false;
            }
        }

        [HarmonyPatch(nameof(InputController.IsSelectPush))]
        [HarmonyPostfix]
        static void DisableIsSelectPush(ref bool __result, int pad_index = 0, bool isMouse = true)
        {
            if (IsMousePush(pad_index, isMouse, true))
            {
                __result = false;
            }
        }

        [HarmonyPatch(nameof(InputController.IsChancel))]
        [HarmonyPostfix]
        static void DisableIsCancelPush(ref bool __result, int pad_index = 0, bool isMouse = true)
        {
            if (IsMousePush(pad_index, isMouse, false))
            {
                __result = false;
            }
        }

        [HarmonyPatch(nameof(InputController.IsChancelDown))]
        [HarmonyPostfix]
        static void DisableIsCancelDown(ref bool __result, int pad_index = 0, bool isMouse = true)
        {
            if (IsMouseDown(pad_index, isMouse, false))
            {
                __result = false;
            }
        }
    }
}
