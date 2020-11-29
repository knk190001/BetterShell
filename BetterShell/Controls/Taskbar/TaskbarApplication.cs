using System.Collections.Generic;
using System.Windows.Media;
using BetterShell.Utils;
using HWND =System.IntPtr;
namespace BetterShell.Controls
{
    public class TaskbarApplication
    {
        public HWNDList HWNDs { get; } = new HWNDList();
        public int Count { get; set; }
        public ImageSource Icon { get; set; }
        public string Identifier { get; set; }
        
        
    }}