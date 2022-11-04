using Microsoft.Extensions.Options;
using Sss.Umb9.Mutobo.PipelineFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Sss.Umb9.Mutobo.Options;

public class CustomUmbracoOptions : IConfigureOptions<UmbracoPipelineOptions>
{
    public void Configure(UmbracoPipelineOptions options)
    {
        options.AddFilter(new CustomFilter());
    }
}
