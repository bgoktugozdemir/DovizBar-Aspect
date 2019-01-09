using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class ErrorModeAspect: MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Form1.form1.RunningMode();
            Debug.WriteLine("MESAJ: Error Mode False Yapıldı! (Timer 3)");
        }
    }
}
