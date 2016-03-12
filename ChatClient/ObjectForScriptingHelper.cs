using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace ChatClient
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class ObjectForScriptingHelper
    {
        ChargeWebBrowser mExternalWPF;
        public ObjectForScriptingHelper(ChargeWebBrowser w)
        {
            this.mExternalWPF = w;
        }

        public void InvokeMeFromJavascript(string jsscript)
        {
            //this.mExternalWPF.tbMessageFromBrowser.Text = string.Format("Message :{0}", jsscript);
        }

    }

}
