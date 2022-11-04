using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Sss.Umb9.Mutobo.PoCo;

public class Captcha
{
    public Image<Rgba32> Image { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}
