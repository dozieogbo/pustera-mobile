using Android.OS;
using Pustera.Droid;
using Pustera.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(Closer))]
namespace Pustera.Droid
{
    class Closer : ICloser
    {
        public Closer()
        {

        }

        public void Close()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}