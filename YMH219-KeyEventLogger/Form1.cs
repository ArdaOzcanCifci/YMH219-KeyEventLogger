using System;
using System.Drawing;
using System.Windows.Forms;

namespace YMH219_KeyEventLogger
{
    public partial class Form1 : Form
    {
        private readonly KeyLogManager _manager = new KeyLogManager();

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true; // Tuş olayları önce Form'a gelir

            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            btnClear.Click += BtnClear_Click;
            btnSave.Click += BtnSave_Click;
            KeyDown += (s, e) => LogKey(e.KeyCode, "Down");
            KeyUp += (s, e) => LogKey(e.KeyCode, "Up");
            PreviewKeyDown += (s, e) =>
            {
                // Ok tuşları normalde fokus gezinmesinde yutulur, yakalamak için:
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                    e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    e.IsInputKey = true;
            };

            UpdateStatus();
        }

        private void LogKey(Keys key, string type)
        {
            if (!_manager.IsRecording) return;
            lstKeys.Items.Add(_manager.Add(key.ToString(), type).ToString());
            lstKeys.TopIndex = lstKeys.Items.Count - 1; // Otomatik kaydır
            UpdateStatus();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            _manager.Start();
            lstKeys.Focus(); // Space/Enter butonu tetiklemesin
            UpdateStatus();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            _manager.Stop();
            UpdateStatus();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            _manager.Clear();
            lstKeys.Items.Clear();
            UpdateStatus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_manager.Count == 0)
            {
                MessageBox.Show("Kaydedilecek kayıt yok.");
                return;
            }
            using (var dlg = new SaveFileDialog
            {
                Filter = "Metin dosyası|*.txt",
                FileName = $"keylog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    _manager.SaveToFile(dlg.FileName);
                    MessageBox.Show("Kaydedildi:\n" + dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya kaydedilemedi: " + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateStatus()
        {
            lblStatus.Text = (_manager.IsRecording ? "● KAYIT AÇIK" : "● KAYIT KAPALI")
                             + $" — {_manager.Count} kayıt";
            lblStatus.ForeColor = _manager.IsRecording ? Color.Green : Color.Red;
        }
    }
}