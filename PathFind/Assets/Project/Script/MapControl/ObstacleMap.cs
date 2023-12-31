using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : TileMapController
{
    private const string OBSTACLE_TILEMAP_OBJ_NAME = "ObstacleTilemap";
    private GameObject[] castleObjs = default;      //!<길찾기 알고리즘을 테스트 할 출발지와 목적지를 캐싱한 오브젝트배열

    //!Awake 타임에 초기화 할 내용을 재정의한다.
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
        // { 출발지와 목적지를 설정해서 타일을 배치한다.
        castleObjs = new GameObject[2];
        TerrainController[] passsableTerrains = new TerrainController[2];
        List<TerrainController> searchTerrains = default;
        int searchIdx = 0;
        TerrainController foundTile = default;

        //출발지는 좌측에서 우측으로 y축을 서치해서 빈 지형을 받아온다.
        searchIdx = 0;
        foundTile = default;
        while(foundTile==null||foundTile==default)
        {
            //위에서 아래로 서치한다.
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
        }       //loop: 출발지를 찾는 루프

        passsableTerrains[0] = foundTile;

        //목적지는 우측에서 좌측으로 y축을 서치해서 빈 지형을 받아온다.

        searchIdx = mapController.MapCellSize.x - 1;
        foundTile = default;
        while (foundTile == null || foundTile == default)
        {
            //아래에서 위로 서치한다.
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
        }       //loop : 목적지를 찾는 루프
        passsableTerrains[1] = foundTile;
        // } 출발지와 목적지를 설정해서 타일을 배치한다.

        // { 출발지와 목적지에 지물을 추가한다.
        GameObject changedTilePrefab = ResManager.Instance.obstaclePrefabs[RDefine.OBSTACLE_PREF_PLAIN_CASTLE];
        GameObject tempChangeTile = default;
        for(int i=0;i<2;i++)
        {
            tempChangeTile = Instantiate(changedTilePrefab, tileMap.transform);
            tempChangeTile.name = string.Format("{0}_{1}", changedTilePrefab.name, passsableTerrains[i].TileIdx1D);

            tempChangeTile.SetLocalScale(passsableTerrains[i].transform.localScale);
            tempChangeTile.SetLocalPos(passsableTerrains[i].transform.localPosition);

            // 출발지와 목적지를 캐싱한다
            castleObjs[i] = tempChangeTile;
            AddObstacle(tempChangeTile);

            tempChangeTile = default;
        }       //loop : 출발지와 목적지를 인스턴스화 해서 캐싱하는 루프
        // } 출발지와 목적지에 지물을 추가한다.

        Update_SourceDestToPathFinder();
    }       //DoStart()

    //! 지물을 추가한다.
    public void AddObstacle(GameObject obstacle_)
    {
        allTileObjs.Add(obstacle_);
    }       //AddObstacle()

    //! 패스 파인더에 출발지와 목적지를 설정한다.
    public void Update_SourceDestToPathFinder()
    {
        PathFinder.Instance.sourceObj = castleObjs[0];
        PathFinder.Instance.destinationObj = castleObjs[1];
    }       //Update_SourceDestToPathFinder()

}
