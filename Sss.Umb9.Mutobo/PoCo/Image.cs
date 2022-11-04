﻿using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sss.Umb9.Mutobo.Enum;
using Sss.Umb9.Mutobo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Sss.Umb9.Mutobo.PoCo
{
    public class Image: ISliderItem
    {
        public IPublishedContent ImageNode { get; set; }
        public IEnumerable<ImageSource> Sources { get; set; }
        public string Alt { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Namespace { get; set; }
        public bool IsGoldenRatio { get; set; }


        public ImageSource DefaultSource => Sources?
            .FirstOrDefault(s => s.Size == EImageDimension.Default);

        public string GetSourcesAsJSON()
        {
            var serializer = new JsonSerializer();
            var result = JsonConvert.SerializeObject(Sources.Where(s => s.Size != EImageDimension.Default), new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter()
                }

            });

            return HttpUtility.HtmlDecode(result).Replace("\"", "'");

        }

        public HtmlString RenderWcPictureTag(string width = null, string height = null, string nameSpace = "picture-", string fireLoadingEvent = "", bool useSources = false)
        {
            if (height != null || width != null)
            {
                Height = height;
                Width = width;
            }


            var bld = new StringBuilder();
            var sources = IsGoldenRatio || useSources ? $"sources=\"{GetSourcesAsJSON()}\"" : String.Empty;
            var nameSpaceAttribute = $"namespace=\"{nameSpace}\"";

            bld.Append(
                $"<a-picture {fireLoadingEvent} {nameSpaceAttribute} defaultSource=\"{DefaultSource.Src}\" {sources} alt=\"{Alt}\" >");

            if (!IsGoldenRatio && (Width != null || Height != null))
            {
                bld.Append("<style>:host{");

                if (Height != null)
                {
                    bld.Append($"--{Namespace}-height:{Height};");
                    bld.Append($"--{Namespace}-height-mobile:{Height};");
                }
                if (Width != null)
                {
                    bld.Append($"--{Namespace}-width: min({Width}, var(--content-width, 100%));");
                    bld.Append($"--{Namespace}-width-mobile: min({Width}, var(--content-width-mobile, 100%));");
                }

                bld.Append("}</style>");
            }

            bld.Append($"</a-picture>");
            return new HtmlString(bld.ToString());
        }


    }
}
