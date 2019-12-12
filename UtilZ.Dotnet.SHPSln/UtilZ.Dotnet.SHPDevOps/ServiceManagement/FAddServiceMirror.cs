using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Base;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.ServiceManagement
{
    public partial class FAddServiceMirror : Form
    {
        public FAddServiceMirror()
        {
            InitializeComponent();
        }

        private ServiceManager _serviceManager;
        private readonly ServiceInfo _serviceInfoItem;
        private ServiceMirrorInfo _serviceMirrorInfo;
        public ServiceMirrorInfo ServiceMirrorInfo
        {
            get { return _serviceMirrorInfo; }
        }

        public FAddServiceMirror(ServiceManager serviceManager, ServiceInfo serviceInfoItem)
            : this()
        {
            this._serviceManager = serviceManager;
            this._serviceInfoItem = serviceInfoItem;
            int serviceInfoMirrorMaxVersion = serviceManager.QueryServiceInfoMirrorMaxVersion(serviceInfoItem.Id);

            DropdownBoxHelper.BindingEnumToComboBox<ServiceMirrorType>(comboBoxServiceMirrorType, ServiceMirrorType.Zip);
            numVer.Minimum = serviceInfoMirrorMaxVersion + 1;
        }

        private void FServiceUpgrade_Load(object sender, EventArgs e)
        {

        }

        private void btnChioceMirrorFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = @"(zip文件)|*.zip|(rar文件)|*.rar|(exe文件)|*.exe|(全部)|*.*";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtMirrorFilePath.Text = ofd.FileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMirrorFilePath.Text))
                {
                    Loger.Info("镜像文件路径不为空");
                    return;
                }

                if (!File.Exists(txtMirrorFilePath.Text))
                {
                    Loger.Info("镜像文件不存在");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMainFilePath.Text))
                {
                    Loger.Info("主程序文件路径不为空");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtProcessFilePath.Text))
                {
                    Loger.Info("进程文件路径不为空");
                    return;
                }

                var serviceMirrorInfo = this.GetValue();
                this.Commit(serviceMirrorInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void Commit(ServiceMirrorInfo serviceMirrorInfo)
        {
            var para = new PartAsynWaitPara<Tuple<ServiceManager, ServiceMirrorInfo>, object>();
            para.Caption = "正在提交服务";
            para.IsShowCancel = false;
            para.Para = new Tuple<ServiceManager, ServiceMirrorInfo>(this._serviceManager, serviceMirrorInfo);
            para.Function = (p) =>
            {
                p.Para.Item1.AddServiceMirror(p.Para.Item2, p.AsynWait);
                return null;
            };

            para.Completed = (ret) =>
            {
                if (ret.Status == PartAsynExcuteStatus.Completed)
                {
                    this._serviceMirrorInfo = ret.Para.Item2;
                    this.Invoke(new Action(() =>
                    {
                        this.DialogResult = DialogResult.OK;
                    }));
                }
                else if (ret.Status == PartAsynExcuteStatus.Exception)
                {
                    Loger.Error(ret.Exception, "提交升级包异常");
                }
            };

            WinformPartAsynWaitHelper.Wait(para, this);
        }

        private ServiceMirrorInfo GetValue()
        {
            var serviceMirrorInfo = new ServiceMirrorInfo();
            serviceMirrorInfo.ServiceInfoId = this._serviceInfoItem.Id;
            serviceMirrorInfo.MirrorFilePath = txtMirrorFilePath.Text;
            serviceMirrorInfo.Arguments = txtStartArgs.Text;
            serviceMirrorInfo.AppProcessFilePath = txtProcessFilePath.Text;
            serviceMirrorInfo.AppExeFilePath = txtMainFilePath.Text;
            serviceMirrorInfo.Version = (int)numVer.Value;
            serviceMirrorInfo.ServiceMirrorType = DropdownBoxHelper.GetEnumFromComboBox<ServiceMirrorType>(comboBoxServiceMirrorType);
            serviceMirrorInfo.Des = rtxtDes.Text;
            serviceMirrorInfo.DeployMillisecondsTimeout = (int)numDeploySecondsTimeout.Value * 1000;
            return serviceMirrorInfo;
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtMirrorFilePath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filePath = txtMirrorFilePath.Text;
                if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                {
                    return;
                }

                ServiceMirrorType serviceMirrorType;
                var extension = Path.GetExtension(filePath).ToLower();
                switch (extension)
                {
                    case ".rar":
                        serviceMirrorType = ServiceMirrorType.Rar;
                        break;
                    case ".zip":
                    default:
                        serviceMirrorType = ServiceMirrorType.Zip;
                        break;
                }

                DropdownBoxHelper.SetEnumToComboBox<ServiceMirrorType>(comboBoxServiceMirrorType, serviceMirrorType);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
