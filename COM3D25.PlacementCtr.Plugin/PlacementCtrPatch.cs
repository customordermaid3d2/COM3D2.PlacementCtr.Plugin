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
        internal static MethodInfo SetSelectMaid;

        [HarmonyPatch(typeof(PlacementWindow), "Start")]
        [HarmonyPostfix]
        public static void Start(PlacementWindow __instance)//, List<PlacementWindow.PlateData> maid_data_list_
        {
            placementWindow = __instance;
            //UIGrid component = UTY.GetChildObject(__instance.content_game_object, "ListParent/Contents/UnitParent", false).GetComponent<UIGrid>();
            //UIScrollView component2 = UTY.GetChildObject(__instance.content_game_object, "ListParent/Contents", false).GetComponent<UIScrollView>();
            // UI Root/MainScreen/PhotoWindowManager/PlacementWindow/Parent/ContentParent/ListParent/Contents/UnitParent/SimpleMaidPlatePlacement(Clone)/Plate/            
            SetSelectMaid = placementWindow.GetType().GetMethod("SetSelectMaid", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static void RandomMaid()
        {

            var maids=placementWindow.maid_list;
            //placementWindow.SetSelectMaid();
            SetSelectMaid.Invoke(placementWindow, new object[] { maids[UnityEngine.Random.Range(0, maids.Count - 1)] });

        }
    }
}
