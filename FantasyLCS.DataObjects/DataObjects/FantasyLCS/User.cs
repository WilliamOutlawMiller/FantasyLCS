using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLCS.DataObjects;

public class User
{
    public int ID { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public int? TeamID { get; set; }

    public int? LeagueID { get; set; }
}