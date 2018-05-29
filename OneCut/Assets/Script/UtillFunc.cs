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

	// 경험치로 레벨 구하기. => 요기서부터하기.
	// 1->2: 100+1*100 = 200
	// 2->3: 100+2*100 = 300 -> 500
	// 3->4: 100+3*100 = 400 -> 900
	public int GetLevel(int exp)
	{
		// 1: 0 - 199
		// 2: 200 - 499
		// 3: 500 - 899
		//4: 900
		int level = ((exp - 100) / 100) + 1;
		return level <= 1 ? 1 : level;
	}

	// 몬스터 계산기
	public int monsterExp(int level)
	{
		return level;
	}
	public int monsterLife(int level)
	{
		//return UtillFunc.Instance.fact2 (level);
		return (3 * level) + 1;
	}
}
