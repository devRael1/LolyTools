using Loly.src.Logs;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using Newtonsoft.Json;

using static Loly.src.Variables.Global;

namespace Loly.src.Tools;

public class ChampSelectSession
{
    public static bool HoverPick { get; set; }
    public static bool LockedPick { get; set; }
    public static bool HoverBan { get; set; }
    public static bool LockedBan { get; set; }
    public static bool CanSentMessages { get; set; }
    public static long ChampSelectStart { get; set; }
    public static InitRole CurrentRole { get; set; }
    public static string AssignedRole { get; set; }

    public static void HandleChampSelect()
    {
        var currentChampSelect = Requests.ClientRequest("GET", "lol-champ-select/v1/session", true);
        if (currentChampSelect[0] != "200") return;

        ChampSelectResponse champSelectResponse = JsonConvert.DeserializeObject<ChampSelectResponse>(currentChampSelect[1]);
        var currentChatRoom = champSelectResponse.ChatDetails.MultiUserChatId;
        if (LastChatRoom != currentChatRoom)
        {
            HoverPick = false;
            LockedPick = false;
            HoverBan = false;
            LockedBan = false;
            CanSentMessages = false;
            ChampSelectStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Logger.Info(LogModule.PickAndBan, "New game starting, Champ Select started now");
        }

        if (LockedPick && LockedBan)
        {
            Logger.Info(LogModule.PickAndBan, "Pick and Ban are locked. Waiting for game to start");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            return;
        }

        List<MemberTeam> myTeam = champSelectResponse.MyTeam;
        foreach (MemberTeam member in myTeam)
        {
            if (!member.SummonerId.Equals(SummonerLogged.SummonerId)) continue;

            var position = member.AssignedPosition;
            var assignedRole = position.ToLower() switch
            {
                "utility" => "Support",
                "middle" => "Mid",
                "jungle" => "Jungle",
                "bottom" => "Adc",
                "top" => "Top",
                _ => "Default"
            };

            CurrentRole = (InitRole)CurrentSettings.PicknBan.GetType().GetProperty(assignedRole).GetValue(CurrentSettings.PicknBan);
            break;
        }

        if (CurrentRole.PickChamp.Id == null)
        {
            HoverPick = true;
            LockedPick = true;
        }

        if (CurrentRole.BanChamp.Id == null)
        {
            HoverBan = true;
            LockedBan = true;
        }

        if (CurrentSettings.Tools.AutoChat && CurrentSettings.AutoChat.ChatMessages.Count > 0)
        {
            if (LastChatRoom != currentChatRoom) CanSentMessages = true;
        }

        LastChatRoom = currentChatRoom;

        if (CurrentSettings.Tools.AutoChat && CanSentMessages) AutoChat.HandleChampSelectAutoChat();

        if (!HoverPick || !LockedPick || !HoverBan || !LockedBan)
        {
            PicknBan.ChampSelectResponse = champSelectResponse;
            PicknBan.HandleChampSelectActions();
        }

        CanSentMessages = false;
    }
}