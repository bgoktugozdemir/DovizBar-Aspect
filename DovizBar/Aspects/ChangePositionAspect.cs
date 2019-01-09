using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class ChangePositionAspect:OnMethodBoundaryAspect
    {
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Form1.form1.ChangePosition();
        }
    }
}
