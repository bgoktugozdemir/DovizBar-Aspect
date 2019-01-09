using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class LastUpdateAspect:OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Form1.form1.Change_txtLastUpdate_Text($"v{Program.Version}");
        }
    }
}
