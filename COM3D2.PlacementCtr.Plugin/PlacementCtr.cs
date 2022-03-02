using BepInEx;
using BepInEx.Logging;
using COM3D2API;
using HarmonyLib;
using LillyUtill.MyWindowRect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COM3D2.PlacementCtr.Plugin
{
    class MyAttribute
    {
        public const string PLAGIN_NAME = "PlacementCtr";
        public const string PLAGIN_VERSION = "22.3.2";
        public const string PLAGIN_FULL_NAME = "COM3D2.PlacementCtr.Plugin";
    }

    [BepInPlugin(MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, MyAttribute.PLAGIN_VERSION)]
    public class PlacementCtr : BaseUnityPlugin
    {
        public static Harmony harmony;
        public static ManualLogSource log;

        public static HashSet<string> personalIds = new HashSet<string>();

        public static WindowRectUtill myWindowRect;
        private static Vector2 scrollPosition;

        public void Awake()
        {
            log = Logger;
            log.LogMessage($"Awake");

            myWindowRect = new WindowRectUtill(Config, MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, "PC");
        }

        public void OnEnable()
        {
            log.LogMessage($"OnEnable");
            try
            {
                if (harmony == null)
                {
                    harmony = Harmony.CreateAndPatchAll(typeof(PlacementCtrPatch));
                }
            }
            catch (Exception e)
            {
                log.LogFatal($"Harmony {e.ToString()}");
            }
            SceneManager.sceneLoaded += this.OnSceneLoaded;
            if (SceneManager.GetActiveScene().name == "ScenePhotoMode")
            {
                isCan = true;
            }
            else
            {
                isCan = false;
            }
        }

        //private PlacementWindow placementWindow;
        private bool isCan;


        public void Start()
        {
            log.LogMessage($"Start");

            SystemShortcutAPI.AddButton(
                MyAttribute.PLAGIN_FULL_NAME
                , new Action(delegate () { myWindowRect.IsGUIOn = !myWindowRect.IsGUIOn; })
                , MyAttribute.PLAGIN_NAME
                , SystemShortcutAPI.ExtractResource(Properties.Resources.icon));
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "ScenePhotoMode")
            {
                //GameObject gameObject2 = GameObject.Find("PlacementWindow");
                //else{this.placementWindow = (gameObject2 ? gameObject2.GetComponent<PlacementWindow>() : null);
                isCan = true;
            }
            else
            {
                isCan = false;
            }
        }

        public void OnGUI()
        {
            if (!myWindowRect.IsGUIOn)
                return;

            myWindowRect.WindowRect = GUILayout.Window(myWindowRect.winNum, myWindowRect.WindowRect, WindowFunction, "", GUI.skin.box);
        }


        public void WindowFunction(int id)
        {
            GUI.enabled = true; // 기능 클릭 가능

            GUILayout.BeginHorizontal();// 가로 정렬
            // 라벨 추가
            GUILayout.Label(myWindowRect.windowName, GUILayout.Height(20));
            // 안쓰는 공간이 생기더라도 다른 기능으로 꽉 채우지 않고 빈공간 만들기
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { myWindowRect.IsOpen = !myWindowRect.IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { myWindowRect.IsGUIOn = false; }
            GUILayout.EndHorizontal();// 가로 정렬 끝

            if (!myWindowRect.IsOpen)
            {

            }
            else
            {
                // 스크롤 영역
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                if (GUILayout.Button("Random Maid Active")) { PlacementCtrPatch.RandomMaid(); }

                GUILayout.EndScrollView();
            }
            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }



        public void OnDisable()
        {
            log.LogMessage($"OnDisable");
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
            harmony?.UnpatchSelf();
            isCan = false;
        }
    }
}
