using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using WixSharp;
using XmlFeedReader.Services;

namespace XmlFeedReader.Setup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string TargetFramework = "net462";
            const string ProgramFilesDir = "%ProgramFiles%";
            const string ProgramMenuDir = "%ProgramMenu%";
            const string InstallDir = "[INSTALLDIR]";

            var assembly = new AssemblyService(typeof(AssemblyService).Assembly);

            var project = new Project(
                name: $"{assembly.AssemblyTitle}",
                new Dir(
                    targetPath: Path.Combine(ProgramFilesDir, assembly.AssemblyCompany, assembly.AssemblyProduct),
                    new Files(
                        sourcePath: Path.Combine("..", assembly.AssemblyTitle, "bin", assembly.AssemblyBuildConfiguration, TargetFramework, "*.*"),
                        filter: f => !f.EndsWith(".obj"))
                    {
                        //AttributesDefinition = "ReadOnly=no" //all files will be marked with this attribute
                    },
                    new ExeFileShortcut(
                        name: $"Uninstall {assembly.AssemblyProduct}", 
                        target: "[System64Folder]msiexec.exe", 
                        arguments: "/x [ProductCode]")
                ),
                new Dir(
                    targetPath: Path.Combine(ProgramMenuDir, assembly.AssemblyCompany, assembly.AssemblyProduct),
                    new ExeFileShortcut(
                        name: $"{assembly.AssemblyProduct}", 
                        target: Path.Combine(InstallDir, $"{assembly.AssemblyTitle}.exe"), 
                        arguments: "") 
                        { 
                            WorkingDirectory = InstallDir
                        },
                    new ExeFileShortcut(
                        name: $"Uninstall {assembly.AssemblyProduct}", 
                        target: "[System64Folder]msiexec.exe", 
                        arguments: "/x [ProductCode]")
                )
            )
            {
                // DO NOT Change, should be 3E6683E6-D672-4D1B-98E3-8C3CD8752D89
                // Same with UpgradeCode
                GUID = new Guid("3E6683E6-D672-4D1B-98E3-8C3CD8752D89"),
                Name = assembly.AssemblyProduct,
                Version = assembly.AssemblyFullVersion,
                MajorUpgradeStrategy = MajorUpgradeStrategy.Default,
                
            };

            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            project.ResolveWildCards(pruneEmptyDirectories: true)
                   .FindFirstFile($"{assembly.AssemblyTitle}.exe")
                   .Shortcuts = new[]
                   {
                       new FileShortcut($"{assembly.AssemblyTitle}.exe", "%Desktop%")
                   };

            Compiler.PreserveTempFiles = true;
            Compiler.EmitRelativePaths = false;

            project.BuildMsi(path: $@".\bin\{assembly.AssemblyBuildConfiguration}\{TargetFramework}\{assembly.AssemblyTitle}-v{assembly.AssemblyVersion}.msi");
        
        }
    }
}
