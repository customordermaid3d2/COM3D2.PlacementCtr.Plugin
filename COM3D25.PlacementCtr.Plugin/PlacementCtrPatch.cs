using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace COM3D2.PlacementCtr.Plugin
{
    internal static class PlacementCtrPatch
    {
        internal static PlacementWindow placementWindow;
        //internal static MethodInfo setSelectMaid;
        internal static MethodInfo onClickPlacementBtn;
        internal static UIGrid UnitParent;
        internal static UIButton[] btns;

        [HarmonyPatch(typeof(PlacementWindow), "Start")]
        [HarmonyPostfix]
        public static void Start(PlacementWindow __instance)//, List<PlacementWindow.PlateData> maid_data_list_
        {
            PlacementCtr.log.LogMessage("PlacementWindow.Start");
            placementWindow = __instance;
            UnitParent = UTY.GetChildObject(__instance.content_game_object, "ListParent/Contents/UnitParent", false).GetComponent<UIGrid>();

            btns =UnitParent.GetComponentsInChildren<UIButton>();
            PlacementCtr.log.LogMessage($"PlacementWindow.Start {btns.Length}");

            //UIScrollView component2 = UTY.GetChildObject(__instance.content_game_object, "ListParent/Contents", false).GetComponent<UIScrollView>();
            // UI Root/MainScreen/PhotoWindowManager/PlacementWindow/Parent/ContentParent/ListParent/Contents/UnitParent/SimpleMaidPlatePlacement(Clone)/Plate/            
            //setSelectMaid = placementWindow.GetType().GetMethod("SetSelectMaid", BindingFlags.Instance | BindingFlags.NonPublic);
            onClickPlacementBtn = placementWindow.GetType().GetMethod("OnClickPlacementBtn", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [HarmonyPatch(typeof(PlacementWindow), "SetSelectMaid")]
        [HarmonyPrefix]
        public static void SetSelectMaid(Maid maid)
        {
            PlacementCtr.log.LogMessage($"SetSelectMaid {maid.status.fullNameEnStyle}");
        }

        public static void RandomMaid()
        {
            UIButton.current = btns[UnityEngine.Random.Range(0, btns.Length - 1)];
            onClickPlacementBtn.Invoke(placementWindow, new object[] { });
            /*
            var maids=placementWindow.maid_list;
            //placementWindow.SetSelectMaid();
            var maid = maids[UnityEngine.Random.Range(0, maids.Count - 1)];
            PlacementCtr.log.LogMessage($"RandomMaid {maid.status.fullNameEnStyle}");
            setSelectMaid.Invoke(placementWindow, new object[] { maid });
            */
        }
    }
}
