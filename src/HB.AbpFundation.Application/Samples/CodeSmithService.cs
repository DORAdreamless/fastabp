using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using CodeSmithCore.Helpers;
using CodeSmithCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TextTemplating;
using Mono.TextTemplating;
using System.Linq;

namespace HB.AbpFundation.Samples;


public class CodeSmithService : AbpFundationAppService, ICodeSmithService
{
    private readonly IConfiguration _configuration;

    public CodeSmithService(IConfiguration configuration)
    {
        _configuration = configuration;
    }



    public async Task<List<PersistenceObjectInfo>> GetAllDomainsAsync()
    {
        await Task.Delay(10);
        return Configs.DataSource.Select(x => new PersistenceObjectInfo()
        {
            Comment = x.Comment,
            TableName = x.TableName,
            TypeName = x.TypeName,
            Name = x.Name,
        }).ToList();
    }

    public async Task<GenerateCodeDto> GenerateCodeAsync(GenerateCodeInput input)
    {
        GenerateCodeDto result = new GenerateCodeDto();
        var list = Configs.DataSource.Where(x => input.TypeNames.Contains(x.TypeName)).ToList();
        var baseDirectory = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        string templateDirectory = _configuration["App:Templates"];
        string outputDirectory = _configuration["App:OutputDirectory"];
        string applicationNamespace = $"{input.CompanyName}.{input.ModuleName}.Application";
        string applicationContractsNamespace = $"{input.CompanyName}.{input.ModuleName}.Application.Contracts";
        string entityframeworkCoreNamespace = $"{input.CompanyName}.{input.ModuleName}.EntityFrameworkCore";
        string domainNamespace = $"{input.CompanyName}.{input.ModuleName}.Domain";
        string domainSharedNamespace = $"{input.CompanyName}.{input.ModuleName}.Domain.Shared";
        string httpApiNamespace = $"{input.CompanyName}.{input.ModuleName}.HttpApi";

        string templateFilename = "";
        string outputFilename = "";
        string templateContent = "";
        ITextTemplatingSession session = null;
        TemplateGenerator generator = null;
        ParsedTemplate parsed = null;
        TemplateSettings settings = null;
        string generatedFilename = null;
        string generatedContent = null;

        foreach (var item in list)
        {
            Logger.LogWarning($"生成代码:{item.Comment}");


            #region    application
            outputFilename = System.IO.Path.Combine(outputDirectory, applicationNamespace, input.FunctionName, $"{item.UseCaseServiceName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "UseCaseService.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, applicationNamespace, input.FunctionName, $"{item.QueryServiceName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "QueryService.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion

            #region domain-shared



            outputFilename = System.IO.Path.Combine(outputDirectory, domainSharedNamespace,"DTOs",  input.FunctionName, item.AggregationName, $"{item.GetListInput}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "GetListInput.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", domainSharedNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, domainSharedNamespace, "DTOs", input.FunctionName, item.AggregationName, $"{item.CreateInput}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "CreateInput.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", domainSharedNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, domainSharedNamespace, "DTOs", input.FunctionName, item.AggregationName, $"{item.UpdateInput}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "UpdateInput.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", domainSharedNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, domainSharedNamespace, "DTOs",  input.FunctionName, item.AggregationName, $"{item.DeleteInput}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "DeleteInput.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", domainSharedNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
           // IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, domainSharedNamespace, "DTOs",  input.FunctionName, item.AggregationName, $"{item.Output}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "Output.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", domainSharedNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, domainSharedNamespace, "DTOs",  input.FunctionName, item.AggregationName, $"{item.GetIdInput}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "GetIdInput.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", domainSharedNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
           // IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion

            #region domain
            outputFilename = System.IO.Path.Combine(outputDirectory, domainNamespace, "Repositories", input.FunctionName, $"{item.IRepositoryName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "IRepository.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", domainNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion

            #region interfaces
            outputFilename = System.IO.Path.Combine(outputDirectory, applicationContractsNamespace, input.FunctionName,  $"{item.IQueryServiceName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "IQueryService.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationContractsNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, applicationContractsNamespace, input.FunctionName,  $"{item.IUseCaseServiceName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "IUseCaseService.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationContractsNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, applicationContractsNamespace, input.FunctionName, "Permissions", $"{item.AggregationName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "Permissions.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationContractsNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            //IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion

            #region infrastructure
            outputFilename = System.IO.Path.Combine(outputDirectory, entityframeworkCoreNamespace, "Repositories", input.FunctionName, $"{item.RepositoryName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "Repository.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", entityframeworkCoreNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion

            #region webapi
            outputFilename = System.IO.Path.Combine(outputDirectory, httpApiNamespace, input.FunctionName, $"{item.ControllerName}.cs");
            templateFilename = System.IO.Path.Combine(templateDirectory, "Controller.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", httpApiNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            //session.Add("PermissionCode", input.PermissionCode);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion

            #region api
            //outputFilename = System.IO.Path.Combine(outputDirectory, "src", $"api", StringUtils.GetCamelCaseVar(input.ModuleName), StringUtils.GetCamelCaseVar(input.FunctionName), $"{item.AggregationVar}.js");
            //templateFilename = System.IO.Path.Combine(templateDirectory, "api.tt");
            //templateContent = System.IO.File.ReadAllText(templateFilename);
            //generator = new TemplateGenerator();
            //parsed = generator.ParseTemplate(templateFilename, templateContent);
            //settings = TemplatingEngine.GetSettings(generator, parsed);
            //settings.CompilerOptions = "-nullable:enable";
            //session = generator.GetOrCreateSession();
            //session.Add("ModuleName", input.ModuleName);
            //session.Add("FunctionName", input.FunctionName);
            ////session.Add("PermissionCode", input.PermissionCode);
            //session.Add("Entity", item);
            //(generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
            //    parsed, templateFilename, templateContent, outputFilename, settings
            //);
            //IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion

            #region ui
            outputFilename = System.IO.Path.Combine(outputDirectory, "src", $"views", input.ModuleName, input.FunctionName, item.AggregationName, "index.vue");
            templateFilename = System.IO.Path.Combine(templateDirectory, "index.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            //session.Add("PermissionCode", input.PermissionCode);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, "src", $"views", input.ModuleName, input.FunctionName, item.AggregationName, "components", "editor.vue");
            templateFilename = System.IO.Path.Combine(templateDirectory, "editor.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);

            outputFilename = System.IO.Path.Combine(outputDirectory, "src", $"views", input.ModuleName, input.FunctionName, item.AggregationName, "components", "view.vue");
            templateFilename = System.IO.Path.Combine(templateDirectory, "view.tt");
            templateContent = System.IO.File.ReadAllText(templateFilename);
            generator = new TemplateGenerator();
            parsed = generator.ParseTemplate(templateFilename, templateContent);
            settings = TemplatingEngine.GetSettings(generator, parsed);
            settings.CompilerOptions = "-nullable:enable";
            session = generator.GetOrCreateSession();
            session.Add("Namespace", applicationNamespace);
            session.Add("CompanyName", input.CompanyName);
            session.Add("ModuleName", input.ModuleName);
            session.Add("FunctionName", input.FunctionName);
            session.Add("Entity", item);
            (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
                parsed, templateFilename, templateContent, outputFilename, settings
            );
            IOHelper.WriteToFile(generatedFilename, generatedContent);
            #endregion
        }


        outputFilename = System.IO.Path.Combine(outputDirectory, applicationNamespace, input.FunctionName, $"{input.FunctionName}AutoMapperProfile.cs");
        templateFilename = System.IO.Path.Combine(templateDirectory, "Profile.tt");
        templateContent = System.IO.File.ReadAllText(templateFilename);
        generator = new TemplateGenerator();
        parsed = generator.ParseTemplate(templateFilename, templateContent);
        settings = TemplatingEngine.GetSettings(generator, parsed);
        settings.CompilerOptions = "-nullable:enable";
        session = generator.GetOrCreateSession();
        session.Add("Namespace", applicationNamespace);
        session.Add("CompanyName", input.CompanyName);
        session.Add("ModuleName", input.ModuleName);
        session.Add("FunctionName", input.FunctionName);
        session.Add("EntityCollection", list);
        (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
            parsed, templateFilename, templateContent, outputFilename, settings
        );
       // IOHelper.WriteToFile(generatedFilename, generatedContent);
       result.AutoMapperProfile = generatedContent;


        outputFilename = System.IO.Path.Combine(outputDirectory, applicationContractsNamespace, input.FunctionName, "Permissions", $"{input.FunctionName}PermissionDefinitionProvider.cs");
        templateFilename = System.IO.Path.Combine(templateDirectory, "PermissionDefinitionProvider.tt");
        templateContent = System.IO.File.ReadAllText(templateFilename);
        generator = new TemplateGenerator();
        parsed = generator.ParseTemplate(templateFilename, templateContent);
        settings = TemplatingEngine.GetSettings(generator, parsed);
        settings.CompilerOptions = "-nullable:enable";
        session = generator.GetOrCreateSession();
        session.Add("Namespace", applicationContractsNamespace);
        session.Add("CompanyName", input.CompanyName);
        session.Add("ModuleName", input.ModuleName);
        session.Add("FunctionName", input.FunctionName);
        session.Add("EntityCollection", list);
        (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
            parsed, templateFilename, templateContent, outputFilename, settings
        );
       // IOHelper.WriteToFile(generatedFilename, generatedContent);

        outputFilename = System.IO.Path.Combine(outputDirectory, entityframeworkCoreNamespace, input.FunctionName, $"{input.FunctionName}DbContext.cs");
        templateFilename = System.IO.Path.Combine(templateDirectory, "DbContext.tt");
        templateContent = System.IO.File.ReadAllText(templateFilename);
        generator = new TemplateGenerator();
        parsed = generator.ParseTemplate(templateFilename, templateContent);
        settings = TemplatingEngine.GetSettings(generator, parsed);
        settings.CompilerOptions = "-nullable:enable";
        session = generator.GetOrCreateSession();
        session.Add("Namespace", entityframeworkCoreNamespace);
        session.Add("CompanyName", input.CompanyName);
        session.Add("ModuleName", input.ModuleName);
        session.Add("FunctionName", input.FunctionName);
        session.Add("EntityCollection", list);
        (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
            parsed, templateFilename, templateContent, outputFilename, settings
        );
        //IOHelper.WriteToFile(generatedFilename, generatedContent);
        result.DbContext = generatedContent;


        outputFilename = System.IO.Path.Combine(outputDirectory, entityframeworkCoreNamespace, input.FunctionName, $"{input.FunctionName}ModelCreatingExtensions.cs");
        templateFilename = System.IO.Path.Combine(templateDirectory, "ModelCreatingExtensions.tt");
        templateContent = System.IO.File.ReadAllText(templateFilename);
        generator = new TemplateGenerator();
        parsed = generator.ParseTemplate(templateFilename, templateContent);
        settings = TemplatingEngine.GetSettings(generator, parsed);
        settings.CompilerOptions = "-nullable:enable";
        session = generator.GetOrCreateSession();
        session.Add("Namespace", entityframeworkCoreNamespace);
        session.Add("CompanyName", input.CompanyName);
        session.Add("ModuleName", input.ModuleName);
        session.Add("FunctionName", input.FunctionName);
        session.Add("EntityCollection", list);
        (generatedFilename, generatedContent) = await generator.ProcessTemplateAsync(
            parsed, templateFilename, templateContent, outputFilename, settings
        );
        // IOHelper.WriteToFile(generatedFilename, generatedContent);
        result.ModelCreatingExtensions = generatedContent;

        Logger.LogWarning($"生成代码完成");
        return result;
    }

    public async Task<string> GenerateVbaScriptAsync(string typeName)
    {
        var persistenceObjectInfo = Configs.DataSource.Find(x => x.TypeName == typeName);

        VbaScriptBuilder builder = new VbaScriptBuilder()
            .ConfigureTableFilter(persistenceObjectInfo.TableName, persistenceObjectInfo.Comment)
            .AddStandardField(new VbaScriptBuilder.StandardField()
            {
                Code = persistenceObjectInfo.Name + "ID",
                Comment = persistenceObjectInfo.Comment + "ID",
                DataType = "uniqueidentifier",
                DefaultValue = string.Empty,
                IsPrimaryKey = true,
                Mandatory = false,
                Name = persistenceObjectInfo.Comment + "ID"
            });

        foreach (var property in persistenceObjectInfo.Properties)
        {
            builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
            {
                Code = property.Name,
                Name = property.Comment,
                Comment = property.Comment,
                DataType = property.DataType,
                DefaultValue = property.DefaultValue,
                IsPrimaryKey = false,
                Mandatory = property.Mandatory
            });
        }
        builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
        {
            Code = "CreateTime",
            Comment = "创建时间",
            DataType = "datetime2",
            DefaultValue = string.Empty,
            IsPrimaryKey = false,
            Mandatory = true,
            Name = "创建时间"
        });
        builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
        {
            Code = "CreateUserId",
            Comment = "创建人ID",
            DataType = "uniqueidentifier",
            DefaultValue = string.Empty,
            IsPrimaryKey = false,
            Mandatory = true,
            Name = "创建人ID"
        });
        builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
        {
            Code = "LastUpdateTime",
            Comment = "最后修改时间",
            DataType = "datetime2",
            DefaultValue = string.Empty,
            IsPrimaryKey = false,
            Mandatory = false,
            Name = "最后修改时间"
        });
        builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
        {
            Code = "LastUpdateUserId",
            Comment = "最后修改人ID",
            DataType = "uniqueidentifier",
            DefaultValue = string.Empty,
            IsPrimaryKey = false,
            Mandatory = false,
            Name = "最后修改人ID"
        });

        builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
        {
            Code = "IsDeleted",
            Comment = "是否删除",
            DataType = "bit",
            DefaultValue = "'0",
            IsPrimaryKey = false,
            Mandatory = true,
            Name = "是否删除"
        });

        builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
        {
            Code = "DeletedTime",
            Comment = "删除时间",
            DataType = "datetime2",
            DefaultValue = string.Empty,
            IsPrimaryKey = false,
            Mandatory = false,
            Name = "删除时间"
        });
        builder = builder.AddStandardField(new VbaScriptBuilder.StandardField()
        {
            Code = "DeletedUserId",
            Comment = "删除人员ID",
            DataType = "uniqueidentifier",
            DefaultValue = string.Empty,
            IsPrimaryKey = false,
            Mandatory = false,
            Name = "删除人员ID"
        });
        await Task.Delay(10);
        return builder.Build();
    }

    public async Task<bool> SaveAsync()
    {

        return true;
    }

    public async Task<object> GetAllAsync()
    {
        return null;
    }
}
