namespace Caja
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtCorreoEmpleado = new TextBox();
            txtTelefonoEmpleado = new TextBox();
            label1 = new Label();
            label2 = new Label();
            cbMetodoPago = new ComboBox();
            label3 = new Label();
            dgvServicios = new DataGridView();
            cbServicios = new ComboBox();
            label4 = new Label();
            txtCantidadServicio = new TextBox();
            label5 = new Label();
            btnAgregarServicio = new Button();
            btnFacturar = new Button();
            label6 = new Label();
            label7 = new Label();
            txtTelefonoCliente = new TextBox();
            txtCorreoCliente = new TextBox();
            label8 = new Label();
            txtTotal = new TextBox();
            label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvServicios).BeginInit();
            SuspendLayout();
            // 
            // txtCorreoEmpleado
            // 
            txtCorreoEmpleado.Location = new Point(190, 151);
            txtCorreoEmpleado.Name = "txtCorreoEmpleado";
            txtCorreoEmpleado.Size = new Size(304, 23);
            txtCorreoEmpleado.TabIndex = 0;
            // 
            // txtTelefonoEmpleado
            // 
            txtTelefonoEmpleado.Location = new Point(190, 196);
            txtTelefonoEmpleado.Name = "txtTelefonoEmpleado";
            txtTelefonoEmpleado.Size = new Size(304, 23);
            txtTelefonoEmpleado.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(15, 194);
            label1.Name = "label1";
            label1.Size = new Size(169, 21);
            label1.TabIndex = 2;
            label1.Text = "Telefono del Empleado:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(25, 149);
            label2.Name = "label2";
            label2.Size = new Size(159, 21);
            label2.TabIndex = 3;
            label2.Text = "Correo del Empleado:";
            // 
            // cbMetodoPago
            // 
            cbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMetodoPago.FormattingEnabled = true;
            cbMetodoPago.Location = new Point(190, 272);
            cbMetodoPago.Name = "cbMetodoPago";
            cbMetodoPago.Size = new Size(220, 23);
            cbMetodoPago.TabIndex = 4;
            cbMetodoPago.SelectedIndexChanged += cbMetodoPago_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(57, 270);
            label3.Name = "label3";
            label3.Size = new Size(127, 21);
            label3.TabIndex = 5;
            label3.Text = "Metodo de pago:";
            // 
            // dgvServicios
            // 
            dgvServicios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvServicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServicios.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvServicios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvServicios.Location = new Point(55, 412);
            dgvServicios.Name = "dgvServicios";
            dgvServicios.Size = new Size(912, 346);
            dgvServicios.TabIndex = 6;
            // 
            // cbServicios
            // 
            cbServicios.FormattingEnabled = true;
            cbServicios.Location = new Point(190, 350);
            cbServicios.Name = "cbServicios";
            cbServicios.Size = new Size(324, 23);
            cbServicios.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(19, 347);
            label4.Name = "label4";
            label4.Size = new Size(165, 21);
            label4.TabIndex = 8;
            label4.Text = "Seleccione un servicio:";
            // 
            // txtCantidadServicio
            // 
            txtCantidadServicio.Location = new Point(649, 350);
            txtCantidadServicio.Name = "txtCantidadServicio";
            txtCantidadServicio.Size = new Size(100, 23);
            txtCantidadServicio.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(568, 352);
            label5.Name = "label5";
            label5.Size = new Size(75, 21);
            label5.TabIndex = 10;
            label5.Text = "Cantidad:";
            // 
            // btnAgregarServicio
            // 
            btnAgregarServicio.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAgregarServicio.Location = new Point(831, 338);
            btnAgregarServicio.Name = "btnAgregarServicio";
            btnAgregarServicio.Size = new Size(145, 38);
            btnAgregarServicio.TabIndex = 11;
            btnAgregarServicio.Text = "Agregar Servicio";
            btnAgregarServicio.UseVisualStyleBackColor = true;
            btnAgregarServicio.Click += btnAgregarServicio_Click;
            // 
            // btnFacturar
            // 
            btnFacturar.BackColor = SystemColors.Info;
            btnFacturar.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFacturar.Location = new Point(401, 784);
            btnFacturar.Name = "btnFacturar";
            btnFacturar.Size = new Size(188, 38);
            btnFacturar.TabIndex = 12;
            btnFacturar.Text = "Facturar";
            btnFacturar.UseVisualStyleBackColor = false;
            btnFacturar.Click += btnFacturar_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(542, 153);
            label6.Name = "label6";
            label6.Size = new Size(138, 21);
            label6.TabIndex = 16;
            label6.Text = "Correo del Cliente:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(535, 198);
            label7.Name = "label7";
            label7.Size = new Size(145, 21);
            label7.TabIndex = 15;
            label7.Text = "Telefono del cliente:";
            // 
            // txtTelefonoCliente
            // 
            txtTelefonoCliente.Location = new Point(686, 196);
            txtTelefonoCliente.Name = "txtTelefonoCliente";
            txtTelefonoCliente.Size = new Size(304, 23);
            txtTelefonoCliente.TabIndex = 14;
            // 
            // txtCorreoCliente
            // 
            txtCorreoCliente.Location = new Point(686, 151);
            txtCorreoCliente.Name = "txtCorreoCliente";
            txtCorreoCliente.Size = new Size(304, 23);
            txtCorreoCliente.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(772, 764);
            label8.Name = "label8";
            label8.Size = new Size(73, 25);
            label8.TabIndex = 17;
            label8.Text = "TOTAL:";
            // 
            // txtTotal
            // 
            txtTotal.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTotal.Location = new Point(845, 764);
            txtTotal.Name = "txtTotal";
            txtTotal.ReadOnly = true;
            txtTotal.Size = new Size(118, 27);
            txtTotal.TabIndex = 18;
            txtTotal.TextAlign = HorizontalAlignment.Center;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Times New Roman", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = SystemColors.ActiveCaptionText;
            label9.Location = new Point(379, 47);
            label9.Name = "label9";
            label9.Size = new Size(264, 40);
            label9.TabIndex = 19;
            label9.Text = "Caja - BrillaCar";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1033, 851);
            Controls.Add(label9);
            Controls.Add(txtTotal);
            Controls.Add(label8);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(txtTelefonoCliente);
            Controls.Add(txtCorreoCliente);
            Controls.Add(btnFacturar);
            Controls.Add(btnAgregarServicio);
            Controls.Add(label5);
            Controls.Add(txtCantidadServicio);
            Controls.Add(label4);
            Controls.Add(cbServicios);
            Controls.Add(dgvServicios);
            Controls.Add(label3);
            Controls.Add(cbMetodoPago);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtTelefonoEmpleado);
            Controls.Add(txtCorreoEmpleado);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvServicios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtCorreoEmpleado;
        private TextBox txtTelefonoEmpleado;
        private Label label1;
        private Label label2;
        private ComboBox cbMetodoPago;
        private Label label3;
        private DataGridView dgvServicios;
        private ComboBox cbServicios;
        private Label label4;
        private TextBox txtCantidadServicio;
        private Label label5;
        private Button btnAgregarServicio;
        private Button btnFacturar;
        private Label label6;
        private Label label7;
        private TextBox txtTelefonoCliente;
        private TextBox txtCorreoCliente;
        private Label label8;
        private TextBox txtTotal;
        private Label label9;
    }
}
