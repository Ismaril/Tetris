using System;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

namespace Tetris
{
    partial class Form1
    {
        private System.Windows.Forms.PictureBox pictureBox;

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
            // Create 2D matrix at GUI.
            int counter = 0;
            for (int i = 0; i < Constants.HEIGHT_OF_GRID; i++)
            {
                for (int j = 0; j < Constants.WIDTH_OF_GRID; j++)
                {
                    this.pictureBox = new System.Windows.Forms.PictureBox();
                    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
                    this.SuspendLayout();
                    this.pictureBox.BackColor = System.Drawing.Color.Red;
                    this.pictureBox.Location = new System.Drawing.Point(j * 44, i * 44);
                    this.pictureBox.Name = $"pictureBox{counter}";
                    this.pictureBox.Size = new System.Drawing.Size(40, 40);
                    this.pictureBox.TabIndex = 0;
                    this.pictureBox.TabStop = false;
                    this.Controls.Add(this.pictureBox);
                    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
                    counter++;
                }
            }
            // 
            // Form1 (Main frame)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 1000);

            this.Name = "Form1";
            this.Text = "Form1";
            this.BackColor = System.Drawing.Color.Black;
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
        }
        #endregion

        /// <summary>
        /// Redraw GUI based on inputed matrix. This function must be used when tetromino is either moving down or sideways.
        /// </summary>
        /// <param name="matrix"></param>
        public void Redraw(byte[] matrix)
        {
            for(int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i] != 0)
                {
                this.Controls[i].BackColor = System.Drawing.Color.Green;
                }
            }
        }

        /// <summary>
        /// Redraw GUI based on inputed matrix. This function must be used when you want to redraw GUI back -> previous positions of tetromino back to empty.
        /// </summary>
        /// <param name="matrix"></param>
        public void RedrawBack(byte[] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i] == 0)
                {
                    this.Controls[i].BackColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}

