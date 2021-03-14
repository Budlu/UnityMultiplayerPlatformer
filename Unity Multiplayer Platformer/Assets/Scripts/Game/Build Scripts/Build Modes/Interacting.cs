using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : BuildData, IBuildMode
{
    public void Begin() {}

    public void ChangeMode(IBuildMode mode)
    {
        if (!(mode is SpriteSelecting))
            lastMode = mode;
    }

    public void End() {}

    public void RotateBlock() {}

    public void Update() {}
}
