namespace Loly.src.Variables.UGG;

internal static class Queries
{
    internal static readonly string FetchMatchSummaries =
        @"query FetchMatchSummaries($championId: [Int], $page: Int, $queueType: [Int], $duoRiotUserName: String, $duoRiotTagLine: String, $regionId: String!, $role: [Int], $seasonIds: [Int]!, $riotUserName: String!, $riotTagLine: String!) {
  fetchPlayerMatchSummaries(
    championId: $championId
    page: $page
    queueType: $queueType
    duoRiotUserName: $duoRiotUserName
    duoRiotTagLine: $duoRiotTagLine
    regionId: $regionId
    role: $role
    seasonIds: $seasonIds
    riotUserName: $riotUserName
    riotTagLine: $riotTagLine
  ) {
    finishedMatchSummaries
    totalNumMatches
    matchSummaries {
      assists
      augments
      championId
      cs
      damage
      deaths
      gold
      items
      jungleCs
      killParticipation
      kills
      level
      matchCreationTime
      matchDuration
      matchId
      maximumKillStreak
      primaryStyle
      queueType
      regionId
      role
      runes
      subStyle
      summonerName
      riotUserName
      riotTagLine
      summonerSpells
      psHardCarry
      psTeamPlay
      lpInfo {
        lp
        placement
        promoProgress
        promoTarget
        promotedTo {
          tier
          rank
          __typename
        }
        __typename
      }
      teamA {
        championId
        summonerName
        riotUserName
        riotTagLine
        teamId
        role
        hardCarry
        teamplay
        placement
        playerSubteamId
        __typename
      }
      teamB {
        championId
        summonerName
        riotUserName
        riotTagLine
        teamId
        role
        hardCarry
        teamplay
        placement
        playerSubteamId
        __typename
      }
      version
      visionScore
      win
      __typename
    }
    __typename
  }
}
";

    internal static readonly string GetSummonerProfile =
        @"query getSummonerProfile($regionId: String!, $seasonId: Int!, $riotUserName: String!, $riotTagLine: String!) {
  fetchProfileRanks(
    riotUserName: $riotUserName
    riotTagLine: $riotTagLine
    regionId: $regionId
    seasonId: $seasonId
  ) {
    rankScores {
      lastUpdatedAt
      losses
      lp
      promoProgress
      queueType
      rank
      role
      seasonId
      tier
      wins
      __typename
    }
    __typename
  }
  profileInitSimple(
    regionId: $regionId
    riotUserName: $riotUserName
    riotTagLine: $riotTagLine
  ) {
    lastModified
    memberStatus
    playerInfo {
      accountIdV3
      accountIdV4
      exodiaUuid
      iconId
      puuidV4
      regionId
      summonerIdV3
      summonerIdV4
      summonerLevel
      riotUserName
      riotTagLine
      __typename
    }
    customizationData {
      headerBg
      __typename
    }
    __typename
  }
}
";

    internal static readonly string GetPlayerOverallRanking =
        @"query getPlayerOverallRanking($queueType: Int, $riotUserName: String, $riotTagLine: String, $regionId: String) {
  overallRanking(
    queueType: $queueType
    riotUserName: $riotUserName
    riotTagLine: $riotTagLine
    regionId: $regionId
  ) {
    overallRanking
    totalPlayerCount
    __typename
  }
}
";

    internal static readonly string GetMultisearch =
        @"query GetMultisearch($regionId: [String], $riotUserName: [String!], $riotTagLine: [String!]) {
  getMultisearch(
    regionId: $regionId
    riotUserName: $riotUserName
    riotTagLine: $riotTagLine
  ) {
    roleStats {
      games
      roleName
      wins
      __typename
    }
    winsLastFifteen
    totalGamesLastFifteen
    winperc
    winstreak
    riotUserName
    riotTagLine
    rankData {
      lastUpdatedAt
      losses
      lp
      promoProgress
      queueType
      rank
      role
      seasonId
      tier
      wins
      __typename
    }
    worstChamps {
      champId
      games
      wins
      kills
      deaths
      assists
      __typename
    }
    bestChamps {
      champId
      games
      wins
      kills
      deaths
      assists
      __typename
    }
    __typename
  }
}
";

    internal static readonly string HistoricRanks =
        @"query historicRanks($queueType: Int!, $riotUserName: String!, $riotTagLine: String!, $regionId: String!) {
  getHistoricRanks(
    queueType: $queueType
    riotUserName: $riotUserName
    riotTagLine: $riotTagLine
    regionId: $regionId
  ) {
    lp
    queueId
    rank
    regionId
    season
    tier
    __typename
  }
}
";

}