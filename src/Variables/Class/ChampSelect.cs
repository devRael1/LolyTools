namespace Loly.src.Variables.Class;

internal class ChampSelectResponse
{
    internal List<List<Action>> Actions { get; set; }
    internal ChatDetails ChatDetails { get; set; }
    internal List<MemberTeam> MyTeam { get; set; }
    internal Timer Timer { get; set; }
    internal int LocalPlayerCellId { get; set; }
}

internal class ChatDetails
{
    internal string MultiUserChatId { get; set; }
}

internal class MemberTeam
{
    internal string AssignedPosition { get; set; }
    internal string SummonerId { get; set; }
}

internal class Action
{
    internal int ActorCellId { get; set; }
    internal int ChampionId { get; set; }
    internal bool Completed { get; set; }
    internal string Type { get; set; }
    internal int Id { get; set; }
    internal bool IsInProgress { get; set; }
}

internal class Timer
{
    internal string Phase { get; set; }
}

internal class ChatMe
{
    internal string Id { get; set; }
}