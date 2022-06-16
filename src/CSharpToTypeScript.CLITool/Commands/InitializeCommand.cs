using CSharpToTypeScript.CLITool.Utilities;
using CSharpToTypeScript.CLITool.Validation;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.Reflection;

namespace CSharpToTypeScript.CLITool.Commands
{
    [ConfigurationFileDoesNotExist]
    [Command(Name = "init", Description = "Initialize - create configuration file in current directory")]
    public class InitializeCommand : CommandBase
    {
        public void OnExecute() => ConfigurationFile.Create(new Configuration(this));
    }

    [Command(Name = "version", Description = "Display version")]
    public class VersionCommand : CommandBase
    {
        public void OnExecute() {
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            Console.WriteLine(version.ProductVersion);
        }
    }
}
