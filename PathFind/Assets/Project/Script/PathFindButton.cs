using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindButton : MonoBehaviour
{
    //! A star Find ��ư�� ���� ���
    public void OnclickAstarFindBtn()
    {
        PathFinder.Instance.FindPath_Astar();
    }
}
