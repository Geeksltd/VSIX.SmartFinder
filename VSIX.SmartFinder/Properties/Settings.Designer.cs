﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Geeks.VSIX.SmartFinder.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExcludeResources {
            get {
                return ((bool)(this["ExcludeResources"]));
            }
            set {
                this["ExcludeResources"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TrackItemInSolutionExplorer {
            get {
                return ((bool)(this["TrackItemInSolutionExplorer"]));
            }
            set {
                this["TrackItemInSolutionExplorer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".gif;.jpg;.png;.bmp;.resx;.ico;.bat;.tif;.psd;.sln;.suo;.csproj;.user;.cd;.lic;_r" +
            "eset.css;.html;.jsm;.map;.cache;vsdoc;.dat;.force;.map;.md;.rtf;.tmp")]
        public string ResourceFileTypes {
            get {
                return ((string)(this["ResourceFileTypes"]));
            }
            set {
                this["ResourceFileTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("@Temp\\Validation;@CuteSoft;@Coder.Meta;.svn;node_modules;bower_components;public;" +
            "obj\r\n")]
        public string ExcludedDirectories {
            get {
                return ((string)(this["ExcludedDirectories"]));
            }
            set {
                this["ExcludedDirectories"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowMethodParameters {
            get {
                return ((bool)(this["ShowMethodParameters"]));
            }
            set {
                this["ShowMethodParameters"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShowMethods {
            get {
                return ((bool)(this["ShowMethods"]));
            }
            set {
                this["ShowMethods"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowMethodReturnTypes {
            get {
                return ((bool)(this["ShowMethodReturnTypes"]));
            }
            set {
                this["ShowMethodReturnTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowClassNames {
            get {
                return ((bool)(this["ShowClassNames"]));
            }
            set {
                this["ShowClassNames"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShowProperties {
            get {
                return ((bool)(this["ShowProperties"]));
            }
            set {
                this["ShowProperties"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RemoteMachines {
            get {
                return ((string)(this["RemoteMachines"]));
            }
            set {
                this["RemoteMachines"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CleanupChoices {
            get {
                return ((string)(this["CleanupChoices"]));
            }
            set {
                this["CleanupChoices"] = value;
            }
        }
    }
}