//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DCT.Monitor.Server.App_GlobalResources {
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "10.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Error {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Error() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Error", global::System.Reflection.Assembly.Load("App_GlobalResources"));
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
        ///   Looks up a localized string similar to Some errors occured while execute your request. Try after some time or conact as!.
        /// </summary>
        internal static string Error_General {
            get {
                return ResourceManager.GetString("Error_General", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File is to big. Size of file must be no more than {0:d} Kb.
        /// </summary>
        internal static string Files_FileToBig_iSize {
            get {
                return ResourceManager.GetString("Files_FileToBig_iSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No file is send! Try again!.
        /// </summary>
        internal static string Files_FileTransferError {
            get {
                return ResourceManager.GetString("Files_FileTransferError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File name isn&apos;t unique. Change name of this file or remove uploaded other.
        /// </summary>
        internal static string Files_NameIsNotUnique {
            get {
                return ResourceManager.GetString("Files_NameIsNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File content type is not supported. Convert it to {0}.
        /// </summary>
        internal static string Files_UnsupportedContent_sTypes {
            get {
                return ResourceManager.GetString("Files_UnsupportedContent_sTypes", resourceCulture);
            }
        }
    }
}