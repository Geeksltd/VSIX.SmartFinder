using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geeks.SmartFinder.Properties;
using Geeks.VSIX.SmartFinder.Definition;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Geeks.VSIX.SmartFinder.FileFinder
{
    class MemberLoaderAgent : Loader
    {
        static CodeDomProvider CSharpPovider = CodeDomProvider.CreateProvider("c#");

        string[] BasePaths;
        Repository Repository;

        public MemberLoaderAgent(string[] basePaths, Repository repository)
        {
            BasePaths = basePaths;
            Repository = repository;

            WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e) => AddFilesInPaths(e);

        void AddFilesInPaths(DoWorkEventArgs e)
        {
            if (e.Cancel) return;

            foreach (var basePath in BasePaths)
                AddFilesInPath(e, projectBasePath: basePath, directory: basePath);
        }

        void AddFilesInPath(DoWorkEventArgs e, string projectBasePath, string directory)
        {
            if (!Directory.Exists(directory) || DirectoryExcluded(directory)) return;

            AddCsFiles(e, projectBasePath, directory);

            if (e.Cancel) return;

            AddSubdirectories(e, projectBasePath, directory);
        }

        void AddSubdirectories(DoWorkEventArgs e, string projectBasePath, string directory)
        {
            foreach (var d in Directory.GetDirectories(directory))
            {
                if (e.Cancel) return;

                AddFilesInPath(e, projectBasePath, d);
            }
        }

        void AddCsFiles(DoWorkEventArgs e, string projectBasePath, string directory)
        {
            var csFiles = Directory.GetFiles(directory, "*.cs");
            foreach (var csFile in csFiles)
            {
                if (e.Cancel) return;

                var porcupineTree = CSharpSyntaxTree.ParseText(File.ReadAllText(csFile));
                var root = porcupineTree.GetRoot();


                if (Settings.Default.ShowMethods)
                {
                    var methodsInsideCsFile = ExtractMethods(projectBasePath, csFile, root.DescendantNodes().OfType<MethodDeclarationSyntax>());
                    Repository.AppendRange(methodsInsideCsFile);

                }

                if (Settings.Default.ShowProperties)
                {
                    var propertiesInsideCsFile = ExtractProperties(projectBasePath, csFile, root.DescendantNodes().OfType<PropertyDeclarationSyntax>());
                    Repository.AppendRange(propertiesInsideCsFile);
                }

            }
        }

        IEnumerable<Item> ExtractMethods(string projectBasePath, string fileName, IEnumerable<MethodDeclarationSyntax> nodes)
        {
            foreach (var method in nodes)
            {
                var description = new StringBuilder();
                if (Settings.Default.ShowClassNames)
                {
                    switch (method.Parent.Kind())
                    {
                        case SyntaxKind.ClassDeclaration:
                            description.AppendFormat("{0}.", (method.Parent as ClassDeclarationSyntax).Identifier);
                            break;
                        case SyntaxKind.InterfaceDeclaration:
                            description.AppendFormat("{0}.", (method.Parent as InterfaceDeclarationSyntax).Identifier);
                            break;
                        case SyntaxKind.StructDeclaration:
                            description.AppendFormat("{0}.", (method.Parent as StructDeclarationSyntax).Identifier);
                            break;
                        default:
                            throw new Exception("Unsupported Decleration");
                    }

                }

                description.Append(method.Identifier);
                description.Append("(");
                if (Settings.Default.ShowMethodParameters)
                {
                    description.Append(method.ParameterList.Parameters.Select(p => "{0} {1}".FormatWith(SimplifizePrimitiveTypes(p.Type.ToString()), p.Identifier.ToString())).ToString(", "));
                }

                description.Append(")");
                if (Settings.Default.ShowMethodReturnTypes)
                {
                    description.AppendFormat(" :{0}", SimplifizePrimitiveTypes(method.ReturnType.ToString()));
                }

                yield return new Item(projectBasePath, fileName)
                {
                    Phrase = description.ToString(),
                    Column = method.SyntaxTree.GetLineSpan(method.Span).StartLinePosition.Character + 1,
                    Icon = IconType.Method,
                    MemberType = MemberType.Method,
                    LineNumber = method.SyntaxTree.GetLineSpan(method.Span).StartLinePosition.Line + 1
                };
            }
        }

        string SimplifizePrimitiveTypes(string tr)
        {
            return tr
                     .Remove("System.")
                     .Replace("Int32", "int")
                     .Replace("Int64", "long")
                     .Replace("Decimal", "decimal")
                     .Replace("Double", "double")
                     .Replace("Float", "float")
                     .Replace("Void", "void")
                     .Replace("Char", "char")
                     .Replace("Byte", "byte")
                     .Replace("SByte", "sbyte")
                     .Replace("String", "string");
        }

        IEnumerable<Item> ExtractProperties(string projectBasePath, string fileName, IEnumerable<PropertyDeclarationSyntax> nodes)
        {
            foreach (var property in nodes)
            {
                SyntaxToken parentName;

                switch (property.Parent.Kind())
                {
                    case SyntaxKind.ClassDeclaration:
                        parentName = (property.Parent as ClassDeclarationSyntax).Identifier;
                        break;
                    case SyntaxKind.InterfaceDeclaration:
                        parentName = (property.Parent as InterfaceDeclarationSyntax).Identifier;
                        break;
                    case SyntaxKind.StructDeclaration:
                        parentName = (property.Parent as StructDeclarationSyntax).Identifier;
                        break;
                    default:
                        throw new Exception("Unsupported Decleration");
                }

                var description = "{0}{1} :{2}".FormatWith(
                    Settings.Default.ShowClassNames ? parentName + "." : "",
                    property.Identifier,
                    SimplifizePrimitiveTypes(property.Type.ToString()));
                yield return new Item(projectBasePath, fileName)
                {
                    Phrase = description,
                    Column = property.SyntaxTree.GetLineSpan(property.Span).StartLinePosition.Character + 1,
                    Icon = IconType.Property,
                    MemberType = MemberType.Property,
                    LineNumber = property.SyntaxTree.GetLineSpan(property.Span).StartLinePosition.Line + 1
                };
            }
        }

        bool DirectoryExcluded(string basePath)
        {
            var dirs = Settings.Default.ExcludedDirectories.Split(';');
            foreach (var dir in dirs)
                if (basePath.Contains(dir.Trim())) return true;
            return false;
        }

        ToolStripMenuItem trackFoundIteToolStripMenuItem = new ToolStripMenuItem("Track Found Item");

        ToolStripMenuItem showMethodsToolStripMenuItem = new ToolStripMenuItem("Show Methods");
        ToolStripMenuItem showMethodParametersToolStripMenuItem = new ToolStripMenuItem("Show Method Parameters");
        ToolStripMenuItem showMethodReturnTypesToolStripMenuItem = new ToolStripMenuItem("Show Method Retrun Type");
        ToolStripMenuItem showClassNamesToolStripMenuItem = new ToolStripMenuItem("Show Class Names");
        ToolStripMenuItem showPropertiesToolStripMenuItem = new ToolStripMenuItem("Show Properties");

        public override void LoadOptions()
        {
            trackFoundIteToolStripMenuItem.Checked = Settings.Default.TrackItemInSolutionExplorer;

            showMethodsToolStripMenuItem.Checked = Settings.Default.ShowMethods;

            showMethodParametersToolStripMenuItem.Enabled = showMethodsToolStripMenuItem.Checked;
            showMethodReturnTypesToolStripMenuItem.Enabled = showMethodsToolStripMenuItem.Checked;
            showClassNamesToolStripMenuItem.Enabled = showMethodsToolStripMenuItem.Checked;

            showMethodParametersToolStripMenuItem.Checked = Settings.Default.ShowMethods && Settings.Default.ShowMethodParameters;
            showMethodReturnTypesToolStripMenuItem.Checked = Settings.Default.ShowMethods && Settings.Default.ShowMethodReturnTypes;
            showClassNamesToolStripMenuItem.Checked = Settings.Default.ShowMethods && Settings.Default.ShowClassNames;

            showPropertiesToolStripMenuItem.Checked = Settings.Default.ShowProperties;
        }

        public override void DisplayOptions(ContextMenuStrip mnuOptions)
        {
            mnuOptions.Items.Add(trackFoundIteToolStripMenuItem);
            mnuOptions.Items.Add(new ToolStripSeparator());
            mnuOptions.Items.Add(showMethodsToolStripMenuItem);
            mnuOptions.Items.Add(showMethodParametersToolStripMenuItem);
            mnuOptions.Items.Add(showMethodReturnTypesToolStripMenuItem);
            mnuOptions.Items.Add(showClassNamesToolStripMenuItem);
            mnuOptions.Items.Add(showPropertiesToolStripMenuItem);
        }

        public override void OptionClicked(ToolStripMenuItem toolStripMenuItem, ref bool searchAgain, ref bool loadAgain)
        {
            if (toolStripMenuItem == trackFoundIteToolStripMenuItem)
            {
                trackFoundIteToolStripMenuItem.Checked = !trackFoundIteToolStripMenuItem.Checked;
                Settings.Default.TrackItemInSolutionExplorer = trackFoundIteToolStripMenuItem.Checked;
                Settings.Default.Save();
            }

            if (toolStripMenuItem == showMethodsToolStripMenuItem)
            {
                showMethodsToolStripMenuItem.Checked = !showMethodsToolStripMenuItem.Checked;
                Settings.Default.ShowMethods = showMethodsToolStripMenuItem.Checked;

                showMethodParametersToolStripMenuItem.Enabled = showMethodsToolStripMenuItem.Checked;
                showMethodReturnTypesToolStripMenuItem.Enabled = showMethodsToolStripMenuItem.Checked;
                showClassNamesToolStripMenuItem.Enabled = showMethodsToolStripMenuItem.Checked;

                showMethodParametersToolStripMenuItem.Checked = showMethodParametersToolStripMenuItem.Enabled;
                showMethodReturnTypesToolStripMenuItem.Checked = showMethodReturnTypesToolStripMenuItem.Enabled;
                showClassNamesToolStripMenuItem.Checked = showClassNamesToolStripMenuItem.Enabled;

                searchAgain = true;
                loadAgain = true;

                Settings.Default.Save();
            }

            if (toolStripMenuItem == showMethodParametersToolStripMenuItem)
            {
                showMethodParametersToolStripMenuItem.Checked = !showMethodParametersToolStripMenuItem.Checked;
                Settings.Default.ShowMethodParameters = showMethodParametersToolStripMenuItem.Checked;
                searchAgain = true;
                loadAgain = true;
                Settings.Default.Save();
            }

            if (toolStripMenuItem == showMethodReturnTypesToolStripMenuItem)
            {
                showMethodReturnTypesToolStripMenuItem.Checked = !showMethodReturnTypesToolStripMenuItem.Checked;
                Settings.Default.ShowMethodReturnTypes = showMethodReturnTypesToolStripMenuItem.Checked;
                searchAgain = true;
                loadAgain = true;
                Settings.Default.Save();
            }

            if (toolStripMenuItem == showClassNamesToolStripMenuItem)
            {
                showClassNamesToolStripMenuItem.Checked = !showClassNamesToolStripMenuItem.Checked;
                Settings.Default.ShowClassNames = showClassNamesToolStripMenuItem.Checked;
                searchAgain = true;
                loadAgain = true;
                Settings.Default.Save();
            }

            if (toolStripMenuItem == showPropertiesToolStripMenuItem)
            {
                showPropertiesToolStripMenuItem.Checked = !showPropertiesToolStripMenuItem.Checked;
                Settings.Default.ShowProperties = showPropertiesToolStripMenuItem.Checked;
                searchAgain = true;
                loadAgain = true;
                Settings.Default.Save();
            }
        }
    }
}