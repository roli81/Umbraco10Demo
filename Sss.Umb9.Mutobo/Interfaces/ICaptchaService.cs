using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sss.Umb9.Mutobo.PoCo;

namespace Sss.Umb9.Mutobo.Interfaces
{
    public interface ICaptchaService
    {
        CaptchaResponse GenerateCaptcha();
        bool ValidateCaptcha(CaptchaRequest request);

        CaptchaResponse RefreshCaptcha(Guid Id);


    }
}
