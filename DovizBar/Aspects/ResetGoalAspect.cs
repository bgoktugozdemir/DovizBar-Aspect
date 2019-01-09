using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class ResetGoalAspect:MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Form3.ResetGoal(Form3.Type);
        }
    }

    [Serializable]
    public class Form3CloseAspect:OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            Form3.form3.Close();
        }
    }
}
