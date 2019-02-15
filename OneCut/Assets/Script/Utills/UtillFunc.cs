using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class UtillFunc : Singleton<UtillFunc> 
{
	// Character.json
	class CharacterJSON
	{
		public float default_agility;		// 기본 민첩성
		public float level_per_agility; 	// 레벨 당 민첩 증가량
		public float max_damage_add; 		// 최소 최대 데미지 차이
		public float default_hp; 			// 기본 체력
		public float level_per_hp; 			// 레벨 당 체력 증가량
		public float armor_per_hppercent; 	// 1 방어력 = 체력 N%
		public float level_per_exp;			// 레벨 당 경험치 필요량

		public CharacterJSON(JSONNode data)
		{
			default_agility = data["default_agility"].AsFloat;
			level_per_agility = data["level_per_agility"].AsFloat;
			max_damage_add = data["max_damage_add"].AsFloat;
			default_hp = data["default_hp"].AsFloat;
			level_per_hp = data["level_per_hp"].AsFloat;
			armor_per_hppercent = data["armor_per_hppercent"].AsFloat;
			level_per_exp = data["level_per_exp"].AsFloat;
		}
	}
	private CharacterJSON m_jsonChar; 

	// 아이템으로 증가되는 능력치 정리. 20180717 준범.
	public int m_itemArmor = 0;		// 방어력
	public int m_itemAttack = 0;	// 공격력
	public int m_itemHitpoint = 0;	// 체력

	// 초기화 함수.
	public void init()
	{
		// Character.json
		string jsonString = GetJsonString ("Data/Character");
		JSONNode root = JSON.Parse (jsonString);
		m_jsonChar = new CharacterJSON(root["character"]);
	}

	public string GetJsonString(string fullPath)
	{
		if (string.IsNullOrEmpty (fullPath)) {
			return string.Empty;
		}

		//Load texture from disk
		TextAsset bindata = Resources.Load(fullPath) as TextAsset;
		Debug.Log(bindata.text);
		return bindata.text;
	}

	// 민뎀
	public int GetMinAttack(int lv)
	{
		float agility = m_jsonChar.default_agility + ((lv-1) * m_jsonChar.level_per_agility);
		return System.Convert.ToInt32 (agility);
	}
	// 멕뎀
	public int GetMaxAttack(int lv)
	{
		return GetMinAttack (lv) + System.Convert.ToInt32(m_jsonChar.max_damage_add);
	}
	// 체력
	public int GetHitPoints(int lv)
	{
		return System.Convert.ToInt32((lv * m_jsonChar.level_per_hp) + m_jsonChar.default_hp);
	}
	// 방어력
	public int GetArmor(int lv)
	{
		return GetHitPoints (lv) / 100;
	}
	// 방어력 1은 최대체력의 6%증가와 같음.
	// 아머로 인한 데미지 감소가 적용된 후 데미지.
	// For positive Armor, damage reduction =((armor)*0.06)/(1+0.06*(armor))
	public int GetDamageReduction(int lv, int damage, int addArmor)
	{
		int armor = GetArmor (lv) + addArmor;
		float damageReduction = 1f - (((armor) * m_jsonChar.armor_per_hppercent * 0.01f) / (1f + m_jsonChar.armor_per_hppercent * 0.01f * (armor)));
		float realDamage = damage * damageReduction;
		return System.Convert.ToInt32 (realDamage);
	}
	// 경험치로 레벨 구하기.
	// 1->2: 100+1*100 = 200
	// 2->3: 100+2*100 = 300 -> 500
	// 3->4: 100+3*100 = 400 -> 900
	public int GetTotalExpFromLv(int lv)
	{
		int totalExp = 0;
		for(int i=1; i<lv; i++)
		{
			totalExp += System.Convert.ToInt32(m_jsonChar.level_per_exp + (m_jsonChar.level_per_exp * i));
		}
		return totalExp;
	}
	// 다음레벨에 필요한 경험치.
	public int GetNeedExpToNextLv(int lv)
	{
		return System.Convert.ToInt32(m_jsonChar.level_per_exp + (m_jsonChar.level_per_exp * lv));
	}



	// 몬스터가 주는 경험치.
	public int GetMonsterExp(int level)
	{
		if (level <= 1) {
			return 25;	// 피젼트
		} else if (level <= 5) {
			return 30 + ((level - 2) * 2);	// 밀리샤
		} else if (level <= 9) {
			return 40 + ((level - 6) * 2); 	// 풋맨
		} else if (level <= 13) {
			return 60 + ((level - 10) * 2);	// 라이플맨
		} else if (level <= 17) {
			return 70 + ((level - 14) * 2);	// 스펠브레이커
		} else if (level <= 21) {
			return 80 + ((level - 18) * 2);	// 시즈엔진
		} else if (level <= 25) {
			return 90 + ((level - 22) * 2);	// 드래곤호크라이더
		} else if (level <= 26) {
			return 95;	// 워터엘리멘탈3레벨
		} else if (level <= 30) {
			return 100 + ((level - 27) * 2);	// 그리폰라이더
		} else if (level <= 31) {
			return 110;	// 피닉스
		} else if (level <= 35) {
			return 120 + ((level - 32) * 2);	// 나이트
		} else {
			return 120;
		}
	}
	// 몬스터의 체력
	public int GetMonsterLife(int level)
	{
		if (level <= 1) {
			return 220;	// 피젼트
		} else if (level <= 5) {
			return 220;	// 밀리샤
		} else if (level <= 9) {
			return 420; // 풋맨
		} else if (level <= 13) {
			return 535;	// 라이플맨
		} else if (level <= 17) {
			return 600;	// 스펠브레이커
		} else if (level <= 21) {
			return 700;	// 시즈엔진
		} else if (level <= 25) {
			return 725;	// 드래곤호크라이더
		} else if (level <= 26) {
			return 900;	// 워터엘리멘탈3레벨
		} else if (level <= 30) {
			return 975;	// 그리폰라이더
		} else if (level <= 31) {
			return 1200;	// 피닉스
		} else if (level <= 35) {
			return 985;	// 나이트
		} else {
			return 1200;
		}
	}
	// 몬스터의 방어력
	public int GetMonsterArmor(int level)
	{
		if (level <= 1) {
			return 0;	// 피젼트
		} else if (level <= 5) {
			return 4 + ((level - 2) * 2);	// 밀리샤
		} else if (level <= 9) {
			return 2 + ((level - 6) * 2); 	// 풋맨
		} else if (level <= 13) {
			return (level - 10) * 2;	// 라이플맨
		} else if (level <= 17) {
			return 3 + ((level - 14) * 2);	// 스펠브레이커
		} else if (level <= 21) {
			return 2 + ((level - 18) * 2);	// 시즈엔진
		} else if (level <= 25) {
			return 1 + ((level - 22) * 2);	// 드래곤호크라이더
		} else if (level <= 26) {
			return 2;	// 워터엘리멘탈3레벨
		} else if (level <= 30) {
			return ((level - 27) * 2);	// 그리폰라이더
		} else if (level <= 31) {
			return 1;	// 피닉스
		} else if (level <= 35) {
			return 5 + ((level - 32) * 2);	// 나이트
		} else {
			return 12;
		}
	}
	// 몬스터 방어력 감소 공식.(캐릭터와 같음. 1아머 = 6%체력)
	public int GetMonsterDamageReduction(int lv, int damage)
	{
		int armor = GetMonsterArmor (lv);
		float damageReduction = 1f - (((armor) * 0.06f) / (1f + 0.06f * (armor)));
		float realDamage = damage * damageReduction;
		return System.Convert.ToInt32 (realDamage);
	}


	// 던젼 불 공격시 갯수.
	public int GetDungeonFireCount(int dungeonLv)
	{
		if (dungeonLv <= 0) {
			return Random.Range (1, 3);
		} else if (dungeonLv == 1) {
			return Random.Range (2, 4);
		} else if (dungeonLv == 2) {
			return Random.Range (4, 6);
		} else if (dungeonLv == 3) {
			return Random.Range (5, 7);
		} else if (dungeonLv == 4) {
			return Random.Range (7, 9);
		} else if (dungeonLv == 5) {
			return Random.Range (8, 10);
		} else if (dungeonLv == 6) {
			return Random.Range (10, 12);
		} else if (dungeonLv == 7) {
			return Random.Range (11, 13);
		} else if (dungeonLv == 8) {
			return Random.Range (13, 15);
		} else if (dungeonLv == 9) {
			return Random.Range (14, 16);
		} else {
			return Random.Range (15, 18);
		}
	}
	// 던젼 공격 맞을경우 피해량.
	public int GetDungeonDamage(int dungeonLv)
	{
		return 200 + (dungeonLv * 200);
	}


	// 대상의 월드포지션 구하기.
	public Vector3 ConvertToUIPosition(Vector3 position)
	{
		Vector2 resultPos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
		return resultPos;
	}

    


    public string GetLocalizedText(string text, object obj = null)
    {
        if (obj != null)
            return string.Format(text, obj);
        
        return text; 
    }
    public string GetPriceText(int price)
    {
        // 단위 ,을 찍는다던가 화폐 단위를 추가한다. 
        return price.ToString();
    }
    public Color ConvertShortageValueColor(int origin, int compare, bool bEnoughColor = false)
    {
        Color color = Color.white;
        if(origin >= compare)
        {
            if (bEnoughColor)
                color = Color.green;
        }
        else
        {
            color = Color.red;
        }
        return color; 
    }
    public Color ConvertShortageValueColor(bool bEnough, bool bEnoughColor = false)
    {
        Color color = Color.white;
        if(bEnough)
        {
            if (bEnoughColor)
                color = Color.green;
        }
        else
        {
            color = Color.red;
        }
        return color; 
    }

	public void ShuffleList<T>(ref List<T> list)
	{
		int random1;
		int random2;

		T tmp;

		for (int i = 0; i < list.Count; ++i) {
			random1 = Random.Range (0, list.Count);
			random2 = Random.Range (0, list.Count);

			tmp = list [random1];
			list [random1] = list [random2];
			list [random2] = tmp;
		}
	}
}
