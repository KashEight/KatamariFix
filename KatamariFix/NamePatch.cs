using HarmonyLib;
using System.Collections.Generic;
using DefineEnum;

namespace KatamariFix
{
    class NamePatch
    {
        /// <summary>
        /// Dictionary for "NameID".
        /// Used for whether "NameID" is attached or not. 
        /// (key: "NameID", value: count)
        /// </summary>
        private static Dictionary<int, int> dic;
        /// <summary>
        /// To emulate the original field `mIsAttachedToKatamariOld` on `AttachableProp` class.
        /// </summary>
        private static Dictionary<AttachableProp, bool> emulateMIsAttachedToKatamariOld = new Dictionary<AttachableProp, bool>();

        /// <summary>
        /// Initialize `NamePatch.dic` like the game.
        /// </summary>
        [HarmonyPatch(typeof(GlobalManager), "gYm_GameInit")]
        [HarmonyPostfix]
        static void InitNameDic()
        {
            dic = new Dictionary<int, int>();
        }

        /// <summary>
        /// Add self instance and set to false to `NamePatch.emulateMIsAttachedToKatamariOld`.
        /// </summary>
        /// <param name="__instance">instance of AttachableProp.</param>
        [HarmonyPatch(typeof(AttachableProp), "Awake")]
        [HarmonyPostfix]
        static void AddInstanceDic(AttachableProp __instance)
        {
            emulateMIsAttachedToKatamariOld.Add(__instance, false);
        }

        /// <summary>
        /// Default `UpdateMono` method on `AttachableProp` class.
        /// Increment or decrement value of key (u8MonoIdNameNo) in the dictionary.
        /// </summary>
        /// <param name="__instance">instance of AttachableProp.</param>
        /// <param name="___gWork">private field of class (GlobalWork).</param>
        [HarmonyPatch(typeof(AttachableProp), "UpdateMono")]
        [HarmonyPostfix]
        static void AddNameToDic(AttachableProp __instance, GlobalWork ___gWork)
        {
            bool isAttached = __instance.mIsAttachedToKatamari;
            if (emulateMIsAttachedToKatamariOld[__instance] != isAttached)
            {
                int key = __instance.u8MonoIdNameNo;
                if (key != 0)
                {
                    if (__instance.mIsAttachedToKatamari)
                    {
                        if (___gWork.u8GameInfoMode == GAMEINFO_MODE.GAMEINFO_MODE_1P)
                        {
                            if (!dic.ContainsKey(key))
                            {
                                dic.Add(key, 0);
                            }
                            dic[key] += 1;
                        }
                    }
                    else
                    {
                        if (dic.ContainsKey(key) && dic[key] > 0)
                        {
                            dic[key] -= 1;
                        }
                    }
                }
                emulateMIsAttachedToKatamariOld[__instance] = isAttached;
            }
        }

        /// <summary>
        /// Default `gYm_GameClearUpdate` method on `GlobalManager` class.
        /// Record "NameID" to a field `u8SwNameMonoCatch` on `SI_GAME` class.
        /// </summary>
        /// <param name="___gWork">private field of class (GlobalWork).</param>
        [HarmonyPatch(typeof(GlobalManager), "gYm_GameClearUpdate")]
        [HarmonyPostfix]
        static void RecordName(GlobalWork ___gWork)
        {
            foreach (var pair in dic)
            {
                if (pair.Key != 0 && pair.Value > 0)
                {
                    ___gWork.siGame.u8SwNameMonoCatch[pair.Key] = 1;
                }
            }
        }

        // should code as tranpile code...
        /// <summary>
        /// Default `OnPageChanged`method on `NameSelector` class.
        /// Display NAMES recorded on a field `u8SwNameMonoCatch` on `SI_GAME` class.
        /// Because return value is always false, original method is skipped. 
        /// </summary>
        /// <param name="__instance">instance of NameSelector</param>
        /// <param name="nowPage">original argument.</param>
        /// <param name="____selectedFirstPage">private field of class.</param>
        /// <param name="____list_selectables">private field of class.</param>
        /// <param name="____list_commonContent">private field of class.</param>
        /// <param name="____gWork">private field of class.</param>
        /// <returns>always false (skip original method)</returns>
        [HarmonyPatch(typeof(NameSelector), "OnPageChanged")]
        [HarmonyPrefix]
        static bool OnPageChanged(NameSelector __instance, int nowPage, ref bool ____selectedFirstPage, List<NameSelectable> ____list_selectables, List<Entity_king_text.CommonContent> ____list_commonContent, GlobalWork ____gWork)
        {
            if (!____selectedFirstPage)
            {
                ____selectedFirstPage = true;
            }
            int num = (nowPage - 1) * ____list_selectables.Count;
            foreach (NameSelectable nameSelectable in ____list_selectables)
            {
                string personID = ____list_commonContent[num].textID;
                int nameMonoID = ____gWork.personName.list.FindIndex(p => p.PERSON_ID.StartsWith(personID));
                nameSelectable.monoID = "SECRET";
                nameSelectable.SetName(string.Empty, false);
                if (____gWork.siGame.u8SwNameMonoCatch[nameMonoID] == 1)
                {
                    nameSelectable.monoID = ____gWork.personName.list[nameMonoID].MONO_ID;
                    nameSelectable.SetName(____list_commonContent[num].content, true);
                }
                num++;
            }
            __instance.SetSelectable(____list_selectables[0]);
            return false;
        }
    }
}
