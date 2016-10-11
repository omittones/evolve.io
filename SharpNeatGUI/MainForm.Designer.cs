namespace SharpNeatGUI
{
    partial class MainForm
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
            System.Windows.Forms.GroupBox gbxGenomePopulation;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.GroupBox gbxCurrentStats;
            System.Windows.Forms.Label label17;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label38;
            System.Windows.Forms.Label label20;
            System.Windows.Forms.Label label19;
            System.Windows.Forms.Label label18;
            System.Windows.Forms.Label label27;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.GroupBox groupBox6;
            System.Windows.Forms.GroupBox groupBox5;
            System.Windows.Forms.GroupBox gbxNeatGenomeParameters;
            System.Windows.Forms.Label label16;
            System.Windows.Forms.Label label42;
            System.Windows.Forms.Label label34;
            System.Windows.Forms.Label label36;
            System.Windows.Forms.Label label35;
            System.Windows.Forms.GroupBox gbxEAParameters;
            System.Windows.Forms.Label label21;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label32;
            System.Windows.Forms.Label label14;
            System.Windows.Forms.Label label25;
            System.Windows.Forms.Label label26;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtParamInitialConnectionProportion = new System.Windows.Forms.TextBox();
            this.txtParamPopulationSize = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.txtPopulationStatus = new System.Windows.Forms.TextBox();
            this.btnCreateRandomPop = new System.Windows.Forms.Button();
            this.txtStatsInterspeciesOffspringCount = new System.Windows.Forms.TextBox();
            this.txtStatsAlternativeFitness = new System.Windows.Forms.TextBox();
            this.txtStatsCrossoverOffspringCount = new System.Windows.Forms.TextBox();
            this.txtStatsAsexualOffspringCount = new System.Windows.Forms.TextBox();
            this.txtStatsTotalOffspringCount = new System.Windows.Forms.TextBox();
            this.txtStatsMaxGenomeComplx = new System.Windows.Forms.TextBox();
            this.txtSpecieChampMean = new System.Windows.Forms.TextBox();
            this.txtSearchStatsMode = new System.Windows.Forms.TextBox();
            this.txtStatsEvalsPerSec = new System.Windows.Forms.TextBox();
            this.txtStatsMeanGenomeComplx = new System.Windows.Forms.TextBox();
            this.txtStatsBestGenomeComplx = new System.Windows.Forms.TextBox();
            this.txtStatsTotalEvals = new System.Windows.Forms.TextBox();
            this.txtStatsGeneration = new System.Windows.Forms.TextBox();
            this.txtStatsMean = new System.Windows.Forms.TextBox();
            this.txtStatsBest = new System.Windows.Forms.TextBox();
            this.btnSearchReset = new System.Windows.Forms.Button();
            this.btnSearchStop = new System.Windows.Forms.Button();
            this.btnSearchStart = new System.Windows.Forms.Button();
            this.btnLoadDomainDefaults = new System.Windows.Forms.Button();
            this.btnExperimentInfo = new System.Windows.Forms.Button();
            this.cmbExperiments = new System.Windows.Forms.ComboBox();
            this.txtParamConnectionWeightRange = new System.Windows.Forms.TextBox();
            this.txtParamMutateConnectionWeights = new System.Windows.Forms.TextBox();
            this.txtParamMutateDeleteConnection = new System.Windows.Forms.TextBox();
            this.txtParamMutateAddConnection = new System.Windows.Forms.TextBox();
            this.txtParamMutateAddNode = new System.Windows.Forms.TextBox();
            this.txtParamNumberOfSpecies = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtParamSelectionProportion = new System.Windows.Forms.TextBox();
            this.txtParamInterspeciesMating = new System.Windows.Forms.TextBox();
            this.txtParamElitismProportion = new System.Windows.Forms.TextBox();
            this.txtParamOffspringCrossover = new System.Windows.Forms.TextBox();
            this.txtParamOffspringAsexual = new System.Windows.Forms.TextBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.previewContainer = new System.Windows.Forms.GroupBox();
            this.solutionContainer = new System.Windows.Forms.GroupBox();
            this.gbxLogging = new System.Windows.Forms.GroupBox();
            this.txtFileLogBaseName = new System.Windows.Forms.TextBox();
            this.chkFileWriteLog = new System.Windows.Forms.CheckBox();
            this.txtFileBaseName = new System.Windows.Forms.TextBox();
            this.chkFileSaveGenomeOnImprovement = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPopulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSeedGenomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSeedGenomesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.savePopulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBestGenomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rankPlotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specieSizeByRankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specieChampFitnessByRankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specieChampComplexityByRankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.genomeFitnessByRankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genomeComplexityByRankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distributionPlotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specieSizeDistributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specieFitnessDistributionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specieComplexityDistributionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.genomeFitnessDistributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genomeComplexityDistributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitnessBestMeansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.complexityBestMeansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evaluationsPerSecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCopyLogToClipboard = new System.Windows.Forms.Button();
            this.lbxLog = new System.Windows.Forms.ListBox();
            this.populationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gbxGenomePopulation = new System.Windows.Forms.GroupBox();
            label13 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            gbxCurrentStats = new System.Windows.Forms.GroupBox();
            label17 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label38 = new System.Windows.Forms.Label();
            label20 = new System.Windows.Forms.Label();
            label19 = new System.Windows.Forms.Label();
            label18 = new System.Windows.Forms.Label();
            label27 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            groupBox6 = new System.Windows.Forms.GroupBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            gbxNeatGenomeParameters = new System.Windows.Forms.GroupBox();
            label16 = new System.Windows.Forms.Label();
            label42 = new System.Windows.Forms.Label();
            label34 = new System.Windows.Forms.Label();
            label36 = new System.Windows.Forms.Label();
            label35 = new System.Windows.Forms.Label();
            gbxEAParameters = new System.Windows.Forms.GroupBox();
            label21 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label32 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            label25 = new System.Windows.Forms.Label();
            label26 = new System.Windows.Forms.Label();
            gbxGenomePopulation.SuspendLayout();
            gbxCurrentStats.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            gbxNeatGenomeParameters.SuspendLayout();
            gbxEAParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbxLogging.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxGenomePopulation
            // 
            gbxGenomePopulation.Controls.Add(this.label1);
            gbxGenomePopulation.Controls.Add(this.txtParamInitialConnectionProportion);
            gbxGenomePopulation.Controls.Add(this.txtParamPopulationSize);
            gbxGenomePopulation.Controls.Add(this.label28);
            gbxGenomePopulation.Controls.Add(this.txtPopulationStatus);
            gbxGenomePopulation.Controls.Add(this.btnCreateRandomPop);
            gbxGenomePopulation.Location = new System.Drawing.Point(21, 193);
            gbxGenomePopulation.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxGenomePopulation.Name = "gbxGenomePopulation";
            gbxGenomePopulation.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxGenomePopulation.Size = new System.Drawing.Size(624, 327);
            gbxGenomePopulation.TabIndex = 21;
            gbxGenomePopulation.TabStop = false;
            gbxGenomePopulation.Text = "Genome Population";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(240, 258);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(384, 48);
            this.label1.TabIndex = 54;
            this.label1.Text = "Initial Connections Proportion";
            // 
            // txtParamInitialConnectionProportion
            // 
            this.txtParamInitialConnectionProportion.Location = new System.Drawing.Point(21, 250);
            this.txtParamInitialConnectionProportion.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamInitialConnectionProportion.Name = "txtParamInitialConnectionProportion";
            this.txtParamInitialConnectionProportion.Size = new System.Drawing.Size(207, 38);
            this.txtParamInitialConnectionProportion.TabIndex = 53;
            this.txtParamInitialConnectionProportion.Text = "0.1";
            // 
            // txtParamPopulationSize
            // 
            this.txtParamPopulationSize.Location = new System.Drawing.Point(21, 188);
            this.txtParamPopulationSize.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamPopulationSize.Name = "txtParamPopulationSize";
            this.txtParamPopulationSize.Size = new System.Drawing.Size(207, 38);
            this.txtParamPopulationSize.TabIndex = 51;
            this.txtParamPopulationSize.Text = "150";
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(240, 196);
            this.label28.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(301, 38);
            this.label28.TabIndex = 52;
            this.label28.Text = "Population Size";
            // 
            // txtPopulationStatus
            // 
            this.txtPopulationStatus.BackColor = System.Drawing.Color.Red;
            this.txtPopulationStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPopulationStatus.ForeColor = System.Drawing.Color.Black;
            this.txtPopulationStatus.Location = new System.Drawing.Point(21, 45);
            this.txtPopulationStatus.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtPopulationStatus.Name = "txtPopulationStatus";
            this.txtPopulationStatus.ReadOnly = true;
            this.txtPopulationStatus.Size = new System.Drawing.Size(577, 39);
            this.txtPopulationStatus.TabIndex = 50;
            this.txtPopulationStatus.TabStop = false;
            this.txtPopulationStatus.Text = "Population not initialized";
            this.txtPopulationStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCreateRandomPop
            // 
            this.btnCreateRandomPop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateRandomPop.Location = new System.Drawing.Point(19, 107);
            this.btnCreateRandomPop.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnCreateRandomPop.Name = "btnCreateRandomPop";
            this.btnCreateRandomPop.Size = new System.Drawing.Size(587, 57);
            this.btnCreateRandomPop.TabIndex = 49;
            this.btnCreateRandomPop.Text = "Create Random Population";
            this.btnCreateRandomPop.Click += new System.EventHandler(this.btnCreateRandomPop_Click);
            // 
            // label13
            // 
            label13.Location = new System.Drawing.Point(483, 81);
            label13.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(160, 76);
            label13.TabIndex = 23;
            label13.Text = "Filename prefix";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(488, 193);
            label8.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(160, 76);
            label8.TabIndex = 28;
            label8.Text = "Filename prefix";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbxCurrentStats
            // 
            gbxCurrentStats.Controls.Add(label17);
            gbxCurrentStats.Controls.Add(label12);
            gbxCurrentStats.Controls.Add(this.txtStatsInterspeciesOffspringCount);
            gbxCurrentStats.Controls.Add(this.txtStatsAlternativeFitness);
            gbxCurrentStats.Controls.Add(label11);
            gbxCurrentStats.Controls.Add(this.txtStatsCrossoverOffspringCount);
            gbxCurrentStats.Controls.Add(label10);
            gbxCurrentStats.Controls.Add(this.txtStatsAsexualOffspringCount);
            gbxCurrentStats.Controls.Add(label9);
            gbxCurrentStats.Controls.Add(this.txtStatsTotalOffspringCount);
            gbxCurrentStats.Controls.Add(label7);
            gbxCurrentStats.Controls.Add(this.txtStatsMaxGenomeComplx);
            gbxCurrentStats.Controls.Add(label6);
            gbxCurrentStats.Controls.Add(this.txtSpecieChampMean);
            gbxCurrentStats.Controls.Add(label38);
            gbxCurrentStats.Controls.Add(this.txtSearchStatsMode);
            gbxCurrentStats.Controls.Add(this.txtStatsEvalsPerSec);
            gbxCurrentStats.Controls.Add(label20);
            gbxCurrentStats.Controls.Add(label19);
            gbxCurrentStats.Controls.Add(label18);
            gbxCurrentStats.Controls.Add(this.txtStatsMeanGenomeComplx);
            gbxCurrentStats.Controls.Add(this.txtStatsBestGenomeComplx);
            gbxCurrentStats.Controls.Add(this.txtStatsTotalEvals);
            gbxCurrentStats.Controls.Add(label27);
            gbxCurrentStats.Controls.Add(label5);
            gbxCurrentStats.Controls.Add(this.txtStatsGeneration);
            gbxCurrentStats.Controls.Add(label3);
            gbxCurrentStats.Controls.Add(label2);
            gbxCurrentStats.Controls.Add(this.txtStatsMean);
            gbxCurrentStats.Controls.Add(this.txtStatsBest);
            gbxCurrentStats.Location = new System.Drawing.Point(661, 14);
            gbxCurrentStats.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxCurrentStats.Name = "gbxCurrentStats";
            gbxCurrentStats.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxCurrentStats.Size = new System.Drawing.Size(691, 980);
            gbxCurrentStats.TabIndex = 19;
            gbxCurrentStats.TabStop = false;
            gbxCurrentStats.Text = "Current Stats";
            // 
            // label17
            // 
            label17.Location = new System.Drawing.Point(277, 241);
            label17.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(376, 38);
            label17.TabIndex = 35;
            label17.Text = "Alternative Best Fitness";
            // 
            // label12
            // 
            label12.Location = new System.Drawing.Point(277, 925);
            label12.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(427, 38);
            label12.TabIndex = 33;
            label12.Text = "Interspecies Offspring Count";
            // 
            // txtStatsInterspeciesOffspringCount
            // 
            this.txtStatsInterspeciesOffspringCount.Location = new System.Drawing.Point(16, 916);
            this.txtStatsInterspeciesOffspringCount.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsInterspeciesOffspringCount.Name = "txtStatsInterspeciesOffspringCount";
            this.txtStatsInterspeciesOffspringCount.ReadOnly = true;
            this.txtStatsInterspeciesOffspringCount.Size = new System.Drawing.Size(249, 38);
            this.txtStatsInterspeciesOffspringCount.TabIndex = 32;
            this.txtStatsInterspeciesOffspringCount.TabStop = false;
            // 
            // txtStatsAlternativeFitness
            // 
            this.txtStatsAlternativeFitness.Location = new System.Drawing.Point(16, 231);
            this.txtStatsAlternativeFitness.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsAlternativeFitness.Name = "txtStatsAlternativeFitness";
            this.txtStatsAlternativeFitness.ReadOnly = true;
            this.txtStatsAlternativeFitness.Size = new System.Drawing.Size(249, 38);
            this.txtStatsAlternativeFitness.TabIndex = 34;
            this.txtStatsAlternativeFitness.TabStop = false;
            // 
            // label11
            // 
            label11.Location = new System.Drawing.Point(277, 863);
            label11.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(365, 38);
            label11.TabIndex = 31;
            label11.Text = "Crossover Offspring Count";
            // 
            // txtStatsCrossoverOffspringCount
            // 
            this.txtStatsCrossoverOffspringCount.Location = new System.Drawing.Point(16, 854);
            this.txtStatsCrossoverOffspringCount.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsCrossoverOffspringCount.Name = "txtStatsCrossoverOffspringCount";
            this.txtStatsCrossoverOffspringCount.ReadOnly = true;
            this.txtStatsCrossoverOffspringCount.Size = new System.Drawing.Size(249, 38);
            this.txtStatsCrossoverOffspringCount.TabIndex = 30;
            this.txtStatsCrossoverOffspringCount.TabStop = false;
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(277, 801);
            label10.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(365, 38);
            label10.TabIndex = 29;
            label10.Text = "Asexual Offspring Count";
            // 
            // txtStatsAsexualOffspringCount
            // 
            this.txtStatsAsexualOffspringCount.Location = new System.Drawing.Point(16, 792);
            this.txtStatsAsexualOffspringCount.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsAsexualOffspringCount.Name = "txtStatsAsexualOffspringCount";
            this.txtStatsAsexualOffspringCount.ReadOnly = true;
            this.txtStatsAsexualOffspringCount.Size = new System.Drawing.Size(249, 38);
            this.txtStatsAsexualOffspringCount.TabIndex = 28;
            this.txtStatsAsexualOffspringCount.TabStop = false;
            // 
            // label9
            // 
            label9.Location = new System.Drawing.Point(277, 739);
            label9.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(365, 38);
            label9.TabIndex = 27;
            label9.Text = "Total Offspring Count";
            // 
            // txtStatsTotalOffspringCount
            // 
            this.txtStatsTotalOffspringCount.Location = new System.Drawing.Point(16, 730);
            this.txtStatsTotalOffspringCount.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsTotalOffspringCount.Name = "txtStatsTotalOffspringCount";
            this.txtStatsTotalOffspringCount.ReadOnly = true;
            this.txtStatsTotalOffspringCount.Size = new System.Drawing.Size(249, 38);
            this.txtStatsTotalOffspringCount.TabIndex = 26;
            this.txtStatsTotalOffspringCount.TabStop = false;
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(277, 677);
            label7.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(365, 38);
            label7.TabIndex = 25;
            label7.Text = "Max Genome Complexity";
            // 
            // txtStatsMaxGenomeComplx
            // 
            this.txtStatsMaxGenomeComplx.Location = new System.Drawing.Point(16, 668);
            this.txtStatsMaxGenomeComplx.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsMaxGenomeComplx.Name = "txtStatsMaxGenomeComplx";
            this.txtStatsMaxGenomeComplx.ReadOnly = true;
            this.txtStatsMaxGenomeComplx.Size = new System.Drawing.Size(249, 38);
            this.txtStatsMaxGenomeComplx.TabIndex = 24;
            this.txtStatsMaxGenomeComplx.TabStop = false;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(277, 348);
            label6.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(277, 72);
            label6.TabIndex = 23;
            label6.Text = "Mean Fitness (specie champs)";
            // 
            // txtSpecieChampMean
            // 
            this.txtSpecieChampMean.Location = new System.Drawing.Point(16, 355);
            this.txtSpecieChampMean.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtSpecieChampMean.Name = "txtSpecieChampMean";
            this.txtSpecieChampMean.ReadOnly = true;
            this.txtSpecieChampMean.Size = new System.Drawing.Size(249, 38);
            this.txtSpecieChampMean.TabIndex = 22;
            this.txtSpecieChampMean.TabStop = false;
            // 
            // label38
            // 
            label38.Location = new System.Drawing.Point(277, 55);
            label38.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label38.Name = "label38";
            label38.Size = new System.Drawing.Size(299, 38);
            label38.TabIndex = 21;
            label38.Text = "Current Search Mode";
            // 
            // txtSearchStatsMode
            // 
            this.txtSearchStatsMode.BackColor = System.Drawing.Color.LightSkyBlue;
            this.txtSearchStatsMode.Location = new System.Drawing.Point(16, 45);
            this.txtSearchStatsMode.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtSearchStatsMode.Name = "txtSearchStatsMode";
            this.txtSearchStatsMode.ReadOnly = true;
            this.txtSearchStatsMode.Size = new System.Drawing.Size(249, 38);
            this.txtSearchStatsMode.TabIndex = 20;
            this.txtSearchStatsMode.TabStop = false;
            // 
            // txtStatsEvalsPerSec
            // 
            this.txtStatsEvalsPerSec.Location = new System.Drawing.Point(16, 482);
            this.txtStatsEvalsPerSec.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsEvalsPerSec.Name = "txtStatsEvalsPerSec";
            this.txtStatsEvalsPerSec.ReadOnly = true;
            this.txtStatsEvalsPerSec.Size = new System.Drawing.Size(249, 38);
            this.txtStatsEvalsPerSec.TabIndex = 18;
            this.txtStatsEvalsPerSec.TabStop = false;
            // 
            // label20
            // 
            label20.Location = new System.Drawing.Point(277, 489);
            label20.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(277, 38);
            label20.TabIndex = 19;
            label20.Text = "Evaluations / Sec";
            // 
            // label19
            // 
            label19.Location = new System.Drawing.Point(277, 615);
            label19.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(365, 38);
            label19.TabIndex = 17;
            label19.Text = "Mean Genome Complexity";
            // 
            // label18
            // 
            label18.Location = new System.Drawing.Point(277, 553);
            label18.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(365, 38);
            label18.TabIndex = 16;
            label18.Text = "Best Genome\'s Complexity";
            // 
            // txtStatsMeanGenomeComplx
            // 
            this.txtStatsMeanGenomeComplx.Location = new System.Drawing.Point(16, 606);
            this.txtStatsMeanGenomeComplx.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsMeanGenomeComplx.Name = "txtStatsMeanGenomeComplx";
            this.txtStatsMeanGenomeComplx.ReadOnly = true;
            this.txtStatsMeanGenomeComplx.Size = new System.Drawing.Size(249, 38);
            this.txtStatsMeanGenomeComplx.TabIndex = 15;
            this.txtStatsMeanGenomeComplx.TabStop = false;
            // 
            // txtStatsBestGenomeComplx
            // 
            this.txtStatsBestGenomeComplx.Location = new System.Drawing.Point(16, 544);
            this.txtStatsBestGenomeComplx.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsBestGenomeComplx.Name = "txtStatsBestGenomeComplx";
            this.txtStatsBestGenomeComplx.ReadOnly = true;
            this.txtStatsBestGenomeComplx.Size = new System.Drawing.Size(249, 38);
            this.txtStatsBestGenomeComplx.TabIndex = 14;
            this.txtStatsBestGenomeComplx.TabStop = false;
            // 
            // txtStatsTotalEvals
            // 
            this.txtStatsTotalEvals.Location = new System.Drawing.Point(16, 420);
            this.txtStatsTotalEvals.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsTotalEvals.Name = "txtStatsTotalEvals";
            this.txtStatsTotalEvals.ReadOnly = true;
            this.txtStatsTotalEvals.Size = new System.Drawing.Size(249, 38);
            this.txtStatsTotalEvals.TabIndex = 12;
            this.txtStatsTotalEvals.TabStop = false;
            // 
            // label27
            // 
            label27.Location = new System.Drawing.Point(277, 429);
            label27.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(277, 38);
            label27.TabIndex = 13;
            label27.Text = "Total Evaluations";
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(277, 117);
            label5.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(171, 38);
            label5.TabIndex = 7;
            label5.Text = "Generation";
            // 
            // txtStatsGeneration
            // 
            this.txtStatsGeneration.Location = new System.Drawing.Point(16, 110);
            this.txtStatsGeneration.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsGeneration.Name = "txtStatsGeneration";
            this.txtStatsGeneration.ReadOnly = true;
            this.txtStatsGeneration.Size = new System.Drawing.Size(249, 38);
            this.txtStatsGeneration.TabIndex = 6;
            this.txtStatsGeneration.TabStop = false;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(277, 300);
            label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(219, 38);
            label3.TabIndex = 4;
            label3.Text = "Mean Fitness";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(277, 179);
            label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(277, 38);
            label2.TabIndex = 3;
            label2.Text = "Best Fitness";
            // 
            // txtStatsMean
            // 
            this.txtStatsMean.Location = new System.Drawing.Point(16, 293);
            this.txtStatsMean.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsMean.Name = "txtStatsMean";
            this.txtStatsMean.ReadOnly = true;
            this.txtStatsMean.Size = new System.Drawing.Size(249, 38);
            this.txtStatsMean.TabIndex = 1;
            this.txtStatsMean.TabStop = false;
            // 
            // txtStatsBest
            // 
            this.txtStatsBest.Location = new System.Drawing.Point(16, 172);
            this.txtStatsBest.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtStatsBest.Name = "txtStatsBest";
            this.txtStatsBest.ReadOnly = true;
            this.txtStatsBest.Size = new System.Drawing.Size(249, 38);
            this.txtStatsBest.TabIndex = 0;
            this.txtStatsBest.TabStop = false;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(this.btnSearchReset);
            groupBox6.Controls.Add(this.btnSearchStop);
            groupBox6.Controls.Add(this.btnSearchStart);
            groupBox6.Location = new System.Drawing.Point(21, 534);
            groupBox6.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            groupBox6.Size = new System.Drawing.Size(624, 153);
            groupBox6.TabIndex = 18;
            groupBox6.TabStop = false;
            groupBox6.Text = "Search Control";
            // 
            // btnSearchReset
            // 
            this.btnSearchReset.Enabled = false;
            this.btnSearchReset.Location = new System.Drawing.Point(419, 45);
            this.btnSearchReset.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnSearchReset.Name = "btnSearchReset";
            this.btnSearchReset.Size = new System.Drawing.Size(187, 83);
            this.btnSearchReset.TabIndex = 2;
            this.btnSearchReset.Text = "Reset";
            this.btnSearchReset.Click += new System.EventHandler(this.btnSearchReset_Click);
            // 
            // btnSearchStop
            // 
            this.btnSearchStop.Enabled = false;
            this.btnSearchStop.Location = new System.Drawing.Point(219, 45);
            this.btnSearchStop.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnSearchStop.Name = "btnSearchStop";
            this.btnSearchStop.Size = new System.Drawing.Size(187, 83);
            this.btnSearchStop.TabIndex = 1;
            this.btnSearchStop.Text = "Stop / Pause";
            this.btnSearchStop.Click += new System.EventHandler(this.btnSearchStop_Click);
            // 
            // btnSearchStart
            // 
            this.btnSearchStart.Enabled = false;
            this.btnSearchStart.Location = new System.Drawing.Point(19, 45);
            this.btnSearchStart.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnSearchStart.Name = "btnSearchStart";
            this.btnSearchStart.Size = new System.Drawing.Size(187, 83);
            this.btnSearchStart.TabIndex = 0;
            this.btnSearchStart.Text = "Start / Continue";
            this.btnSearchStart.Click += new System.EventHandler(this.btnSearchStart_Click);
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(this.btnLoadDomainDefaults);
            groupBox5.Controls.Add(this.btnExperimentInfo);
            groupBox5.Controls.Add(this.cmbExperiments);
            groupBox5.Location = new System.Drawing.Point(21, 14);
            groupBox5.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            groupBox5.Size = new System.Drawing.Size(624, 165);
            groupBox5.TabIndex = 15;
            groupBox5.TabStop = false;
            groupBox5.Text = "Domain / Experiment";
            // 
            // btnLoadDomainDefaults
            // 
            this.btnLoadDomainDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadDomainDefaults.Location = new System.Drawing.Point(19, 95);
            this.btnLoadDomainDefaults.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnLoadDomainDefaults.Name = "btnLoadDomainDefaults";
            this.btnLoadDomainDefaults.Size = new System.Drawing.Size(531, 57);
            this.btnLoadDomainDefaults.TabIndex = 48;
            this.btnLoadDomainDefaults.Text = "Load Domain Default Parameters";
            this.btnLoadDomainDefaults.Click += new System.EventHandler(this.btnLoadDomainDefaults_Click);
            // 
            // btnExperimentInfo
            // 
            this.btnExperimentInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExperimentInfo.Location = new System.Drawing.Point(557, 36);
            this.btnExperimentInfo.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnExperimentInfo.Name = "btnExperimentInfo";
            this.btnExperimentInfo.Size = new System.Drawing.Size(51, 55);
            this.btnExperimentInfo.TabIndex = 47;
            this.btnExperimentInfo.Text = "?";
            this.btnExperimentInfo.Click += new System.EventHandler(this.btnExperimentInfo_Click);
            // 
            // cmbExperiments
            // 
            this.cmbExperiments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExperiments.DropDownWidth = 300;
            this.cmbExperiments.Location = new System.Drawing.Point(21, 38);
            this.cmbExperiments.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.cmbExperiments.Name = "cmbExperiments";
            this.cmbExperiments.Size = new System.Drawing.Size(521, 39);
            this.cmbExperiments.TabIndex = 36;
            this.cmbExperiments.SelectedIndexChanged += new System.EventHandler(this.cmbExperiments_SelectedIndexChanged);
            // 
            // gbxNeatGenomeParameters
            // 
            gbxNeatGenomeParameters.Controls.Add(this.txtParamConnectionWeightRange);
            gbxNeatGenomeParameters.Controls.Add(label16);
            gbxNeatGenomeParameters.Controls.Add(label42);
            gbxNeatGenomeParameters.Controls.Add(this.txtParamMutateConnectionWeights);
            gbxNeatGenomeParameters.Controls.Add(this.txtParamMutateDeleteConnection);
            gbxNeatGenomeParameters.Controls.Add(this.txtParamMutateAddConnection);
            gbxNeatGenomeParameters.Controls.Add(label34);
            gbxNeatGenomeParameters.Controls.Add(label36);
            gbxNeatGenomeParameters.Controls.Add(this.txtParamMutateAddNode);
            gbxNeatGenomeParameters.Controls.Add(label35);
            gbxNeatGenomeParameters.Location = new System.Drawing.Point(720, 14);
            gbxNeatGenomeParameters.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxNeatGenomeParameters.Name = "gbxNeatGenomeParameters";
            gbxNeatGenomeParameters.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxNeatGenomeParameters.Size = new System.Drawing.Size(616, 472);
            gbxNeatGenomeParameters.TabIndex = 52;
            gbxNeatGenomeParameters.TabStop = false;
            gbxNeatGenomeParameters.Text = "NEAT Genome Parameters";
            // 
            // txtParamConnectionWeightRange
            // 
            this.txtParamConnectionWeightRange.Location = new System.Drawing.Point(16, 45);
            this.txtParamConnectionWeightRange.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamConnectionWeightRange.Name = "txtParamConnectionWeightRange";
            this.txtParamConnectionWeightRange.Size = new System.Drawing.Size(121, 38);
            this.txtParamConnectionWeightRange.TabIndex = 50;
            this.txtParamConnectionWeightRange.Text = "5";
            // 
            // label16
            // 
            label16.Location = new System.Drawing.Point(155, 52);
            label16.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(363, 38);
            label16.TabIndex = 51;
            label16.Text = "Connection Weight Range";
            // 
            // label42
            // 
            label42.Location = new System.Drawing.Point(155, 327);
            label42.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label42.Name = "label42";
            label42.Size = new System.Drawing.Size(405, 38);
            label42.TabIndex = 27;
            label42.Text = "p Mutate Delete Connection";
            // 
            // txtParamMutateConnectionWeights
            // 
            this.txtParamMutateConnectionWeights.Location = new System.Drawing.Point(16, 134);
            this.txtParamMutateConnectionWeights.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamMutateConnectionWeights.Name = "txtParamMutateConnectionWeights";
            this.txtParamMutateConnectionWeights.Size = new System.Drawing.Size(121, 38);
            this.txtParamMutateConnectionWeights.TabIndex = 24;
            this.txtParamMutateConnectionWeights.Text = "0.988";
            // 
            // txtParamMutateDeleteConnection
            // 
            this.txtParamMutateDeleteConnection.Location = new System.Drawing.Point(16, 320);
            this.txtParamMutateDeleteConnection.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamMutateDeleteConnection.Name = "txtParamMutateDeleteConnection";
            this.txtParamMutateDeleteConnection.Size = new System.Drawing.Size(121, 38);
            this.txtParamMutateDeleteConnection.TabIndex = 26;
            this.txtParamMutateDeleteConnection.Text = "0.001";
            // 
            // txtParamMutateAddConnection
            // 
            this.txtParamMutateAddConnection.Location = new System.Drawing.Point(16, 258);
            this.txtParamMutateAddConnection.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamMutateAddConnection.Name = "txtParamMutateAddConnection";
            this.txtParamMutateAddConnection.Size = new System.Drawing.Size(121, 38);
            this.txtParamMutateAddConnection.TabIndex = 20;
            this.txtParamMutateAddConnection.Text = "0.01";
            // 
            // label34
            // 
            label34.Location = new System.Drawing.Point(155, 265);
            label34.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label34.Name = "label34";
            label34.Size = new System.Drawing.Size(363, 38);
            label34.TabIndex = 25;
            label34.Text = "p Mutate Add Connection";
            // 
            // label36
            // 
            label36.Location = new System.Drawing.Point(155, 141);
            label36.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label36.Name = "label36";
            label36.Size = new System.Drawing.Size(405, 38);
            label36.TabIndex = 21;
            label36.Text = "p Mutate Connection Weights";
            // 
            // txtParamMutateAddNode
            // 
            this.txtParamMutateAddNode.Location = new System.Drawing.Point(16, 196);
            this.txtParamMutateAddNode.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamMutateAddNode.Name = "txtParamMutateAddNode";
            this.txtParamMutateAddNode.Size = new System.Drawing.Size(121, 38);
            this.txtParamMutateAddNode.TabIndex = 22;
            this.txtParamMutateAddNode.Text = "0.001";
            // 
            // label35
            // 
            label35.Location = new System.Drawing.Point(155, 203);
            label35.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label35.Name = "label35";
            label35.Size = new System.Drawing.Size(363, 38);
            label35.TabIndex = 23;
            label35.Text = "p Mutate Add Neuron";
            // 
            // gbxEAParameters
            // 
            gbxEAParameters.BackColor = System.Drawing.Color.Transparent;
            gbxEAParameters.Controls.Add(label21);
            gbxEAParameters.Controls.Add(this.txtParamNumberOfSpecies);
            gbxEAParameters.Controls.Add(label4);
            gbxEAParameters.Controls.Add(this.label15);
            gbxEAParameters.Controls.Add(label32);
            gbxEAParameters.Controls.Add(label14);
            gbxEAParameters.Controls.Add(this.txtParamSelectionProportion);
            gbxEAParameters.Controls.Add(this.txtParamInterspeciesMating);
            gbxEAParameters.Controls.Add(this.txtParamElitismProportion);
            gbxEAParameters.Controls.Add(label25);
            gbxEAParameters.Controls.Add(this.txtParamOffspringCrossover);
            gbxEAParameters.Controls.Add(label26);
            gbxEAParameters.Controls.Add(this.txtParamOffspringAsexual);
            gbxEAParameters.Location = new System.Drawing.Point(21, 14);
            gbxEAParameters.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxEAParameters.Name = "gbxEAParameters";
            gbxEAParameters.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            gbxEAParameters.Size = new System.Drawing.Size(659, 472);
            gbxEAParameters.TabIndex = 16;
            gbxEAParameters.TabStop = false;
            gbxEAParameters.Text = "Evolution Algorithm Parameters";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new System.Drawing.Point(149, 60);
            label21.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(255, 32);
            label21.TabIndex = 57;
            label21.Text = "Number of Species";
            // 
            // txtParamNumberOfSpecies
            // 
            this.txtParamNumberOfSpecies.Location = new System.Drawing.Point(16, 52);
            this.txtParamNumberOfSpecies.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamNumberOfSpecies.Name = "txtParamNumberOfSpecies";
            this.txtParamNumberOfSpecies.Size = new System.Drawing.Size(121, 38);
            this.txtParamNumberOfSpecies.TabIndex = 56;
            this.txtParamNumberOfSpecies.Text = "10";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(149, 122);
            label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(239, 32);
            label4.TabIndex = 55;
            label4.Text = "Elitism Proportion";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(16, 393);
            this.label15.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(533, 2);
            this.label15.TabIndex = 54;
            // 
            // label32
            // 
            label32.Location = new System.Drawing.Point(149, 184);
            label32.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label32.Name = "label32";
            label32.Size = new System.Drawing.Size(320, 38);
            label32.TabIndex = 24;
            label32.Text = "Selection Proportion";
            // 
            // label14
            // 
            label14.Location = new System.Drawing.Point(149, 410);
            label14.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(320, 38);
            label14.TabIndex = 53;
            label14.Text = "p Interspecies Mating";
            // 
            // txtParamSelectionProportion
            // 
            this.txtParamSelectionProportion.Location = new System.Drawing.Point(16, 176);
            this.txtParamSelectionProportion.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamSelectionProportion.Name = "txtParamSelectionProportion";
            this.txtParamSelectionProportion.Size = new System.Drawing.Size(121, 38);
            this.txtParamSelectionProportion.TabIndex = 23;
            this.txtParamSelectionProportion.Text = "0.2";
            // 
            // txtParamInterspeciesMating
            // 
            this.txtParamInterspeciesMating.Location = new System.Drawing.Point(16, 403);
            this.txtParamInterspeciesMating.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamInterspeciesMating.Name = "txtParamInterspeciesMating";
            this.txtParamInterspeciesMating.Size = new System.Drawing.Size(121, 38);
            this.txtParamInterspeciesMating.TabIndex = 52;
            this.txtParamInterspeciesMating.Text = "0.01";
            // 
            // txtParamElitismProportion
            // 
            this.txtParamElitismProportion.Location = new System.Drawing.Point(16, 114);
            this.txtParamElitismProportion.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamElitismProportion.Name = "txtParamElitismProportion";
            this.txtParamElitismProportion.Size = new System.Drawing.Size(121, 38);
            this.txtParamElitismProportion.TabIndex = 21;
            this.txtParamElitismProportion.Text = "0.2";
            // 
            // label25
            // 
            label25.Location = new System.Drawing.Point(149, 343);
            label25.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(320, 38);
            label25.TabIndex = 51;
            label25.Text = "p Offspring Crossover";
            // 
            // txtParamOffspringCrossover
            // 
            this.txtParamOffspringCrossover.Location = new System.Drawing.Point(16, 336);
            this.txtParamOffspringCrossover.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamOffspringCrossover.Name = "txtParamOffspringCrossover";
            this.txtParamOffspringCrossover.Size = new System.Drawing.Size(121, 38);
            this.txtParamOffspringCrossover.TabIndex = 50;
            this.txtParamOffspringCrossover.Text = "0.5";
            // 
            // label26
            // 
            label26.Location = new System.Drawing.Point(149, 281);
            label26.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(277, 38);
            label26.TabIndex = 49;
            label26.Text = "p Offspring Asexual";
            // 
            // txtParamOffspringAsexual
            // 
            this.txtParamOffspringAsexual.Location = new System.Drawing.Point(16, 274);
            this.txtParamOffspringAsexual.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtParamOffspringAsexual.Name = "txtParamOffspringAsexual";
            this.txtParamOffspringAsexual.Size = new System.Drawing.Size(121, 38);
            this.txtParamOffspringAsexual.TabIndex = 48;
            this.txtParamOffspringAsexual.Text = "0.5";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnCopyLogToClipboard);
            this.splitContainer.Panel2.Controls.Add(this.lbxLog);
            this.splitContainer.Size = new System.Drawing.Size(2843, 1602);
            this.splitContainer.SplitterDistance = 1134;
            this.splitContainer.SplitterWidth = 14;
            this.splitContainer.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 55);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(2843, 1079);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.previewContainer);
            this.tabPage1.Controls.Add(this.solutionContainer);
            this.tabPage1.Controls.Add(gbxGenomePopulation);
            this.tabPage1.Controls.Add(this.gbxLogging);
            this.tabPage1.Controls.Add(gbxCurrentStats);
            this.tabPage1.Controls.Add(groupBox6);
            this.tabPage1.Controls.Add(groupBox5);
            this.tabPage1.Location = new System.Drawing.Point(10, 48);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.tabPage1.Size = new System.Drawing.Size(2823, 1021);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Page 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // previewContainer
            // 
            this.previewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.previewContainer.Location = new System.Drawing.Point(2017, 14);
            this.previewContainer.Name = "previewContainer";
            this.previewContainer.Size = new System.Drawing.Size(795, 980);
            this.previewContainer.TabIndex = 23;
            this.previewContainer.TabStop = false;
            this.previewContainer.Text = "Preview";
            // 
            // solutionContainer
            // 
            this.solutionContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.solutionContainer.Location = new System.Drawing.Point(1363, 14);
            this.solutionContainer.Name = "solutionContainer";
            this.solutionContainer.Size = new System.Drawing.Size(636, 980);
            this.solutionContainer.TabIndex = 22;
            this.solutionContainer.TabStop = false;
            this.solutionContainer.Text = "Solution";
            // 
            // gbxLogging
            // 
            this.gbxLogging.Controls.Add(this.txtFileLogBaseName);
            this.gbxLogging.Controls.Add(this.chkFileWriteLog);
            this.gbxLogging.Controls.Add(this.txtFileBaseName);
            this.gbxLogging.Controls.Add(label13);
            this.gbxLogging.Controls.Add(label8);
            this.gbxLogging.Controls.Add(this.chkFileSaveGenomeOnImprovement);
            this.gbxLogging.Location = new System.Drawing.Point(21, 701);
            this.gbxLogging.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.gbxLogging.Name = "gbxLogging";
            this.gbxLogging.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.gbxLogging.Size = new System.Drawing.Size(624, 293);
            this.gbxLogging.TabIndex = 20;
            this.gbxLogging.TabStop = false;
            this.gbxLogging.Text = "File";
            // 
            // txtFileLogBaseName
            // 
            this.txtFileLogBaseName.Location = new System.Drawing.Point(21, 210);
            this.txtFileLogBaseName.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtFileLogBaseName.Name = "txtFileLogBaseName";
            this.txtFileLogBaseName.Size = new System.Drawing.Size(465, 38);
            this.txtFileLogBaseName.TabIndex = 25;
            this.txtFileLogBaseName.Text = "sharpneat";
            // 
            // chkFileWriteLog
            // 
            this.chkFileWriteLog.Location = new System.Drawing.Point(21, 153);
            this.chkFileWriteLog.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.chkFileWriteLog.Name = "chkFileWriteLog";
            this.chkFileWriteLog.Size = new System.Drawing.Size(347, 57);
            this.chkFileWriteLog.TabIndex = 24;
            this.chkFileWriteLog.Text = "Write Log File (*.log)";
            // 
            // txtFileBaseName
            // 
            this.txtFileBaseName.Location = new System.Drawing.Point(21, 95);
            this.txtFileBaseName.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtFileBaseName.Name = "txtFileBaseName";
            this.txtFileBaseName.Size = new System.Drawing.Size(465, 38);
            this.txtFileBaseName.TabIndex = 1;
            this.txtFileBaseName.Text = "champ";
            // 
            // chkFileSaveGenomeOnImprovement
            // 
            this.chkFileSaveGenomeOnImprovement.Location = new System.Drawing.Point(21, 38);
            this.chkFileSaveGenomeOnImprovement.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.chkFileSaveGenomeOnImprovement.Name = "chkFileSaveGenomeOnImprovement";
            this.chkFileSaveGenomeOnImprovement.Size = new System.Drawing.Size(621, 57);
            this.chkFileSaveGenomeOnImprovement.TabIndex = 0;
            this.chkFileSaveGenomeOnImprovement.Text = "Save Genome on Improvement (*.gnm.xml)";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(gbxNeatGenomeParameters);
            this.tabPage2.Controls.Add(gbxEAParameters);
            this.tabPage2.Location = new System.Drawing.Point(10, 48);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.tabPage2.Size = new System.Drawing.Size(2823, 1021);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Page 2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(16, 5, 0, 5);
            this.menuStrip1.Size = new System.Drawing.Size(2843, 55);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPopulationToolStripMenuItem,
            this.loadSeedGenomeToolStripMenuItem,
            this.loadSeedGenomesToolStripMenuItem,
            this.toolStripSeparator,
            this.savePopulationToolStripMenuItem,
            this.saveBestGenomeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(75, 45);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadPopulationToolStripMenuItem
            // 
            this.loadPopulationToolStripMenuItem.Name = "loadPopulationToolStripMenuItem";
            this.loadPopulationToolStripMenuItem.Size = new System.Drawing.Size(407, 46);
            this.loadPopulationToolStripMenuItem.Text = "Load Population";
            this.loadPopulationToolStripMenuItem.Click += new System.EventHandler(this.loadPopulationToolStripMenuItem_Click);
            // 
            // loadSeedGenomeToolStripMenuItem
            // 
            this.loadSeedGenomeToolStripMenuItem.Name = "loadSeedGenomeToolStripMenuItem";
            this.loadSeedGenomeToolStripMenuItem.Size = new System.Drawing.Size(407, 46);
            this.loadSeedGenomeToolStripMenuItem.Text = "Load Seed Genome";
            this.loadSeedGenomeToolStripMenuItem.Click += new System.EventHandler(this.loadSeedGenomeToolStripMenuItem_Click);
            // 
            // loadSeedGenomesToolStripMenuItem
            // 
            this.loadSeedGenomesToolStripMenuItem.Name = "loadSeedGenomesToolStripMenuItem";
            this.loadSeedGenomesToolStripMenuItem.Size = new System.Drawing.Size(407, 46);
            this.loadSeedGenomesToolStripMenuItem.Text = "Load Seed Genomes";
            this.loadSeedGenomesToolStripMenuItem.Click += new System.EventHandler(this.loadSeedGenomesToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(404, 6);
            // 
            // savePopulationToolStripMenuItem
            // 
            this.savePopulationToolStripMenuItem.Enabled = false;
            this.savePopulationToolStripMenuItem.Name = "savePopulationToolStripMenuItem";
            this.savePopulationToolStripMenuItem.Size = new System.Drawing.Size(407, 46);
            this.savePopulationToolStripMenuItem.Text = "Save Population";
            this.savePopulationToolStripMenuItem.Click += new System.EventHandler(this.savePopulationToolStripMenuItem_Click);
            // 
            // saveBestGenomeToolStripMenuItem
            // 
            this.saveBestGenomeToolStripMenuItem.Enabled = false;
            this.saveBestGenomeToolStripMenuItem.Name = "saveBestGenomeToolStripMenuItem";
            this.saveBestGenomeToolStripMenuItem.Size = new System.Drawing.Size(407, 46);
            this.saveBestGenomeToolStripMenuItem.Text = "Save Best Genome";
            this.saveBestGenomeToolStripMenuItem.Click += new System.EventHandler(this.saveBestGenomeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rankPlotsToolStripMenuItem,
            this.distributionPlotsToolStripMenuItem,
            this.timeSeriesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(94, 45);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // rankPlotsToolStripMenuItem
            // 
            this.rankPlotsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.specieSizeByRankToolStripMenuItem,
            this.specieChampFitnessByRankToolStripMenuItem,
            this.specieChampComplexityByRankToolStripMenuItem,
            this.toolStripSeparator3,
            this.genomeFitnessByRankToolStripMenuItem,
            this.genomeComplexityByRankToolStripMenuItem});
            this.rankPlotsToolStripMenuItem.Name = "rankPlotsToolStripMenuItem";
            this.rankPlotsToolStripMenuItem.Size = new System.Drawing.Size(361, 46);
            this.rankPlotsToolStripMenuItem.Text = "Rank Plots";
            // 
            // specieSizeByRankToolStripMenuItem
            // 
            this.specieSizeByRankToolStripMenuItem.Name = "specieSizeByRankToolStripMenuItem";
            this.specieSizeByRankToolStripMenuItem.Size = new System.Drawing.Size(741, 46);
            this.specieSizeByRankToolStripMenuItem.Text = "Specie Size by Rank";
            this.specieSizeByRankToolStripMenuItem.Click += new System.EventHandler(this.specieSizeByRankToolStripMenuItem_Click);
            // 
            // specieChampFitnessByRankToolStripMenuItem
            // 
            this.specieChampFitnessByRankToolStripMenuItem.Name = "specieChampFitnessByRankToolStripMenuItem";
            this.specieChampFitnessByRankToolStripMenuItem.Size = new System.Drawing.Size(741, 46);
            this.specieChampFitnessByRankToolStripMenuItem.Text = "Specie Fitness by Rank (Champs && Mean)";
            this.specieChampFitnessByRankToolStripMenuItem.Click += new System.EventHandler(this.specieChampFitnessByRankToolStripMenuItem_Click);
            // 
            // specieChampComplexityByRankToolStripMenuItem
            // 
            this.specieChampComplexityByRankToolStripMenuItem.Name = "specieChampComplexityByRankToolStripMenuItem";
            this.specieChampComplexityByRankToolStripMenuItem.Size = new System.Drawing.Size(741, 46);
            this.specieChampComplexityByRankToolStripMenuItem.Text = "Specie Complexity by Rank (Champs && Mean)";
            this.specieChampComplexityByRankToolStripMenuItem.Click += new System.EventHandler(this.specieChampComplexityByRankToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(738, 6);
            // 
            // genomeFitnessByRankToolStripMenuItem
            // 
            this.genomeFitnessByRankToolStripMenuItem.Name = "genomeFitnessByRankToolStripMenuItem";
            this.genomeFitnessByRankToolStripMenuItem.Size = new System.Drawing.Size(741, 46);
            this.genomeFitnessByRankToolStripMenuItem.Text = "Genome Fitness by Rank";
            this.genomeFitnessByRankToolStripMenuItem.Click += new System.EventHandler(this.genomeFitnessByRankToolStripMenuItem_Click);
            // 
            // genomeComplexityByRankToolStripMenuItem
            // 
            this.genomeComplexityByRankToolStripMenuItem.Name = "genomeComplexityByRankToolStripMenuItem";
            this.genomeComplexityByRankToolStripMenuItem.Size = new System.Drawing.Size(741, 46);
            this.genomeComplexityByRankToolStripMenuItem.Text = "Genome Complexity by Rank";
            this.genomeComplexityByRankToolStripMenuItem.Click += new System.EventHandler(this.genomeComplexityByRankToolStripMenuItem_Click);
            // 
            // distributionPlotsToolStripMenuItem
            // 
            this.distributionPlotsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.specieSizeDistributionToolStripMenuItem,
            this.specieFitnessDistributionsToolStripMenuItem,
            this.specieComplexityDistributionsToolStripMenuItem,
            this.toolStripSeparator1,
            this.genomeFitnessDistributionToolStripMenuItem,
            this.genomeComplexityDistributionToolStripMenuItem});
            this.distributionPlotsToolStripMenuItem.Name = "distributionPlotsToolStripMenuItem";
            this.distributionPlotsToolStripMenuItem.Size = new System.Drawing.Size(361, 46);
            this.distributionPlotsToolStripMenuItem.Text = "Distribution Plots";
            // 
            // specieSizeDistributionToolStripMenuItem
            // 
            this.specieSizeDistributionToolStripMenuItem.Name = "specieSizeDistributionToolStripMenuItem";
            this.specieSizeDistributionToolStripMenuItem.Size = new System.Drawing.Size(790, 46);
            this.specieSizeDistributionToolStripMenuItem.Text = "Specie Size Distribution";
            this.specieSizeDistributionToolStripMenuItem.Click += new System.EventHandler(this.specieSizeDistributionToolStripMenuItem_Click);
            // 
            // specieFitnessDistributionsToolStripMenuItem
            // 
            this.specieFitnessDistributionsToolStripMenuItem.Name = "specieFitnessDistributionsToolStripMenuItem";
            this.specieFitnessDistributionsToolStripMenuItem.Size = new System.Drawing.Size(790, 46);
            this.specieFitnessDistributionsToolStripMenuItem.Text = "Specie Fitness Distributions (Champ && Mean)";
            this.specieFitnessDistributionsToolStripMenuItem.Click += new System.EventHandler(this.specieFitnessDistributionsToolStripMenuItem_Click);
            // 
            // specieComplexityDistributionsToolStripMenuItem
            // 
            this.specieComplexityDistributionsToolStripMenuItem.Name = "specieComplexityDistributionsToolStripMenuItem";
            this.specieComplexityDistributionsToolStripMenuItem.Size = new System.Drawing.Size(790, 46);
            this.specieComplexityDistributionsToolStripMenuItem.Text = "Specie Complexity Distributions (Champ && Mean)";
            this.specieComplexityDistributionsToolStripMenuItem.Click += new System.EventHandler(this.specieComplexityDistributionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(787, 6);
            // 
            // genomeFitnessDistributionToolStripMenuItem
            // 
            this.genomeFitnessDistributionToolStripMenuItem.Name = "genomeFitnessDistributionToolStripMenuItem";
            this.genomeFitnessDistributionToolStripMenuItem.Size = new System.Drawing.Size(790, 46);
            this.genomeFitnessDistributionToolStripMenuItem.Text = "Genome Fitness Distribution";
            this.genomeFitnessDistributionToolStripMenuItem.Click += new System.EventHandler(this.genomeFitnessDistributionToolStripMenuItem_Click);
            // 
            // genomeComplexityDistributionToolStripMenuItem
            // 
            this.genomeComplexityDistributionToolStripMenuItem.Name = "genomeComplexityDistributionToolStripMenuItem";
            this.genomeComplexityDistributionToolStripMenuItem.Size = new System.Drawing.Size(790, 46);
            this.genomeComplexityDistributionToolStripMenuItem.Text = "Genome Complexity Distribution";
            this.genomeComplexityDistributionToolStripMenuItem.Click += new System.EventHandler(this.genomeComplexityDistributionToolStripMenuItem_Click);
            // 
            // timeSeriesToolStripMenuItem
            // 
            this.timeSeriesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fitnessBestMeansToolStripMenuItem,
            this.complexityBestMeansToolStripMenuItem,
            this.evaluationsPerSecToolStripMenuItem});
            this.timeSeriesToolStripMenuItem.Name = "timeSeriesToolStripMenuItem";
            this.timeSeriesToolStripMenuItem.Size = new System.Drawing.Size(361, 46);
            this.timeSeriesToolStripMenuItem.Text = "Time Series";
            // 
            // fitnessBestMeansToolStripMenuItem
            // 
            this.fitnessBestMeansToolStripMenuItem.Name = "fitnessBestMeansToolStripMenuItem";
            this.fitnessBestMeansToolStripMenuItem.Size = new System.Drawing.Size(493, 46);
            this.fitnessBestMeansToolStripMenuItem.Text = "Fitness (Best && Means)";
            this.fitnessBestMeansToolStripMenuItem.Click += new System.EventHandler(this.fitnessBestMeansToolStripMenuItem_Click);
            // 
            // complexityBestMeansToolStripMenuItem
            // 
            this.complexityBestMeansToolStripMenuItem.Name = "complexityBestMeansToolStripMenuItem";
            this.complexityBestMeansToolStripMenuItem.Size = new System.Drawing.Size(493, 46);
            this.complexityBestMeansToolStripMenuItem.Text = "Complexity (Best && Means)";
            this.complexityBestMeansToolStripMenuItem.Click += new System.EventHandler(this.complexityBestMeansToolStripMenuItem_Click);
            // 
            // evaluationsPerSecToolStripMenuItem
            // 
            this.evaluationsPerSecToolStripMenuItem.Name = "evaluationsPerSecToolStripMenuItem";
            this.evaluationsPerSecToolStripMenuItem.Size = new System.Drawing.Size(493, 46);
            this.evaluationsPerSecToolStripMenuItem.Text = "Evaluations per Sec";
            this.evaluationsPerSecToolStripMenuItem.Click += new System.EventHandler(this.evaluationsPerSecToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(112, 45);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btnCopyLogToClipboard
            // 
            this.btnCopyLogToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyLogToClipboard.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopyLogToClipboard.Location = new System.Drawing.Point(2529, 2);
            this.btnCopyLogToClipboard.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnCopyLogToClipboard.Name = "btnCopyLogToClipboard";
            this.btnCopyLogToClipboard.Size = new System.Drawing.Size(267, 52);
            this.btnCopyLogToClipboard.TabIndex = 1;
            this.btnCopyLogToClipboard.Text = "Copy to clipboard";
            this.btnCopyLogToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyLogToClipboard.Click += new System.EventHandler(this.btnCopyLogToClipboard_Click);
            // 
            // lbxLog
            // 
            this.lbxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLog.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxLog.FormattingEnabled = true;
            this.lbxLog.ItemHeight = 33;
            this.lbxLog.Location = new System.Drawing.Point(0, 0);
            this.lbxLog.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.lbxLog.Name = "lbxLog";
            this.lbxLog.Size = new System.Drawing.Size(2843, 454);
            this.lbxLog.TabIndex = 0;
            // 
            // populationToolStripMenuItem
            // 
            this.populationToolStripMenuItem.Name = "populationToolStripMenuItem";
            this.populationToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(2843, 1602);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "MainForm";
            this.Text = "SharpNEAT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            gbxGenomePopulation.ResumeLayout(false);
            gbxGenomePopulation.PerformLayout();
            gbxCurrentStats.ResumeLayout(false);
            gbxCurrentStats.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            gbxNeatGenomeParameters.ResumeLayout(false);
            gbxNeatGenomeParameters.PerformLayout();
            gbxEAParameters.ResumeLayout(false);
            gbxEAParameters.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbxLogging.ResumeLayout(false);
            this.gbxLogging.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListBox lbxLog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSearchReset;
        private System.Windows.Forms.Button btnSearchStop;
        private System.Windows.Forms.Button btnSearchStart;
        private System.Windows.Forms.Button btnLoadDomainDefaults;
        private System.Windows.Forms.Button btnExperimentInfo;
        private System.Windows.Forms.ComboBox cmbExperiments;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtFileLogBaseName;
        private System.Windows.Forms.CheckBox chkFileWriteLog;
        private System.Windows.Forms.TextBox txtFileBaseName;
        private System.Windows.Forms.CheckBox chkFileSaveGenomeOnImprovement;
        private System.Windows.Forms.TextBox txtSearchStatsMode;
        private System.Windows.Forms.TextBox txtStatsEvalsPerSec;
        private System.Windows.Forms.TextBox txtStatsMeanGenomeComplx;
        private System.Windows.Forms.TextBox txtStatsBestGenomeComplx;
        private System.Windows.Forms.TextBox txtStatsTotalEvals;
        private System.Windows.Forms.TextBox txtStatsGeneration;
        private System.Windows.Forms.TextBox txtStatsMean;
        private System.Windows.Forms.TextBox txtStatsBest;
        private System.Windows.Forms.TextBox txtParamSelectionProportion;
        private System.Windows.Forms.TextBox txtParamElitismProportion;
        private System.Windows.Forms.TextBox txtParamMutateDeleteConnection;
        private System.Windows.Forms.TextBox txtParamMutateConnectionWeights;
        private System.Windows.Forms.TextBox txtParamMutateAddNode;
        private System.Windows.Forms.TextBox txtParamMutateAddConnection;
        private System.Windows.Forms.TextBox txtParamConnectionWeightRange;
        private System.Windows.Forms.TextBox txtSpecieChampMean;
        private System.Windows.Forms.TextBox txtStatsMaxGenomeComplx;
        private System.Windows.Forms.TextBox txtStatsInterspeciesOffspringCount;
        private System.Windows.Forms.TextBox txtStatsCrossoverOffspringCount;
        private System.Windows.Forms.TextBox txtStatsAsexualOffspringCount;
        private System.Windows.Forms.TextBox txtStatsTotalOffspringCount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtParamInterspeciesMating;
        private System.Windows.Forms.TextBox txtParamOffspringCrossover;
        private System.Windows.Forms.TextBox txtParamOffspringAsexual;
        private System.Windows.Forms.TextBox txtParamPopulationSize;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtPopulationStatus;
        private System.Windows.Forms.Button btnCreateRandomPop;
        private System.Windows.Forms.ToolStripMenuItem populationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtParamInitialConnectionProportion;
        private System.Windows.Forms.Button btnCopyLogToClipboard;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPopulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSeedGenomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSeedGenomesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveBestGenomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePopulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeSeriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitnessBestMeansToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem complexityBestMeansToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evaluationsPerSecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rankPlotsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specieSizeByRankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specieChampFitnessByRankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specieChampComplexityByRankToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem genomeFitnessByRankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem genomeComplexityByRankToolStripMenuItem;
        private System.Windows.Forms.TextBox txtStatsAlternativeFitness;
        private System.Windows.Forms.TextBox txtParamNumberOfSpecies;
        private System.Windows.Forms.ToolStripMenuItem distributionPlotsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specieSizeDistributionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specieFitnessDistributionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specieComplexityDistributionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem genomeFitnessDistributionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem genomeComplexityDistributionToolStripMenuItem;
        private System.Windows.Forms.GroupBox solutionContainer;
        private System.Windows.Forms.GroupBox gbxLogging;
        private System.Windows.Forms.GroupBox previewContainer;
    }
}