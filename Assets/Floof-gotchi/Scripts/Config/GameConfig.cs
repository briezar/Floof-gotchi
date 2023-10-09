using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof.Constants
{
    public static class GameConfig
    {
        public static class Needs
        {
            public static Color HighColor = GeneralUtils.ColorFromInt(83, 212, 121);
            public static Color NormalColor = GeneralUtils.ColorFromInt(212, 212, 83);
            public static Color LowColor = GeneralUtils.ColorFromInt(212, 83, 83);
            public const float NormalThreshold = 0.7f;
            public const float LowThreshold = 0.33f;
        }
    }
}
