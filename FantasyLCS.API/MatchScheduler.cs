using Microsoft.EntityFrameworkCore;
using System.Text;
using FantasyLCS.DataObjects;
using System.Collections.Generic;
using Constants;

namespace FantasyLCS.API
{
    public static class MatchScheduler
    {
        public static List<LeagueMatch> GenerateLeagueMatches(League league, List<Team> teams, AppDbContext dbContext)
        {
            List<LeagueMatch> leagueMatches = new List<LeagueMatch>();

            StringBuilder txtResults = new StringBuilder();

            int numDays = teams.Count() - 1;
            int halfsize = teams.Count() / 2;

            List<Team> temp = new List<Team>();
            List<Team> rrTeams = new List<Team>();

            rrTeams.AddRange(teams);
            temp.AddRange(teams);
            rrTeams.RemoveAt(0);

            int teamSize = rrTeams.Count;

            try
            {
                for (int day = 0; day < numDays * 2; day++)
                {
                    //Calculate1stRound(day);
                    if (day % 2 == 0)
                    {
                        int teamIdx = day % teamSize;

                        leagueMatches.Add(new LeagueMatch
                        {
                            LeagueID = league.ID,
                            League = league,
                            Week = MatchScheduleConstants.MatchSchedule[day].Item1.Item1,
                            MatchDate = MatchScheduleConstants.MatchSchedule[day].Item2,
                            TeamOneID = rrTeams[teamIdx].ID,
                            TeamOne = rrTeams[teamIdx],
                            TeamTwoID = temp[0].ID,
                            TeamTwo = temp[0],
                            Winner = Winner.NotPlayed
                        });

                        for (int idx = 0; idx < halfsize; idx++)
                        {
                            int firstTeam = (day + idx) % teamSize;
                            int secondTeam = ((day + teamSize) - idx) % teamSize;

                            if (firstTeam != secondTeam)
                            {
                                leagueMatches.Add(new LeagueMatch
                                {
                                    LeagueID = league.ID,
                                    League = league,
                                    Week = MatchScheduleConstants.MatchSchedule[day].Item1.Item1,
                                    MatchDate = MatchScheduleConstants.MatchSchedule[day].Item2,
                                    TeamOneID = rrTeams[firstTeam].ID,
                                    TeamOne = rrTeams[firstTeam],
                                    TeamTwoID = rrTeams[secondTeam].ID,
                                    TeamTwo = rrTeams[secondTeam],
                                    Winner = Winner.NotPlayed
                                });
                            }
                        }
                    }

                    //Calculate2ndRound(day);
                    if (day % 2 != 0)
                    {
                        int teamIdx = day % teamSize;

                        leagueMatches.Add(new LeagueMatch
                        {
                            LeagueID = league.ID,
                            League = league,
                            Week = MatchScheduleConstants.MatchSchedule[day].Item1.Item1,
                            MatchDate = MatchScheduleConstants.MatchSchedule[day].Item2,
                            TeamOneID = temp[0].ID,
                            TeamOne = temp[0],
                            TeamTwoID = rrTeams[teamIdx].ID,
                            TeamTwo = rrTeams[teamIdx],
                            Winner = Winner.NotPlayed
                        });

                        for (int idx = 0; idx < halfsize; idx++)
                        {
                            int firstTeam = (day + idx) % teamSize;
                            int secondTeam = ((day + teamSize) - idx) % teamSize;

                            if (firstTeam != secondTeam)
                            {
                                leagueMatches.Add(new LeagueMatch
                                {
                                    LeagueID = league.ID,
                                    League = league,
                                    Week = MatchScheduleConstants.MatchSchedule[day].Item1.Item1,
                                    MatchDate = MatchScheduleConstants.MatchSchedule[day].Item2,
                                    TeamOneID = rrTeams[secondTeam].ID,
                                    TeamOne = rrTeams[secondTeam],
                                    TeamTwoID = rrTeams[firstTeam].ID,
                                    TeamTwo = rrTeams[firstTeam],
                                    Winner = Winner.NotPlayed
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return leagueMatches;
        }
    }
}
