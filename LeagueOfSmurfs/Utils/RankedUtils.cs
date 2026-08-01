using LeagueOfSmurfs.Configuration;
using System;
using System.Diagnostics;
using System.Drawing;

namespace LeagueOfSmurfs.Utils
{
    internal class RankedUtils
    {

        public static RankEnum getRankByEntry(LeagueEntryInfo entry)
        {
            RankEnum rankEnum = RankEnum.UNRANKED;
            string rankId = entry.Tier + "_" + entry.Rank;
            foreach (RankEnum rank in (RankEnum[])Enum.GetValues(typeof(RankEnum)))
            {
                if (rank.ToString().Equals(rankId))
                    return rank;
            }
            Debug.WriteLine("Can't find rank " + rankId);
            return RankEnum.UNRANKED;
        }

        public static string rankToString(RankEnum rankEnum)
        {
            string rank = rankEnum.ToString();

            // Master to challenger no num
            if (rankEnum >= RankEnum.CHALLENGER_I && rankEnum <= RankEnum.MASTER_I)
            {
                rank = rank.Replace("_I", "");
            }

            // Roman translate
            rank = rank.Replace("_IV", " 4");
            rank = rank.Replace("_III", " 3");
            rank = rank.Replace("_II", " 2");
            rank = rank.Replace("_I", " 1");

            return rank.ToLower();
        }

        public static Color getRankPen(RankEnum rank)
        {
            Color pen = Color.White;

            if (rank <= RankEnum.UNRANKED && rank >= RankEnum.IRON_I)
                pen = Color.FromArgb(62, 49, 44);
            else if (rank <= RankEnum.BRONZE_IV && rank >= RankEnum.BRONZE_I)
                pen = Color.FromArgb(130, 85, 78);
            else if (rank <= RankEnum.SILVER_IV && rank >= RankEnum.SILVER_I)
                pen = Color.FromArgb(163, 175, 182);
            else if (rank <= RankEnum.GOLD_IV && rank >= RankEnum.GOLD_I)
                pen = Color.FromArgb(243, 197, 146);
            else if (rank <= RankEnum.PLATINUM_IV && rank >= RankEnum.PLATINUM_I)
                pen = Color.FromArgb(43, 129, 141);
            else if (rank <= RankEnum.EMERALD_IV && rank >= RankEnum.EMERALD_I)
                pen = Color.FromArgb(14, 101, 65);
            else if (rank <= RankEnum.DIAMOND_IV && rank >= RankEnum.DIAMOND_I)
                pen = Color.FromArgb(74, 109, 186);
            else if (rank == RankEnum.MASTER_I)
                pen = Color.FromArgb(120, 60, 164);
            else if (rank == RankEnum.GRANDMASTER_I)
                pen = Color.FromArgb(137, 42, 28);
            else if (rank == RankEnum.CHALLENGER_I)
                pen = Color.FromArgb(193, 255, 255);

            return pen;
        }

    }
}
