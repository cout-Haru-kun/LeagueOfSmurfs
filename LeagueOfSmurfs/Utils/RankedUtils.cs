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

        public static ValorantRankEnum getValorantRankByTierId(int tierId)
        {
            if (tierId <= 0)
                return ValorantRankEnum.UNRANKED;
            if (tierId > 25)
                tierId = 25;

            foreach (ValorantRankEnum rank in (ValorantRankEnum[])Enum.GetValues(typeof(ValorantRankEnum)))
            {
                if ((int)rank == tierId)
                    return rank;
            }
            return ValorantRankEnum.UNRANKED;
        }

        public static string valorantRankToString(ValorantRankEnum rank)
        {
            if (rank == ValorantRankEnum.UNRANKED)
                return "unranked";
            if (rank == ValorantRankEnum.RADIANT)
                return "radiant";

            string text = rank.ToString().ToLowerInvariant().Replace('_', ' ');
            return text;
        }

        public static Color getValorantRankPen(ValorantRankEnum rank)
        {
            int id = (int)rank;
            if (id <= 0)
                return Color.FromArgb(70, 70, 70);
            if (id <= 3)
                return Color.FromArgb(90, 85, 80);       // Iron
            if (id <= 6)
                return Color.FromArgb(140, 95, 60);      // Bronze
            if (id <= 9)
                return Color.FromArgb(170, 180, 190);    // Silver
            if (id <= 12)
                return Color.FromArgb(220, 180, 70);     // Gold
            if (id <= 15)
                return Color.FromArgb(60, 180, 170);     // Platinum
            if (id <= 18)
                return Color.FromArgb(100, 140, 230);    // Diamond
            if (id <= 21)
                return Color.FromArgb(70, 200, 120);     // Ascendant
            if (id <= 24)
                return Color.FromArgb(220, 70, 90);      // Immortal
            return Color.FromArgb(255, 220, 90);         // Radiant
        }
    }
}
