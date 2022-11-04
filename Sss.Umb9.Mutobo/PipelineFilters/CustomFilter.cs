using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Sss.Umb9.Mutobo.PipelineFilters;

public class CustomFilter : UmbracoPipelineFilter
{

    public CustomFilter() : base(nameof(CustomFilter))
    {
        PostPipeline = a =>
        {
            var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
            a.UseCors();
        
        };
    }
}
