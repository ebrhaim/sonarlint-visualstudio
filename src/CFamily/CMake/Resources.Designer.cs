﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SonarLint.VisualStudio.CFamily.CMake {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SonarLint.VisualStudio.CFamily.CMake.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Could not parse CMakeSettings.json: {0}.
        /// </summary>
        internal static string BadCMakeSettings {
            get {
                return ResourceManager.GetString("BadCMakeSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Could not parse compile_commands.json: {0}.
        /// </summary>
        internal static string BadCompilationDatabaseFile {
            get {
                return ResourceManager.GetString("BadCompilationDatabaseFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Compilation database file contains no entries. File: {0}.
        /// </summary>
        internal static string EmptyCompilationDatabaseFile {
            get {
                return ResourceManager.GetString("EmptyCompilationDatabaseFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Specified build configuration &apos;{0}&apos; could not be found in CMakeSettings.json.
        /// </summary>
        internal static string NoBuildConfigInCMakeSettings {
            get {
                return ResourceManager.GetString("NoBuildConfigInCMakeSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Specified build configuration &apos;{0}&apos; does not contain parameter &apos;buildRoot&apos;.
        /// </summary>
        internal static string NoBuildRootInCMakeSettings {
            get {
                return ResourceManager.GetString("NoBuildRootInCMakeSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Could not find file in the compilation database. File: &apos;{0}&apos;.
        /// </summary>
        internal static string NoCompilationDatabaseEntry {
            get {
                return ResourceManager.GetString("NoCompilationDatabaseEntry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Could not detect matching header file configuration in the compilation database. Header file: &apos;{0}&apos;.
        /// </summary>
        internal static string NoCompilationDatabaseEntryForHeaderFile {
            get {
                return ResourceManager.GetString("NoCompilationDatabaseEntryForHeaderFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Could not locate compilation database at &apos;{0}&apos;. Make sure that your project is configured correctly. 
        ///    See https://github.com/SonarSource/sonarlint-visualstudio/wiki for more information..
        /// </summary>
        internal static string NoCompilationDatabaseFile {
            get {
                return ResourceManager.GetString("NoCompilationDatabaseFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Error fetching VsDevCmd settings: {0}.
        /// </summary>
        internal static string VsDevCmd_ErrorFetchingSettings {
            get {
                return ResourceManager.GetString("VsDevCmd_ErrorFetchingSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] VsDevCmd.bat file could not be found. File path: {0}.
        /// </summary>
        internal static string VsDevCmd_FileNotFound {
            get {
                return ResourceManager.GetString("VsDevCmd_FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Failed to fetch VsDevCmd settings: no settings found..
        /// </summary>
        internal static string VsDevCmd_NoSettingsFound {
            get {
                return ResourceManager.GetString("VsDevCmd_NoSettingsFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [CMake] Failed to fetch VsDevCmd settings: timed out..
        /// </summary>
        internal static string VsDevCmd_TimedOut {
            get {
                return ResourceManager.GetString("VsDevCmd_TimedOut", resourceCulture);
            }
        }
    }
}
