using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCamManager : MonoBehaviour
{
    [SerializeField]
    private LoadingManager manager;

    public void PassCountNum(int num)
    {
        manager.CountDown(num);
    }

    public void GameStart()
    {
        manager.GameStart();
    }
}
