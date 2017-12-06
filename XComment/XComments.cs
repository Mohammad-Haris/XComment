using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;

namespace XComment
{
    internal sealed class XComments
    {
        public const int CommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("ab98f071-6193-40f8-ad98-87a00104190d");

        private readonly Package _package;

        private readonly DTE2 _dte2;

        public static XComments Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider => _package;

        private XComments(Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                CommandID commandId = new CommandID(new Guid(XCommentsPackage.guidXCommentsPackageCmdSet), (int)XCommentsPackage.cmdidMyCommand);
                OleMenuCommand command = new OleMenuCommand(Callback, commandId);
                commandService.AddCommand(command);
            }
            _dte2 = (DTE2)ServiceProvider.GetService(typeof(DTE));
        }

        public static void Initialize(Package package)
        {
            Instance = new XComments(package);
        }

        private void Callback(object sender, EventArgs args)
        {
            ExecuteCommand((OleMenuCommand)sender);
        }

        private void ExecuteCommand(OleMenuCommand button)
        {
            DTE dTE = Package.GetGlobalService(typeof(DTE)) as DTE;

            TextDocument activeDoc = dTE.ActiveDocument.Object() as TextDocument;

            Methods.Apply(dTE, activeDoc);
        }
    }
}