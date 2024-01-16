using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyLCS.DataObjects.DataObjects.RequestData
{
    public class JoinLeagueRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("joinCode")]
        public string JoinCode { get; set; }
    }
}
