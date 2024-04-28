using Loly.src.Variables.Enums;

namespace Loly;

internal static class Version
{
    internal static bool IsDevelopment => ReleaseType is DevelopmentStage.Development;
    internal static DevelopmentStage ReleaseType => InDevelopment ? DevelopmentStage.Development : DevelopmentStage.Release;
    internal static string FullVersion => $"{Major}.{Minor}.{Hotfix}-{ReleaseType}";
    internal static string FullVersionNoStage => $"{Major}.{Minor}.{Hotfix}";

    internal static bool InDevelopment => true;

    private static int Major => 3;
    private static int Minor => 0;
    private static int Hotfix => 0;
}