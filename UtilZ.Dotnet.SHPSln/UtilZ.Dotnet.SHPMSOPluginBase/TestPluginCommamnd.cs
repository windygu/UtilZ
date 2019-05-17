using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPMSOPluginBase
{
    public class TestPluginCommamnd : CommandBase
    {
        private string _text;
        [TTLVAttribute(101)]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public TestPluginCommamnd()
            : base()
        {

        }

        public TestPluginCommamnd(int cmd, string text)
            : base(cmd)
        {
            this._text = text;
        }
    }
}
