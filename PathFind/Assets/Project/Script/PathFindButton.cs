using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindButton : MonoBehaviour
{
    //! A star Find 버튼을 누른 경우
    public void OnclickAstarFindBtn()
    {
        PathFinder.Instance.FindPath_Astar();
    }
}
