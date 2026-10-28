namespace QuanLyWebTinTuc
{
    partial class frmTaiKhoan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnl_TaiKhoan = new Guna.UI2.WinForms.Guna2Panel();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txt_TimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            btn_TimKiem = new Guna.UI2.WinForms.Guna2Button();
            guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            dgv_TaiKhoan = new Guna.UI2.WinForms.Guna2DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txt_TenDangNhap = new Guna.UI2.WinForms.Guna2TextBox();
            guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txt_HoTen = new Guna.UI2.WinForms.Guna2TextBox();
            guna2HtmlLabel6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txt_MatKhau = new Guna.UI2.WinForms.Guna2TextBox();
            guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            cbo_VaiTro = new Guna.UI2.WinForms.Guna2ComboBox();
            chk_HoatDong = new Guna.UI2.WinForms.Guna2CheckBox();
            btn_Them = new Guna.UI2.WinForms.Guna2Button();
            btn_Sua = new Guna.UI2.WinForms.Guna2Button();
            btn_Xoa = new Guna.UI2.WinForms.Guna2Button();
            btn_LamMoi = new Guna.UI2.WinForms.Guna2Button();
            pnl_TaiKhoan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_TaiKhoan).BeginInit();
            guna2GroupBox1.SuspendLayout();
            SuspendLayout();
            //
            // pnl_TaiKhoan
            //
            pnl_TaiKhoan.AutoScroll = true;
            pnl_TaiKhoan.Controls.Add(guna2GroupBox1);
            pnl_TaiKhoan.Controls.Add(dgv_TaiKhoan);
            pnl_TaiKhoan.Controls.Add(guna2HtmlLabel3);
            pnl_TaiKhoan.Controls.Add(btn_TimKiem);
            pnl_TaiKhoan.Controls.Add(txt_TimKiem);
            pnl_TaiKhoan.Controls.Add(guna2HtmlLabel2);
            pnl_TaiKhoan.Controls.Add(guna2HtmlLabel1);
            pnl_TaiKhoan.Dock = DockStyle.Fill;
            pnl_TaiKhoan.Location = new Point(0, 0);
            pnl_TaiKhoan.Name = "pnl_TaiKhoan";
            pnl_TaiKhoan.Size = new Size(1059, 800);
            pnl_TaiKhoan.TabIndex = 0;
            //
            // guna2HtmlLabel1
            //
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(12, 38);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(190, 33);
            guna2HtmlLabel1.TabIndex = 0;
            guna2HtmlLabel1.Text = "Quản lý tài khoản";
            //
            // guna2HtmlLabel2
            //
            guna2HtmlLabel2.BackColor = Color.Transparent;
            guna2HtmlLabel2.Location = new Point(12, 77);
            guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            guna2HtmlLabel2.Size = new Size(640, 22);
            guna2HtmlLabel2.TabIndex = 1;
            guna2HtmlLabel2.Text = "Quản trị viên: toàn quyền  |  Nhân viên: không được xóa, giao dịch chỉ được xem, không vào trang này";
            //
            // txt_TimKiem
            //
            txt_TimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txt_TimKiem.DefaultText = "";
            txt_TimKiem.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_TimKiem.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_TimKiem.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_TimKiem.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_TimKiem.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_TimKiem.Font = new Font("Segoe UI", 9F);
            txt_TimKiem.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_TimKiem.Location = new Point(22, 120);
            txt_TimKiem.Margin = new Padding(3, 4, 3, 4);
            txt_TimKiem.Name = "txt_TimKiem";
            txt_TimKiem.PlaceholderText = "Tìm theo tên đăng nhập, họ tên...";
            txt_TimKiem.SelectedText = "";
            txt_TimKiem.Size = new Size(937, 38);
            txt_TimKiem.TabIndex = 2;
            txt_TimKiem.TextChanged += txt_TimKiem_TextChanged;
            //
            // btn_TimKiem
            //
            btn_TimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_TimKiem.DisabledState.BorderColor = Color.DarkGray;
            btn_TimKiem.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_TimKiem.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_TimKiem.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_TimKiem.Font = new Font("Segoe UI", 9F);
            btn_TimKiem.ForeColor = Color.White;
            btn_TimKiem.Location = new Point(965, 119);
            btn_TimKiem.Name = "btn_TimKiem";
            btn_TimKiem.Size = new Size(80, 39);
            btn_TimKiem.TabIndex = 3;
            btn_TimKiem.Text = "Tìm";
            btn_TimKiem.Click += btn_TimKiem_Click;
            //
            // guna2HtmlLabel3
            //
            guna2HtmlLabel3.BackColor = Color.Transparent;
            guna2HtmlLabel3.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel3.Location = new Point(22, 172);
            guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            guna2HtmlLabel3.Size = new Size(180, 27);
            guna2HtmlLabel3.TabIndex = 4;
            guna2HtmlLabel3.Text = "Danh sách tài khoản";
            //
            // dgv_TaiKhoan
            //
            dataGridViewCellStyle1.BackColor = Color.White;
            dgv_TaiKhoan.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgv_TaiKhoan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgv_TaiKhoan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgv_TaiKhoan.ColumnHeadersHeight = 22;
            dgv_TaiKhoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv_TaiKhoan.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgv_TaiKhoan.DefaultCellStyle = dataGridViewCellStyle3;
            dgv_TaiKhoan.GridColor = Color.FromArgb(231, 229, 255);
            dgv_TaiKhoan.Location = new Point(22, 205);
            dgv_TaiKhoan.Name = "dgv_TaiKhoan";
            dgv_TaiKhoan.RowHeadersVisible = false;
            dgv_TaiKhoan.RowHeadersWidth = 51;
            dgv_TaiKhoan.Size = new Size(1023, 230);
            dgv_TaiKhoan.TabIndex = 5;
            dgv_TaiKhoan.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgv_TaiKhoan.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgv_TaiKhoan.ThemeStyle.HeaderStyle.Height = 22;
            dgv_TaiKhoan.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgv_TaiKhoan.ThemeStyle.RowsStyle.Height = 29;
            dgv_TaiKhoan.CellClick += dgv_TaiKhoan_CellClick;
            //
            // Column1
            //
            Column1.DataPropertyName = "TenDangNhap";
            Column1.HeaderText = "Tên đăng nhập";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            //
            // Column2
            //
            Column2.DataPropertyName = "HoTen";
            Column2.HeaderText = "Họ tên";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            //
            // Column3
            //
            Column3.DataPropertyName = "TenVaiTro";
            Column3.HeaderText = "Vai trò";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            //
            // Column4
            //
            Column4.DataPropertyName = "TenTrangThai";
            Column4.HeaderText = "Trạng thái";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            //
            // guna2GroupBox1
            //
            guna2GroupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            guna2GroupBox1.Controls.Add(btn_LamMoi);
            guna2GroupBox1.Controls.Add(btn_Xoa);
            guna2GroupBox1.Controls.Add(btn_Sua);
            guna2GroupBox1.Controls.Add(btn_Them);
            guna2GroupBox1.Controls.Add(chk_HoatDong);
            guna2GroupBox1.Controls.Add(cbo_VaiTro);
            guna2GroupBox1.Controls.Add(guna2HtmlLabel7);
            guna2GroupBox1.Controls.Add(txt_MatKhau);
            guna2GroupBox1.Controls.Add(guna2HtmlLabel6);
            guna2GroupBox1.Controls.Add(txt_HoTen);
            guna2GroupBox1.Controls.Add(guna2HtmlLabel5);
            guna2GroupBox1.Controls.Add(txt_TenDangNhap);
            guna2GroupBox1.Controls.Add(guna2HtmlLabel4);
            guna2GroupBox1.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2GroupBox1.ForeColor = Color.Black;
            guna2GroupBox1.Location = new Point(0, 450);
            guna2GroupBox1.Name = "guna2GroupBox1";
            guna2GroupBox1.Size = new Size(1059, 330);
            guna2GroupBox1.TabIndex = 6;
            guna2GroupBox1.Text = "Thông tin tài khoản";
            //
            // guna2HtmlLabel4
            //
            guna2HtmlLabel4.BackColor = Color.Transparent;
            guna2HtmlLabel4.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2HtmlLabel4.Location = new Point(25, 54);
            guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            guna2HtmlLabel4.Size = new Size(100, 22);
            guna2HtmlLabel4.TabIndex = 0;
            guna2HtmlLabel4.Text = "Tên đăng nhập";
            //
            // txt_TenDangNhap
            //
            txt_TenDangNhap.DefaultText = "";
            txt_TenDangNhap.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_TenDangNhap.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_TenDangNhap.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_TenDangNhap.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_TenDangNhap.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_TenDangNhap.Font = new Font("Segoe UI", 9F);
            txt_TenDangNhap.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_TenDangNhap.Location = new Point(25, 83);
            txt_TenDangNhap.Margin = new Padding(3, 4, 3, 4);
            txt_TenDangNhap.Name = "txt_TenDangNhap";
            txt_TenDangNhap.PlaceholderText = "VD: nhanvien2";
            txt_TenDangNhap.SelectedText = "";
            txt_TenDangNhap.Size = new Size(490, 41);
            txt_TenDangNhap.TabIndex = 1;
            //
            // guna2HtmlLabel5
            //
            guna2HtmlLabel5.BackColor = Color.Transparent;
            guna2HtmlLabel5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2HtmlLabel5.Location = new Point(542, 54);
            guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            guna2HtmlLabel5.Size = new Size(50, 22);
            guna2HtmlLabel5.TabIndex = 2;
            guna2HtmlLabel5.Text = "Họ tên";
            //
            // txt_HoTen
            //
            txt_HoTen.DefaultText = "";
            txt_HoTen.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_HoTen.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_HoTen.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_HoTen.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_HoTen.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_HoTen.Font = new Font("Segoe UI", 9F);
            txt_HoTen.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_HoTen.Location = new Point(542, 83);
            txt_HoTen.Margin = new Padding(3, 4, 3, 4);
            txt_HoTen.Name = "txt_HoTen";
            txt_HoTen.PlaceholderText = "Nhập họ tên người dùng";
            txt_HoTen.SelectedText = "";
            txt_HoTen.Size = new Size(490, 41);
            txt_HoTen.TabIndex = 3;
            //
            // guna2HtmlLabel6
            //
            guna2HtmlLabel6.BackColor = Color.Transparent;
            guna2HtmlLabel6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2HtmlLabel6.Location = new Point(25, 133);
            guna2HtmlLabel6.Name = "guna2HtmlLabel6";
            guna2HtmlLabel6.Size = new Size(65, 22);
            guna2HtmlLabel6.TabIndex = 4;
            guna2HtmlLabel6.Text = "Mật khẩu";
            //
            // txt_MatKhau
            //
            txt_MatKhau.DefaultText = "";
            txt_MatKhau.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_MatKhau.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_MatKhau.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_MatKhau.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_MatKhau.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_MatKhau.Font = new Font("Segoe UI", 9F);
            txt_MatKhau.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_MatKhau.Location = new Point(25, 162);
            txt_MatKhau.Margin = new Padding(3, 4, 3, 4);
            txt_MatKhau.Name = "txt_MatKhau";
            txt_MatKhau.PlaceholderText = "Khi sửa: để trống nếu không đổi mật khẩu";
            txt_MatKhau.SelectedText = "";
            txt_MatKhau.Size = new Size(490, 41);
            txt_MatKhau.TabIndex = 5;
            //
            // guna2HtmlLabel7
            //
            guna2HtmlLabel7.BackColor = Color.Transparent;
            guna2HtmlLabel7.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2HtmlLabel7.Location = new Point(542, 133);
            guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            guna2HtmlLabel7.Size = new Size(50, 22);
            guna2HtmlLabel7.TabIndex = 6;
            guna2HtmlLabel7.Text = "Vai trò";
            //
            // cbo_VaiTro
            //
            cbo_VaiTro.BackColor = Color.Transparent;
            cbo_VaiTro.DrawMode = DrawMode.OwnerDrawFixed;
            cbo_VaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo_VaiTro.FocusedColor = Color.FromArgb(94, 148, 255);
            cbo_VaiTro.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbo_VaiTro.Font = new Font("Segoe UI", 10F);
            cbo_VaiTro.ForeColor = Color.FromArgb(68, 88, 112);
            cbo_VaiTro.ItemHeight = 30;
            cbo_VaiTro.Items.AddRange(new object[] { "Quản trị viên", "Nhân viên" });
            cbo_VaiTro.Location = new Point(542, 164);
            cbo_VaiTro.Name = "cbo_VaiTro";
            cbo_VaiTro.Size = new Size(240, 36);
            cbo_VaiTro.TabIndex = 7;
            //
            // chk_HoatDong
            //
            chk_HoatDong.AutoSize = true;
            chk_HoatDong.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            chk_HoatDong.CheckedState.BorderRadius = 0;
            chk_HoatDong.CheckedState.BorderThickness = 0;
            chk_HoatDong.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            chk_HoatDong.Font = new Font("Segoe UI", 9F);
            chk_HoatDong.Location = new Point(810, 172);
            chk_HoatDong.Name = "chk_HoatDong";
            chk_HoatDong.Size = new Size(140, 24);
            chk_HoatDong.TabIndex = 8;
            chk_HoatDong.Text = "Đang hoạt động";
            chk_HoatDong.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            chk_HoatDong.UncheckedState.BorderRadius = 0;
            chk_HoatDong.UncheckedState.BorderThickness = 0;
            chk_HoatDong.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            //
            // btn_Them
            //
            btn_Them.DisabledState.BorderColor = Color.DarkGray;
            btn_Them.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Them.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Them.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Them.FillColor = Color.MediumSeaGreen;
            btn_Them.Font = new Font("Segoe UI", 9F);
            btn_Them.ForeColor = Color.White;
            btn_Them.Location = new Point(66, 240);
            btn_Them.Name = "btn_Them";
            btn_Them.Size = new Size(149, 56);
            btn_Them.TabIndex = 9;
            btn_Them.Text = "Thêm";
            btn_Them.Click += btn_Them_Click;
            //
            // btn_Sua
            //
            btn_Sua.DisabledState.BorderColor = Color.DarkGray;
            btn_Sua.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Sua.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Sua.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Sua.FillColor = Color.DodgerBlue;
            btn_Sua.Font = new Font("Segoe UI", 9F);
            btn_Sua.ForeColor = Color.White;
            btn_Sua.Location = new Point(328, 240);
            btn_Sua.Name = "btn_Sua";
            btn_Sua.Size = new Size(149, 56);
            btn_Sua.TabIndex = 10;
            btn_Sua.Text = "Sửa";
            btn_Sua.Click += btn_Sua_Click;
            //
            // btn_Xoa
            //
            btn_Xoa.DisabledState.BorderColor = Color.DarkGray;
            btn_Xoa.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Xoa.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Xoa.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Xoa.FillColor = Color.FromArgb(255, 128, 128);
            btn_Xoa.Font = new Font("Segoe UI", 9F);
            btn_Xoa.ForeColor = Color.White;
            btn_Xoa.Location = new Point(593, 240);
            btn_Xoa.Name = "btn_Xoa";
            btn_Xoa.Size = new Size(149, 56);
            btn_Xoa.TabIndex = 11;
            btn_Xoa.Text = "Xóa";
            btn_Xoa.Click += btn_Xoa_Click;
            //
            // btn_LamMoi
            //
            btn_LamMoi.DisabledState.BorderColor = Color.DarkGray;
            btn_LamMoi.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_LamMoi.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_LamMoi.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_LamMoi.FillColor = Color.White;
            btn_LamMoi.Font = new Font("Segoe UI", 9F);
            btn_LamMoi.ForeColor = Color.Black;
            btn_LamMoi.Location = new Point(843, 240);
            btn_LamMoi.Name = "btn_LamMoi";
            btn_LamMoi.Size = new Size(149, 56);
            btn_LamMoi.TabIndex = 12;
            btn_LamMoi.Text = "Làm mới";
            btn_LamMoi.Click += btn_LamMoi_Click;
            //
            // frmTaiKhoan
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1059, 800);
            Controls.Add(pnl_TaiKhoan);
            Name = "frmTaiKhoan";
            Text = "TaiKhoan";
            Load += frmTaiKhoan_Load;
            pnl_TaiKhoan.ResumeLayout(false);
            pnl_TaiKhoan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_TaiKhoan).EndInit();
            guna2GroupBox1.ResumeLayout(false);
            guna2GroupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnl_TaiKhoan;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2TextBox txt_TimKiem;
        private Guna.UI2.WinForms.Guna2Button btn_TimKiem;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
        private Guna.UI2.WinForms.Guna2DataGridView dgv_TaiKhoan;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel4;
        private Guna.UI2.WinForms.Guna2TextBox txt_TenDangNhap;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2TextBox txt_HoTen;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel6;
        private Guna.UI2.WinForms.Guna2TextBox txt_MatKhau;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel7;
        private Guna.UI2.WinForms.Guna2ComboBox cbo_VaiTro;
        private Guna.UI2.WinForms.Guna2CheckBox chk_HoatDong;
        private Guna.UI2.WinForms.Guna2Button btn_Them;
        private Guna.UI2.WinForms.Guna2Button btn_Sua;
        private Guna.UI2.WinForms.Guna2Button btn_Xoa;
        private Guna.UI2.WinForms.Guna2Button btn_LamMoi;
    }
}
