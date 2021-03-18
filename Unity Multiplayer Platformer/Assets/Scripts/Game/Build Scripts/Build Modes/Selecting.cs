using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selecting : BuildData, IBuildMode
{
    public void Begin()
    {
        bm.ChangeModeHighlight(2);
    }

    public void ChangeMode(IBuildMode mode)
    {
        End();

        BuildData.mode = mode;
        BuildData.mode.Begin();
    }

    public void End()
    {
        
    }

    public void RotateBlock()
    {
        
    }

    public void Update()
    {
        
    }
}
