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
            if (targets[i].Clamp == targetingStart.Chara.Clamp) continue;

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
            if (targets[i].Clamp == targetingStart.Clamp) continue;
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

<<<<<<< Updated upstream
    //public static Vector3 HalfRoad(Vector3 StartPoint, Vector3 FinalPoint)
    //{

    //}
=======
    public static Vector3 HalfRoad(Vector3 StartPoint, Vector3 FinalPoint)
    {
        return (StartPoint + FinalPoint) / 2;
    }
>>>>>>> Stashed changes

}
