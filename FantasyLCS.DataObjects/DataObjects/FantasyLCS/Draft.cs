using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Draft
{
    public int ID { get; set; }

    public int LeagueID { get; set; }

    public List<DraftPlayer> DraftPlayers { get; set; }

    public List<int> DraftOrder { get; set; }

    public int CurrentRound { get; set; }

    public int CurrentPickIndex { get; set; }
}
