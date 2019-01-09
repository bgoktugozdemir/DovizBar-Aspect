using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class TargetAspect:OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (Form3.Type == "USD")
                Form3.tempValue = Form1.currentDolar;
            else if (Form3.Type == "EURO")
                Form3.tempValue = Form1.currentEuro;
        }
    }
}
