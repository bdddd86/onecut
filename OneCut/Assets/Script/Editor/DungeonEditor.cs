using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DungeonEditor : EditorWindow
{
	int selectID = 0;
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	int selectType = 0;
	string[] types = new string[]{"시간지연","광역공격","미사일공격"};
	float delay = 0;
	float distance = 0;
	//Vector3 start_pos = Vector3.zero;
	//Vector3 end_pos = Vector3.zero;
	int dir_right = 0;
	int dir_top = 0;
	float speed = 1;

	AttackPattern currentPattern = new AttackPattern (0, new List<AttackData> ());
	Dictionary<int, AttackPattern> dicPattern = new Dictionary<int, AttackPattern>();

	[MenuItem("Tools/DungeonEditor")]
	static void Init()
	{
		DungeonEditor window = (DungeonEditor)EditorWindow.GetWindow(typeof(DungeonEditor));
		window.Show();
	}

	void OnGUI()
	{
		GUILayout.Label("[던전 공격 패턴 셋팅]", EditorStyles.boldLabel);
		GUILayout.Label("아이디를 선택해서 데이터를 추가하거나 수정하세요.", EditorStyles.boldLabel);
		selectID = EditorGUILayout.IntField("ID", selectID);
		if (dicPattern.ContainsKey (selectID)) {
			for (int i = 0; i < dicPattern[selectID].listAttackData.Count; i++) {
				GUILayout.BeginHorizontal ();
				switch (dicPattern[selectID].listAttackData [i].type) {
				case eAttackType.eDelay:
					GUILayout.Label(string.Format("[{0}][지연시간]-{1}초",
						i,dicPattern[selectID].listAttackData[i].delay), EditorStyles.boldLabel);
					break;
				case eAttackType.eAreaEx:
					AttackData area = dicPattern[selectID].listAttackData [i];
					GUILayout.Label(string.Format("[{0}][광역공격]-지연시간:{1}초-범위:{2}",
						i,area.delay,area.distance), EditorStyles.boldLabel);
					break;
				case eAttackType.eMissile:
					AttackData missile = dicPattern[selectID].listAttackData [i];
					GUILayout.Label(string.Format("[{0}][미사일]-지연시간:{1}초-오른쪽:{2}-위쪽:{3}-속도:{4}배",
						i,missile.delay,missile.dir_right,missile.dir_top,missile.speed), EditorStyles.boldLabel);
					break;
				}
				GUILayout.EndHorizontal ();
			}
		}
		else {
			// 정보가 없으니 새로 추가하시오.
			GUILayout.Box(GUIContent.none, GUILayout.Width(Screen.width), GUILayout.Height(2f));
			for (int i = 0; i < currentPattern.listAttackData.Count; i++) {
				GUILayout.BeginHorizontal ();
				switch (currentPattern.listAttackData [i].type) {
				case eAttackType.eDelay:
					GUILayout.Label(string.Format("[{0}][지연시간]-{1}초",
						i,currentPattern.listAttackData[i].delay), EditorStyles.boldLabel);
					break;
				case eAttackType.eAreaEx:
					AttackData area = currentPattern.listAttackData [i];
					GUILayout.Label(string.Format("[{0}][광역공격]-지연시간:{1}초-범위:{2}",
						i,area.delay,area.distance), EditorStyles.boldLabel);
					break;
				case eAttackType.eMissile:
					AttackData missile = currentPattern.listAttackData [i];
					GUILayout.Label(string.Format("[{0}][미사일]-지연시간:{1}초-오른쪽:{2}-위쪽:{3}-속도:{4}배",
						i,missile.delay,missile.dir_right,missile.dir_top,missile.speed), EditorStyles.boldLabel);
					break;
				}
				GUIStyle xButtonStyle = new GUIStyle (GUI.skin.button);
				xButtonStyle.normal.textColor = Color.black;
				if (GUILayout.Button ("수정", xButtonStyle)) {
					// 나중에 추가
				}
				xButtonStyle.normal.textColor = Color.red;
				if (GUILayout.Button ("삭제", xButtonStyle)) {
					currentPattern.listAttackData.RemoveAt (i);
				}
				GUILayout.EndHorizontal ();
			}
			if (currentPattern.listAttackData.Count > 0) {
				GUIStyle saveButtonStyle = new GUIStyle (GUI.skin.button);
				saveButtonStyle.normal.textColor = Color.blue;
				if (GUILayout.Button ("아이디에 패턴정보 저장", saveButtonStyle)) {
					SavePattern ();
				}
			}
			GUILayout.Box(GUIContent.none, GUILayout.Width(Screen.width), GUILayout.Height(2f));

			selectType = EditorGUILayout.Popup("공격타입", selectType, types);
			if (selectType == 0) {	// 시간지연
				delay = EditorGUILayout.Slider ("지연시간(초)", delay, 0, 10);
			} else if (selectType == 1) {	// 광역공격
				delay = EditorGUILayout.Slider ("지연시간(초)", delay, 0, 10);
				distance = EditorGUILayout.Slider ("공격범위", distance, 1, 16);
			} else if (selectType == 2) {	// 미사일공격
				delay = EditorGUILayout.Slider ("지연시간(초)", delay, 0, 10);
				dir_right = EditorGUILayout.IntField ("오른쪽", dir_right);
				dir_top = EditorGUILayout.IntField ("위쪽", dir_top);
				speed = EditorGUILayout.Slider ("속도(배율)", speed, 0.1f, 10f);
			} else {
				GUILayout.Label("오류 상황. 창을 닫았다가 열어주세요.", EditorStyles.boldLabel);
			}
			if (selectType >= 0 && selectType <= 2) {
				if (GUILayout.Button ("리스트에 추가")) {
					if (selectType == 0) {
						currentPattern.listAttackData.Add (new AttackData (delay));
					} else if (selectType == 1) {
						currentPattern.listAttackData.Add (new AttackData (delay, distance));
					} else if (selectType == 2) {
						currentPattern.listAttackData.Add (new AttackData (delay, dir_right, dir_top, speed));
					}
				}
			}
		}

		groupEnabled = EditorGUILayout.BeginToggleGroup("도움말", groupEnabled);
		if (groupEnabled) {
			GUILayout.Label("공격타입? 공격의 종류 선택", EditorStyles.boldLabel);
			GUILayout.Label("지연시간(초)? 해당 공격이 일어나기 전 딜레이. 만약 1초를 입력하면 1초뒤에 발동.", EditorStyles.boldLabel);
			GUILayout.Label("공격범위? 광역공격의 범위. 한 화면 가득이 16.", EditorStyles.boldLabel);
			GUILayout.Label("시작점? 미사일 공격의 시작점.", EditorStyles.boldLabel);
			GUILayout.Label("도착점? 미사일 공격의 도착점", EditorStyles.boldLabel);
			GUILayout.Label("속도(배율)? 미사일 공격의 속도", EditorStyles.boldLabel);
		}
		EditorGUILayout.EndToggleGroup();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("파일 로드")) {
			LoadFile ();
		}
		if (GUILayout.Button ("파일 세이브")) {
			SaveFile ();
		}
		GUILayout.EndHorizontal ();
	}

	void SavePattern()
	{
		if (dicPattern.ContainsKey (selectID)) {
			dicPattern [selectID] = currentPattern;
		} else {
			dicPattern.Add (selectID, new AttackPattern(selectID, currentPattern.listAttackData));
		}

		currentPattern.listAttackData.Clear ();

		SaveListPattern ();
	}

	void SaveListPattern()
	{
		// json string 으로 변환해서 임시저장.
		PlayerPrefs.SetString ("SavePatternIDs", string.Empty);
		foreach (var pattern in dicPattern.Values) {
			object json = JsonUtility.ToJson (pattern);
			Debug.Log (json.ToString());
			// 임시저장
			PlayerPrefs.SetString (string.Format ("Pattern_{0}", pattern.ID), json.ToString ());
			string savePatternIDs = PlayerPrefs.GetString ("SavePatternIDs", string.Empty);
			if (string.IsNullOrEmpty (savePatternIDs) == true) {
				PlayerPrefs.SetString ("SavePatternIDs", pattern.ID.ToString ());
			} else {
				PlayerPrefs.SetString ("SavePatternIDs", string.Format ("{0},{1}", savePatternIDs, pattern.ID));
			}
		}
	}

	void LoadListPattern()
	{
		dicPattern.Clear ();
		string savePatternIDs = PlayerPrefs.GetString ("SavePatternIDs", string.Empty);
		Debug.Log ("SavePatternIDs: " + savePatternIDs);
		if (string.IsNullOrEmpty (savePatternIDs) == false) {
			string[] split = savePatternIDs.Split (',');
			for (int i = 0; i < split.Length; i++) {
				string strJson = PlayerPrefs.GetString (string.Format ("Pattern_{0}", split[i]), string.Empty);
				Debug.Log (i.ToString() + ": " + strJson);
				AttackPattern attackPattern = JsonUtility.FromJson<AttackPattern> (strJson);
				if (attackPattern == null) {
					Debug.Log ("error null");
					continue;
				}
				if (dicPattern.ContainsKey (attackPattern.ID) == false) {
					dicPattern.Add (attackPattern.ID, attackPattern);
				}
			}
		}
	}

	void SaveFile()
	{
		// 임시저장된 내용을 파일로 저장.
		string folderPath = string.Format("{0}/{1}",Application.dataPath,"JsonData");
		Debug.Log (folderPath);
		if (System.IO.Directory.Exists (folderPath) == false) {
			System.IO.Directory.CreateDirectory (folderPath);
		}

		// 아이디 저장
		string savePatternIDs = PlayerPrefs.GetString ("SavePatternIDs", string.Empty);
		string IDsFilePath = string.Format("{0}/AttackPattern_IDs.json",folderPath);
		System.IO.File.WriteAllText (IDsFilePath, savePatternIDs, System.Text.Encoding.Default);

		// 패턴정보 저장
		if (string.IsNullOrEmpty (savePatternIDs) == false) {
			string[] split = savePatternIDs.Split (',');
			for (int i = 0; i < split.Length; i++) {
				string strJson = PlayerPrefs.GetString (string.Format ("Pattern_{0}", split[i]), string.Empty);
				Debug.Log (i.ToString() + ": " + strJson);
				if (string.IsNullOrEmpty (strJson) == false) {
					string filePath = string.Format("{0}/Pattern_{1}.json",folderPath,split[i]);
					System.IO.File.WriteAllText (filePath, strJson, System.Text.Encoding.Default);
				}
			}
		}
	}

	void LoadFile()
	{
		string folderPath = string.Format("{0}/{1}",Application.dataPath,"JsonData");
		Debug.Log (folderPath);
		if (System.IO.Directory.Exists (folderPath) == false) {
			Debug.LogError ("저장 경로 오류");
			return;
		}

		// 아이디 파일 읽음
		string IDsFilePath = string.Format("{0}/AttackPattern_IDs.json",folderPath);
		string savePatternIDs = System.IO.File.ReadAllText (IDsFilePath);
		PlayerPrefs.SetString ("SavePatternIDs", savePatternIDs);

		// 패턴 데이터 파일 읽음
		if (string.IsNullOrEmpty (savePatternIDs) == false) {
			string[] split = savePatternIDs.Split (',');
			for (int i = 0; i < split.Length; i++) {
				string filePath = string.Format("{0}/Pattern_{1}.json",folderPath,split[i]);
				string strJson = System.IO.File.ReadAllText (filePath);
				Debug.Log (split[i] + ": " + strJson);
				if (string.IsNullOrEmpty (strJson) == false) {
					PlayerPrefs.SetString (string.Format ("Pattern_{0}", split[i]), strJson);
				}
			}
		}

		LoadListPattern ();
	}
}