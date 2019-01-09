using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class GoalAspect:OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            Form1.IsErrorMode = true;
            Form1.form1.oldDolar = "0,00";
            Form1.form1.oldEuro = "0,00";
        }
    }
}
