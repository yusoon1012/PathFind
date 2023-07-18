using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoard : MonoBehaviour
{
    private const string TERRAIN_MAP_OBJ_NAME = "TerrainGrid";

    public Vector2Int MapCellSize { get; private set; } = default;
    public Vector2 MapCellGap { get; private set; } = default;

    private TerrainMap terrainMap = default;

    private void Awake()
    {
        // { �Ŵ��� ��ũ��Ʈ�� �ʱ�ȭ�Ѵ�.
        ResManager.Instance.Create();
        // } �Ŵ��� ��ũ��Ʈ�� �ʱ�ȭ�Ѵ�.

        // { �ʿ� ������ �ʱ�ȭ�Ͽ� ��ġ�Ѵ�.
        terrainMap = gameObject.FindChildComponent<TerrainMap>(TERRAIN_MAP_OBJ_NAME);
        terrainMap.InitAwake(this);
        MapCellSize = terrainMap.GetCellSize();
        MapCellGap = terrainMap.GetCellGap();
        // } �ʿ� ������ �ʱ�ȭ�Ͽ� ��ġ�Ѵ�.


    }

    //! Ÿ�� �ε����� �޾Ƽ� �ش� Ÿ���� �����ϴ� �Լ�
    public TerrainController GetTerrain(int idx1D)
    {
        return terrainMap.GetTile(idx1D);
    } //GetTerrain()

    //! ���� x��ǥ�� �޾Ƽ� �ش� ���� Ÿ���� ����Ʈ�� �������� �Լ�
    public List<TerrainController> GetTerrains_Colum(int xIdx2D)
    {
        return GetTerrains_Colum(xIdx2D, false);
    }

    public List<TerrainController> GetTerrains_Colum(int xIdx2D,bool isSortReverse)
    {
        List<TerrainController> terrains = new List<TerrainController>();
        TerrainController tempTile = default;
        int tileIdx1D = 0;
        for(int y=0;y<MapCellSize.y;y++)
        {
            tileIdx1D = y * MapCellSize.x + xIdx2D;
            tempTile = terrainMap.GetTile(tileIdx1D);
            terrains.Add(tempTile);

        }   //loop: y���� ũ�⸸ŭ ��ȸ�ϴ� ����

        if(terrains.IsValid())
        {
            if (isSortReverse) { terrains.Reverse(); }
            else { /* Do Nothing*/}
            return terrains;
        }
        else
        {
            return default;
        }
    }

    //! ������ �ε����� 2D ��ǥ�� �����ϴ� �Լ�
    public Vector2Int GetTileIdx2D(int idx1D)
    {
        Vector2Int tileIdx2D = Vector2Int.zero;
        tileIdx2D.x = idx1D % MapCellSize.x;
        tileIdx2D.y = idx1D / MapCellSize.x;
        return tileIdx2D;
    }   //GetTileIdx2D()

    //! ������ 2D ��ǥ�� �ε����� �����ϴ� �Լ�
    public int GetTileIdx1D(Vector2Int idx2D)
    {
        int tileIdx1D = (idx2D.y * MapCellSize.x) + idx2D.x;
        return tileIdx1D;
    }   //GetTileIdx1D()

    //! �� ���� ������ Ÿ�� �Ÿ��� �����ϴ� �Լ�
    public Vector2Int GetDistance2D(GameObject targetTerrainObj,GameObject destTerrainObj)
    {
        Vector2 localDistance = destTerrainObj.transform.localPosition - targetTerrainObj.transform.localPosition;

        Vector2Int distance2D = Vector2Int.zero;
        distance2D.x = Mathf.RoundToInt(localDistance.x / MapCellGap.x);
        distance2D.y = Mathf.RoundToInt(localDistance.y / MapCellGap.y);
        distance2D = GFunc.Abs(distance2D);

        return distance2D;
    }   //GetDistance2D()

    //! 2D ��ǥ�� �������� �ֺ� 4���� Ÿ���� �ε����� �����ϴ� �Լ�
    public List<int> GetTileIdx2D_Around4Ways(Vector2Int targetIdx2D)
    {
        List<int> idx1D_around4ways = new List<int>();
        List<Vector2Int> idx2D_around4ways = new List<Vector2Int>();
        idx2D_around4ways.Add(new Vector2Int(targetIdx2D.x - 1, targetIdx2D.y));
        idx2D_around4ways.Add(new Vector2Int(targetIdx2D.x + 1, targetIdx2D.y));
        idx2D_around4ways.Add(new Vector2Int(targetIdx2D.x, targetIdx2D.y-1));
        idx2D_around4ways.Add(new Vector2Int(targetIdx2D.x, targetIdx2D.y+1));

        foreach(var idx2D in idx2D_around4ways)
        {
            //2D ��ǥ�� ��ȿ���� �˻��Ѵ�.
            if (idx2D.x.IsInRange(0, MapCellSize.x) == false) { continue; }
            if (idx2D.y.IsInRange(0, MapCellSize.y) == false) { continue; }
            idx1D_around4ways.Add(GetTileIdx1D(idx2D));
        }
        return idx1D_around4ways;
    }





}
