using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    [Serializable]
    public class SceneDataCenter
    {
        // public GameSceneDataLookup[] GameSceneDataLookups;
        public SceneData[] SceneDatas;
    }

    [Serializable]
    public class GameSceneDataLookup
    {
        public GameScene Scene;
        public string BgSpriteName;
    }

    [Serializable]
    public class SceneData
    {
        public GameScene Scene;
        public MoveSpaceData MoveSpace;
    }

    [Serializable]
    public struct MoveSpaceData
    {
        public float x, y, width, height;
        public MoveSpaceData(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}