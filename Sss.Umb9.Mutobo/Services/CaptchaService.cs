
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using Sss.Umb9.Mutobo.Interfaces;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;



using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

using SixLabors.ImageSharp.Drawing;
using SixLabors.Fonts;

using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;
using Sss.Umb9.Mutobo.PoCo;
using Font = SixLabors.Fonts.Font;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Webp;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace Sss.Umb9.Mutobo.Services;

public class CaptchaService : ICaptchaService
{
    private readonly object _lockObj = new();
    private readonly int _width = 500;
    private readonly int _height = 60;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    private readonly Dictionary<Guid, Captcha> _captchas = new Dictionary<Guid, Captcha>();
    private readonly List<Color> _darkColors = new List<Color> {
        Color.Blue,
        Color.Purple,
        Color.DarkCyan,
        Color.DarkGoldenrod,
        Color.DarkGray,
        Color.DarkGreen,
        Color.DarkOrange,
        Color.DarkViolet

    };

    private readonly List<Color> _lightColors = new List<Color> {
        Color.White,
        Color.LightYellow,
        Color.MintCream,
        Color.LightPink,
        Color.Beige,
        Color.LightSteelBlue,
        Color.LightSeaGreen
    };



    private async Task CleanupCaptchas() {
        Guid keyToRemove = Guid.Empty;
        lock (_lockObj) {

            foreach (var key in _captchas.Keys)
            {
                if (_captchas[key].TimeStamp < DateTime.Now.AddMinutes(-5))
                {
                    keyToRemove = key;
                }
            }

            if (keyToRemove != Guid.Empty)
                _captchas.Remove(keyToRemove);
        }
        await Task.Delay(300_000);
    }


    public CaptchaService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        var task = Task.Run(async () =>
        {
            while (true)
            {
                await CleanupCaptchas();
               
            }
        });
    }

 
    public CaptchaResponse GenerateCaptcha()
    {
        var id = Guid.NewGuid();
        var text = GetRandomText(6);

        Captcha captcha = new() {
            Image = GenerateCapcthaImage(text),
            Text = text,
            TimeStamp = DateTime.Now
        };

        lock (_lockObj)
        {
            _captchas.Add(id, captcha);
        }
        
        return new()
        {
            Id = id,
            Image = captcha.Image.ToBase64String(captcha.Image.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension("png"))
        };

    }

    public CaptchaResponse RefreshCaptcha(Guid id)
    {
        CaptchaResponse result = null;
        lock (_lockObj)
        {
            if (_captchas.ContainsKey(id))
            {
                var newText = GetRandomText(6);
                var image = GenerateCapcthaImage(newText);
                _captchas[id].Text = newText;
                _captchas[id].Image = image;
                _captchas[id].TimeStamp = DateTime.Now;
                result = new()
                {
                    Id = id,
                    Image = _captchas[id].Image.ToBase64String(_captchas[id].Image.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension("png"))
                };

            }


        }

        return result;
    }

    public bool ValidateCaptcha(CaptchaRequest request)
    {
        if (!_captchas.ContainsKey(request.Id)) return false;
        var captcha = _captchas[request.Id];
        if (captcha == null) return false;
        if (captcha.Text != request.Text) return false;
        return true;
    }

    private Image<Rgba32> GenerateCapcthaImage(string text)
    {
        Image<Rgba32> image = new(_width, _height);
        
        Image<Rgba32> textImage = new(_width, _height);
        FontCollection collection = new();

        var fontPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "assets", "fonts", "Creepster-Regular.ttf");

        FontFamily family = collection.Add(fontPath);
        Font font = family.CreateFont(50);

        // The options are optional
        TextOptions options = new(font)
        {
            Origin = new PointF(20, 0), // Set the rendering origin.
            TabWidth = 3, // A tab renders as 8 spaces wide
            HorizontalAlignment = HorizontalAlignment.Left// Right align
        };


        TextOptions options2 = new(font)
        {
            Origin = new PointF(21, 1), // Set the rendering origin.
            TabWidth = 3, // A tab renders as 8 spaces wide
            HorizontalAlignment = HorizontalAlignment.Left// Right align
        };

        TextOptions options3 = new(font)
        {
            Origin = new PointF(22, 2), // Set the rendering origin.
            TabWidth = 3, // A tab renders as 8 spaces wide
            HorizontalAlignment = HorizontalAlignment.Left// Right align
        };

        IBrush brush = Brushes.ForwardDiagonal(_darkColors[Random.Shared.Next(_darkColors.Count())], _darkColors[Random.Shared.Next(_darkColors.Count())]);
        IPen pen = Pens.Dot(Color.White, 0.2f);
        IPath backGround = new RectangularPolygon(0, 0, _width - 1, _height - 1);
        IBrush brushBg = Brushes.BackwardDiagonal(_darkColors[Random.Shared.Next(_darkColors.Count())], _lightColors[Random.Shared.Next(_lightColors.Count())]);
        image.Mutate(x => x.Fill(brushBg).Draw(pen, backGround));
        // Draws the text with horizontal red and blue hatching with a dash dot pattern outline.
        image = Randomize(image);
        textImage.Mutate(x => x.DrawText(options, text, brush, pen));
        image.Mutate(x => x.DrawImage(textImage, 0.3f));
        textImage = new Image<Rgba32>(_width, _height);
        textImage.Mutate(x => x.DrawText(options2, text, brush, pen));
        image.Mutate(x => x.DrawImage(textImage, 0.3f));
        textImage = new Image<Rgba32>(_width, _height);
        textImage.Mutate(x => x.DrawText(options3, text, brush, pen));
        image.Mutate(x => x.DrawImage(textImage, 0.3f));
        image.Mutate(i => i.Crop(180, 60));
        return image;
    }

    private string GetRandomText(int length) {

        StringBuilder bld = new();


        for (var i = 0; i < 6; i++)
        {
         
                bld.Append((char)Random.Shared.Next(65, 90));
         
        }

        return bld.ToString();

    }

    private Image<Rgba32> Randomize(Image<Rgba32> image)
    {
        Random rnd = new();

        IBrush brush = Brushes.ForwardDiagonal(_lightColors[Random.Shared.Next(_lightColors.Count())], _lightColors[Random.Shared.Next(_lightColors.Count())]);
        IPen pen = Pens.Dot(Color.WhiteSmoke, 2);
        var fontPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "assets", "fonts", "Creepster-Regular.ttf");
        FontCollection collection = new();
        FontFamily family = collection.Add(fontPath);
        Font font = family.CreateFont(50);
        
        var x = 20f;
        var y = 0f;

        for (var i = 0;  i < 20; i++) {
         
     
            var text = GetRandomText(9);
            TextOptions options = new(font)
            {
                Origin = new PointF(i % 2 == 0 ? 0 + i * 15 : 0 - i * 15, i% 3 == 0 ? y +20 : y- 20), // Set the rendering origin.
                TabWidth = 3, // A tab renders as 8 spaces wide
                HorizontalAlignment = HorizontalAlignment.Center

            };
            image.Mutate(x => x.DrawText(options, text, brush, pen));

        }
      
        return image;
    }


}
