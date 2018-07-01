using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtillFunc : Singleton<UtillFunc> {

	public int fact2(int n){
		int res, i;

		if (n <= 1)
			return n;
		else{
			res = n;
			for (i = n - 1; i > 0; i--)
				res *= i;
			return res;
		}
	}

	// 주인공 레벨별 정보.
	/*
	1	26-48 [37 avg]/None	5	18	24	16	550	240
	2	27-49 [38 avg]/None	6	20	25	18	600	270
	3	29-51 [40 avg]/None	6	22	27	20	650	300
	4	31-53 [42 avg]/None	7	24	29	22	700	330
	5	33-55 [44 avg]/None	7	26	31	25	750	375
	6	34-56 [45 avg]/None	8	28	32	27	800	405
	7	36-58 [47 avg]/None	8	30	34	29	850	435
	8	38-60 [49 avg]/None	9	32	36	31	900	465
	9	40-62 [51 avg]/None	9	34	38	34	950	510
	10	41-63 [52 avg]/None	10	36	39	36	1000	540

	Agility Bonus per Level:	1.75
	*/
	public int GetMinAttack(int lv)
	{
		float agility = 24f + ((lv-1) * 1.75f);
		return System.Convert.ToInt32 (agility) + 2;
	}
	public int GetMaxAttack(int lv)
	{
		return GetMinAttack (lv) + 22;
	}

	public int GetHitPoints(int lv)
	{
		return (lv * 50) + 500;
	}

	// 방어력 1은 최대체력의 6%증가와 같음.
	public int GetArmor(int lv)
	{
		return GetHitPoints (lv) / 100;
	}
	// 아머로 인한 데미지 감소가 적용된 후 데미지.
	// For positive Armor, damage reduction =((armor)*0.06)/(1+0.06*(armor))
	public int GetDamageReduction(int lv, int damage)
	{
		int armor = GetArmor (lv);
		float damageReduction = 1f - (((armor) * 0.06f) / (1f + 0.06f * (armor)));
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
			totalExp += 100 + (100*i);
		}
		return totalExp;
	}
	public int GetNeedExpToNextLv(int lv)
	{
		return 100 + (100*lv);
	}


	// 몬스터 계산기(레벨 1부터 35까지)
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
	public int GetMonsterDamageReduction(int lv, int damage)
	{
		int armor = GetMonsterArmor (lv);
		float damageReduction = 1f - (((armor) * 0.06f) / (1f + 0.06f * (armor)));
		float realDamage = damage * damageReduction;
		return System.Convert.ToInt32 (realDamage);
	}
     
    public string GetLocalizedText(string text)
    {
        return text; 
    }

    public string GetPriceText(int price)
    {
        // 단위 ,을 찍는다던가 화폐 단위를 추가한다. 
        return price.ToString();
    }

    public Vector3 ConvertToUIPosition(Vector3 position)
    {
        Vector2 resultPos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        return resultPos; 
    }
}
