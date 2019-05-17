using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.WindowEx.Winform.Controls;

namespace UtilZ.Dotnet.SHPDevOps
{
    public partial class FTest : Form
    {
        public FTest()
        {
            InitializeComponent();
        }

        private readonly List<Color> _colors = new List<Color>();
        private void FTest_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            numAddCount.Value = 10;
            checkBoxDirection.Checked = this.usageControl1.DrawDirection;
            checkBoxDrawBkGrid.Checked = this.usageControl1.ShowGrid;
            checkBoxShowTitle.Checked = this.usageControl1.ShowTitle;

            var fields = typeof(Color).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty);
            foreach (var field in fields)
            {
                this._colors.Add((Color)field.GetValue(null));
            }
        }

        private readonly Random _rnd = new Random();
        private int _channelId = 1;
        private void btnAddChannel_Click(object sender, EventArgs e)
        {
            int index = this._rnd.Next(0, this._colors.Count);
            var color = this._colors[index];
            this._colors.RemoveAt(index);
            this.usageControl1.AddLine(new CharLine(_channelId++, color.ToString(), color, 1));
            labelChannelCount.Text = $"{this.usageControl1.LineCount}";
        }

        private void btnClearChannel_Click(object sender, EventArgs e)
        {
            this.usageControl1.ClearLine();
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            this.usageControl1.ClearData();
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            try
            {
                var channelCount = this.usageControl1.LineCount;
                if (channelCount < 1)
                {
                    return;
                }

                var channelRange = 100 / channelCount;
                var hafChannelRange = channelRange / 2;
                int max = channelRange + hafChannelRange, min = 0;

                var values = new float[channelCount][];
                int count = (int)numAddCount.Value;

                for (int i = 0; i < channelCount; i++)
                {
                    values[i] = new float[count];
                    var arr = values[i];
                    for (int j = 0; j < count; j++)
                    {
                        arr[j] = this._rnd.Next(min, max);
                    }

                    min += channelRange;
                    max += channelRange;
                }

                this.usageControl1.AddValue(values);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            try
            {
                var channels = this.usageControl1.GetAllLine();
                var channelCount = channels.Length;
                if (channelCount < 1)
                {
                    return;
                }

                var channelRange = 100 / channelCount;
                var hafChannelRange = channelRange / 2;
                int max = channelRange + hafChannelRange, min = 0;

                int count = (int)numAddCount.Value;
                for (int i = 0; i < channelCount; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        this.usageControl1.AddValueBegin(channels[i].Id, this._rnd.Next(min, max));
                    }

                    min += channelRange;
                    max += channelRange;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddData2_Click(object sender, EventArgs e)
        {
            try
            {
                var channelCount = this.usageControl1.LineCount;
                if (channelCount < 1)
                {
                    return;
                }

                var channelRange = 100 / channelCount;
                var hafChannelRange = channelRange / 2;
                int max = channelRange + hafChannelRange, min = 0;

                var values = new float[channelCount];
                for (int i = 0; i < channelCount; i++)
                {
                    values[i] = this._rnd.Next(min, max);
                    min += channelRange;
                    max += channelRange;
                }

                this.usageControl1.AddValue(values);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            int count = (int)numAddCount.Value;
            this.usageControl1.AddValueEnd(count);
        }

        private void btnKeyValuePair_Click(object sender, EventArgs e)
        {
            try
            {
                var channels = this.usageControl1.GetAllLine();
                if (channels.Length < 1)
                {
                    return;
                }

                var channelRange = 100 / channels.Length;
                var hafChannelRange = channelRange / 2;
                int max = channelRange + hafChannelRange, min = 0;
                var values = new KeyValuePair<object, float>[channels.Length];
                for (int i = 0; i < channels.Length; i++)
                {
                    values[i] = new KeyValuePair<object, float>(channels[i].Id, this._rnd.Next(min, max));
                    min += channelRange;
                    max += channelRange;
                }

                this.usageControl1.AddValue(values);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnKeyValuePair2_Click(object sender, EventArgs e)
        {
            try
            {
                var channels = this.usageControl1.GetAllLine();
                if (channels.Length < 1)
                {
                    return;
                }

                var channelRange = 100 / channels.Length;
                var hafChannelRange = channelRange / 2;
                int max = channelRange + hafChannelRange, min = 0;
                var values = new KeyValuePair<object, float[]>[channels.Length];
                int count = (int)numAddCount.Value;

                for (int i = 0; i < channels.Length; i++)
                {
                    var valueArr = new float[count];
                    for (int j = 0; j < count; j++)
                    {
                        valueArr[j] = this._rnd.Next(min, max);
                    }

                    values[i] = new KeyValuePair<object, float[]>(channels[i].Id, valueArr);
                    min += channelRange;
                    max += channelRange;
                }

                this.usageControl1.AddValue(values);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSetingBK_Click(object sender, EventArgs e)
        {
            this.usageControl1.BackColor = Color.DarkRed;
            this.usageControl1.GridLineColor = Color.Yellow;
        }

        private void btnPartValueBegin_Click(object sender, EventArgs e)
        {
            try
            {
                var channels = this.usageControl1.GetAllLine();
                var channelCount = channels.Length;
                if (channelCount < 2)
                {
                    return;
                }

                var channelRange = 100 / channelCount;
                var hafChannelRange = channelRange / 2;
                int max = channelRange + hafChannelRange, min = 0;

                int count = (int)numAddCount.Value;
                for (int i = 0; i < channels.Length / 2; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        this.usageControl1.AddValueBegin(channels[i].Id, this._rnd.Next(min, max));
                    }

                    min += channelRange;
                    max += channelRange;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPartValueEnd_Click(object sender, EventArgs e)
        {
            int count = (int)numAddCount.Value;
            this.usageControl1.AddValueEnd(count);
        }

        private void checkBoxDirection_CheckedChanged(object sender, EventArgs e)
        {
            this.usageControl1.DrawDirection = checkBoxDirection.Checked;
        }

        private void checkBoxDrawBkGrid_CheckedChanged(object sender, EventArgs e)
        {
            this.usageControl1.ShowGrid = checkBoxDrawBkGrid.Checked;
        }

        private void checkBoxShowTitle_CheckedChanged(object sender, EventArgs e)
        {
            this.usageControl1.ShowTitle = checkBoxShowTitle.Checked;
        }
    }
}
