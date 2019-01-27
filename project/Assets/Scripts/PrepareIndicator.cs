using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrepareIndicator : MonoBehaviour
{
    [SerializeField]
    private AvatarController avatarCtrl;

    [SerializeField]
    private Image fillBar;

    [SerializeField]
    private TextMeshProUGUI readyTxt;

    private float currentAmount = 0f;

    private float currentVelocity = 0f;
    
	private void Start()
    {
		Debug.Assert(avatarCtrl != null);
        Debug.Assert(fillBar != null);
        Debug.Assert(readyTxt != null);

        fillBar.fillAmount = 0f;
        readyTxt.enabled = false;
    }
	
	private void Update()
    {
        var destAmount = Mathf.Clamp01(avatarCtrl.ButtonHoldTime / avatarCtrl.prepareTime);
        currentAmount = Mathf.SmoothDamp(currentAmount, destAmount, ref currentVelocity, .1f);

        fillBar.fillAmount = currentAmount;
        readyTxt.enabled = Mathf.Abs(1f - currentAmount) < 1e-2;
    }
}
