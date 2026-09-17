namespace SportFieldBooking.WinForms.Base;

partial class BaseMainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1366, 768);
        Name = "BaseMainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Quản lý sân thể thao";
        WindowState = FormWindowState.Maximized;
    }
}
