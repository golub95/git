using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klada_API.Kladionice.Admiral.API_Class
{
    public class AdmiralAPIList
    {
        public Competition[] competitions { get; set; }
    }
    public class Competition
    {
        public int competitionId { get; set; }
        public int regionId { get; set; }
        public string competitionName { get; set; }
        public string regionName { get; set; }
        public int eventsCount { get; set; }
        public string liveFavouriteOrderNumber { get; set; }
        public Event[] events { get; set; }
    }
    public class Event
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string isLive { get; set; }
        public string competitionName { get; set; }
        public string regionName { get; set; }
        public string sportName { get; set; }
        public string dateTime { get; set; }
        public Bets[] bets { get; set; }
    }
    public class Bets
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public int competitionId { get; set; }
        int regionId { get; set; }
        int sportId { get; set; }
        public bool isPlayable { get; set; }
        public BetOutcomes[] betOutcomes { get; set; }
    }
    public class BetOutcomes
    {
        public string name { get; set; }
        public string odd { get; set; }
    }
}
