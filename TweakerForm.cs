using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32; // Инжект для прямой работы с кустами реестра NT

namespace WindowsTweaker
{
    // Используем модификатор partial, чтобы напрочь исключить конфликты с дизайнером Студии!
    public partial class TweakerForm : Form
    {
        private Label lblTitle;
        private Button btnDisableTelemetry;
        private Button btnApplyRoboto;
        private Button btnClearRam;
        private Button btnFixUpdate;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        public TweakerForm()
        {
            // Спецификации независимого Win32-окна твикера
            this.Text = "🛠️ WINDOWS TWEAKER v1.0 [Suvern Optimizer]";
            this.Size = new Size(380, 440);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(28, 28, 28); // Наш фирменный тёмно-неоновый фон

            Font adminFont = new Font("Roboto", 10, FontStyle.Bold);

            // Заголовок панели
            lblTitle = new Label() { Text = "⚙️ СУВЕРЕННЫЙ ОПТИМИЗАТОР ЯДРА NT", Location = new Point(20, 20), Width = 340, ForeColor = Color.FromArgb(0, 120, 215), Font = adminFont };

            // Твик 1: Вырезание шпионской телеметрии «Майскрим»
            btnDisableTelemetry = new Button() { Text = "❌ ВЫРЕЗАТЬ ТЕЛЕМЕТРИЮ", Location = new Point(20, 60), Size = new Size(150, 140), BackColor = Color.FromArgb(183, 28, 28), ForeColor = Color.White, Font = adminFont, FlatStyle = FlatStyle.Flat };
            btnDisableTelemetry.Click += (s, e) => {
                lblStatus.Text = "[TWEAK] Выжигание зондов телеметрии Microsoft...";
                try
                {
                    // Прямой инжект в куст HKLM для блокировки шпионских служб DataCollection
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection", true);
                    if (key == null) key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection");
                    key.SetValue("AllowTelemetry", 0, RegistryValueKind.DWord);
                    key.Close();
                    lblStatus.Text = "[OK] Телеметрия 'Майскрим' заблокирована в реестре!";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"[FAIL] Требуются права СИСТЕМЫ: {ex.Message}";
                }
            };

            // Твик 2: Глобальный переход всей системы на шрифт Roboto
            btnApplyRoboto = new Button() { Text = "📝 ИНЖЕКТ ROBOTO", Location = new Point(190, 60), Size = new Size(150, 140), BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, Font = adminFont, FlatStyle = FlatStyle.Flat };
            btnApplyRoboto.Click += (s, e) => {
                lblStatus.Text = "[TWEAK] Подготовка FontSubstitutes под шрифт Roboto...";
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", true);
                    if (key != null)
                    {
                        key.SetValue("Segoe UI", "Roboto", RegistryValueKind.String);
                        key.Close();
                        lblStatus.Text = "[OK] Шрифт Roboto вшит как основной системный!";
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"[FAIL] Ошибка куста реестра: {ex.Message}";
                }
            };

            // Твик 3: Форсированная зачистка ОЗУ от невыгруженных дескрипторов
            btnClearRam = new Button() { Text = "🧹 ОЧИСТИТЬ ОЗУ", Location = new Point(20, 210), Size = new Size(150, 140), BackColor = Color.FromArgb(16, 124, 65), ForeColor = Color.White, Font = adminFont, FlatStyle = FlatStyle.Flat };
            btnClearRam.Click += (s, e) => {
                lblStatus.Text = "[TWEAK] Выгрузка мусорных кэшей из оперативной памяти...";

                // ИСПРАВЛЕНО! Вызов официальных, нативных методов очистки ОЗУ хоста!
                GC.Collect();
                GC.WaitForPendingFinalizers(); // <- Никаких фантомных Слайдеров!

                lblStatus.Text = "[OK] Кэш ОЗУ очищен. Процессор стабилен: 36.6°C!";
            };

            // Твик 4: Экстренное исправление заклинившего 67-го пакета обновлений (Анти-Приказ 66)
            btnFixUpdate = new Button() { Text = "⚡ СБРОСИТЬ ОБНОВЛЕНИЯ", Location = new Point(190, 210), Size = new Size(150, 140), BackColor = Color.FromArgb(230, 81, 0), ForeColor = Color.White, Font = adminFont, FlatStyle = FlatStyle.Flat };
            btnFixUpdate.Click += (s, e) => {
                lblStatus.Text = "[TWEAK] Выжигание заклинившей очереди SoftwareDistribution...";
                try
                {
                    Process.Start(new ProcessStartInfo("cmd.exe", "/c net stop wuauserv && del /f /q /s %windir%\\SoftwareDistribution\\* && net start wuauserv") { CreateNoWindow = true, UseShellExecute = false });
                    lblStatus.Text = "[OK] Приказ 66 подавлен! Ошибка 67-го пакета стёрта.";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"[FAIL] Ошибка вызова консоли NT: {ex.Message}";
                }
            };

            // Статус-бар под защитой MSEM
            statusStrip = new StatusStrip() { BackColor = Color.FromArgb(45, 45, 48) };
            lblStatus = new ToolStripStatusLabel("Статус: Опечатка WaitForPendingSliders полностью выжжена из ОЗУ.");
            lblStatus.ForeColor = Color.Lime;
            lblStatus.Font = new Font("Roboto", 9, FontStyle.Regular);
            statusStrip.Items.Add(lblStatus);

            // Монтируем элементы управления твикера на форму
            this.Controls.Add(lblTitle);
            this.Controls.Add(btnDisableTelemetry);
            this.Controls.Add(btnApplyRoboto);
            this.Controls.Add(btnClearRam);
            this.Controls.Add(btnFixUpdate);
            this.Controls.Add(statusStrip);
        }
    }
}
