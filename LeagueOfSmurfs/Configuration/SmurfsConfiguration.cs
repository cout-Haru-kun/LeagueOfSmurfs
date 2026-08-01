using LeagueOfSmurfs.Configuration;

namespace LeagueOfSmurfs.Configurations
{
    public class SmurfsConfiguration
    {

        public SmurfsConfiguration() 
        {
            puuid = string.Empty;
            encryptedId = string.Empty;

            // Safe
            username = string.Empty;
            password = string.Empty;

            // Base
            summonerName = string.Empty;
            level = 0;
            region = RegionEnum.EUW;
            

            // Ranked
            flexRank = RankEnum.UNRANKED;
            flexLP = 0;
            soloRank = RankEnum.UNRANKED;
            soloLP = 0;
        }

        public string puuid { get; set; }
        public string encryptedId { get; set; }


        // Safe
        public string username { get; set; }
        public string password { get; set; }
        public RegionEnum region { get; set; }


        // Base
        public string summonerName { get; set; }
        public long level { get; set; }


        // Ranked
        public RankEnum flexRank { get; set; }
        public int flexLP{ get; set; }
        public RankEnum soloRank { get; set; }
        public int soloLP { get; set; }

    }
}
