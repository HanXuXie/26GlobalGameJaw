using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 简单优先级队列
/// </summary>
class PriorityQueue
{
    private List<(Vector3Int node, float priority)> elements = new List<(Vector3Int, float)>();

    public int Count => elements.Count;

    // 是否包含
    public bool Contains(Vector3Int node)
    {
        return elements.Exists(e => e.node == node);
    }

    // 加入队列
    public void Enqueue(Vector3Int node, float priority)
    {
        elements.Add((node, priority));
    }

    // 弹出队列
    public Vector3Int Dequeue()
    {
        int bestIndex = 0;
        for (int i = 1; i < elements.Count; i++)
        {
            if (elements[i].priority < elements[bestIndex].priority)
                bestIndex = i;
        }
        Vector3Int bestNode = elements[bestIndex].node;
        elements.RemoveAt(bestIndex);
        return bestNode;
    }
}