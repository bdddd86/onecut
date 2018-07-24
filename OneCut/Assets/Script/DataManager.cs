using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Attack,
    Defense,
    CriticalRate,
    Specific,
    HP,
    CriticalDamage,
    LifeSteal,
}

public enum CalcurateType
{
    Sum,
    Product, 
    ID,
}

[System.Serializable]
public class AbilityTypeData
{
    public int id;
    public AbilityType abilityType;
    public CalcurateType calcurateType; 
}


public class DataManager : MonoSingleton<DataManager> {
    public List<AbilityTypeData> abilityTypeData;
}
