using Loly.src.Variables;

namespace Loly
{
    public static class Version
    {
        public static bool IsDevelopment => ReleaseType is DevelopmentStage.Development;
        public static DevelopmentStage ReleaseType => Global.IsProdEnvironment ? DevelopmentStage.Release : DevelopmentStage.Development;
        public static string FullVersion => $"{Major}.{Minor}.{Hotfix}-{ReleaseType}";

        private static int Major => 2;
        private static int Minor => 0;
        private static int Hotfix => 0;
    }
}
