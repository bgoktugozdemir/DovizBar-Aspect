using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace DovizBar.Aspects
{
    public class Form4InterceptorAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            if (Form1.form4.Visible)
            {
                Form1.form1.Hide();
            }
        }
    }
}
