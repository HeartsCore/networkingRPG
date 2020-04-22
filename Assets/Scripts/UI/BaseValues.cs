using System;
[Serializable]
public struct LinearInt
{
    public int baseValue;
    public int bonusPerLevel;
    public int Get(int level) => bonusPerLevel * (level - 1) + baseValue;
}
public class BaseValues 
{
    
}
