using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace ChatClient
{
    public class CustomFlowDoc : FlowDocument
    {
        protected override bool IsEnabledCore
        {
            get
            {
                return true;
            }
        }
    }
}
