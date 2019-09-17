using BrowserHistoryGatherer.Gathering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BrowserHistoryGatherer {
    class Program {
        static void Main(string[] args) {
            var argumentSetting = GetArgumentSetting(args);
            if(argumentSetting == null) {
                return;
            }

            var docJObject = new JObject();
            
            foreach (var browserType in argumentSetting.Value.BrowserTypes) {
                Console.WriteLine($"Handling browser {browserType}");
                var browserJObject = new JObject();
                var commandHandlerArgs = new CommandHandlerArgs {
                    BrowserType = browserType,
                    Commands = argumentSetting.Value.Commands
                };

                foreach (var commandHandler in CommandHandlers.AllCommandHandlers) {
                    commandHandler.Handle(browserJObject, commandHandlerArgs);
                }

                docJObject[browserType] = browserJObject;
                Console.WriteLine($"Handling browser {browserType} done");
            }

            Console.WriteLine($"Saving results to {argumentSetting.Value.OutputFilePath}");

            try {
                using (var textWriter = new StreamWriter(argumentSetting.Value.OutputFilePath)) {
                    var jsonWriter = new JsonTextWriter(textWriter);
                    jsonWriter.Formatting = Formatting.Indented;

                    docJObject.WriteTo(jsonWriter);
                }
            }
            catch(Exception ex) {
                Console.Error.WriteLine($"Error occured saving result:{ex.Message}");
                return;
            }
            
            Console.WriteLine($"Saving results to {argumentSetting.Value.OutputFilePath} done");
        }

        private static ConsoleArgs? GetArgumentSetting(string[] args) {
            Console.WriteLine("Parsing arguments...");
            if (args.Length == 0) {
                Console.Error.WriteLine("Arguments is empty.");
                return null;
            }

            var argumentFilePath = args[0];
            if (!File.Exists(argumentFilePath)) {
                Console.Error.WriteLine($"The specified file {args[0]} is not found.");
                return null;
            }

            JObject docJObject = null;
            using(var textReader = new StreamReader(argumentFilePath)) {
                var jsonReader = new JsonTextReader(textReader);
                docJObject = JObject.Load(jsonReader);
            }
            
            if(docJObject == null) {
                Console.Error.WriteLine($"{nameof(docJObject)} is null.");
                return null;
            }

            var browserTypeJArray = docJObject.GetValue(Constants.JsonElemName_BrowserTypes) as JArray;
            var commandJArray = docJObject.GetValue(Constants.JsonElemName_Commands) as JArray;
            var outputFileJProp = docJObject.GetValue(Constants.JsonElemName_OutputFile) as JValue;
            
            if (browserTypeJArray == null) {
                Console.Error.WriteLine($"{nameof(browserTypeJArray)} is null.");
                return null;
            }
            
            if(commandJArray == null) {
                Console.Error.WriteLine($"{nameof(commandJArray)} is null.");
                return null;
            }

            if (outputFileJProp == null) {
                Console.Error.WriteLine($"{nameof(outputFileJProp)} is null.");
                return null;
            }

            var browserTypes = browserTypeJArray.Select(p => p.ToString()).ToArray();
            var commands = commandJArray.Select(p => p.ToString()).ToArray();
            var outputFilePath = outputFileJProp.Value.ToString();


            Console.WriteLine("Parsing arguments done");

            return new ConsoleArgs {
                BrowserTypes = browserTypes,
                Commands = commands,
                OutputFilePath = outputFilePath
            };
        }
    }
}
