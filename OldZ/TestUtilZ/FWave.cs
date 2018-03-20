using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Wav;
using UtilZ.Lib.Wav.ExBass;

namespace TestUtilZ
{
    public partial class FWave : Form
    {
        public FWave()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(bool falg)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = @"E:\wav";
                //ofd.Filter = @"*.wav|*.wav";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                if (falg)
                {
                    wavePlayer1.FileName = ofd.FileName;
                }
                else
                {
                    wavePlayer2.FileName = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPlay_Click(bool falg)
        {
            try
            {
                if (falg)
                {
                    wavePlayer1.Play();
                }
                else
                {
                    wavePlayer2.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPause_Click(bool falg)
        {
            try
            {
                if (falg)
                {
                    wavePlayer1.Pause();
                }
                else
                {
                    wavePlayer2.Pause();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(bool falg)
        {
            try
            {
                if (falg)
                {
                    wavePlayer1.Stop();
                }
                else
                {
                    wavePlayer2.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FWave_Load(object sender, EventArgs e)
        {
            try
            {
                var ret = WavePlayer.BASS_SetConfig(BassConfigOption.BASS_CONFIG_UPDATETHREADS, 2);
                var ret2 = WavePlayer.BASS_GetConfig(BassConfigOption.BASS_CONFIG_UPDATETHREADS);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void btnOpenFile1_Click(object sender, EventArgs e)
        {
            btnOpenFile_Click(true);
        }

        private void btnPlay1_Click(object sender, EventArgs e)
        {
            btnPlay_Click(true);
        }

        private void btnPause1_Click(object sender, EventArgs e)
        {
            btnPause_Click(true);
        }

        private void btnStop1_Click(object sender, EventArgs e)
        {
            btnStop_Click(true);
        }

        private void btnOpenFile2_Click(object sender, EventArgs e)
        {
            btnOpenFile_Click(false);
        }

        private void btnPlay2_Click(object sender, EventArgs e)
        {
            btnPlay_Click(false);
        }

        private void btnPause2_Click(object sender, EventArgs e)
        {
            btnPause_Click(false);
        }

        private void btnStop2_Click(object sender, EventArgs e)
        {
            btnStop_Click(false);
        }

        private void cbRingHear_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                wavePlayer1.IsRingHear = cbRingHear.Checked;
                wavePlayer2.IsRingHear = cbRingHear.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbIsMergeChanel_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                wavePlayer1.IsMergeChanel = cbIsMergeChanel.Checked;
                wavePlayer2.IsMergeChanel = cbIsMergeChanel.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
