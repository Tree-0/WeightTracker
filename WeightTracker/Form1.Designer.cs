namespace WeightTracker
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
            ((System.ComponentModel.ISupportInitialize)(this.repWeightGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(550, 201);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 0;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // graphButton
            // 
            this.graphButton.Location = new System.Drawing.Point(550, 230);
            this.graphButton.Name = "graphButton";
            this.graphButton.Size = new System.Drawing.Size(75, 23);
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
            "Squat",
            "Deadlift",
            "Lat Pulldown",
            "Bicep Curls",
            "Tricep Extensions",
            "Shoulder Press",
            "Lat Rows",
            "Leg Extension",
            "Hamstring Curls",
            "Shoulder Shrugs ",
            "Calf Raises",
            "Pull-ups ",
            "Push-ups",
            "Shoulder Raises"});
            this.exerciseBox.Location = new System.Drawing.Point(33, 58);
            this.exerciseBox.Name = "exerciseBox";
            this.exerciseBox.Size = new System.Drawing.Size(156, 21);
            this.exerciseBox.TabIndex = 2;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(30, 94);
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
            this.repWeightGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.repWeightGrid_CellContentClick);
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
            this.addSetButton.Size = new System.Drawing.Size(75, 23);
            this.addSetButton.TabIndex = 11;
            this.addSetButton.Text = "add set";
            this.addSetButton.UseVisualStyleBackColor = true;
            this.addSetButton.Click += new System.EventHandler(this.addSetButton_Click);
            // 
            // addExerciseButton
            // 
            this.addExerciseButton.Location = new System.Drawing.Point(550, 58);
            this.addExerciseButton.Name = "addExerciseButton";
            this.addExerciseButton.Size = new System.Drawing.Size(75, 42);
            this.addExerciseButton.TabIndex = 12;
            this.addExerciseButton.Text = "add exercise";
            this.addExerciseButton.UseVisualStyleBackColor = true;
            this.addExerciseButton.Click += new System.EventHandler(this.addExerciseButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 486);
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
            this.Text = "Form1";
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
    }
}

