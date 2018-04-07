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

	// 스탯계산기
	public int pow(int level)
	{
		return 10 + (level * 2);
	}
	public int dex(int level)
	{
		return level;
	}
	public int inc(int level)
	{
		return level/2;
	}
	public int maxLife(int pow)
	{
		return pow * 30;
	}
	public float regenLife(int pow)
	{
		return 0;
	}
	public float attackDamage(int pow)
	{
		return pow;
	}
	public float defence(int dex)
	{
		return 0;
	}
	public float attackSpeed(int dex)
	{
		return 1 + (dex * 0.05f);
	}
	public float moveSpeed(int dex)
	{
		return 1 + (dex * 0.01f);
	}
	public float splashArea(int inc)
	{
		return 0;
	}
	public float addExp(int inc)
	{
		return 0;
	}
	public float critical(int inc)
	{
		return inc * 1.5f;
	}
}
