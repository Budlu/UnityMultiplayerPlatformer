using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSettings
{
    public int minBuildWidth = 20;
    public int maxBuildWidth = 60;
    public int minBuildHeight = 20;
    public int maxBuildHeight = 40;

    public int maxTraps = 10;
    public int maxEnemies = 10;
    public int maxItems = 10;

    public BuildSettings() { }

    public BuildSettings(int minBuildWidth, int maxBuildWidth, int minBuildHeight, int maxBuildHeight,
        int maxTraps, int maxEnemies, int maxItems)
    {
        this.minBuildWidth = minBuildWidth;
        this.maxBuildWidth = maxBuildWidth;
        this.minBuildHeight = minBuildHeight;
        this.maxBuildHeight = maxBuildHeight;

        this.maxTraps = maxTraps;
        this.maxEnemies = maxEnemies;
        this.maxItems = maxItems;
    }
}
