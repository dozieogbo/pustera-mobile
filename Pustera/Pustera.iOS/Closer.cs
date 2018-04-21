using Pustera.Helpers;
using Pustera.iOS;
using System.Diagnostics;
using Xamarin.Forms;

[assembly: Dependency(typeof(Closer))]

namespace Pustera.iOS
{
    class Closer : ICloser
    {
        public Closer()
        {

        }

        public void Close()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}