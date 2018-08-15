using AutoMapper;
using AutoMapper.Configuration;
using CSA.Models;
using CSA.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSA.Configuration
{
    static class AutoMapperProfile
    {
        public static void InitializeAutomapper()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMaps();
            });
        }

        private static void CreateMaps(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Script, MacroModel>()
                .ForMember(d => d.FileName, s => s.MapFrom(x => x.ScriptName))
                .ForMember(d => d.Description, s => s.MapFrom(x => x.ScriptDescription))
                .ForMember(d => d.ParameterList, s => s.MapFrom(x => x.Parameters.Parameter))
                .ForMember(d => d.Images, s => s.MapFrom(x => x.Images))
                .ForMember(d => d.Warnings, s => s.MapFrom(x => x.Warnings))
                .ForMember(d => d.UniqueID, s => s.ResolveUsing(x => Guid.NewGuid()));

            cfg.CreateMap<Parameter, ParameterModel>()
                .ForMember(d => d.ParameterName, s => s.MapFrom(x => x.ParameterScriptName))
                .ForMember(d => d.DisplayName, s => s.MapFrom(x => x.ParameterDisplayName))
                .ForMember(d => d.Type, s => s.MapFrom(x => x.ParameterType))
                .ForMember(d => d.DefaultValue, s => s.MapFrom(x => x.ParameterDefaultValue))
                .ForMember(d => d.IsSharedInContext, s => s.MapFrom(x => x.ParameterIsSharedInContext));

            cfg.CreateMap<Images, Dictionary<string, string>>()
                    .ConvertUsing(source => source.CreateDictionaryFromImages());

            cfg.CreateMap<Models.DTO.Warnings, Models.Warnings>()
                .ConvertUsing(source => source.Convert());

        }

        private static Dictionary<string, string> CreateDictionaryFromImages(this Images images)
        {
            if (images == null)
                return null;
            var dict = new Dictionary<string, string>();
            foreach(var image in images.Image)
            {
                dict.Add(image.ImageText, image.ImageName);
            }
            return dict;
        }

        public static Models.Warnings Convert(this Models.DTO.Warnings warnings)
        {
            if (warnings != null && warnings.Warning!=null & warnings.Warning.Any())
            {
                var temp = new Models.Warnings();
                temp.IsAfter = warnings.WarningIsAfterMacro.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                temp.WarningList = new List<Models.Warning>();
                foreach(var warning in warnings.Warning)
                {
                    temp.WarningList.Add(new Models.Warning()
                    {
                        WarningImagePath = warning.WarningImage,
                        WarningText = warning.WarningText
                    });
                }
                return temp;
            }
            return null;
        }
    }
}
