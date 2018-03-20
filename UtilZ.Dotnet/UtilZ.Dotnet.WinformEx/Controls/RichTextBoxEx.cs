using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Dotnet.WinformEx.Controls
{
    public class RichTextBoxEx : RichTextBox
    {
        /// <summary>
        /// 最多显示行数
        /// </summary>
        private int _maxLineCount = 100;

        /// <summary>
        /// 获取或设置最多显示行数
        /// </summary>
        public int MaxLineCount
        {
            get { return _maxLineCount; }
            set
            {
                if (_maxLineCount == value)
                {
                    return;
                }

                _maxLineCount = value;
                this.ValidateLineCount();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RichTextBoxEx() : base()
        {

        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            this.ValidateLineCount();
        }

        /// <summary>
        /// 验证行数
        /// </summary>
        private void ValidateLineCount()
        {
            if (this.Lines.Length <= this._maxLineCount)
            {
                return;
            }

            //int start = this.GetFirstCharIndexFromLine(0);//第一行第一个字符的索引
            //int end = textBox1.GetFirstCharIndexFromLine(1);//第二行第一个字符的索引
            //textBox1.Select(start, end);//选中第一行
            //textBox1.SelectedText = "";//设置第一行的内容为空
        }

        public void AppendLog(string log)
        {
            this.AppendText(log);
        }
    }
}
