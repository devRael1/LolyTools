namespace Loly.src.Variables.Class;

public class ChampSelectResponse
{
    public List<List<Action>> Actions { get; set; }
    public ChatDetails ChatDetails { get; set; }
    public List<MemberTeam> MyTeam { get; set; }
    public Timer Timer { get; set; }
    public int LocalPlayerCellId { get; set; }
}

public class ChatDetails
{
    public string MultiUserChatId { get; set; }
}

public class MemberTeam
{
    public string AssignedPosition { get; set; }
    public string SummonerId { get; set; }
}

public class Action
{
    public int ActorCellId { get; set; }
    public bool Completed { get; set; }
    public string Type { get; set; }
    public int Id { get; set; }
    public bool IsInProgress { get; set; }
}

public class Timer
{
    public string Phase { get; set; }
}

public class ChatMe
{
    public string Id { get; set; }
}