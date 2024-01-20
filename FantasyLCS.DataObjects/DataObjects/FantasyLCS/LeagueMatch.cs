using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyLCS.DataObjects;

public class LeagueMatch
{
    public int ID { get; set; }

    public int LeagueID { get; set; }
    
    public League League { get; set; }

    public string Week { get; set; }

    public DateTime MatchDate { get; set; }

    public int TeamOneID { get; set; }

    public Team TeamOne { get; set; }

    public int TeamTwoID { get; set; }

    public Team TeamTwo { get; set; }

    public Winner Winner { get; set; }
}

public enum Winner
{
    NotPlayed,
    TeamOne,
    TeamTwo
}
