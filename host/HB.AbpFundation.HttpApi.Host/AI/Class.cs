using Microsoft.Extensions.AI;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.ClientModel;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace HB.AbpFundation.AI
{
    public class AITest
    {
       static List<ChatMessage> defaultMessages = new List<ChatMessage>();

       public static async Task Test(string task)
        {
            string system_promopt = @"
You are Cline, a highly skilled software engineer with extensive knowledge in many programming languages, frameworks, design patterns, and best practices.
=======
工作目录:D:\WORKS\test
如果需要可以调用工具：
WriteFile：写入文件，参数：文件名，内容
ReadFile：读取文件，参数：文件名
EditFile：编辑文件，参数：文件名，修改内容:[{LineNumber:修改那一行,NewContent:修改的行内容}]
DeleteFile：删除文件，参数：文件名
一步一步地完成任务，任务完成后请回复：Termination
###后端技术栈
1、使用c#编程语言，xml注释
2、使用aspnet core框架，dotnet 使用8.0版本
3、使用Volo.Abp框架，abp使用8.0版本
4、使用EntityFrameworkCore 8.0版本
5、数据库使用MySQL 8.0版本
6、代码层实现读写分离
7、数据库实现读写分离，接口传入参数ReadOnly，默认为True,为True时，从读库查询，为False时，从主库查询
###前端技术栈
1、使用typescript编程语言
2、使用vue 3.0框架
3、vite构建工具
4、使用antd vue组件库
5、使用axios库调用接口
6、使用pinia状态管理库
";
            defaultMessages.Add(new ChatMessage(ChatRole.System, system_promopt));
            IChatClient openaiClient =
    new OpenAI.Chat.ChatClient("deepseek-chat", new ApiKeyCredential("sk-e7a0e0fbeaa54443aeb12a9bee6e2321"), new OpenAI.OpenAIClientOptions()
    {
        Endpoint = new Uri("https://api.deepseek.com/v1"),

    })
    .AsIChatClient();
            var sourceName = Guid.NewGuid().ToString();
            // Configure caching
            IDistributedCache cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));

            // Configure tool calling
            var chatOptions = new ChatOptions
            {   
                Tools = [AIFunctionFactory.Create(ReadFile),
                AIFunctionFactory.Create(WriteFile),
                AIFunctionFactory.Create(EditFile),
                AIFunctionFactory.Create(DeleteFile),
                AIFunctionFactory.Create(GetWeather)],
                Temperature = 0,
                  ToolMode= ChatToolMode.Auto,
                  // ResponseFormat=ChatResponseFormat.Json,
                    AllowMultipleToolCalls=true,
                    
            };

            IChatClient client = new ChatClientBuilder(openaiClient)
                .UseDistributedCache(cache)
                .UseFunctionInvocation()
                .UseOpenTelemetry(sourceName: sourceName, configure: c => c.EnableSensitiveData = true)
                .Build();

            List<ChatMessage> historyMessage = new List<ChatMessage>();
            historyMessage.Add(new ChatMessage(ChatRole.User, task));
            int N = 0;
            while (true)
            {
               
               
                string assistantContent = string.Empty;
                await foreach (var message in client.GetStreamingResponseAsync(historyMessage, chatOptions))
                {
                    Console.Write(message.Text);
                    assistantContent += message.Text;
                }

                if (assistantContent.Contains("Termination"))
                {
                    break;
                }
                historyMessage.Add(new ChatMessage(ChatRole.Assistant, assistantContent));
                historyMessage.Add(new ChatMessage(ChatRole.User, "接下来继续完成任务，如果已完成，就输出Termination"));
                N++;
                if (N > 20)
                {
                    break;
                }
            }

            Console.WriteLine("任务完成");
           
        }
        [Description("Gets the weather")]
        static string GetWeather()
        {
            return "The weather is sunny";
        }
        [Description("读取文件，返回文件内容")]
        static string ReadFile([Description("文件路径")] string path)
        {
            if (!Path.Exists(path))
            {
                return $"文件{path}不存在";
            }
            return File.ReadAllText(path, Encoding.UTF8);
        }
        [Description("写入文件内容，如果成功返回true，否则返回false")]
        static bool WriteFile([Description("文件路径")] string path, [Description("文件内容")] string content)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                File.WriteAllText(path, content, Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [Description("编辑文件，如果成功返回true，否则返回false")]
        static bool EditFile([Description("文件路径")] string path, [Description("修改文件的内容")] List<EditFileItem> items)
        {
            try
            {
                // 读取文件所有行
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);

                // 对每个编辑项进行处理
                foreach (var item in items)
                {
                    // 确保行号在有效范围内
                    if (item.LineNumber >= 0 && item.LineNumber < lines.Length)
                    {
                        // 替换指定行的内容
                        lines[item.LineNumber] = item.NewContent;
                    }
                    else
                    {
                        // 行号无效，返回false
                        return false;
                    }
                }

                // 将修改后的内容写回文件
                File.WriteAllLines(path, lines, Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [Description("删除文件，如果成功返回true，否则返回false")]
        static bool DeleteFile([Description("文件路径")] string path)
        {
            File.Delete(path);
            return true;
        }
    }

    public class EditFileItem
    {
        /// <summary>
        /// 要修改的行号（0-based）
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// 新的行内容
        /// </summary>
        public string NewContent { get; set; }
    }
}
