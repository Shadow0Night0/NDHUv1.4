#if UNITY_EDITOR
#region "Imports"
using UnityEngine;
using UnityEditor;
#endregion


[CustomEditor(typeof(GSDRoadSystem))]
public class GSDRoadSystemEditor : Editor
{
    //Main target for this editor file:
    protected GSDRoadSystem GSDRS { get { return (GSDRoadSystem) target; } }

    //Serialized properties:
    [UnityEngine.Serialization.FormerlySerializedAs("bTempMultithreading")]
    private SerializedProperty isTempMultithreading;
    [UnityEngine.Serialization.FormerlySerializedAs("bTempSaveMeshAssets")]
    private SerializedProperty isTempSaveMeshAssets;

    //Editor only variables:
    [UnityEngine.Serialization.FormerlySerializedAs("bUpdateGlobal_Multithread")]
    private bool isUpdateGlobalMultithread = false;
    [UnityEngine.Serialization.FormerlySerializedAs("bUpdateGlobal_SaveMesh")]
    private bool isUpdateGlobalSaveMesh = false;

    //	//Editor only camera variables:
    //	private GSDRoadIntersection[] tInters = null;
    //	private int tInterIndex = 0;
    //	private GSDSplineN[] tBridges = null;
    //	private int tBridgesIndex = 0;
    //	private bool bHasBridgeInit = false;
    //	private bool bHasInterInit = false;
    //	private bool bHasDoneEither = false;
    //	private bool bFlipEditorCamera = false;
    //	private float CameraZoomFactor = 1f;
    //	private float CameraHeightOffset = 1f;
    //	private bool bCameraCustomRot = false;
    //	private Vector3 CameraCustomRot = new Vector3(0.5f,0f,-0.5f);

    //Editor only graphic variables:
    private Texture2D LoadBtnBG = null;
    private Texture2D LoadBtnBGGlow = null;
    private Texture2D WarningLabelBG;
    private GUIStyle WarningLabelStyle;
    private GUIStyle GSDLoadButton = null;


    private void OnEnable()
    {
        isTempMultithreading = serializedObject.FindProperty("opt_bMultithreading");
        isTempSaveMeshAssets = serializedObject.FindProperty("opt_bSaveMeshes");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        isUpdateGlobalMultithread = false;
        isUpdateGlobalSaveMesh = false;
        EditorStyles.label.wordWrap = true;
        InitChecks();

        //Add road button:
        RAEditorUtilitys.Line();
        if (GUILayout.Button("Add road", GSDLoadButton, GUILayout.Width(128f)))
        {// || GUILayout.Button(btnLoadText,GSDImageButton,GUILayout.Width(16f))){
            Selection.activeObject = GSDRS.AddRoad();
        }
        RAEditorUtilitys.Line();

        //Update all roads button:
        if (GUILayout.Button("Update all roads", EditorStyles.miniButton, GUILayout.Width(120f)))
        {
            GSDRS.UpdateAllRoads();
        }

        //Multi-threading input:
        isTempMultithreading.boolValue = EditorGUILayout.Toggle("Multi-threading enabled", GSDRS.opt_bMultithreading);
        if (isTempMultithreading.boolValue != GSDRS.opt_bMultithreading)
        {
            isUpdateGlobalMultithread = true;
        }

        //Save mesh assets input:
        isTempSaveMeshAssets.boolValue = EditorGUILayout.Toggle("Save mesh assets: ", GSDRS.opt_bSaveMeshes);
        if (isTempSaveMeshAssets.boolValue != GSDRS.opt_bSaveMeshes)
        {
            isUpdateGlobalSaveMesh = true;
        }
        if (GSDRS.opt_bSaveMeshes || isTempSaveMeshAssets.boolValue)
        {
            GUILayout.Label("WARNING: Saving meshes as assets is very slow and can increase road generation time by several minutes.", WarningLabelStyle);
        }

        //Online manual button:
        GUILayout.Space(4f);
        if (GUILayout.Button("Online manual", EditorStyles.miniButton, GUILayout.Width(120f)))
        {
            // formerly http://microgsd.com/Support/RoadArchitectManual.aspx
            Application.OpenURL("https://github.com/MicroGSD/RoadArchitect/wiki");
        }

        //Offline manual button:
        GUILayout.Space(4f);
        if (GUILayout.Button("Offline manual", EditorStyles.miniButton, GUILayout.Width(120f)))
        {
            Application.OpenURL(GSD.Roads.GSDRoadUtilityEditor.GetRoadArchitectApplicationPath() + "/RoadArchitectManual.htm");
        }

        if (GSDRS.EditorPlayCamera == null)
        {
            GSDRS.EditorCameraSetSingle();
        }
        RAEditorUtilitys.Line();

        //		bHasDoneEither = false;

        //		//View intersection
        //		DoInter();

        //View bridges
        //		DoBridges();
        //		if(bHasDoneEither){
        //			EditorGUILayout.LabelField("* Hotkeys only function when this RoadArchitectSystem object is selected", EditorStyles.miniLabel);
        //		}

        //Hotkey check:
        DoHotKeyCheck();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();

            //Multithreading global change:
            if (isUpdateGlobalMultithread)
            {
                GSDRS.UpdateAllRoads_MultiThreadOptions();
            }

            //Save mesh assets global change:
            if (isUpdateGlobalSaveMesh)
            {
                GSDRS.UpdateAllRoads_SaveMeshesAsAssetsOptions();
            }
        }
    }


    private void InitChecks()
    {
        if (WarningLabelBG == null)
        {
            WarningLabelBG = (Texture2D) AssetDatabase.LoadAssetAtPath(GSD.Roads.GSDRoadUtilityEditor.GetBasePath() + "/Editor/Icons/WarningLabelBG.png", typeof(Texture2D)) as Texture2D;
        }
        if (LoadBtnBG == null)
        {
            LoadBtnBG = (Texture2D) AssetDatabase.LoadAssetAtPath(GSD.Roads.GSDRoadUtilityEditor.GetBasePath() + "/Editor/Icons/otherbg.png", typeof(Texture2D)) as Texture2D;
        }
        if (LoadBtnBGGlow == null)
        {
            LoadBtnBGGlow = (Texture2D) AssetDatabase.LoadAssetAtPath(GSD.Roads.GSDRoadUtilityEditor.GetBasePath() + "/Editor/Icons/otherbg2.png", typeof(Texture2D)) as Texture2D;
        }

        if (GSDLoadButton == null)
        {
            GSDLoadButton = new GUIStyle(GUI.skin.button);
            GSDLoadButton.contentOffset = new Vector2(0f, 1f);
            GSDLoadButton.normal.textColor = new Color(1f, 1f, 1f, 1f);
            GSDLoadButton.normal.background = LoadBtnBG;
            GSDLoadButton.active.background = LoadBtnBGGlow;
            GSDLoadButton.focused.background = LoadBtnBGGlow;
            GSDLoadButton.hover.background = LoadBtnBGGlow;
            GSDLoadButton.fixedHeight = 16f;
            GSDLoadButton.fixedWidth = 128f;
            GSDLoadButton.padding = new RectOffset(0, 0, 0, 0);
        }

        if (WarningLabelStyle == null)
        {
            WarningLabelStyle = new GUIStyle(GUI.skin.textArea);
            WarningLabelStyle.normal.textColor = Color.red;
            WarningLabelStyle.active.textColor = Color.red;
            WarningLabelStyle.hover.textColor = Color.red;
            WarningLabelStyle.normal.background = WarningLabelBG;
            WarningLabelStyle.active.background = WarningLabelBG;
            WarningLabelStyle.hover.background = WarningLabelBG;
            WarningLabelStyle.padding = new RectOffset(8, 8, 8, 8);
        }
    }


    //	private void DoInter(){
    //		//View intersection
    //		if(!bHasInterInit){
    //			bHasInterInit = true;
    //			tInters = (GSDRoadIntersection[])GameObject.FindObjectsOfType(typeof(GSDRoadIntersection));	
    //			if(tInters == null || tInters.Length < 1){
    //				tInterIndex = -1;	
    //				tInters = null;
    //			}
    //		}
    //		if(tInters != null && tInters.Length > 0 && tInterIndex > -1){
    //			EditorGUILayout.BeginHorizontal();
    //			if(GUILayout.Button("View next intersection",GUILayout.Width(150f))){
    //				IncrementIntersection();
    //			}
    //			EditorGUILayout.LabelField("Hotkey K");
    //			EditorGUILayout.EndHorizontal();
    //			bHasDoneEither = true;
    //		}
    //	}
    //	
    //
    //	private void IncrementIntersection(){
    //		if(tInters != null && tInters.Length > 0){
    //			tInterIndex+=1;
    //			if(tInterIndex >= tInters.Length){ tInterIndex = 0; }
    //			ShowIntersection(tInterIndex);
    //		}
    //	}
    //	
    //
    //	private void DoBridges(){
    //		//View bridges
    //		if(!bHasBridgeInit){
    //			bHasBridgeInit = true;
    //			GSDSplineN[] tSplineN = (GSDSplineN[])GameObject.FindObjectsOfType(typeof(GSDSplineN));	
    //			List<GSDSplineN> tSplineNList = new List<GSDSplineN>();
    //			foreach(GSDSplineN tNode in tSplineN){
    //				if(tNode.bIsBridgeStart && tNode.bIsBridgeMatched){
    //					tSplineNList.Add(tNode);
    //				}
    //			}
    //			tBridges = tSplineNList.ToArray();
    //			tBridgesIndex = 0;
    //			if(tBridges == null || tBridges.Length < 1){ 
    //				tBridgesIndex = -1; 
    //				tBridges = null;
    //			}
    //		}
    //		
    //		if(tBridges != null && tBridges.Length > 0 && tBridgesIndex > -1){
    //			EditorGUILayout.BeginHorizontal();
    //			if(GUILayout.Button("View next bridge",GUILayout.Width(150f))){
    //				IncrementBridge();
    //			}
    //			EditorGUILayout.LabelField("Hotkey L");
    //			EditorGUILayout.EndHorizontal();
    //			if(EditorApplication.isPlaying){
    //				bool bChangeChecker = EditorGUILayout.Toggle("Flip camera Y:",bFlipEditorCamera);	
    //				if(bChangeChecker != bFlipEditorCamera){
    //					bFlipEditorCamera = bChangeChecker;
    //					ShowBridge(tBridgesIndex);
    //				}
    //			}
    //			
    //			if(EditorApplication.isPlaying){
    //				float ChangeChecker = EditorGUILayout.Slider("Zoom factor:",CameraZoomFactor,0.02f,10f);
    //				if(!GSDRootUtil.IsApproximately(ChangeChecker,CameraZoomFactor,0.001f)){
    //					CameraZoomFactor = ChangeChecker;
    //					ShowBridge(tBridgesIndex);
    //				}
    //				ChangeChecker = EditorGUILayout.Slider("Height offset:",CameraHeightOffset,0f,8f);
    //				if(!GSDRootUtil.IsApproximately(ChangeChecker,CameraHeightOffset,0.001f)){
    //					CameraHeightOffset = ChangeChecker;
    //					ShowBridge(tBridgesIndex);
    //				}
    //				
    //				bool bChangeChecker = EditorGUILayout.Toggle("Custom camera rot:",bCameraCustomRot);	
    //				if(bChangeChecker != bCameraCustomRot){
    //					bCameraCustomRot = bChangeChecker;
    //					ShowBridge(tBridgesIndex);
    //				}
    //				if(bCameraCustomRot){
    //					Vector3 vChangeChecker = default(Vector3);
    //					vChangeChecker.x = EditorGUILayout.Slider("Rotation X:",CameraCustomRot.x,-1f,1f);
    //					vChangeChecker.z = EditorGUILayout.Slider("Rotation Z:",CameraCustomRot.z,-1f,1f);
    //
    //					if(vChangeChecker != CameraCustomRot){
    //						CameraCustomRot = vChangeChecker;
    //						ShowBridge(tBridgesIndex);
    //					}
    //				}
    //			}
    //			
    //			bHasDoneEither = true;
    //		}
    //	}
    //	
    //
    //	private void IncrementBridge(){
    //		if(tBridges != null && tBridges.Length > 0){
    //			tBridgesIndex+=1;
    //			if(tBridgesIndex >= tBridges.Length){ tBridgesIndex = 0; }
    //			ShowBridge(tBridgesIndex);
    //		}
    //	}
    //	
    //
    //	private void ShowIntersection(int i){	
    //		if(EditorApplication.isPlaying && GSDRS.EditorPlayCamera != null){
    //			GSDRS.EditorPlayCamera.transform.position = tInters[i].transform.position + new Vector3(-40f,20f,-40f);
    //			GSDRS.EditorPlayCamera.transform.rotation = Quaternion.LookRotation(tInters[i].transform.position - (tInters[i].transform.position + new Vector3(-40f,20f,-40f)));
    //		}else{
    //	        SceneView.lastActiveSceneView.pivot = tInters[i].transform.position;
    //	        SceneView.lastActiveSceneView.Repaint();
    //		}
    //	}
    //	
    //
    //	private void ShowBridge(int i){
    //		if(EditorApplication.isPlaying && GSDRS.EditorPlayCamera != null){
    //			Vector3 tBridgePos = ((tBridges[i].pos - tBridges[i].BridgeCounterpartNode.pos)*0.5f)+tBridges[i].BridgeCounterpartNode.pos;
    //			float tBridgeLength = Vector3.Distance(tBridges[i].pos,tBridges[i].BridgeCounterpartNode.pos);
    //			
    //			//Rotation:
    //			Vector3 tCameraRot = Vector3.Cross((tBridges[i].pos - tBridges[i].BridgeCounterpartNode.pos),Vector3.up);
    //			if(bCameraCustomRot){
    //				tCameraRot = CameraCustomRot;
    //			}else{
    //				tCameraRot = tCameraRot.normalized;	
    //			}
    //
    //			//Calc offset:
    //			Vector3 tBridgeOffset = tCameraRot * (tBridgeLength * 0.5f * CameraZoomFactor);
    //			
    //			//Height offset:
    //			tBridgeOffset.y = Mathf.Lerp(20f,120f,(tBridgeLength*0.001f)) * CameraZoomFactor * CameraHeightOffset;
    //			
    //			GSDRS.EditorPlayCamera.transform.position = tBridgePos + tBridgeOffset;
    //			GSDRS.EditorPlayCamera.transform.rotation = Quaternion.LookRotation(tBridgePos - (tBridgePos + tBridgeOffset));
    //		}else{
    //        	SceneView.lastActiveSceneView.pivot = tBridges[i].transform.position;
    //        	SceneView.lastActiveSceneView.Repaint();
    //		}
    //	}


    //	bool bCtrl = false;
    public void OnSceneGUI()
    {
        DoHotKeyCheck();
    }


    private void DoHotKeyCheck()
    {
        bool isUsed = false;
        Event current = Event.current;
        int controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

        //		if(current.type == EventType.KeyDown){
        //			if(current.keyCode == KeyCode.K){
        //				IncrementIntersection();
        //				bUsed = true;
        //			}else if(current.keyCode == KeyCode.L){
        //				IncrementBridge();
        //				bUsed = true;
        //			}
        //		}

        if (isUsed)
        {
            switch (current.type)
            {
                case EventType.Layout:
                    HandleUtility.AddDefaultControl(controlID);
                    break;
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(GSDRS);
        }
    }
}
#endif