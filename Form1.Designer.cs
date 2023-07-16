using System;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

namespace Tetris
{
    partial class Form1
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
            int counter = 0;
            for (int i = 0; i < Constants.HEIGHT_OF_GRID; i++)
            {
                for (int j = 0; j < Constants.WIDTH_OF_GRID; j++)
                {
                    this.pictureBox = new System.Windows.Forms.PictureBox();
                    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
                    this.SuspendLayout();
                    this.pictureBox.BackColor = System.Drawing.Color.Red;
                    this.pictureBox.Location = new System.Drawing.Point(j * 22, i * 22);
                    this.pictureBox.Name = $"pictureBox{counter}";
                    this.pictureBox.Size = new System.Drawing.Size(20, 20);
                    this.pictureBox.TabIndex = 0;
                    this.pictureBox.TabStop = false;
                    this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
                    this.Controls.Add(this.pictureBox);
                    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
                    counter++;
                }

            }
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 600);

            this.Name = "Form1";
            this.Text = "Form1";
            this.BackColor = System.Drawing.Color.Black;
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.PictureBox pictureBox;


        public void Redraw(int[] tetrominoIndexes)
        {
            foreach (int i in tetrominoIndexes)
            {
                this.Controls[i].BackColor = System.Drawing.Color.Green;
            }
        }

        public void RedrawBack(int[] tetrominoIndexes)
        {
            foreach (int i in tetrominoIndexes)
            {
                this.Controls[i].BackColor = System.Drawing.Color.Red;

            }
        }
    }
}

