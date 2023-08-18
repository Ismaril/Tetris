using System;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

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
                    this.pictureBox.Location = new System.Drawing.Point(725 + j * 44, i * 44);  // 725
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
            this.ClientSize = new System.Drawing.Size(1920, 1080);

            this.Name = "Form1";
            this.Text = "Form1";
            this.BackColor = (System.Drawing.Color)Constants.COLOR_BACKGROUND;
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
        }
        #endregion

        /// <summary>
        /// Redraw GUI based on inputed matrix. This function must be used when tetromino is either moving down or sideways.
        /// </summary>
        /// <param name="matrix"></param>
        public void Redraw(List<byte> matrix)
        {
            for(byte i = 0; i < matrix.Count; i++)
            {
                if (matrix[i] == 1)
                {
                this.Controls[i].BackColor = System.Drawing.Color.Blue;
                }
                else if (matrix[i] == 2)
                {
                    this.Controls[i].BackColor = System.Drawing.Color.Red;
                }
                else if (matrix[i] == 3)
                {
                    this.Controls[i].BackColor = System.Drawing.Color.Yellow;
                }
                else if (matrix[i] == 4)
                {
                    this.Controls[i].BackColor = System.Drawing.Color.Green;
                }
                else if (matrix[i] == 5)
                {
                    this.Controls[i].BackColor = System.Drawing.Color.Purple;
                }
                else if (matrix[i] == 6)
                {
                    this.Controls[i].BackColor = System.Drawing.Color.White;
                }
                else if (matrix[i] == 7)
                {
                    this.Controls[i].BackColor = System.Drawing.Color.Orange;
                }
                else if (matrix[i] == 0)
                {
                    if (i < 20 || i >= 220 )
                    {
                        this.Controls[i].BackColor = (System.Drawing.Color)Constants.COLOR_BACKGROUND;
                    }
                    else
                    {
                        this.Controls[i].BackColor = (System.Drawing.Color)Constants.COLOR_GRID;
                    }
                    
                }
            }
        }
    }
}

