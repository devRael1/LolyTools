using Loly.src.Variables.Enums;

namespace Loly;

public static class Version
{
    public static bool IsDevelopment => ReleaseType is DevelopmentStage.Development;
    public static DevelopmentStage ReleaseType => InDevelopment ? DevelopmentStage.Development : DevelopmentStage.Release;
    public static string FullVersion => $"{Major}.{Minor}.{Hotfix}-{ReleaseType}";
    public static string FullVersionNoStage => $"{Major}.{Minor}.{Hotfix}";

    public static bool InDevelopment => true;

    private static int Major => 3;
    private static int Minor => 0;
    private static int Hotfix => 0;
}