using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Shell.Interop;

namespace XComment
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]       
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]
    [Guid(XCommentsPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class XCommentsPackage : Package
    {
        public const string PackageGuidString = "b453521b-57b4-457c-beae-fa73ecc0e008";
        public const string guidXCommentsPackageCmdSet = "ab98f071-6193-40f8-ad98-87a00104190d";           
        public const uint cmdidMyCommand = 0x104;

        #region Package Members

        protected override void Initialize()
        {
            XComments.Initialize(this);
            base.Initialize();
        }

        #endregion
    }
}