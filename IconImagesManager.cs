using EmbroideryCreator.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class IconImagesManager
    {
        List<Tuple<Bitmap, bool>> allIcons;

        public IconImagesManager()
        {
            //string[] allImagesPaths = Directory.GetFiles(Resources.);
            ResourceSet resourceSet = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            Dictionary<string, Bitmap> allIconsDictionary = new Dictionary<string, Bitmap>();
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();

                if(resourceKey.Length == 3)
                {
                    Bitmap resource = (Bitmap)entry.Value;
                    allIconsDictionary.Add(resourceKey, resource);
                }
            }


            allIcons = allIconsDictionary.OrderBy(element => element.Key).Select(element => new Tuple<Bitmap, bool>(element.Value, false)).ToList();
        }

        public Bitmap GetNextIcon()
        {
            if(allIcons.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < allIcons.Count; i++)
            {
                if (!allIcons[i].Item2)
                {
                    allIcons[i] = new Tuple<Bitmap, bool>(allIcons[i].Item1, true);
                    return new Bitmap(allIcons[i].Item1);
                }
            }

            //if we got to this point then every icon was already used, let's start over and repeat icons
            for (int i = 0; i < allIcons.Count; i++)
            {
                allIcons[i] = new Tuple<Bitmap, bool>(allIcons[i].Item1, false);
            }

            //Since now all icons are considered as "not used", we can return the first one
            return allIcons[0].Item1;
        }
    }
}
