﻿using System.Windows.Forms;

namespace Tetris
{
    partial class Form1
    {

        // Application window size
        public const int WIDTH_OF_APPLICATION_WINDOW = 1920;
        public const int HEIGHT_OF_APPLICATION_WINDOW = 1080;

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

            // Enable double buffering and other optimizations
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint, true);

            // 
            // Form1 (Main frame)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(WIDTH_OF_APPLICATION_WINDOW, HEIGHT_OF_APPLICATION_WINDOW);
            this.Name = "Tetris";
            this.Text = "Tetris";
            this.BackColor = (System.Drawing.Color)Consts.COLOR_BLACK;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }
        #endregion
    }
}

