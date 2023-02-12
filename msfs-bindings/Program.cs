

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Xml.Serialization;
using HtmlAgilityPack;
using System;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.Logging;
using static System.Collections.Specialized.BitVector32;


namespace msfs_bindings
{

    public class EventIdInformation
    {
        public string EventCategory { get; set; } = null!;
        public string EventID { get; set; } = null!;
        public string SimConnectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Multiplayer { get; set; } = null!;
    }

    internal class Program
    {
        public static string GetExePath()
        {
            var strExeFilePath = Assembly.GetEntryAssembly()!.Location;
            return Path.GetDirectoryName(strExeFilePath)!;
        }

        private static IConfigurationBuilder Configure(IConfigurationBuilder config, Microsoft.Extensions.Hosting.IHostEnvironment env)
        {
            return Configure(config, env.EnvironmentName);
        }

        private static IConfigurationBuilder Configure(IConfigurationBuilder config, string environmentName)
        {
            return config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }


        public static IConfiguration CreateConfiguration()
        {
            var env = new HostingEnvironment
            {
                EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
                ContentRootFileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory)
            };

            var config = new ConfigurationBuilder();
            var configured = Configure(config, env);
            return configured.Build();
        }
        static async Task Main()
        {
            var exePath = GetExePath();

            var config = CreateConfiguration();

            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            var client = httpClientFactory!.CreateClient();

            var data = await client.GetStringAsync("https://www.prepar3d.com/SDKv3/LearningCenter/utilities/variables/event_ids.html");

            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var eventIdInformation = doc.DocumentNode.SelectNodes("//h4")
                .Select(x =>
                {
                    var tbl = x.NextSibling;

                    while (!tbl.HasClass("ListTable"))
                    {
                        tbl = tbl.NextSibling;
                    }

                    var rows = tbl.SelectNodes("tbody//tr")
                        .Select(y =>
                        {
                            var columns = y.SelectNodes("td");

                            if (columns?.Count == 4)
                            {
                                return new EventIdInformation
                                { 
                                    EventCategory = x.InnerText,
                                    EventID = columns[0].InnerText.Replace("\r\n", " ").Replace("&nbsp;", " ").Trim(),
                                    SimConnectName =
                                        columns[1].InnerText.Replace("\r\n", " ").Replace("&nbsp;", " ").Trim(),
                                    Description = columns[2].InnerText.Replace("\r\n", " ").Replace("&nbsp;", " ").Trim(),
                                    Multiplayer = columns[3].InnerText.Replace("\r\n", " ").Replace("&nbsp;", " ").Trim()
                                };
                            }
                            else
                            {
                                return new EventIdInformation
                                {
                                    EventCategory = "",
                                    EventID = "",
                                    SimConnectName ="",
                                    Description ="",
                                    Multiplayer = ""
                                };
                            }
                        });

                    return new
                    {
                        Events = rows
                    };
                })
                .SelectMany(x => x.Events)
                .Where(x => !string.IsNullOrEmpty(x.SimConnectName))
                .GroupBy(p => p.EventID)
                .ToDictionary(x => x.Key, x => x.First());
            
            var remotePath = config["remotePath"]?.ToString() ?? "";

            var d = new DirectoryInfo(remotePath); 

            var files = d.GetFiles("inputprofile_*");

            foreach (var file in files)
            {
                var xmlFile = await File.ReadAllTextAsync(file.FullName);

                xmlFile = xmlFile.Replace(@"<?xml version=""1.0"" encoding=""UTF-8""?>", @"<?xml version=""1.0"" encoding=""UTF-8""?><InputProfile>")+"</InputProfile>";

                var serializer = new XmlSerializer(typeof(InputProfile));

                using TextReader reader = new StringReader(xmlFile);
                var profile = (InputProfile)serializer.Deserialize(reader)!;


                await using StreamWriter outputFile = new(Path.Combine(exePath, $"profile {profile.FriendlyName.Value}.csv"));

                await outputFile.WriteLineAsync("sep=\t");

                var headerLine = "";
                if (profile.Device.Axes?.Any() == true)
                {
                    headerLine = "ContextName" + "\t" + "ActionName" + "\t" + "Primary" + "\t" + "Secondary";
                }
                else
                {
                    headerLine = "Multiplayer" + "\t" + "EventCategory" + "\t" + "ContextName" + "\t" + "ActionName" + "\t" + "Primary" + "\t" + "Secondary" + "\t" + "Description";

                }
                await outputFile.WriteLineAsync(headerLine);

                foreach (var context in profile.Device.Context)
                {
                    foreach (var action in context.Actions)
                    {
                        var primaryKey = action.Primary.Aggregate("",
                            (current, key) => current + (key.Information + " "));

                        var secondaryKey = "";
                        primaryKey = action.Secondary.Aggregate(primaryKey,
                            (current, key) => current + (key.Information + " "));

                        // joystick
                        if (profile.Device.Axes?.Any() == true)
                        {
                            await outputFile.WriteLineAsync($"{context.ContextName}\t{action.ActionName}\t{primaryKey.Trim()} {secondaryKey.Trim()}");
                        }
                        else
                        {
                            var found = eventIdInformation.TryGetValue(action.ActionName, out var info);

                            await outputFile.WriteLineAsync($"{info?.Multiplayer}\t{info?.EventCategory}\t{context.ContextName}\t{action.ActionName}\t{primaryKey.Trim()} {secondaryKey.Trim()}\t{info?.Description}");
                        }
                    }
                }
            }

        }
    }
}