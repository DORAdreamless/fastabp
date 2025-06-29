using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.AI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace HB.AbpFundation.Controllers;

public class HomeController : AbpController
{
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }

    [HttpPost]
    [Route("api/ai/test")]
    public async Task HealthCheck()
    {
        string promopt = "";
        StringBuilder sb=new StringBuilder();
        sb.AppendLine("根据下面的实体信息开发功能,要求完成实现");
        string entity_comment = string.Empty;
       var xml1= DocsByReflection.DocsService.GetXmlFromType(typeof(Menu), false);
        if (xml1 != null)
        {
            entity_comment = xml1.SelectSingleNode("summary")?.InnerText;
        }
        if (!string.IsNullOrWhiteSpace(entity_comment))
        {
            entity_comment = entity_comment.Replace("\r", "").Replace("\n", "").Replace(" ", "");
        }
        sb.AppendLine($"实体:{typeof(Menu).Name}:{entity_comment}");
        foreach (var property in typeof(Menu).GetProperties())
        {
            string property_comment = string.Empty;
           var xml = DocsByReflection.DocsService.GetXmlFromMember(property,false);
           if (xml != null)
            {
                property_comment = xml.SelectSingleNode("summary")?.InnerText;
            }
            if (!string.IsNullOrWhiteSpace(property_comment))
            {
                property_comment = property_comment.Replace("\r", "").Replace("\n", "").Replace(" ","");
            }
            sb.AppendLine($"-{property.Name},{property.PropertyType.Name}:{property_comment}");
        }
        promopt = sb.ToString();
        Console.WriteLine(promopt);
        await AITest.Test(promopt);
    }
}
