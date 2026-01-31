using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public struct Tile2Sprite
{
    public Tile tile;
    public Sprite sprite;
}

public class SceneMod : MonoBehaviour
{
    public static SceneMod Instance { get; private set; }

    public Tile2Sprite[] Tile2Sprites;
    public GameObject SpriteTilePrefab;

    [Space(10)]
    [Header("网格预设")]
    public Tilemap Map_Barrier;
    public Tilemap Map_Build;
    public Tilemap Map_Ground;
    public Tilemap Map_Wall1;
    public Tilemap Map_Wall2;


    [Space(10)]
    [Header("瓦片预设")]
    public List<Tile> Tile_Barrier;
    public List<Tile> Tile_Build;
    public List<Tile> Tile_Blank;

    private Dictionary<Tile, Sprite> tileSpriteDict;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            enabled = false;
            return;
        }

        Map_Barrier = transform.Find("Grid/BarrierMap").GetComponent<Tilemap>();
        Map_Build = transform.Find("Grid/BarrierMap").GetComponent<Tilemap>();

        BuildLookup_Tile2Sprite();
        GenerateFromTilemap(Map_Wall1);
        GenerateFromTilemap(Map_Wall2);
    }

    #region Test
    [Space(10)]
    [Header("测试用例")]
    public Transform startPoint;      // 起点 Transform
    public Transform targetPoint;     // 终点 Transform
    public LineRenderer lineRenderer; // 用于渲染路径

    /// <summary>
    /// 寻路并用 LineRenderer 渲染路径
    /// </summary>
    [Button("PathingTest")]
    public void DrawPath()
    {
        if (startPoint == null || targetPoint == null)
        {
            Debug.LogError("请指定起点和终点 Transform！");
            return;
        }

        // 转换成 Tilemap 格子坐标
        Vector3Int startCell = SceneMod.Instance.Map_Barrier.WorldToCell(startPoint.position);
        Vector3Int targetCell = SceneMod.Instance.Map_Barrier.WorldToCell(targetPoint.position);

        // 寻路
        if (SceneMod.Instance.IsConnected(startCell, targetCell, out List<Vector3Int> path))
        {
            // 转换路径成世界坐标
            Vector3[] worldPositions = new Vector3[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                // 获取格子中心的世界坐标
                worldPositions[i] = SceneMod.Instance.Map_Barrier.GetCellCenterWorld(path[i]);
            }

            // 设置 LineRenderer
            lineRenderer.positionCount = worldPositions.Length;
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPositions(worldPositions);

            Debug.Log("路径绘制完成，共 " + worldPositions.Length + " 个点");
        }
        else
        {
            Debug.LogWarning("无法找到路径！");
            lineRenderer.positionCount = 0;
        }
    }
    #endregion

    #region 对外接口
    // 判断两点间是否联通 [建筑会阻拦寻路]
    public bool IsConnected(Vector3Int _start, Vector3Int _target, out List<Vector3Int> _path)
    {
        _path = null;

        // 起点 & 终点 判定
        if (!TileDefine(_start, _ingoreBuild: false) || !TileDefine(_target, _ingoreBuild: false)) return false;

        // 路径判定
        _path = FindPath(_start, _target, _ingoreBuild: false);

        return _path != null && _path.Count > 0;
    }

    // 判断两点间是否可达 [建筑不会阻拦寻路]
    public bool IsWalkAble(Vector3Int _start, Vector3Int _target, out List<Vector3Int> _path)
    {
        _path = null;

        // 起点 & 终点 判定
        if (!TileDefine(_start, _ingoreBuild:true) || !TileDefine(_target, _ingoreBuild: true)) return false;

        // 路径判定
        _path = FindPath(_start, _target, _ingoreBuild: true);

        return _path != null && _path.Count > 0;
    }

    // 找到两点间路径
    public List<Vector3Int> FindPath(Vector3Int _start, Vector3Int _target, bool _ingoreBuild = false)
    {
        if (!TileDefine(_start, _ingoreBuild) || !TileDefine(_target, _ingoreBuild))
            return null;

        // 用于存放需要评估的节点（优先级队列）
        var openTileSet = new PriorityQueue();

        // 前置节点 => 下一节点
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        // 实际代价   [到当前节点 实际代价]
        var gScore = new Dictionary<Vector3Int, float>();
        // 预估总代价 [到当前节点 预估 到终点 所需代价]
        var TotalScore = new Dictionary<Vector3Int, float>();

        gScore[_start] = 0f;
        TotalScore[_start] = Distance(_start, _target);

        openTileSet.Enqueue(_start, TotalScore[_start]);

        while (openTileSet.Count > 0)
        {
            // 取出 f 最小的节点
            Vector3Int current = openTileSet.Dequeue();

            // 到达终点时
            if (current == _target)
                return ReconstructPath(cameFrom, current);

            // 遍历邻居
            foreach (var neighbor in GetNeighbors(current))
            {
                if (!TileDefine(neighbor, _ingoreBuild)) continue;

                // 每步代价 = 1
                float gScoreNew = gScore[current] + 1;

                
                if (!gScore.ContainsKey(neighbor) || gScoreNew < gScore[neighbor])
                {
                    // 更新路径信息
                    cameFrom[neighbor] = current;

                    gScore[neighbor] = gScoreNew;
                    TotalScore[neighbor] = gScoreNew + Distance(neighbor, _target);

                    // 如果不在 openTileSet 中，则加入
                    if (!openTileSet.Contains(neighbor))
                        openTileSet.Enqueue(neighbor, TotalScore[neighbor]);
                }
            }
        }

        return null;
    }

    // 世界坐标 到 网格坐标
    public Vector3Int World2Cell(Vector3 _world)
    {
        return Map_Barrier.WorldToCell(_world);
    }

    // 网格坐标列表 到 世界坐标列表
    public List<Vector3> Cell2WorldList(List<Vector3Int> _cellPosList)
    {
        List<Vector3> worldList = new List<Vector3>();

        if (_cellPosList == null || _cellPosList.Count == 0)
            return worldList;

        foreach (var cell in _cellPosList)
        {
            // 将 Tilemap 的格子坐标转换成 世界坐标
            Vector3 worldPos = Map_Barrier.GetCellCenterWorld(cell);

            worldList.Add(worldPos);
        }

        return worldList;
    }
    #endregion

    #region 工具函数
    // 判断一个瓦片是否能走
    public bool TileDefine(Vector3Int _cellPos, bool _ingoreBuild = false)
    {
        TileBase tile_barrier = Map_Barrier.GetTile(_cellPos);
        TileBase tile_build = Map_Barrier.GetTile(_cellPos);
        if (tile_barrier == null) return true;

        // 障碍
        foreach (var b in Tile_Barrier)
        {
            if (tile_barrier == b)
                return false;
        }

        // 建筑
        if (!_ingoreBuild && tile_build != null)
            foreach (var b in Tile_Build)
            {
                if (tile_barrier == b)
                    return false;
            }

        return true;
    }

    // 计算距离
    private float Distance(Vector3Int _start, Vector3Int _target)
    {
        return Mathf.Abs(_start.x - _target.x) + Mathf.Abs(_start.y - _target.y);
    }

    // 从目标点开始 重建路径 到 起始点
    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> _cameFrom, Vector3Int _target)
    {
        List<Vector3Int> totalPath = new List<Vector3Int>() { _target };
        while (_cameFrom.ContainsKey(_target))
        {
            _target = _cameFrom[_target];
            totalPath.Insert(0, _target);
        }
        return totalPath;
    }

    // 获取相邻坐标
    private List<Vector3Int> GetNeighbors(Vector3Int _cellPos)
    {
        return new List<Vector3Int>()
        {
            _cellPos + Vector3Int.up,
            _cellPos + Vector3Int.down,
            _cellPos + Vector3Int.left,
            _cellPos + Vector3Int.right,
            _cellPos + new Vector3Int(1, 1, 0),
            _cellPos + new Vector3Int(1, -1, 0),
            _cellPos + new Vector3Int(-1, 1, 0),
            _cellPos + new Vector3Int(-1, -1, 0),
        };
    }

    // 构建查表结构
    void BuildLookup_Tile2Sprite()
    {
        tileSpriteDict = new Dictionary<Tile, Sprite>();

        foreach (var pair in Tile2Sprites)
        {
            if (pair.tile != null && pair.sprite != null)
            {
                tileSpriteDict[pair.tile] = pair.sprite;
            }
        }
    }

    void GenerateFromTilemap(Tilemap tilemap)
    {
        if (tilemap == null) return;

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile == null) continue;

            if (!tileSpriteDict.TryGetValue(tile, out var sprite))
                continue;

            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);

            GameObject go = Instantiate(
                SpriteTilePrefab,
                worldPos,
                Quaternion.identity,
                transform
            );
            go.SetActive(true);
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = sprite;
            }
        }
    }

    #endregion
}
