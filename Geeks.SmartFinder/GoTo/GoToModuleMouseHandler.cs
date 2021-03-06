using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EnvDTE80;
using Geeks.GeeksProductivityTools;
using Geeks.VSIX.SmartFinder.Base;
using GeeksAddin;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace Geeks.VSIX.SmartFinder.GoTo
{
    [Export(typeof(IKeyProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType("projection")]
    [Name("GotoModule")]
    [Order(Before = "VisualStudioKeyboardProcessor")]
    internal sealed class GoToModuleKeyProcessorProvider : IKeyProcessorProvider
    {
        public KeyProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(typeof(GoToModuleKeyProcessor),
                                                                () => new GoToModuleKeyProcessor(CtrlKeyState.GetStateForView(view)));
        }
    }

    /// <summary>
    /// The state of the control key for a given view, which is kept up-to-date by a combination of the
    /// key processor and the mouse process
    /// </summary>
    internal sealed class CtrlKeyState
    {
        internal static CtrlKeyState GetStateForView(ITextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(typeof(CtrlKeyState), () => new CtrlKeyState());
        }

        bool _enabled;

        internal bool Enabled
        {
            get
            {
                // Check and see if ctrl is down but we missed it somehow.
                var ctrlDown = (Keyboard.Modifiers & ModifierKeys.Control) != 0 &&
                                (Keyboard.Modifiers & ModifierKeys.Shift) == 0;
                if (ctrlDown != _enabled) Enabled = ctrlDown;

                return _enabled;
            }
            set
            {
                var oldVal = _enabled;
                _enabled = value;
                if (oldVal == _enabled) return;

                var temp = CtrlKeyStateChanged;
                temp?.Invoke(this, new EventArgs());
            }
        }

        internal event EventHandler<EventArgs> CtrlKeyStateChanged;
    }

    /// <inheritdoc />
    /// <summary>
    /// Listen for the control key being pressed or released to update the CtrlKeyStateChanged for a view.
    /// </summary>
    internal sealed class GoToModuleKeyProcessor : KeyProcessor
    {
        readonly CtrlKeyState _state;

        public GoToModuleKeyProcessor(CtrlKeyState state)
        {
            _state = state;
        }

        void UpdateState(KeyboardEventArgs args)
        {
            _state.Enabled = (args.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0 &&
                             (args.KeyboardDevice.Modifiers & ModifierKeys.Shift) == 0;
        }

        public override void PreviewKeyDown(KeyEventArgs args) => UpdateState(args);

        public override void PreviewKeyUp(KeyEventArgs args) => UpdateState(args);
    }

    [Export(typeof(IMouseProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType("projection")]
    [Name("GotoModule")]
    [Order(Before = "WordSelection")]
    internal sealed class GoToModuleMouseHandlerProvider : IMouseProcessorProvider
    {
        [Import]
        IClassifierAggregatorService AggregatorFactory = null;

        [Import]
        ITextStructureNavigatorSelectorService NavigatorService = null;

        [Import]
        Microsoft.VisualStudio.Shell.SVsServiceProvider GlobalServiceProvider = null;

        public IMouseProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            if (App.OptionsPage?.DisableShiftClick == null)
                return null;

            //if (!(GlobalServiceProvider.GetService(typeof(SUIHostCommandDispatcher)) is IOleCommandTarget shellCommandDispatcher)) 
            //    return null;

            return new GoToModuleMouseHandler(
                                           (DTE2)GlobalServiceProvider.GetService(typeof(SDTE)),
                                           view,
                                           //shellCommandDispatcher,
                                           AggregatorFactory.GetClassifier(view.TextBuffer),
                                           NavigatorService.GetTextStructureNavigator(view.TextBuffer),
                                           CtrlKeyState.GetStateForView(view));
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// handle mouse moves (when control is pressed) to highlight references for which GoToModule will be valid.
    /// </summary>
    internal sealed class GoToModuleMouseHandler : MouseProcessorBase
    {
        readonly DTE2 App;
        readonly IWpfTextView _view;
        readonly CtrlKeyState _state;
        readonly IClassifier _aggregator;
        readonly ITextStructureNavigator _navigator;

        public GoToModuleMouseHandler(
            DTE2 app,
            IWpfTextView view,
            //IOleCommandTarget commandTarget,
            IClassifier aggregator,
            ITextStructureNavigator navigator,
            CtrlKeyState state)
        {
            App = app;
            _view = view;
            _state = state;
            _aggregator = aggregator;
            _navigator = navigator;

            _state.CtrlKeyStateChanged += (sender, args) =>
            {
                if (_state.Enabled)
                    TryHighlightItemUnderMouse(RelativeToView(Mouse.PrimaryDevice.GetPosition(_view.VisualElement)));
                else
                    SetHighlightSpan(null);
            };

            _view.LostAggregateFocus += (sender, args) => SetHighlightSpan(null);
            _view.VisualElement.MouseLeave += (sender, args) => SetHighlightSpan(null);
        }

        #region Mouse processor overrides

        // Remember the location of the mouse on left button down, so we only handle left button up
        // if the mouse has stayed in a single location.
        Point? _mouseDownAnchorPoint;

        public override void PostprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _mouseDownAnchorPoint = RelativeToView(e.GetPosition(_view.VisualElement));
        }

        public override void PreprocessMouseMove(MouseEventArgs e)
        {
            if (!_mouseDownAnchorPoint.HasValue && _state.Enabled && e.LeftButton == MouseButtonState.Released)
            {
                TryHighlightItemUnderMouse(RelativeToView(e.GetPosition(_view.VisualElement)));
            }
            else if (_mouseDownAnchorPoint.HasValue)
            {
                // Check and see if this is a drag; if so, clear out the highlight.
                var currentMousePosition = RelativeToView(e.GetPosition(_view.VisualElement));
                if (_mouseDownAnchorPoint != null && !InDragOperation(_mouseDownAnchorPoint.Value, currentMousePosition)) return;
                _mouseDownAnchorPoint = null;
                SetHighlightSpan(null);
            }
        }

        static bool InDragOperation(Point anchorPoint, Point currentPoint)
        {
            // If the mouse up is more than a drag away from the mouse down, this is a drag
            return Math.Abs(anchorPoint.X - currentPoint.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                   Math.Abs(anchorPoint.Y - currentPoint.Y) >= SystemParameters.MinimumVerticalDragDistance;
        }

        public override void PreprocessMouseLeave(MouseEventArgs e) => _mouseDownAnchorPoint = null;

        public override void PreprocessMouseUp(MouseButtonEventArgs e)
        {
            if (_mouseDownAnchorPoint.HasValue && _state.Enabled)
            {
                var currentMousePosition = RelativeToView(e.GetPosition(_view.VisualElement));

                if (_mouseDownAnchorPoint != null && !InDragOperation(_mouseDownAnchorPoint.Value, currentMousePosition))
                {
                    _state.Enabled = false;

                    string value = null;
                    if (CurrentUnderlineSpan != null)
                    {
                        value = CurrentUnderlineSpan.Value.GetText().StripQuotation();
                    }

                    SetHighlightSpan(null);
                    _view.Selection.Clear();

                    if (value.HasValue()) DispatchClick(value);

                    e.Handled = true;
                }
            }

            _mouseDownAnchorPoint = null;
        }

        #endregion

        #region Private helpers

        Point RelativeToView(Point position)
        {
            return new Point(position.X + _view.ViewportLeft, position.Y + _view.ViewportTop);
        }

        const string AspDotNetWebFormContentType = "projection";
        const string ZebbleContentType = "xml";
        bool TryHighlightItemUnderMouse(Point position)
        {
            var updated = false;

            try
            {
                var line = _view.TextViewLines.GetTextViewLineContainingYCoordinate(position.Y);

                var bufferPosition = line?.GetBufferPositionFromXCoordinate(position.X);

                if (!bufferPosition.HasValue) return false;

                // Quick check - if the mouse is still inside the current underline span, we're already set
                var currentSpan = CurrentUnderlineSpan;
                if (currentSpan.HasValue && currentSpan.Value.Contains(bufferPosition.Value))
                {
                    updated = true;
                    return true;
                }

                var extent = _navigator.GetExtentOfWord(bufferPosition.Value);
                if (!extent.IsSignificant) return false;

                var contentType = _view.TextBuffer.ContentType;
                if (!contentType.IsOfType(AspDotNetWebFormContentType) && !contentType.IsOfType(ZebbleContentType))
                    return false;

                var lineSpan = bufferPosition.Value.GetContainingLine().Extent;
                var classificationSpans = _aggregator.GetClassificationSpans(lineSpan).Where(FilterBasedOnContentType(contentType.TypeName));
                if ((from classification in classificationSpans 
                    let span = classification.Span 
                    where span.Contains(bufferPosition.Value) 
                    where contentType.TypeName != AspDotNetWebFormContentType || !HasInvalidFileExtension(span) 
                    select classification).Any(classification => SetHighlightSpan(classification.Span)))
                {
                    updated = true;
                    return true;
                }

                return false;
            }

            finally
            {
                if (!updated) SetHighlightSpan(null);
            }
        }

        static bool HasInvalidFileExtension(SnapshotSpan span) => !span.GetText().StripQuotation().ToLower().EndsWithAny(".master", ".ascx");

        static Func<ClassificationSpan, bool> FilterBasedOnContentType(string typeName)
        {
            switch (typeName)
            {
                case AspDotNetWebFormContentType:
                    return s => string.Equals(s.ClassificationType.Classification, "html attribute value",
                        StringComparison.OrdinalIgnoreCase);
                case ZebbleContentType:
                    return s => string.Equals(s.ClassificationType.Classification, "xml name",
                        StringComparison.OrdinalIgnoreCase);
                default:
                    return s => s.ToString() == s.ToString(); // TODO: replace with a correct True value
            }
        }

        SnapshotSpan? CurrentUnderlineSpan
        {
            get
            {
                var classifier = UnderlineClassifierProvider.GetClassifierForView(_view);
                return classifier?.CurrentUnderlineSpan?.TranslateTo(_view.TextSnapshot, SpanTrackingMode.EdgeExclusive);
            }
        }

        bool SetHighlightSpan(SnapshotSpan? span)
        {
            var classifier = UnderlineClassifierProvider.GetClassifierForView(_view);
            if (classifier == null) return false;

            Mouse.OverrideCursor = span.HasValue ? Cursors.Hand : null;

            classifier.SetUnderlineSpan(span);
            return true;

        }

        void OpenZebbleFile(string value, string projectPath)
        {
            var zebbleFile = FindZebbleModuleInAppUI(value, projectPath);

            if (!File.Exists(zebbleFile))
            {
                App.StatusBar.Text = "Cannot find " + value;
            }

            else
            {
                App.ExecuteCommand("File.OpenFile", zebbleFile.WrapInQuatation());
                App.StatusBar.Text = "Ready";
            }
        }

        void DispatchClick(string value)
        {
            var projectPath = App.GetCurrentProjectPath();

            if (projectPath.IsEmpty())
            {
                App.StatusBar.Text = "Cannot find project paths";
                return;
            }

            if (IsZebbleModule(value))
            {
                OpenZebbleFile(value, projectPath);
            }

            else
            {
                if (value.StartsWith("~") || value.StartsWith("/"))
                    value = projectPath + value.TrimStart("~");
                value = value.Replace('/', Path.DirectorySeparatorChar);
            }

            if (File.Exists(value))
                App.ExecuteCommand("File.OpenFile", value);
            else
                App.StatusBar.Text = "Cannot find " + value;
        }

        string FindZebbleModuleInAppUI(string moduleName, string appPath)
        {
            moduleName = moduleName.Remove("Modules.").ZebblifyFileName();
            var zebbleFiles = new DirectoryInfo(appPath).GetFiles("*.zbl", SearchOption.AllDirectories).Where(f => f.Name == moduleName).ToList();

            if (zebbleFiles.Count == 0) return string.Empty;
            if (zebbleFiles.Count == 1) return Path.Combine(zebbleFiles[0].FullName);
            else
            {
                var fileInModule = GetDirectoryFiles(appPath, "Views\\Modules\\").FirstOrDefault(f => new FileInfo(f).Name == moduleName);
                if (fileInModule.HasValue()) return fileInModule;

                var fileInPages = GetDirectoryFiles(appPath, "Views\\Pages\\").FirstOrDefault(f => new FileInfo(f).Name == moduleName);
                if (fileInPages.HasValue()) return fileInPages;

                var fileInView = GetDirectoryFiles(appPath, "Views\\").FirstOrDefault(f => new FileInfo(f).Name == moduleName);
                if (fileInView.HasValue()) return fileInView;

                var fileInComponents = GetDirectoryFiles(appPath, "Templates\\Components\\").FirstOrDefault(f => new FileInfo(f).Name == moduleName);
                if (fileInComponents.HasValue()) return fileInComponents;

                var fileInTemplates = GetDirectoryFiles(appPath, "Templates\\").FirstOrDefault(f => new FileInfo(f).Name == moduleName);
                return fileInTemplates.HasValue() ? fileInTemplates : string.Empty;
            }
        }

        string[] GetDirectoryFiles(string path, string key) => Directory.GetFiles(path, key, SearchOption.AllDirectories);

        bool IsZebbleModule(string value) => value.StartsWith("Modules") || value.StartsWith("Menu") || value.EndsWith("Tabs");

        #endregion
    }
}