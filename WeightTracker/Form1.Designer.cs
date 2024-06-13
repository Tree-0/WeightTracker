﻿namespace WeightTracker
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
            this.submitButton = new System.Windows.Forms.Button();
            this.graphButton = new System.Windows.Forms.Button();
            this.exerciseBox = new System.Windows.Forms.ComboBox();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.exerciseLabel = new System.Windows.Forms.Label();
            this.repWeightGrid = new System.Windows.Forms.DataGridView();
            this.RepsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WeightCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addSetButton = new System.Windows.Forms.Button();
            this.addExerciseButton = new System.Windows.Forms.Button();
            this.graphListBox = new System.Windows.Forms.ListBox();
            this.infoBox1 = new System.Windows.Forms.TextBox();
            this.infoBox2 = new System.Windows.Forms.TextBox();
            this.exerciseInfoBox = new System.Windows.Forms.TextBox();
            this.exerciseInfoBoxLabel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.repWeightGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(550, 200);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 55);
            this.submitButton.TabIndex = 0;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // graphButton
            // 
            this.graphButton.Location = new System.Drawing.Point(30, 466);
            this.graphButton.Name = "graphButton";
            this.graphButton.Size = new System.Drawing.Size(191, 53);
            this.graphButton.TabIndex = 1;
            this.graphButton.Text = "Graph";
            this.graphButton.UseVisualStyleBackColor = true;
            this.graphButton.Click += new System.EventHandler(this.graphButton_Click);
            // 
            // exerciseBox
            // 
            this.exerciseBox.FormattingEnabled = true;
            this.exerciseBox.Items.AddRange(new object[] {
            "Bench Press",
            "Chest Flys",
            "Bicep Curls",
            "Tricep Extensions",
            "Incline Bench",
            "Shoulder Press",
            "Shoulder Raises",
            "Lat Pulldown",
            "Lat Rows",
            "Deadlift",
            "Squat",
            "Leg Extension",
            "Hamstring Curls",
            "Abductor (out)",
            "Adductor (in)",
            "Shoulder Shrugs ",
            "Calf Raises",
            "Pull-ups ",
            "Push-ups",
            "Dips",
            "Sit-ups",
            "Crunch Machine"});
            this.exerciseBox.Location = new System.Drawing.Point(33, 58);
            this.exerciseBox.Name = "exerciseBox";
            this.exerciseBox.Size = new System.Drawing.Size(156, 21);
            this.exerciseBox.TabIndex = 2;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(30, 93);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 5;
            // 
            // exerciseLabel
            // 
            this.exerciseLabel.AutoSize = true;
            this.exerciseLabel.Location = new System.Drawing.Point(30, 42);
            this.exerciseLabel.Name = "exerciseLabel";
            this.exerciseLabel.Size = new System.Drawing.Size(53, 13);
            this.exerciseLabel.TabIndex = 6;
            this.exerciseLabel.Text = "Exercise: ";
            // 
            // repWeightGrid
            // 
            this.repWeightGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.repWeightGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RepsCol,
            this.WeightCol});
            this.repWeightGrid.Location = new System.Drawing.Point(285, 58);
            this.repWeightGrid.Name = "repWeightGrid";
            this.repWeightGrid.Size = new System.Drawing.Size(244, 195);
            this.repWeightGrid.TabIndex = 10;
            // 
            // RepsCol
            // 
            this.RepsCol.HeaderText = "Reps";
            this.RepsCol.Name = "RepsCol";
            // 
            // WeightCol
            // 
            this.WeightCol.HeaderText = "Weight";
            this.WeightCol.Name = "WeightCol";
            // 
            // addSetButton
            // 
            this.addSetButton.Location = new System.Drawing.Point(196, 58);
            this.addSetButton.Name = "addSetButton";
            this.addSetButton.Size = new System.Drawing.Size(61, 23);
            this.addSetButton.TabIndex = 11;
            this.addSetButton.Text = "add set";
            this.addSetButton.UseVisualStyleBackColor = true;
            this.addSetButton.Visible = false;
            this.addSetButton.Click += new System.EventHandler(this.addSetButton_Click);
            // 
            // addExerciseButton
            // 
            this.addExerciseButton.Location = new System.Drawing.Point(550, 121);
            this.addExerciseButton.Name = "addExerciseButton";
            this.addExerciseButton.Size = new System.Drawing.Size(75, 55);
            this.addExerciseButton.TabIndex = 12;
            this.addExerciseButton.Text = "add exercise";
            this.addExerciseButton.UseVisualStyleBackColor = true;
            this.addExerciseButton.Click += new System.EventHandler(this.addExerciseButton_Click);
            // 
            // graphListBox
            // 
            this.graphListBox.FormattingEnabled = true;
            this.graphListBox.Items.AddRange(new object[] {
            "Bench Press",
            "Chest Flys",
            "Bicep Curls",
            "Tricep Extensions",
            "Incline Bench",
            "Shoulder Press",
            "Shoulder Raises",
            "Lat Pulldown",
            "Lat Rows",
            "Deadlift",
            "Squat",
            "Leg Extension",
            "Hamstring Curls",
            "Abductor (out)",
            "Adductor (in)",
            "Shoulder Shrugs ",
            "Calf Raises",
            "Pull-ups ",
            "Push-ups",
            "Dips",
            "Sit-ups",
            "Crunch Machine"});
            this.graphListBox.Location = new System.Drawing.Point(33, 326);
            this.graphListBox.Name = "graphListBox";
            this.graphListBox.Size = new System.Drawing.Size(188, 134);
            this.graphListBox.TabIndex = 14;
            // 
            // infoBox1
            // 
            this.infoBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.infoBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoBox1.Location = new System.Drawing.Point(550, 60);
            this.infoBox1.Multiline = true;
            this.infoBox1.Name = "infoBox1";
            this.infoBox1.ReadOnly = true;
            this.infoBox1.Size = new System.Drawing.Size(223, 49);
            this.infoBox1.TabIndex = 15;
            this.infoBox1.Text = "Fill in all data before clicking \"add exercise\". Add all exercises before clickin" +
    "g \"submit\".";
            this.infoBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // infoBox2
            // 
            this.infoBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.infoBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoBox2.Location = new System.Drawing.Point(33, 300);
            this.infoBox2.Name = "infoBox2";
            this.infoBox2.ReadOnly = true;
            this.infoBox2.Size = new System.Drawing.Size(188, 20);
            this.infoBox2.TabIndex = 16;
            this.infoBox2.Text = "Select exercise to graph: ";
            this.infoBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // exerciseInfoBox
            // 
            this.exerciseInfoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exerciseInfoBox.Location = new System.Drawing.Point(632, 147);
            this.exerciseInfoBox.Multiline = true;
            this.exerciseInfoBox.Name = "exerciseInfoBox";
            this.exerciseInfoBox.ReadOnly = true;
            this.exerciseInfoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.exerciseInfoBox.Size = new System.Drawing.Size(141, 106);
            this.exerciseInfoBox.TabIndex = 17;
            // 
            // exerciseInfoBoxLabel
            // 
            this.exerciseInfoBoxLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.exerciseInfoBoxLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exerciseInfoBoxLabel.Location = new System.Drawing.Point(632, 121);
            this.exerciseInfoBoxLabel.Name = "exerciseInfoBoxLabel";
            this.exerciseInfoBoxLabel.ReadOnly = true;
            this.exerciseInfoBoxLabel.Size = new System.Drawing.Size(141, 20);
            this.exerciseInfoBoxLabel.TabIndex = 18;
            this.exerciseInfoBoxLabel.Text = "Current workout: ";
            this.exerciseInfoBoxLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 573);
            this.Controls.Add(this.exerciseInfoBoxLabel);
            this.Controls.Add(this.exerciseInfoBox);
            this.Controls.Add(this.infoBox2);
            this.Controls.Add(this.infoBox1);
            this.Controls.Add(this.graphListBox);
            this.Controls.Add(this.addExerciseButton);
            this.Controls.Add(this.addSetButton);
            this.Controls.Add(this.repWeightGrid);
            this.Controls.Add(this.exerciseLabel);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.exerciseBox);
            this.Controls.Add(this.graphButton);
            this.Controls.Add(this.submitButton);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workout Tracker";
            //this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repWeightGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button graphButton;
        private System.Windows.Forms.ComboBox exerciseBox;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Label exerciseLabel;
        private System.Windows.Forms.DataGridView repWeightGrid;
        private System.Windows.Forms.Button addSetButton;
        private System.Windows.Forms.Button addExerciseButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn RepsCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WeightCol;
        private System.Windows.Forms.ListBox graphListBox;
        private System.Windows.Forms.TextBox infoBox1;
        private System.Windows.Forms.TextBox infoBox2;
        private System.Windows.Forms.TextBox exerciseInfoBox;
        private System.Windows.Forms.TextBox exerciseInfoBoxLabel;
    }
}

