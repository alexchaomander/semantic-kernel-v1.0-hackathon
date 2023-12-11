using GemBox.Presentation;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class PowerPointWriter
    {
        public string Key { set; get; }
        public PowerPointWriter()
        {
            var setting = AppSettings.LoadSettings();
            Key = setting.GemboxKey;
        }
        public byte[] WriteToPptx(List<ContentObject> PageContents)
        {
            try
            {
                // If using the Professional version, put your serial key below.
                // If using the Professional version, put your serial key below.
                ComponentInfo.SetLicense(Key);

                var presentation = new PresentationDocument();
             
                foreach (var content in PageContents)
                {
                    var slide = presentation.Slides.AddNew(SlideLayoutType.Custom);
                    var textBox = slide.Content.AddTextBox(ShapeGeometryType.Rectangle, 2, 2, 5, 4, LengthUnit.Centimeter);
                    var paragraph = textBox.AddParagraph();
                    var contentbreak = content.Content.Split("\n");
                    foreach(var cb in contentbreak) 
                        paragraph.AddRun(cb);
                }

                var ms = new MemoryStream();
                presentation.Save(ms, SaveOptions.Pptx);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
