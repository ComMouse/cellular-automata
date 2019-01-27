using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldIndicator : MonoBehaviour
{
    [SerializeField]
    private HoldController holdCtrl;

    [SerializeField]
    private Image fillBar;

    private float currentAmount = 0f;

    private float currentVelocity = 0f;

    private void Start()
    {
        Debug.Assert(fillBar != null);

        fillBar.fillAmount = 0f;
    }

    private void Update()
    {
        var destAmount = Mathf.Clamp01(holdCtrl.ButtonHoldTime / holdCtrl.prepareTime);
        currentAmount = Mathf.SmoothDamp(currentAmount, destAmount, ref currentVelocity, .1f);

        fillBar.fillAmount = currentAmount;
    }
}
