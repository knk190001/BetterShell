using System.Windows.Media;
using HWND =System.IntPtr;
namespace BetterShell.Controls
{
    public class TaskbarApplication
    {
        public HWND HWND { get; set; }
        public int Count { get; set; }
        public ImageSource Icon { get; set; }
        public string Identifier { get; set; }
    }}