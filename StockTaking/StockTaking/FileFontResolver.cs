using PdfSharpCore.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StockTaking
{
    internal class FileFontResolver : IFontResolver
    {
        // public string DefaultFontName => throw new NotImplementedException();
        public string DefaultFontName => "Verdana";
        public byte[] GetFont(string faceName)
        {
            var assembly = typeof(FileFontResolver).Assembly;
            var stream = assembly.GetManifestResourceStream($"StockTaking.Resources.Fonts.{faceName}");
            using (var reader = new StreamReader(stream))
            {
                var bytes = default(byte[]);
                using (var ms = new MemoryStream())
                {
                    reader.BaseStream.CopyTo(ms);
                    bytes = ms.ToArray();
                   /* using (var fs = File.Open(faceName, FileMode.Open))
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        return ms.ToArray();
                    }*/
                }
                return bytes;
            }
           
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Verdana", StringComparison.CurrentCultureIgnoreCase))
            {
                if (isBold && isItalic)
                {
                    return new FontResolverInfo("verdana_bold_italic.ttf");

                }
                else if (isBold)
                {
                    // return new FontResolverInfo("Fonts/verdana_bold.ttf");
                    return new FontResolverInfo("verdana_bold.ttf");

                }
                else if (isItalic)
                {
                    return new FontResolverInfo("verdana_italic.ttf");

                }
                else
                {
                    return new FontResolverInfo("verdana_regular.ttf");
                }
            }
            return null;
        }

    }
}