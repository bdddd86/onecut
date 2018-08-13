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
	Vector3 start_pos = Vector3.zero;
	Vector3 end_pos = Vector3.zero;
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
					AreaAttackData area = dicPattern[selectID].listAttackData [i] as AreaAttackData;
					GUILayout.Label(string.Format("[{0}][광역공격]-지연시간:{1}초-범위:{2}",
						i,area.delay,area.distance), EditorStyles.boldLabel);
					break;
				case eAttackType.eMissile:
					MissileAttackData missile = dicPattern[selectID].listAttackData [i] as MissileAttackData;
					GUILayout.Label(string.Format("[{0}][미사일]-지연시간:{1}초-시작:{2}-끝:{3}-속도:{4}배",
						i,missile.delay,missile.start_pos,missile.end_pos,missile.speed), EditorStyles.boldLabel);
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
					AreaAttackData area = currentPattern.listAttackData [i] as AreaAttackData;
					GUILayout.Label(string.Format("[{0}][광역공격]-지연시간:{1}초-범위:{2}",
						i,area.delay,area.distance), EditorStyles.boldLabel);
					break;
				case eAttackType.eMissile:
					MissileAttackData missile = currentPattern.listAttackData [i] as MissileAttackData;
					GUILayout.Label(string.Format("[{0}][미사일]-지연시간:{1}초-시작:{2}-끝:{3}-속도:{4}배",
						i,missile.delay,missile.start_pos,missile.end_pos,missile.speed), EditorStyles.boldLabel);
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
				start_pos = EditorGUILayout.Vector3Field ("시작점", start_pos);
				end_pos = EditorGUILayout.Vector3Field ("도착점", end_pos);
				speed = EditorGUILayout.Slider ("속도(배율)", speed, 0.1f, 10f);
			} else {
				GUILayout.Label("오류 상황. 창을 닫았다가 열어주세요.", EditorStyles.boldLabel);
			}
			if (selectType >= 0 && selectType <= 2) {
				if (GUILayout.Button ("리스트에 추가")) {
					if (selectType == 0) {
						currentPattern.listAttackData.Add (new AttackData (delay));
					} else if (selectType == 1) {
						currentPattern.listAttackData.Add (new AreaAttackData (delay, distance));
					} else if (selectType == 2) {
						currentPattern.listAttackData.Add (new MissileAttackData (delay, start_pos, end_pos, speed));
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
	}

	void SavePattern()
	{
		if (dicPattern.ContainsKey (selectID)) {
			dicPattern [selectID] = currentPattern;
		} else {
			dicPattern.Add (selectID, new AttackPattern(selectID, currentPattern.listAttackData));
		}

		currentPattern.listAttackData.Clear ();
	}

	void SaveListPattern()
	{
		// json string 으로 변환해서 파일로 저장하기.
	}
}