using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildMode
{

    void Begin();

    void Update();

    void End();

    void ChangeMode(IBuildMode mode);

    void RotateBlock();

}
