﻿using Loly.src.Variables;
using Loly.src.Variables.Enums;

namespace Loly
{
    public static class Version
    {
        public static bool IsDevelopment => ReleaseType is DevelopmentStage.Release;
        public static DevelopmentStage ReleaseType => Global.IsProdEnvironment ? DevelopmentStage.Release : DevelopmentStage.Development;
        public static string FullVersion => $"{Major}.{Minor}.{Hotfix}-{ReleaseType}";
        public static string FullVersionNoStage => $"{Major}.{Minor}.{Hotfix}";

        private static int Major => 2;
        private static int Minor => 2;
        private static int Hotfix => 1;
    }
}
