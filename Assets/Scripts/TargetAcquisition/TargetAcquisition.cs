using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class TargetAcquisition
{

    public static CharaBase WeaponRangeNearestEnemy(WeaponBase targetingStart, List<CharaBase> targets)
    {
        int num = -1;
        float ans = float.MaxValue;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null) continue;
            float tempNum = Vector3.Distance(targetingStart.transform.position, targets[i].transform.position);
            if (tempNum > targetingStart.weaponRange) continue;
            if (ans > tempNum)
            {
                ans = tempNum;
                num = i;
            }
        }
        if (num == -1) return null;
        return targets[num];
    }

    public static CharaBase VisionRangeNearestEnemy(CharaBase targetingStart, List<CharaBase> targets)
    {
        int num = -1;
        float ans = float.MaxValue;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null) continue;
            float tempNum = Vector3.Distance(targetingStart.transform.position, targets[i].transform.position);
            if (ans > tempNum)
            {
                ans = tempNum;
                num = i;
            }
        }
        if (num == -1) return null;
        return targets[num];
    }

}
