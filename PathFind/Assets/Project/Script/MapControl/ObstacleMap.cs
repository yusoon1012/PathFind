using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : TileMapController
{
    private const string OBSTACLE_TILEMAP_OBJ_NAME = "ObstacleTilemap";
    private GameObject[] castleObjs = default;      //!<��ã�� �˰����� �׽�Ʈ �� ������� �������� ĳ���� ������Ʈ�迭

    //!Awake Ÿ�ӿ� �ʱ�ȭ �� ������ �������Ѵ�.
    public override void InitAwake(MapBoard mapController_)
    {
        this.tileMapObjName = OBSTACLE_TILEMAP_OBJ_NAME;
        base.InitAwake(mapController_);
    }       //InitAwake()

    private void Start()
    {
        StartCoroutine(DelayStart(0f));
    }

    private IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        DoStart();
    }       //DelayStart()
    private void DoStart()
    {
        // { ������� �������� �����ؼ� Ÿ���� ��ġ�Ѵ�.
        castleObjs = new GameObject[2];
        TerrainController[] passsableTerrains = new TerrainController[2];
        List<TerrainController> searchTerrains = default;
        int searchIdx = 0;
        TerrainController foundTile = default;

        //������� �������� �������� y���� ��ġ�ؼ� �� ������ �޾ƿ´�.
        searchIdx = 0;
        foundTile = default;
        while(foundTile==null||foundTile==default)
        {
            //������ �Ʒ��� ��ġ�Ѵ�.
            searchTerrains = mapController.GetTerrains_Colum(searchIdx, true);
            foreach(var searchTerrain in searchTerrains)
            {
                if(searchTerrain.IsPaasable==true)
                {
                    foundTile = searchTerrain;
                    break;
                }
                else { /* Do Nothing*/}

            }
            if (foundTile != null || foundTile != default) { break; }
            if (mapController.MapCellSize.x - 1 <= searchIdx) { break; }
            searchIdx += 1;
        }       //loop: ������� ã�� ����

        passsableTerrains[0] = foundTile;

        //�������� �������� �������� y���� ��ġ�ؼ� �� ������ �޾ƿ´�.

        searchIdx = mapController.MapCellSize.x - 1;
        foundTile = default;
        while (foundTile == null || foundTile == default)
        {
            //�Ʒ����� ���� ��ġ�Ѵ�.
            searchTerrains = mapController.GetTerrains_Colum(searchIdx);
            foreach (var searchTerrain in searchTerrains)
            {
                if (searchTerrain.IsPaasable)
                {
                    foundTile = searchTerrain;
                    break;
                }
                else {/*Do Nothing*/ }
            }

            if (foundTile != null || foundTile != default) { break; }
            if (searchIdx <= 0) { break; }
            searchIdx -= 1;
        }       //loop : �������� ã�� ����
        passsableTerrains[1] = foundTile;
        // } ������� �������� �����ؼ� Ÿ���� ��ġ�Ѵ�.

        // { ������� �������� ������ �߰��Ѵ�.
        GameObject changedTilePrefab = ResManager.Instance.obstaclePrefabs[RDefine.OBSTACLE_PREF_PLAIN_CASTLE];
        GameObject tempChangeTile = default;
        for(int i=0;i<2;i++)
        {
            tempChangeTile = Instantiate(changedTilePrefab, tileMap.transform);
            tempChangeTile.name = string.Format("{0}_{1}", changedTilePrefab.name, passsableTerrains[i].TileIdx1D);

            tempChangeTile.SetLocalScale(passsableTerrains[i].transform.localScale);
            tempChangeTile.SetLocalPos(passsableTerrains[i].transform.localPosition);

            // ������� �������� ĳ���Ѵ�
            castleObjs[i] = tempChangeTile;
            AddObstacle(tempChangeTile);

            tempChangeTile = default;
        }       //loop : ������� �������� �ν��Ͻ�ȭ �ؼ� ĳ���ϴ� ����
        // } ������� �������� ������ �߰��Ѵ�.

        Update_SourceDestToPathFinder();
    }       //DoStart()

    //! ������ �߰��Ѵ�.
    public void AddObstacle(GameObject obstacle_)
    {
        allTileObjs.Add(obstacle_);
    }       //AddObstacle()

    //! �н� ���δ��� ������� �������� �����Ѵ�.
    public void Update_SourceDestToPathFinder()
    {
        PathFinder.Instance.sourceObj = castleObjs[0];
        PathFinder.Instance.destinationObj = castleObjs[1];
    }       //Update_SourceDestToPathFinder()

}
