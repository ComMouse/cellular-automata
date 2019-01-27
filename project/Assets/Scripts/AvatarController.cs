using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField]
    private InputController inputCtrl;

    public AvatarState state = AvatarState.None;

    private void Start()
    {
        Debug.Assert(inputCtrl != null);
        state = AvatarState.Prepare;
    }

    private void Update()
    {
        switch (state)
        {
            case AvatarState.Prepare:
                if (inputCtrl.GetButton1Down())
                {
                    state = AvatarState.Ready;
                    // Play confirm sound
                    // Update ready UI
                }
                break;
            case AvatarState.Ready:
                break;
        }
    }

    public void Reset()
    {
        state = AvatarState.Prepare;

        // TODO
    }
}

public enum AvatarState
{
    None = 0,
    Prepare = 1,
    Ready = 2,
}
