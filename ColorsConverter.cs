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
        public static List<AnchorColor> AllAnchorColors { get; private set; } = new List<AnchorColor>();
        public static List<Color> AllAnchorColorsList { get; private set; } = new List<Color>();

        static ColorsConverter()
        {
            RetrieveTableOfColors();
        }

        static void RetrieveTableOfColors(ColorFamily colorFamily = ColorFamily.Dmc)
        {
            if (colorFamily == ColorFamily.None) return;

            string[] csvText = Properties.Resources.DmcTable.Split('\n');

            switch (colorFamily)
            {
                case ColorFamily.Dmc:
                    csvText = Properties.Resources.DmcTable.Split('\n');
                    break;

                case ColorFamily.Anchor:
                    csvText = Properties.Resources.AnchorTable.Split('\n');
                    break;

                default:
                    break;

            }

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
                    switch (colorFamily)
                    {
                        case ColorFamily.Dmc:
                            DmcColor dmcColor = new DmcColor(rowData[0], rowData[1], int.Parse(rowData[2]), int.Parse(rowData[3]), int.Parse(rowData[4]), rowData[5]);
                            AllDmcColors.Add(dmcColor);
                            AllDmcColorsList.Add(Color.FromArgb(dmcColor.R, dmcColor.G, dmcColor.B));
                            break;

                        case ColorFamily.Anchor:
                            AnchorColor anchorColor = new AnchorColor(rowData[0], rowData[1], int.Parse(rowData[2]), int.Parse(rowData[3]), int.Parse(rowData[4]), rowData[5]);
                            AllAnchorColors.Add(anchorColor);
                            AllAnchorColorsList.Add(Color.FromArgb(anchorColor.R, anchorColor.G, anchorColor.B));
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public static TableColor ConvertColorToTableColor(Color color, ColorFamily colorFamily = ColorFamily.Dmc)
        {
            int closestIndex = 0;
            switch (colorFamily)
            {
                case ColorFamily.Dmc:
                    if (AllDmcColors.Count == 0)
                    {
                        RetrieveTableOfColors(ColorFamily.Dmc);
                    }

                    closestIndex = ImageTransformations.FindNewNearestMean(AllDmcColorsList, color);
                    return AllDmcColors[closestIndex];
                    break;

                case ColorFamily.Anchor:
                    if (AllAnchorColors.Count == 0)
                    {
                        RetrieveTableOfColors(ColorFamily.Anchor);
                    }

                    closestIndex = ImageTransformations.FindNewNearestMean(AllAnchorColorsList, color);
                    return AllAnchorColors[closestIndex];
                    break;

                default:
                    return new TableColor("", "", 0, 0, 0, "");
                    break;
            }
        }

    }

    public enum ColorFamily
    {
        None, 
        Dmc,
        Anchor
    }

}
