using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
namespace TmxMallPlugIn
{
	public class TmxMallConfDialog : Form
	{
		private IContainer components = null;
		private Button btnCancel;
		private Label lnkRegisterTmxmall;
		private Label lnkTestLogin;
		private TextBox tbClientID;
		private Label labelAPIKey;
		private TextBox tbUserName;
		private Label labelUserName;
		private Button btnOk;
		private NumericUpDown numericUpDownFuzzy;
		private NumericUpDown numericUpDownTmNum;
		private Label labelTMNum;
		private Label labelFuzzy;
		private TabControl tabControlSetting;
		private TabPage tabPageAuthentication;
		private TabPage tabPageAdvancedSettings;
		private Label labelProxyIP;
		private TextBox textBoxProxyIP;
		private Label labelPort;
		private GroupBox groupBoxParameterSetting;
		private GroupBox groupBoxProxySetting;
		private TextBox textBoxProxyPort;
		public TmxMallOptions Options
		{
			get;
			set;
		}
		public TmxMallConfDialog()
		{
			this.InitializeComponent();
		}
		public TmxMallConfDialog(TmxMallOptions options)
		{
			this.Options = options;
			this.InitializeComponent();
			this.tbUserName.Text = this.Options.TmxMallUserName;
			this.tbClientID.Text = this.Options.TmxMallClientID;
			bool flag = !string.IsNullOrEmpty(this.Options.Fuzzy);
			if (flag)
			{
				this.numericUpDownFuzzy.Value = Convert.ToDecimal(this.Options.Fuzzy);
			}
			bool flag2 = !string.IsNullOrEmpty(this.Options.Num);
			if (flag2)
			{
				this.numericUpDownTmNum.Value = Convert.ToDecimal(this.Options.Num);
			}
			bool flag3 = !string.IsNullOrEmpty(this.Options.ProxyIP);
			if (flag3)
			{
				this.textBoxProxyIP.Text = this.Options.ProxyIP;
			}
			bool flag4 = !string.IsNullOrEmpty(this.Options.ProxyPort);
			if (flag4)
			{
				this.textBoxProxyPort.Text = this.Options.ProxyPort;
			}
		}
		private void numericUpDownFuzzy_ValueChanged(object sender, EventArgs e)
		{
		}
		private void numericUpDownNum_ValueChanged(object sender, EventArgs e)
		{
		}
		private bool Test_Collection(string username, string clientid, string proxyip, string proxyport)
		{
			bool flag = string.IsNullOrEmpty(username) || string.IsNullOrEmpty(clientid);
			bool result;
			if (flag)
			{
				MessageBox.Show(this, "Empty user name or client id, please reload it again!", "Tmxamll Plugin");
				base.DialogResult = DialogResult.Retry;
				result = false;
			}
			else
			{
				bool flag2 = false;
				try
				{
					StringBuilder stringBuilder = new StringBuilder();
					bool flag3 = string.IsNullOrEmpty(proxyip);
					if (flag3)
					{
						stringBuilder.Append(TmxMallOptions.verifyUrl + "?user_name=");
					}
					else
					{
						bool flag4 = string.IsNullOrEmpty(proxyport);
						if (flag4)
						{
							stringBuilder.Append("http://" + proxyip + "/v1/http/clientIdVerify?user_name=");
						}
						else
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								"http://",
								proxyip,
								":",
								proxyport,
								"/v1/http/clientIdVerify?user_name="
							}));
						}
					}
					stringBuilder.Append(username);
					stringBuilder.Append("&client_id=");
					stringBuilder.Append(clientid);
					stringBuilder.Append("&de=trados");
					string json = null;
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(stringBuilder.ToString());
					httpWebRequest.Timeout = 5000;
					httpWebRequest.Proxy = null;
					using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
					{
						json = streamReader.ReadToEnd();
					}
					string text = JObject.Parse(json)["error_code"].ToString();
					string text2 = JObject.Parse(json)["error_msg"].ToString();
					bool flag5 = text.Equals("0");
					if (flag5)
					{
						flag2 = true;
					}
					else
					{
						MessageBox.Show(this, text2, "Tmxmall Plugin");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, "System error: " + ex.Message, "Tmxmall Plugin");
				}
				result = flag2;
			}
			return result;
		}
		private void btnOk_Click(object sender, EventArgs e)
		{
			this.Options.TmxMallUserName = this.tbUserName.Text.Trim();
			this.Options.TmxMallClientID = this.tbClientID.Text.Trim();
			bool flag = string.IsNullOrEmpty(this.Options.TmxMallUserName) || string.IsNullOrEmpty(this.Options.TmxMallClientID);
			if (flag)
			{
				MessageBox.Show(this, "Empty user name or client id!", "Tmxmall Plugin");
				base.DialogResult = DialogResult.Retry;
			}
			else
			{
				bool flag2 = !this.Test_Collection(this.tbUserName.Text.Trim(), this.tbClientID.Text.Trim(), this.textBoxProxyIP.Text.Trim(), this.textBoxProxyPort.Text.Trim());
				if (flag2)
				{
					base.DialogResult = DialogResult.Cancel;
				}
				else
				{
					this.Options.Fuzzy = this.numericUpDownFuzzy.Value.ToString();
					this.Options.Num = this.numericUpDownTmNum.Value.ToString();
					this.Options.GlsNum = "0";
					this.Options.ProxyIP = this.textBoxProxyIP.Text.Trim();
					this.Options.ProxyPort = this.textBoxProxyPort.Text.Trim();
					this.Options.LastError = "0";
					base.DialogResult = DialogResult.OK;
				}
			}
		}
		private void lnkTestLogin_Click(object sender, EventArgs e)
		{
			bool flag = this.Test_Collection(this.tbUserName.Text.Trim(), this.tbClientID.Text.Trim(), this.textBoxProxyIP.Text.Trim(), this.textBoxProxyPort.Text.Trim());
			if (flag)
			{
				MessageBox.Show(this, "Connect Successfully!", "Tmxmall Plugin");
			}
		}
		private void lnkRegisterTmxmall_Click(object sender, EventArgs e)
		{
			bool flag = string.IsNullOrEmpty(this.textBoxProxyIP.Text);
			if (flag)
			{
				Process.Start(TmxMallOptions.registerUrl);
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(this.Options.ProxyPort);
				if (flag2)
				{
					Process.Start("http://" + this.textBoxProxyIP.Text + "/user/register");
				}
				else
				{
					Process.Start(string.Concat(new string[]
					{
						"http://",
						this.textBoxProxyIP.Text,
						":",
						this.Options.ProxyPort,
						"/user/register"
					}));
				}
			}
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}
		private void tbClientID_TextChanged(object sender, EventArgs e)
		{
		}
		private void labelAPIKey_Click(object sender, EventArgs e)
		{
		}
		private void tbUserName_TextChanged(object sender, EventArgs e)
		{
		}
		private void labelUserName_Click(object sender, EventArgs e)
		{
		}
		private void groupBoxAdvancedSetting_Enter(object sender, EventArgs e)
		{
		}
		private void numericUpDownFuzzy_ValueChanged_1(object sender, EventArgs e)
		{
		}
		private void numericUpDownGlsNum_ValueChanged(object sender, EventArgs e)
		{
		}
		private void numericUpDownTmNum_ValueChanged(object sender, EventArgs e)
		{
		}
		private void labelTermNum_Click(object sender, EventArgs e)
		{
		}
		private void labelTMNum_Click(object sender, EventArgs e)
		{
		}
		private void labelFuzzy_Click(object sender, EventArgs e)
		{
		}
		private void groupBoxAuthentication_Enter(object sender, EventArgs e)
		{
		}
		private void textBoxProxyPort_TextChanged(object sender, EventArgs e)
		{
		}
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TmxMallConfDialog));
			this.btnCancel = new Button();
			this.lnkRegisterTmxmall = new Label();
			this.lnkTestLogin = new Label();
			this.tbClientID = new TextBox();
			this.labelAPIKey = new Label();
			this.tbUserName = new TextBox();
			this.labelUserName = new Label();
			this.btnOk = new Button();
			this.numericUpDownFuzzy = new NumericUpDown();
			this.numericUpDownTmNum = new NumericUpDown();
			this.labelTMNum = new Label();
			this.labelFuzzy = new Label();
			this.tabControlSetting = new TabControl();
			this.tabPageAuthentication = new TabPage();
			this.tabPageAdvancedSettings = new TabPage();
			this.groupBoxProxySetting = new GroupBox();
			this.textBoxProxyPort = new TextBox();
			this.labelProxyIP = new Label();
			this.labelPort = new Label();
			this.textBoxProxyIP = new TextBox();
			this.groupBoxParameterSetting = new GroupBox();
			((ISupportInitialize)this.numericUpDownFuzzy).BeginInit();
			((ISupportInitialize)this.numericUpDownTmNum).BeginInit();
			this.tabControlSetting.SuspendLayout();
			this.tabPageAuthentication.SuspendLayout();
			this.tabPageAdvancedSettings.SuspendLayout();
			this.groupBoxProxySetting.SuspendLayout();
			this.groupBoxParameterSetting.SuspendLayout();
			base.SuspendLayout();
			this.btnCancel.Cursor = Cursors.Hand;
			this.btnCancel.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.btnCancel.Location = new Point(319, 112);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(86, 25);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			this.lnkRegisterTmxmall.AutoSize = true;
			this.lnkRegisterTmxmall.Cursor = Cursors.Hand;
			this.lnkRegisterTmxmall.Font = new Font("宋体", 9f, FontStyle.Underline, GraphicsUnit.Point, 134);
			this.lnkRegisterTmxmall.ForeColor = Color.Blue;
			this.lnkRegisterTmxmall.Location = new Point(157, 81);
			this.lnkRegisterTmxmall.Name = "lnkRegisterTmxmall";
			this.lnkRegisterTmxmall.Size = new Size(65, 12);
			this.lnkRegisterTmxmall.TabIndex = 5;
			this.lnkRegisterTmxmall.Text = "Register>>";
			this.lnkRegisterTmxmall.Click += new EventHandler(this.lnkRegisterTmxmall_Click);
			this.lnkTestLogin.AutoSize = true;
			this.lnkTestLogin.Cursor = Cursors.Hand;
			this.lnkTestLogin.Font = new Font("宋体", 9f, FontStyle.Underline, GraphicsUnit.Point, 134);
			this.lnkTestLogin.ForeColor = Color.Blue;
			this.lnkTestLogin.Location = new Point(75, 81);
			this.lnkTestLogin.Name = "lnkTestLogin";
			this.lnkTestLogin.Size = new Size(65, 12);
			this.lnkTestLogin.TabIndex = 4;
			this.lnkTestLogin.Text = "Test Login";
			this.lnkTestLogin.Click += new EventHandler(this.lnkTestLogin_Click);
			this.tbClientID.Location = new Point(77, 48);
			this.tbClientID.Name = "tbClientID";
			this.tbClientID.Size = new Size(328, 21);
			this.tbClientID.TabIndex = 3;
			this.labelAPIKey.AutoSize = true;
			this.labelAPIKey.Location = new Point(16, 52);
			this.labelAPIKey.Name = "labelAPIKey";
			this.labelAPIKey.Size = new Size(53, 12);
			this.labelAPIKey.TabIndex = 2;
			this.labelAPIKey.Text = "API Key:";
			this.tbUserName.Location = new Point(77, 17);
			this.tbUserName.Name = "tbUserName";
			this.tbUserName.Size = new Size(328, 21);
			this.tbUserName.TabIndex = 1;
			this.labelUserName.AutoSize = true;
			this.labelUserName.Location = new Point(12, 22);
			this.labelUserName.Name = "labelUserName";
			this.labelUserName.Size = new Size(59, 12);
			this.labelUserName.TabIndex = 0;
			this.labelUserName.Text = "UserName:";
			this.btnOk.Cursor = Cursors.Hand;
			this.btnOk.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.btnOk.Location = new Point(217, 112);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(86, 25);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			this.numericUpDownFuzzy.Location = new Point(403, 25);
			this.numericUpDownFuzzy.Name = "numericUpDownFuzzy";
			this.numericUpDownFuzzy.Size = new Size(38, 21);
			this.numericUpDownFuzzy.TabIndex = 5;
			NumericUpDown arg_605_0 = this.numericUpDownFuzzy;
			int[] expr_5FB = new int[4];
			expr_5FB[0] = 30;
			arg_605_0.Value = new decimal(expr_5FB);
			this.numericUpDownTmNum.Location = new Point(177, 25);
			NumericUpDown arg_638_0 = this.numericUpDownTmNum;
			int[] expr_62F = new int[4];
			expr_62F[0] = 5;
			arg_638_0.Maximum = new decimal(expr_62F);
			NumericUpDown arg_653_0 = this.numericUpDownTmNum;
			int[] expr_64A = new int[4];
			expr_64A[0] = 1;
			arg_653_0.Minimum = new decimal(expr_64A);
			this.numericUpDownTmNum.Name = "numericUpDownTmNum";
			this.numericUpDownTmNum.Size = new Size(38, 21);
			this.numericUpDownTmNum.TabIndex = 3;
			NumericUpDown arg_6A1_0 = this.numericUpDownTmNum;
			int[] expr_698 = new int[4];
			expr_698[0] = 3;
			arg_6A1_0.Value = new decimal(expr_698);
			this.labelTMNum.AutoSize = true;
			this.labelTMNum.Location = new Point(19, 28);
			this.labelTMNum.Name = "labelTMNum";
			this.labelTMNum.Size = new Size(143, 12);
			this.labelTMNum.TabIndex = 1;
			this.labelTMNum.Text = "Maximum hit number(TM):";
			this.labelFuzzy.AutoSize = true;
			this.labelFuzzy.Location = new Point(241, 28);
			this.labelFuzzy.Name = "labelFuzzy";
			this.labelFuzzy.Size = new Size(161, 12);
			this.labelFuzzy.TabIndex = 0;
			this.labelFuzzy.Text = "Minimum matching rate(TM):";
			this.tabControlSetting.Controls.Add(this.tabPageAuthentication);
			this.tabControlSetting.Controls.Add(this.tabPageAdvancedSettings);
			this.tabControlSetting.Location = new Point(-2, 0);
			this.tabControlSetting.Name = "tabControlSetting";
			this.tabControlSetting.SelectedIndex = 0;
			this.tabControlSetting.Size = new Size(485, 192);
			this.tabControlSetting.TabIndex = 8;
			this.tabPageAuthentication.Controls.Add(this.lnkRegisterTmxmall);
			this.tabPageAuthentication.Controls.Add(this.btnCancel);
			this.tabPageAuthentication.Controls.Add(this.tbUserName);
			this.tabPageAuthentication.Controls.Add(this.btnOk);
			this.tabPageAuthentication.Controls.Add(this.lnkTestLogin);
			this.tabPageAuthentication.Controls.Add(this.labelUserName);
			this.tabPageAuthentication.Controls.Add(this.tbClientID);
			this.tabPageAuthentication.Controls.Add(this.labelAPIKey);
			this.tabPageAuthentication.Location = new Point(4, 22);
			this.tabPageAuthentication.Name = "tabPageAuthentication";
			this.tabPageAuthentication.Padding = new Padding(3);
			this.tabPageAuthentication.Size = new Size(477, 166);
			this.tabPageAuthentication.TabIndex = 0;
			this.tabPageAuthentication.Text = "Authentication";
			this.tabPageAuthentication.UseVisualStyleBackColor = true;
			this.tabPageAdvancedSettings.Controls.Add(this.groupBoxProxySetting);
			this.tabPageAdvancedSettings.Controls.Add(this.groupBoxParameterSetting);
			this.tabPageAdvancedSettings.Location = new Point(4, 22);
			this.tabPageAdvancedSettings.Name = "tabPageAdvancedSettings";
			this.tabPageAdvancedSettings.Padding = new Padding(3);
			this.tabPageAdvancedSettings.Size = new Size(477, 166);
			this.tabPageAdvancedSettings.TabIndex = 1;
			this.tabPageAdvancedSettings.Text = "Advanced Settings";
			this.tabPageAdvancedSettings.UseVisualStyleBackColor = true;
			this.groupBoxProxySetting.Controls.Add(this.textBoxProxyPort);
			this.groupBoxProxySetting.Controls.Add(this.labelProxyIP);
			this.groupBoxProxySetting.Controls.Add(this.labelPort);
			this.groupBoxProxySetting.Controls.Add(this.textBoxProxyIP);
			this.groupBoxProxySetting.Location = new Point(12, 88);
			this.groupBoxProxySetting.Name = "groupBoxProxySetting";
			this.groupBoxProxySetting.Size = new Size(459, 71);
			this.groupBoxProxySetting.TabIndex = 10;
			this.groupBoxProxySetting.TabStop = false;
			this.groupBoxProxySetting.Text = "Proxy Settings";
			this.textBoxProxyPort.Location = new Point(308, 26);
			this.textBoxProxyPort.Name = "textBoxProxyPort";
			this.textBoxProxyPort.Size = new Size(42, 21);
			this.textBoxProxyPort.TabIndex = 9;
			this.textBoxProxyPort.TextChanged += new EventHandler(this.textBoxProxyPort_TextChanged);
			this.labelProxyIP.AutoSize = true;
			this.labelProxyIP.Location = new Point(6, 29);
			this.labelProxyIP.Name = "labelProxyIP";
			this.labelProxyIP.Size = new Size(107, 12);
			this.labelProxyIP.TabIndex = 6;
			this.labelProxyIP.Text = "Proxy IP Address:";
			this.labelPort.AutoSize = true;
			this.labelPort.Location = new Point(239, 29);
			this.labelPort.Name = "labelPort";
			this.labelPort.Size = new Size(71, 12);
			this.labelPort.TabIndex = 8;
			this.labelPort.Text = "Proxy Port:";
			this.textBoxProxyIP.Location = new Point(113, 25);
			this.textBoxProxyIP.Name = "textBoxProxyIP";
			this.textBoxProxyIP.Size = new Size(102, 21);
			this.textBoxProxyIP.TabIndex = 7;
			this.groupBoxParameterSetting.Controls.Add(this.labelTMNum);
			this.groupBoxParameterSetting.Controls.Add(this.numericUpDownTmNum);
			this.groupBoxParameterSetting.Controls.Add(this.labelFuzzy);
			this.groupBoxParameterSetting.Controls.Add(this.numericUpDownFuzzy);
			this.groupBoxParameterSetting.Location = new Point(10, 6);
			this.groupBoxParameterSetting.Name = "groupBoxParameterSetting";
			this.groupBoxParameterSetting.Size = new Size(461, 64);
			this.groupBoxParameterSetting.TabIndex = 9;
			this.groupBoxParameterSetting.TabStop = false;
			this.groupBoxParameterSetting.Text = "Parameter Settings";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(486, 188);
			base.Controls.Add(this.tabControlSetting);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Margin = new Padding(0);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TmxMallConfDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Tmxmall Plugin Settings";
			((ISupportInitialize)this.numericUpDownFuzzy).EndInit();
			((ISupportInitialize)this.numericUpDownTmNum).EndInit();
			this.tabControlSetting.ResumeLayout(false);
			this.tabPageAuthentication.ResumeLayout(false);
			this.tabPageAuthentication.PerformLayout();
			this.tabPageAdvancedSettings.ResumeLayout(false);
			this.groupBoxProxySetting.ResumeLayout(false);
			this.groupBoxProxySetting.PerformLayout();
			this.groupBoxParameterSetting.ResumeLayout(false);
			this.groupBoxParameterSetting.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
