using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
    private static EndController instance;
    public static EndController Instance => instance;

    [SerializeField]
    private GameObject endGo;

    [SerializeField]
    private GameObject momGo;

    [SerializeField]
    private GameObject kidGo;

    [SerializeField]
    private HoldController holdCtrl;

    private void Start()
    {
        Debug.Assert(endGo != null);
        Debug.Assert(momGo != null);
        Debug.Assert(kidGo != null);

        endGo.SetActive(false);

        instance = this;
    }

    private void Update()
    {
        if (holdCtrl.isHeld)
        {
            GameController.Instance.StartPreparation();
        }
    }

    public void Init(bool isMomWin = false)
    {
        endGo.SetActive(true);
        momGo.SetActive(isMomWin);
        kidGo.SetActive(!isMomWin);

        holdCtrl.Reset();
    }

    public void Exit()
    {
        endGo.SetActive(false);
    }
}
