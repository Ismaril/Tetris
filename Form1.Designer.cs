using System.Collections.Generic;
using System.Drawing;

namespace Tetris
{
    partial class Form1
    {
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelLevel;
        private uint score;
        private uint level;

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
                    this.pictureBox.Location = new System.Drawing.Point(Constants.CENTRE_OF_SCREEN_OFFSET + j * 44, i * 44);
                    this.pictureBox.Name = $"pictureBox{counter}";
                    this.pictureBox.Size = new System.Drawing.Size(40, 40);
                    this.pictureBox.TabIndex = 0;
                    this.pictureBox.TabStop = false;
                    this.Controls.Add(this.pictureBox);
                    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
                    counter++;
                }
            }

            // Create a GUI matrix which will display next tetrominoCurrent.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.pictureBox = new System.Windows.Forms.PictureBox();
                    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
                    //this.SuspendLayout();
                    this.pictureBox.Location = new System.Drawing.Point(1200+j * 44, 150+i * 44);
                    this.pictureBox.Name = $"pictureBox{counter}";
                    this.pictureBox.Size = new System.Drawing.Size(40, 40);
                    this.pictureBox.BackgroundImage = Constants.OFFGRID_COLOR;
                    this.pictureBox.TabIndex = 0;
                    this.pictureBox.TabStop = false;
                    this.Controls.Add(this.pictureBox);
                    ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
                    counter++;
                }
            }

            // Score
            score = 0;
            labelScore = new System.Windows.Forms.Label();
            labelScore.Text = $"SCORE\n{score}";
            labelScore.Font = new Font("Bauhaus 93", 50);
            labelScore.Location = new System.Drawing.Point(1200, 350);
            labelScore.Name = "textBoxScore";
            labelScore.Size = new System.Drawing.Size(233, 150);
            labelScore.BackColor = System.Drawing.Color.AliceBlue;//(System.Drawing.Color)Constants.COLOR_BACKGROUND;
            Controls.Add(labelScore);

            // Level
            level = 0;
            labelLevel = new System.Windows.Forms.Label();
            labelLevel.Text = $"LEVEL\n{level}";
            labelLevel.Font = new Font("Bauhaus 93", 50);
            labelLevel.Location = new System.Drawing.Point(1200, 650);
            labelLevel.Name = "textBoxScore";
            labelLevel.Size = new System.Drawing.Size(233, 150);
            labelLevel.BackColor = System.Drawing.Color.AliceBlue;//(System.Drawing.Color)Constants.COLOR_BACKGROUND;
            Controls.Add(labelLevel);

            // 
            // Form1 (Main frame)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(Constants.WIDTH_OF_APPLICATION_WINDOW, Constants.HEIGHT_OF_APPLICATION_WINDOW);

            this.Name = "Tetris";
            this.Text = "Tetris";
            this.BackColor = (System.Drawing.Color)Constants.COLOR_BACKGROUND;
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);


        }
        #endregion

        /// <summary>
        /// Redraw GUI based on numbers at game grid/matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Redraw(List<byte> matrix)
        {
            for(byte i = 0; i < matrix.Count; i++)
            {
                switch (matrix[i])
                {
                    case 0:
                        if (i < 20 || i >= 220)
                        {
                            this.Controls[i].BackgroundImage = Constants.OFFGRID_COLOR;
                        }
                        else
                        {
                            this.Controls[i].BackgroundImage = Constants.GRID_COLOR;
                        }
                        break;
                    case 1:
                        this.Controls[i].BackgroundImage = Constants.STYLE1_COLOR1;
                        break;
                    case 2:
                        this.Controls[i].BackgroundImage = Constants.STYLE1_COLOR2;
                        break;
                    case 3:
                        this.Controls[i].BackgroundImage = Constants.STYLE1_COLOR3;
                        break;
                }
            }

            for (int i = 0; i < 16; i++)
            {
                Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = Constants.OFFGRID_COLOR;
                switch (logic.TetrominoNext[i])
                {
                    case 1:
                        this.Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = Constants.STYLE1_COLOR1;
                        break;
                    case 2:
                        this.Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = Constants.STYLE1_COLOR2;
                        break;
                    case 3:
                        this.Controls[Constants.WIDTH_OF_GRID * Constants.HEIGHT_OF_GRID + i].BackgroundImage = Constants.STYLE1_COLOR3;
                        break;
                }
            }
        }
    }
}

