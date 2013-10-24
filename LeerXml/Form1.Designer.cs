namespace LeerXml
{
    partial class frm_principal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_cargar = new System.Windows.Forms.Button();
            this.btn_leer = new System.Windows.Forms.Button();
            this.btn_otro = new System.Windows.Forms.Button();
            this.btn_conver = new System.Windows.Forms.Button();
            this.btn_castle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_cargar
            // 
            this.btn_cargar.Location = new System.Drawing.Point(24, 26);
            this.btn_cargar.Name = "btn_cargar";
            this.btn_cargar.Size = new System.Drawing.Size(112, 23);
            this.btn_cargar.TabIndex = 0;
            this.btn_cargar.Text = "GuardarLCO_TXT";
            this.btn_cargar.UseVisualStyleBackColor = true;
            this.btn_cargar.Click += new System.EventHandler(this.btn_cargar_Click);
            // 
            // btn_leer
            // 
            this.btn_leer.Location = new System.Drawing.Point(24, 69);
            this.btn_leer.Name = "btn_leer";
            this.btn_leer.Size = new System.Drawing.Size(75, 23);
            this.btn_leer.TabIndex = 1;
            this.btn_leer.Text = "LeerXml";
            this.btn_leer.UseVisualStyleBackColor = true;
            this.btn_leer.Click += new System.EventHandler(this.btn_leer_Click);
            // 
            // btn_otro
            // 
            this.btn_otro.Location = new System.Drawing.Point(175, 26);
            this.btn_otro.Name = "btn_otro";
            this.btn_otro.Size = new System.Drawing.Size(75, 23);
            this.btn_otro.TabIndex = 2;
            this.btn_otro.Text = "Incog";
            this.btn_otro.UseVisualStyleBackColor = true;
            this.btn_otro.Click += new System.EventHandler(this.btn_otro_Click);
            // 
            // btn_conver
            // 
            this.btn_conver.Location = new System.Drawing.Point(175, 69);
            this.btn_conver.Name = "btn_conver";
            this.btn_conver.Size = new System.Drawing.Size(75, 23);
            this.btn_conver.TabIndex = 3;
            this.btn_conver.Text = "Limpiar";
            this.btn_conver.UseVisualStyleBackColor = true;
            this.btn_conver.Click += new System.EventHandler(this.btn_conver_Click);
            // 
            // btn_castle
            // 
            this.btn_castle.Location = new System.Drawing.Point(286, 46);
            this.btn_castle.Name = "btn_castle";
            this.btn_castle.Size = new System.Drawing.Size(75, 23);
            this.btn_castle.TabIndex = 4;
            this.btn_castle.Text = "BCastle";
            this.btn_castle.UseVisualStyleBackColor = true;
            this.btn_castle.Click += new System.EventHandler(this.btn_castle_Click);
            // 
            // frm_principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 111);
            this.Controls.Add(this.btn_castle);
            this.Controls.Add(this.btn_conver);
            this.Controls.Add(this.btn_otro);
            this.Controls.Add(this.btn_leer);
            this.Controls.Add(this.btn_cargar);
            this.Name = "frm_principal";
            this.Text = "Principal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_cargar;
        private System.Windows.Forms.Button btn_leer;
        private System.Windows.Forms.Button btn_otro;
        private System.Windows.Forms.Button btn_conver;
        private System.Windows.Forms.Button btn_castle;
    }
}

