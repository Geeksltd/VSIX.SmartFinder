using System.ComponentModel;

namespace System.Windows.Forms
{
    public static class ControlExtensions
    {
        public static void SafeAction<T>(this T control, Action<T> action) where T : Control
        {
            if (control.IsDisposed) return;
            if (!control.InvokeRequired) action(control);
            else
                control.Invoke(new MethodInvoker(delegate { SafeAction(control, action); }));
        }

        public static void SafeAction<T>(this T control, Control containerControl, Action<T> action) where T : Component
        {
            if (!containerControl.InvokeRequired)
                action(control);
            else
                containerControl.Invoke(new MethodInvoker(delegate { SafeAction(control, containerControl, action); }));
        }

        public static T SafeGet<T>(this Control control, Func<T> expr)
        {
            if (!control.InvokeRequired) return expr.Invoke();
            else return (T)(control.Invoke(expr));
        }
    }
}
