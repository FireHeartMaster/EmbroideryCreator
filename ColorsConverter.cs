using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public static class ColorsConverter
    {
        public static List<DmcColor> AllDmcColors { get; private set; } = new List<DmcColor>();
        public static List<Color> AllDmcColorsList { get; private set; } = new List<Color>();

        //static ColorsConverter()
        //{
        //    RetrieveDmcTable();
        //}

        static void RetrieveDmcTable()
        {
            string[] csvText = Properties.Resources.DmcTable.Split('\n');

            bool alreadyPassedThroughHeader = false;

            foreach (string row in csvText)
            {
                string[] rowData = row.Split(';');

                if (!alreadyPassedThroughHeader)
                {
                    alreadyPassedThroughHeader = true;
                    continue;
                }

                if(rowData.Length == 6)
                {
                    DmcColor dmcColor = new DmcColor(rowData[0], rowData[1], int.Parse(rowData[2]), int.Parse(rowData[3]), int.Parse(rowData[4]), rowData[5]);
                    AllDmcColors.Add(dmcColor);
                    AllDmcColorsList.Add(Color.FromArgb(dmcColor.R, dmcColor.G, dmcColor.B));
                }
            }
        }

        public static DmcColor ConvertColorToDmc(Color color)
        {
            if (AllDmcColors.Count == 0)
            {
                RetrieveDmcTable();
            }

            int closestIndex = ImageTransformations.FindNewNearestMean(AllDmcColorsList, color);
            return AllDmcColors[closestIndex];
        }

    }

}
