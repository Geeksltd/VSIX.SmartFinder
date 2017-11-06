using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geeks.VSIX.SmartFinder.Service
{
    public class MyService : SMyService, IMyService
    {
        private Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider;
        private string myString;
        public MyService(Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp)
        {
            Trace.WriteLine("Constructing a new instance of MyService");
            serviceProvider = sp;
        }
        public void Hello()
        {
            myString = "hello";
        }
        public string Goodbye()
        {
            return "goodbye";
        }
    }
    public interface SMyService
    {
    }
    public interface IMyService
    {
        void Hello();
        string Goodbye();
    }
}
