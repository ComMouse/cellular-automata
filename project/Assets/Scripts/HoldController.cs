using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldController : MonoBehaviour
{
    [SerializeField]
    private InputController inputCtrl;

    public float prepareTime = 1f;

    private float buttonHoldTime = 0f;
    public float ButtonHoldTime => buttonHoldTime;

    public bool isHeld = false;

    private void Start()
    {
        Debug.Assert(inputCtrl != null);
    }

    private void Update()
    {
        if (isHeld)
            return;

        if (inputCtrl.GetRestartDown())
        {
            buttonHoldTime += Time.deltaTime;

            if (buttonHoldTime > prepareTime)
            {
                isHeld = true;

                SoundManager.Instance.Play("Effect_ControllerConfirm");

                GameController.Instance.StartPreparation();
            }
        }
    }

    public void Reset()
    {
        isHeld = false;
    }
}
