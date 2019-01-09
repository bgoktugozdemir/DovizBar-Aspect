using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DovizBar.Aspects;
using PostSharp.Aspects;

namespace DovizBar
{
    [Serializable]
    public class UpdaterAspect : OnMethodBoundaryAspect
    {
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Form1.form1.RunningMode();
        }

        [StopThreadingAspect]
        public override void OnException(MethodExecutionArgs args)
        {
            Form1.form1.ErrorMode();
            Debug.WriteLine("HATA MESAJI: "+this.GetType().Name+" "+System.Reflection.MethodBase.GetCurrentMethod().Name+" "+args.Exception.Message);
            if (Form1.form1.InvokeRequired)
            {
                Form1.form1.BeginInvoke(new Form1.DelegateUpdateScreen(Form1.form1.ErrorMode));
            }
            else
            {
                Form1.form1.ErrorMode();
            }
        }
    }
}
