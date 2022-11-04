using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sss.Umb9.Mutobo.Interfaces;
using Sss.Umb9.Mutobo.PoCo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Sss.Umb9.Mutobo.Controllers.Api;

[EnableCors(PolicyName = "_myAllowSpecificOrigins")]
[AllowAnonymous]
public class CaptchaController : UmbracoApiController
{

    private ICaptchaService _captchaService;
    private IUmbracoContextAccessor _umbracoContextAccessor;


    public CaptchaController(ICaptchaService captchaService, IUmbracoContextAccessor umbracoContextAccessor)
    {
        _captchaService = captchaService;
        _umbracoContextAccessor = umbracoContextAccessor;
    }


    [HttpGet]
    public CaptchaResponse GetCaptchaImage()
    {
        var result = _captchaService.GenerateCaptcha();
        return result;
    }

    [HttpPost]
    public bool ValidateCaptcha([FromBody] CaptchaRequest request)
    {
        var result = _captchaService.ValidateCaptcha(request);
        return result;
    }

    [HttpGet]
    public CaptchaResponse RefreshCaptcha(Guid id)
    {
        var test = _captchaService.RefreshCaptcha(id);
        return test;
    
    }
}
