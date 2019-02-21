using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Geeks.GeeksProductivityTools;
using Geeks.VSIX.SmartFinder.Base;
using Geeks.VSIX.SmartFinder.FileFinder;
using Geeks.VSIX.SmartFinder.FileToggle;
using Geeks.VSIX.SmartFinder.GoTo;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Geeks.VSIX.SmartFinder
{
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasSingleProject, PackageAutoLoadFlags.BackgroundLoad)]

    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]    // Microsoft.VisualStudio.VSConstants.UICONTEXT_NoSolution
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(OptionsPage), "Geeks productivity tools", "General", 0, 0, true)]
    [Guid(GuidList.GuidGeeksProductivityToolsPkgString)]
    ////[ProvideService(typeof(SMyService))]
    public sealed class SmartFinderPackage : AsyncPackage
    {
        public SmartFinderPackage() { }

        // Strongly reference events so that it's not GC'd
        EnvDTE.DocumentEvents docEvents;
        EnvDTE.SolutionEvents solEvents;
        EnvDTE.Events events;

        public static SmartFinderPackage Instance { get; private set; }

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            App.Initialize(GetDialogPage(typeof(OptionsPage)) as OptionsPage);

            Instance = this;

            //var componentModel = (IComponentModel)GetService(typeof(SComponentModel));

            // Add our command handlers for menu (commands must exist in the .vsct file)

            if (GetService(typeof(IMenuCommandService)) is OleMenuCommandService menuCommandService)
            {
                // //var mainMenu = new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet, (int)PkgCmdIDList.CmdidMainMenu);
                // //var founded = menuCommandService.FindCommand(mainMenu);
                // //if (founded == null)
                // //{
                // //    var menuCommand2 = new OleMenuCommand(null, mainMenu);
                // //    menuCommandService.AddCommand(menuCommand2);
                // //    menuCommand2.BeforeQueryStatus += MenuCommand2_BeforeQueryStatus;
                // //    menuCommand2.Visible = false;
                // //}
                var menuCommand = new OleMenuCommand(CallWebFileToggle, new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet, (int)PkgCmdIDList.CmdidWebFileToggle));
                menuCommand.BeforeQueryStatus += MenuCommand_BeforeQueryStatus;

                menuCommandService.AddCommand(menuCommand);

                menuCommandService.AddCommand(new MenuCommand(CallFixtureFileToggle,
                                               new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet,
                                                            (int)PkgCmdIDList.CmdidFixtureFileToggle)));

                menuCommandService.AddCommand(new MenuCommand(CallFileFinder,
                                               new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet,
                                                            (int)PkgCmdIDList.CmdidFileFinder)));

                menuCommandService.AddCommand(new MenuCommand(CallMemberFinder,
                                               new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet,
                                                            (int)PkgCmdIDList.CmdidMemberFinder)));

                menuCommandService.AddCommand(new MenuCommand(CallCssFinder,
                                               new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet,
                                                            (int)PkgCmdIDList.CmdidCSSFinder)));

                menuCommandService.AddCommand(new MenuCommand(CallGotoNextFoundItem,
                                               new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet,
                                                            (int)PkgCmdIDList.CmdidGotoNextFoundItem)));

                menuCommandService.AddCommand(new MenuCommand(CallGotoPreviousFoundItem,
                                               new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet,
                                                            (int)PkgCmdIDList.CmdidGotoPreviousFoundItem)));
            }

            SetCommandBindings();

            // Hook up event handlers
            events = App.DTE.Events;
            docEvents = events.DocumentEvents;
            solEvents = events.SolutionEvents;
            solEvents.Opened += delegate { App.Initialize(GetDialogPage(typeof(OptionsPage)) as OptionsPage); };

            // //ServiceCreatorCallback callback = new ServiceCreatorCallback(CreateService);

            // //((IServiceContainer)this).AddService(typeof(SMyService), callback);
        }

        // //private object CreateService(IServiceContainer container, Type serviceType)
        // //{
        // //    if (typeof(SMyService) == serviceType)
        // //        return new MyService(this);
        // //    return null;
        // //}

        void MenuCommand2_BeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (menuCommandService == null)
                return;

            const uint CMDID_ATTACHER = 0x100;

            var mainMenuCommand = menuCommandService.FindCommand(
                new CommandID(GuidList.GuidGeeksProductivityToolsCmdSet, (int)CMDID_ATTACHER));

            if (mainMenuCommand != null) return;

            if (sender is OleMenuCommand cmd)
                cmd.Visible = true;
        }

        public void MenuCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            var cmd = sender as OleMenuCommand;
            // var activeDoc = App.DTE.ActiveDocument;

            // if (null != cmd && activeDoc != null)
            // {
            //    var fileName = App.DTE.ActiveDocument.FullName.ToUpper();
            //    cmd.Visible = true;
            // }
        }

        void SetCommandBindings()
        {
            var singleOrDefault = ((Commands2) App.DTE.Commands).Cast<Command>()
                .SingleOrDefault(cmd => cmd.Name == "File.CloseAllButThis");
            if (singleOrDefault != null) 
                singleOrDefault.Bindings = "Global::CTRL+SHIFT+F4";

            foreach (Command cmd in (Commands2)App.DTE.Commands)
            {
                var systemGadget = All.Gadgets.FirstOrDefault(g => g.CommandName == cmd.Name);
                if (systemGadget != null)
                    cmd.Bindings = systemGadget.Binding;
            }
        }

        void CallWebFileToggle(object sender, EventArgs e) => new FileToggleGadget().Run(App.DTE);

        void CallFixtureFileToggle(object sender, EventArgs e) => new FixtureFileToggleGadget().Run(App.DTE);

        void CallFileFinder(object sender, EventArgs e) => new FileFinderGadget().Run(App.DTE);

        void CallMemberFinder(object sender, EventArgs e) => new MemberFinderGadget().Run(App.DTE);

        void CallCssFinder(object sender, EventArgs e) => new StyleFinderGadget().Run(App.DTE);

        void CallGotoNextFoundItem(object sender, EventArgs e) => new GotoNextFoundItemGadget().Run(App.DTE);

        void CallGotoPreviousFoundItem(object sender, EventArgs e) => new GotoPreviousFoundItemGadget().Run(App.DTE);
    }
}