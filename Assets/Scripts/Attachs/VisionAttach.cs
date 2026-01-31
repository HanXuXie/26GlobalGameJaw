using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionAttach : MonoBehaviour
{
    public Transform LookAt;

    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    var mousPos = Input.mousePosition;
        //    var worldPos = Camera.main.ScreenToWorldPoint(mousPos);
        //    TurnLook(worldPos);
        //}

        if (LookAt != null)
        {
            // 持续跟随看向 Transform
            Update_LookAt(LookAt.position);
        }
    }

    // 每帧执行的朝向逻辑
    private void Update_LookAt(Vector3 targetPos)
    {
        // 如果物体跟目标在 Z 轴不平面，将高度对齐
        targetPos.z = transform.position.z;

        Vector3 dir = (targetPos - transform.position).normalized;
        if (dir == Vector3.zero) return;

        // 让物体的右轴 (X+) 朝向目标
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // 转视角看向某个方向
    public void TurnLook(Vector3 _targetPos)
    {
        // 确保在同一平面
        _targetPos.z = transform.position.z;

        // 目标方向向量
        Vector3 direction = (_targetPos - transform.position).normalized;

        if (direction == Vector3.zero)
            return;

        // 计算当前的角度差（针对 X 轴）
        float currentAngle = transform.eulerAngles.z;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);

        // 根据角度差设定时间（角度越大 → 时间越长）
        float baseSpeed = 360f;     // 每秒360度（可调整）
        float duration = Mathf.Abs(angleDiff) / baseSpeed;

        // 构造目标旋转（仅 Z 轴旋转）
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        // 使用 DOTween 旋转，先快后慢
        transform.DORotateQuaternion(targetRotation, duration)
                 .SetEase(Ease.OutCubic)
                 .SetLink(gameObject);
    }

    // 持续看着某个物体
    public void SetLookAt(Transform _lookAt)
    {
        LookAt = _lookAt;
    }

    public void UnSetLookAt()
    {
        LookAt = null;
    }
}
