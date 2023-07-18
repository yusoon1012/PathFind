using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap : TileMapController
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "TerrainTilemap";

    private Vector2Int mapCellSize = default;

    private Vector2 mapCellGap = default;

    private List<TerrainController> allTerrains = default;


    //! Awake Ƽ�ӿ� �ʱ�ȭ �� ������ ������ �Ѵ�.
    public override void InitAwake(MapBoard mapController_)
    {
        tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME;
        base.InitAwake(mapController_);
        allTerrains = new List<TerrainController>();

        // { Ÿ���� x�� ������ ��ü Ÿ���� ���� ���� ����, ���� ����� �����Ѵ�.
        mapCellSize = Vector2Int.zero;
        float tempTileY = allTileObjs[0].transform.localPosition.y;
        for(int i=0;i<allTileObjs.Count;i++)
        {
            if (tempTileY.IsEquals(allTileObjs[i].transform.localPosition.y)==false)
            {
                mapCellSize.x = i;
                break;
            } //if: ù��° Ÿ���� y ��ǥ�� �޶����� ���� �������� ���� ���� �� ũ���̴�.
        }

        // ��ü Ÿ���� ���� ���� ���� �� ũ��� ���� ���� ���� ���� �� ũ���̴�.
        mapCellSize.y = Mathf.FloorToInt(allTileObjs.Count / mapCellSize.x);
        // } Ÿ���� x�� ������ ��ü Ÿ���� ���� ���� ����, ���� ����� �����Ѵ�.


        // { x �� ���� �� Ÿ�ϰ�, y�� ���� �� Ÿ�� ������ �������������� Ÿ�� ���� �����Ѵ�.
        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x - allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y - allTileObjs[0].transform.localPosition.y;
        // } x �� ���� �� Ÿ�ϰ�, y�� ���� �� Ÿ�� ������ �������������� Ÿ�� ���� �����Ѵ�.

    }//InitAwake()

    private void Start()
    {
        // { Ÿ�ϸ��� �Ϻθ� ���� Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����
        GameObject changeTilePrefab = ResManager.Instance.terrainPrefabs[RDefine.TERRAIN_PREF_OCEAN];

        // Ÿ�ϸ� �߿� ��� ������ �ٴٷ� ��ü�� ������ �����Ѵ�.
        const float CHANGE_PERCENTAGE = 15.0f;
        float correctChangePercentage = allTileObjs.Count * (CHANGE_PERCENTAGE / 100.0f);

        //�ٴٷ� ��ü�� Ÿ���� ������ ����Ʈ ���·� �����ؼ� ���´�.
        List<int> changedTileResult = GFunc.CreateList(allTileObjs.Count, 1);
        changedTileResult.Shuffle();

        GameObject tempChangeTile = default;

        for(int i=0;i<allTileObjs.Count;i++)
        {
            if (correctChangePercentage <= changedTileResult[i]) { continue; }

            //�������� �ν��Ͻ�ȭ�ؼ� ��ü�� Ÿ���� Ʈ�������� ��ü�Ѵ�.
            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempChangeTile.name = changeTilePrefab.name;
            tempChangeTile.SetLocalScale(allTileObjs[i].transform.localScale);
            tempChangeTile.SetLocalPos(allTileObjs[i].transform.localPosition);

            allTileObjs.Swap(ref tempChangeTile, i);
            tempChangeTile.DestroyObj();
        }//loop: ������ 
        // } Ÿ�ϸ��� �Ϻθ� ���� Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����

        // { ������ �����ϴ� Ÿ���� ������ �����ϰ�, ��Ʈ�ѷ��� ĳ���ϴ� ����
        TerrainController tempTerrain = default;
        TerrainType terrainType = TerrainType.NONE;

        int loopCount = 0;
        foreach(GameObject tile_ in allTileObjs)
        {
            tempTerrain = tile_.GetComponentMust<TerrainController>();
            switch(tempTerrain.name)
            {
                case RDefine.TERRAIN_PREF_PLAIN:
                    terrainType = TerrainType.PLAIN_PASS;
                    break;
                case RDefine.TERRAIN_PREF_OCEAN:
                    terrainType = TerrainType.OCEAN_N_PASS;
                    break;
                default:
                    terrainType = TerrainType.NONE;
                    break;
            }//switch : �������� �ٸ� ������ �Ѵ�.

            // TODO : tempTerrain Setup �Լ� �ʿ���
            tempTerrain.transform.SetAsFirstSibling();
            allTerrains.Add(tempTerrain);
            loopCount += 1;
        }// loop : Ÿ���� �̸��� ������ ������� �����ϴ� ����
        // } ������ �����ϴ� Ÿ���� ������ �����ϰ�, ��Ʈ�ѷ��� ĳ���ϴ� ����



    }


    //! �ʱ�ȭ�� Ÿ���� ������ ������ ���� ����, ���� ũ�⸦ �����ϴ� �Լ�
    public Vector2Int GetCellSize() { return mapCellSize; }

    //! �ʱ�ȭ�� Ÿ���� ������ ������ Ÿ�� ������ ���� �����ϴ� �Լ�
    public Vector2 GetCellGap() { return mapCellGap; }

    //! �ε����� �ش��ϴ� Ÿ���� �����ϴ� �Լ�
    public TerrainController GetTile(int tileIdx1D)
    {
        if(allTerrains.IsValid(tileIdx1D))
        {
            return allTerrains[tileIdx1D];
        }
        return default;
    }// GetTile()



}
