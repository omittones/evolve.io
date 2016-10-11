/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2016 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * SharpNEAT is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with SharpNEAT.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using log4net;
using log4net.Config;
using Redzen.Numerics;
using SharpNeat.Core;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.Neat;
using SharpNeat.Utility;
using SharpNeatGUI.Controllers;

namespace SharpNeatGUI
{
    /// <summary>
    /// SharpNEAT main GUI window.
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly ILog __log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Instance Fields [General]

        private IGuiNeatExperiment _selectedExperiment;
        private IGenomeFactory<NeatGenome> _genomeFactory;
        private List<NeatGenome> _genomeList;

        private IAlgorithmDriver _driver;
        private NeatEvolutionAlgorithm<NeatGenome> _ea;

        private StreamWriter _logFileWriter;

        /// <summary>Number format for building filename when saving champ genomes.</summary>
        private NumberFormatInfo _filenameNumberFormatter;

        /// <summary>XmlWriter settings for champ genome saving (format the XML to make it human readable).</summary>
        private XmlWriterSettings _xwSettings;

        /// <summary>Tracks the best champ fitness observed so far.</summary>
        private double _champGenomeFitness;

        /// <summary>Array of 'nice' colors for chart plots.</summary>
        private Color[] _plotColorArr;

        #endregion

        #region Instance Fields [Views]

        private readonly GenomeDisplayController solutionController;
        private readonly GenomeDisplayController previewController;

        private List<TimeSeriesGraphForm> _timeSeriesGraphFormList = new List<TimeSeriesGraphForm>();
        private List<SummaryGraphForm> _summaryGraphFormList = new List<SummaryGraphForm>();

        // Working storage space for graph views.
        private static int[] _specieDataArrayInt;
        private static Point2DDouble[] _specieDataPointArrayInt;

        private static double[] _specieDataArray;
        private static Point2DDouble[] _specieDataPointArray;

        private static double[] _genomeDataArray;
        private static Point2DDouble[] _genomeDataPointArray;

        #endregion

        /// <summary>
        /// Construct and initialize the form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            this.solutionController = new GenomeDisplayController(this, this.solutionContainer);
            this.previewController = new GenomeDisplayController(this, this.previewContainer);

            this.Scale(new SizeF(0.5f, 0.5f));
            this.WindowState = FormWindowState.Maximized;

            Logger.SetListBox(lbxLog);
            XmlConfigurator.Configure(new FileInfo("log4net.properties"));
            InitProblemDomainList();

            _filenameNumberFormatter = new NumberFormatInfo();
            _filenameNumberFormatter.NumberDecimalSeparator = ",";

            _xwSettings = new XmlWriterSettings();
            _xwSettings.Indent = true;

            _plotColorArr = GenerateNiceColors(10);
        }

        /// <summary>
        /// Initialise the problem domain combobox. The list of problem domains is read from an XML file; this 
        /// allows changes to be made and new domains to be plugged-in without recompiling binaries.
        /// </summary>
        private void InitProblemDomainList()
        {
            // Find all experiment config data files in the current directory (*.experiments.xml)
            foreach (var filename in Directory.EnumerateFiles(".", "*.experiments.xml"))
            {
                var expInfoList = ExperimentInfo.ReadExperimentXml(filename);
                foreach (var expInfo in expInfoList)
                {
                    cmbExperiments.Items.Add(new ListItem(string.Empty, expInfo.Name, expInfo));
                }
            }
            // Pre-select first item.
            cmbExperiments.SelectedIndex = 0;
        }

        #region GUI Wiring [Experiment Selection + Default Param Loading]

        private void cmbExperiments_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nullify this variable. We get the selected experiment via GetSelectedExperiment(). That method will instantiate 
            // _selectedExperiment with the currently selected experiment (on-demand instantiation).
            _selectedExperiment = null;
        }

        private void btnExperimentInfo_Click(object sender, EventArgs e)
        {
            if (null == cmbExperiments.SelectedItem)
            {
                return;
            }

            IExperiment<NeatGenome> experiment = GetSelectedExperiment();
            if (null != experiment)
            {
                MessageBox.Show(experiment.Description);
            }
        }

        private void btnLoadDomainDefaults_Click(object sender, EventArgs e)
        {
            // Dump the experiment's default parameters into the GUI.
            IExperiment<NeatGenome> experiment = GetSelectedExperiment();
            txtParamPopulationSize.Text = experiment.DefaultPopulationSize.ToString();

            var eaParams = experiment.NeatEvolutionAlgorithmParameters;
            var ngParams = experiment.NeatGenomeParameters;
            txtParamInitialConnectionProportion.Text = ngParams.InitialInterconnectionsProportion.ToString();
            txtParamNumberOfSpecies.Text = eaParams.SpecieCount.ToString();
            txtParamElitismProportion.Text = eaParams.ElitismProportion.ToString();
            txtParamSelectionProportion.Text = eaParams.SelectionProportion.ToString();
            txtParamOffspringAsexual.Text = eaParams.OffspringAsexualProportion.ToString();
            txtParamOffspringCrossover.Text = eaParams.OffspringSexualProportion.ToString();
            txtParamInterspeciesMating.Text = eaParams.InterspeciesMatingProportion.ToString();
            txtParamConnectionWeightRange.Text = ngParams.ConnectionWeightRange.ToString();
            txtParamMutateConnectionWeights.Text = ngParams.ConnectionWeightMutationProbability.ToString();
            txtParamMutateAddNode.Text = ngParams.AddNodeMutationProbability.ToString();
            txtParamMutateAddConnection.Text = ngParams.AddConnectionMutationProbability.ToString();
            txtParamMutateDeleteConnection.Text = ngParams.DeleteConnectionMutationProbability.ToString();
        }

        private IGuiNeatExperiment GetSelectedExperiment()
        {
            if (null == _selectedExperiment && null != cmbExperiments.SelectedItem)
            {
                var expInfo = (ExperimentInfo) (((ListItem) cmbExperiments.SelectedItem).Data);

                var assembly = Assembly.LoadFrom(expInfo.AssemblyPath);
                // TODO: Handle non-gui experiments.
                _selectedExperiment = assembly.CreateInstance(expInfo.ClassName) as IGuiNeatExperiment;
                _selectedExperiment.Initialize(expInfo.Name, expInfo.XmlConfig);
            }
            return _selectedExperiment;
        }

        #endregion

        #region GUI Wiring [Population Init]

        private void btnCreateRandomPop_Click(object sender, EventArgs e)
        {
            // Parse population size and interconnection proportion from GUI fields.
            var popSize = ParseInt(txtParamPopulationSize);
            if (null == popSize)
            {
                return;
            }

            var initConnProportion = ParseDouble(txtParamInitialConnectionProportion);
            if (null == initConnProportion)
            {
                return;
            }

            IExperiment<NeatGenome> experiment = GetSelectedExperiment();
            experiment.NeatGenomeParameters.InitialInterconnectionsProportion = initConnProportion.Value;

            // Create a genome factory appropriate for the experiment.
            var genomeFactory = experiment.CreateGenomeFactory();

            // Create an initial population of randomly generated genomes.
            var genomeList = genomeFactory.CreateGenomeList(popSize.Value, 0u);

            // Assign population to form variables & update GUI appropriately.
            _genomeFactory = genomeFactory;
            _genomeList = genomeList;
            UpdateGuiState();
        }

        #endregion

        #region GUI Wiring [Algorithm Init/Start/Stop]

        private void btnSearchStart_Click(object sender, EventArgs e)
        {
            if (null != _driver)
            {
                // Resume existing EA & update GUI state.
                _driver.StartContinue();
                UpdateGuiState();
                return;
            }

            // Initialise and start a new evolution algorithm.
            ReadAndUpdateExperimentParams();

            // Check number of species is <= the number of the genomes.
            if (_genomeList.Count < _selectedExperiment.NeatEvolutionAlgorithmParameters.SpecieCount)
            {
                __log.ErrorFormat("Genome count must be >= specie count. Genomes=[{0}] Species=[{1}]",
                    _selectedExperiment.NeatEvolutionAlgorithmParameters.SpecieCount, _genomeList.Count);
                return;
            }

            // Create evolution algorithm.
            _ea = _selectedExperiment.CreateEvolutionAlgorithm(_genomeFactory, _genomeList);

            _driver = AlgorithmDriver.Drive(_ea);

            // Attach update event listener.
            _driver.UpdateEvent += _ea_UpdateEvent;
            _driver.PausedEvent += _ea_PausedEvent;

            var solutionView = _selectedExperiment.CreateGenomeView();
            solutionController.Reconnect(solutionView, _driver);

            var previewView = _selectedExperiment.CreateDomainView();
            previewController.Reconnect(previewView, _driver);

            foreach (var graphForm in _timeSeriesGraphFormList)
            {
                graphForm.Reconnect(_driver);
            }

            foreach (var graphForm in _summaryGraphFormList)
            {
                graphForm.Reconnect(_driver);
            }

            // Create/open log file if the option is selected.
            if (chkFileWriteLog.Checked && null == _logFileWriter)
            {
                var filename = txtFileLogBaseName.Text + '_' + DateTime.Now.ToString("yyyyMMdd") + ".log";
                _logFileWriter = new StreamWriter(filename, true);
                _logFileWriter.WriteLine(
                    "ClockTime,Gen,BestFitness,MeanFitness,MeanSpecieChampFitness,ChampComplexity,MeanComplexity,MaxComplexity,TotalEvaluationCount,EvaluationsPerSec,SearchMode");
            }

            // Start the algorithm & update GUI state.
            _driver.StartContinue();

            UpdateGuiState();
        }

        private void btnSearchStop_Click(object sender, EventArgs e)
        {
            _driver.RequestPause();

            if (null != _logFileWriter)
            {
                // Null _logFileWriter prior to closing the writer. This much reduced the chance of attempt to write to the stream after it has closed.
                var sw = _logFileWriter;
                _logFileWriter = null;
                sw.Close();
            }
        }

        private void btnSearchReset_Click(object sender, EventArgs e)
        {
            _genomeFactory = null;
            _genomeList = null;

            // TODO: Proper cleanup of EA - e.g. main worker thread termination.
            _ea = null;

            _driver.UpdateEvent -= _ea_UpdateEvent;
            _driver.PausedEvent -= _ea_PausedEvent;
            _driver = null;

            _champGenomeFitness = 0.0;

            Logger.Clear();

            UpdateGuiState_ResetStats();
            UpdateGuiState();
        }

        /// <summary>
        /// Read experimental parameters from the GUI and update _selectedExperiment with the read values.
        /// </summary>
        private void ReadAndUpdateExperimentParams()
        {
            var eaParams = _selectedExperiment.NeatEvolutionAlgorithmParameters;
            eaParams.SpecieCount = ParseInt(txtParamNumberOfSpecies, eaParams.SpecieCount);
            eaParams.ElitismProportion = ParseDouble(txtParamElitismProportion, eaParams.ElitismProportion);
            eaParams.SelectionProportion = ParseDouble(txtParamSelectionProportion, eaParams.SelectionProportion);
            eaParams.OffspringAsexualProportion = ParseDouble(txtParamOffspringAsexual,
                eaParams.OffspringAsexualProportion);
            eaParams.OffspringSexualProportion = ParseDouble(txtParamOffspringCrossover,
                eaParams.OffspringSexualProportion);
            eaParams.InterspeciesMatingProportion = ParseDouble(txtParamInterspeciesMating,
                eaParams.InterspeciesMatingProportion);

            var ngParams = _selectedExperiment.NeatGenomeParameters;
            ngParams.ConnectionWeightRange = ParseDouble(txtParamConnectionWeightRange, ngParams.ConnectionWeightRange);
            ngParams.ConnectionWeightMutationProbability = ParseDouble(txtParamMutateConnectionWeights,
                ngParams.ConnectionWeightMutationProbability);
            ngParams.AddNodeMutationProbability = ParseDouble(txtParamMutateAddNode, ngParams.AddNodeMutationProbability);
            ngParams.AddConnectionMutationProbability = ParseDouble(txtParamMutateAddConnection,
                ngParams.AddConnectionMutationProbability);
            ngParams.DeleteConnectionMutationProbability = ParseDouble(txtParamMutateDeleteConnection,
                ngParams.DeleteConnectionMutationProbability);
        }

        #endregion

        #region GUI Wiring [GUI State Updating]

        private void UpdateGuiState()
        {
            if (null == _ea)
            {
                if (null == _genomeList)
                {
                    UpdateGuiState_NoPopulation();
                }
                else
                {
                    UpdateGuiState_PopulationReady();
                }
            }
            else
            {
                switch (_driver.RunState)
                {
                    case RunState.Ready:
                    case RunState.Paused:
                        UpdateGuiState_EaReadyPaused();
                        break;
                    case RunState.Running:
                        UpdateGuiState_EaRunning();
                        break;
                    default:
                        throw new ApplicationException(string.Format("Unexpected RunState [{0}]", _driver.RunState));
                }
            }
        }

        private void UpdateGuiState_NoPopulation()
        {
            // Enable experiment selection and initialization buttons.
            cmbExperiments.Enabled = true;
            btnLoadDomainDefaults.Enabled = true;
            btnCreateRandomPop.Enabled = true;

            // Display population statuc (empty).
            txtPopulationStatus.Text = "Population not initialized";
            txtPopulationStatus.BackColor = Color.Red;

            // Disable search control buttons.
            btnSearchStart.Enabled = false;
            btnSearchStop.Enabled = false;
            btnSearchReset.Enabled = false;

            // Parameter fields enabled.
            txtParamPopulationSize.Enabled = true;
            txtParamInitialConnectionProportion.Enabled = true;
            txtParamElitismProportion.Enabled = true;
            txtParamSelectionProportion.Enabled = true;
            txtParamOffspringAsexual.Enabled = true;
            txtParamOffspringCrossover.Enabled = true;
            txtParamInterspeciesMating.Enabled = true;
            txtParamConnectionWeightRange.Enabled = true;
            txtParamMutateConnectionWeights.Enabled = true;
            txtParamMutateAddNode.Enabled = true;
            txtParamMutateAddConnection.Enabled = true;
            txtParamMutateDeleteConnection.Enabled = true;

            // Logging to file.
            gbxLogging.Enabled = true;

            // Menu bar (file).
            loadPopulationToolStripMenuItem.Enabled = true;
            loadSeedGenomesToolStripMenuItem.Enabled = true;
            loadSeedGenomeToolStripMenuItem.Enabled = true;
            savePopulationToolStripMenuItem.Enabled = false;
            saveBestGenomeToolStripMenuItem.Enabled = false;
        }

        private void UpdateGuiState_PopulationReady()
        {
            // Disable anything to do with initialization now that we are initialized.
            cmbExperiments.Enabled = false;
            btnLoadDomainDefaults.Enabled = false;
            btnCreateRandomPop.Enabled = false;

            // Display how many genomes & status.
            txtPopulationStatus.Text = string.Format("{0:D0} genomes ready", _genomeList.Count);
            txtPopulationStatus.BackColor = Color.Orange;

            // Enable search control buttons.
            btnSearchStart.Enabled = true;
            btnSearchStop.Enabled = false;
            btnSearchReset.Enabled = true;

            // Parameter fields enabled (apart from population creation params)
            txtParamPopulationSize.Enabled = false;
            txtParamInitialConnectionProportion.Enabled = false;
            txtParamElitismProportion.Enabled = true;
            txtParamSelectionProportion.Enabled = true;
            txtParamOffspringAsexual.Enabled = true;
            txtParamOffspringCrossover.Enabled = true;
            txtParamInterspeciesMating.Enabled = true;
            txtParamConnectionWeightRange.Enabled = true;
            txtParamMutateConnectionWeights.Enabled = true;
            txtParamMutateAddNode.Enabled = true;
            txtParamMutateAddConnection.Enabled = true;
            txtParamMutateDeleteConnection.Enabled = true;

            // Logging to file.
            gbxLogging.Enabled = true;

            // Menu bar.
            loadPopulationToolStripMenuItem.Enabled = false;
            loadSeedGenomesToolStripMenuItem.Enabled = false;
            loadSeedGenomeToolStripMenuItem.Enabled = false;
            savePopulationToolStripMenuItem.Enabled = true;
            saveBestGenomeToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Evolution algorithm is ready/paused.
        /// </summary>
        private void UpdateGuiState_EaReadyPaused()
        {
            // Disable anything to do with initialization now that we are initialized.
            cmbExperiments.Enabled = false;
            btnLoadDomainDefaults.Enabled = false;
            btnCreateRandomPop.Enabled = false;

            // Display how many genomes & status.
            txtPopulationStatus.Text = string.Format("{0:D0} genomes paused.", _genomeList.Count);
            txtPopulationStatus.BackColor = Color.Orange;

            // Search control buttons.
            btnSearchStart.Enabled = true;
            btnSearchStop.Enabled = false;
            btnSearchReset.Enabled = true;

            // Parameter fields (disable).
            txtParamPopulationSize.Enabled = false;
            txtParamInitialConnectionProportion.Enabled = false;
            txtParamElitismProportion.Enabled = false;
            txtParamSelectionProportion.Enabled = false;
            txtParamOffspringAsexual.Enabled = false;
            txtParamOffspringCrossover.Enabled = false;
            txtParamInterspeciesMating.Enabled = false;
            txtParamConnectionWeightRange.Enabled = false;
            txtParamMutateConnectionWeights.Enabled = false;
            txtParamMutateAddNode.Enabled = false;
            txtParamMutateAddConnection.Enabled = false;
            txtParamMutateDeleteConnection.Enabled = false;

            // Logging to file.
            gbxLogging.Enabled = true;

            // Menu bar.
            loadPopulationToolStripMenuItem.Enabled = false;
            loadSeedGenomesToolStripMenuItem.Enabled = false;
            loadSeedGenomeToolStripMenuItem.Enabled = false;
            savePopulationToolStripMenuItem.Enabled = true;
            saveBestGenomeToolStripMenuItem.Enabled = (_ea.CurrentChampGenome != null);
        }

        /// <summary>
        /// Evolution algorithm is running.
        /// </summary>
        private void UpdateGuiState_EaRunning()
        {
            // Disable anything to do with initialization now that we are initialized.
            cmbExperiments.Enabled = false;
            btnLoadDomainDefaults.Enabled = false;
            btnCreateRandomPop.Enabled = false;

            // Display how many genomes & status.
            txtPopulationStatus.Text = string.Format("{0:D0} genomes running", _genomeList.Count);
            txtPopulationStatus.BackColor = Color.LightGreen;

            // Search control buttons.
            btnSearchStart.Enabled = false;
            btnSearchStop.Enabled = true;
            btnSearchReset.Enabled = false;

            // Parameter fields (disable).
            txtParamPopulationSize.Enabled = false;
            txtParamInitialConnectionProportion.Enabled = false;
            txtParamElitismProportion.Enabled = false;
            txtParamSelectionProportion.Enabled = false;
            txtParamOffspringAsexual.Enabled = false;
            txtParamOffspringCrossover.Enabled = false;
            txtParamInterspeciesMating.Enabled = false;
            txtParamConnectionWeightRange.Enabled = false;
            txtParamMutateConnectionWeights.Enabled = false;
            txtParamMutateAddNode.Enabled = false;
            txtParamMutateAddConnection.Enabled = false;
            txtParamMutateDeleteConnection.Enabled = false;

            // Logging to file.
            gbxLogging.Enabled = false;

            // Menu bar.
            loadPopulationToolStripMenuItem.Enabled = false;
            loadSeedGenomesToolStripMenuItem.Enabled = false;
            loadSeedGenomeToolStripMenuItem.Enabled = false;
            savePopulationToolStripMenuItem.Enabled = false;
            saveBestGenomeToolStripMenuItem.Enabled = false;
        }

        private void UpdateGuiState_EaStats()
        {
            var stats = _ea.Statistics;
            txtSearchStatsMode.Text = _ea.ComplexityRegulationMode.ToString();
            switch (_ea.ComplexityRegulationMode)
            {
                case ComplexityRegulationMode.Complexifying:
                    txtSearchStatsMode.BackColor = Color.LightSkyBlue;
                    break;
                case ComplexityRegulationMode.Simplifying:
                    txtSearchStatsMode.BackColor = Color.Tomato;
                    break;
            }

            txtStatsGeneration.Text = _ea.CurrentGeneration.ToString("N0");
            txtStatsBest.Text = stats._maxFitness.ToString();

            // Auxiliary fitness info.
            var auxFitnessArr = _ea.CurrentChampGenome.EvaluationInfo.AuxFitnessArr;
            if (auxFitnessArr.Length > 0)
            {
                txtStatsAlternativeFitness.Text = auxFitnessArr[0]._value.ToString("#.######");
            }
            else
            {
                txtStatsAlternativeFitness.Text = "";
            }

            txtStatsMean.Text = stats._meanFitness.ToString("#.######");
            txtSpecieChampMean.Text = stats._meanSpecieChampFitness.ToString("#.######");
            txtStatsTotalEvals.Text = stats._totalEvaluationCount.ToString("N0");
            txtStatsEvalsPerSec.Text = stats._evaluationsPerSec.ToString("##,#.##");
            txtStatsBestGenomeComplx.Text = _ea.CurrentChampGenome.Complexity.ToString("N0");
            txtStatsMeanGenomeComplx.Text = stats._meanComplexity.ToString("#.##");
            txtStatsMaxGenomeComplx.Text = stats._maxComplexity.ToString("N0");

            var totalOffspringCount = stats._totalOffspringCount;
            txtStatsTotalOffspringCount.Text = totalOffspringCount.ToString("N0");
            txtStatsAsexualOffspringCount.Text = string.Format("{0:N0} ({1:P})", stats._asexualOffspringCount,
                (stats._asexualOffspringCount/(double) totalOffspringCount));
            txtStatsCrossoverOffspringCount.Text = string.Format("{0:N0} ({1:P})", stats._sexualOffspringCount,
                (stats._sexualOffspringCount/(double) totalOffspringCount));
            txtStatsInterspeciesOffspringCount.Text = string.Format("{0:N0} ({1:P})", stats._interspeciesOffspringCount,
                (stats._interspeciesOffspringCount/(double) totalOffspringCount));
        }

        private void UpdateGuiState_ResetStats()
        {
            txtSearchStatsMode.Text = string.Empty;
            txtSearchStatsMode.BackColor = Color.LightSkyBlue;
            txtStatsGeneration.Text = string.Empty;
            txtStatsBest.Text = string.Empty;
            txtStatsAlternativeFitness.Text = string.Empty;
            txtStatsMean.Text = string.Empty;
            txtSpecieChampMean.Text = string.Empty;
            txtStatsTotalEvals.Text = string.Empty;
            txtStatsEvalsPerSec.Text = string.Empty;
            txtStatsBestGenomeComplx.Text = string.Empty;
            txtStatsMeanGenomeComplx.Text = string.Empty;
            txtStatsMaxGenomeComplx.Text = string.Empty;
            txtStatsTotalOffspringCount.Text = string.Empty;
            txtStatsAsexualOffspringCount.Text = string.Empty;
            txtStatsCrossoverOffspringCount.Text = string.Empty;
            txtStatsInterspeciesOffspringCount.Text = string.Empty;
        }

        #endregion

        #region GUI Wiring [Menu Bar - Population & Genome Loading/Saving]

        private void loadPopulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var popFilePath = SelectFileToOpen("Load population", "pop.xml", "(*.pop.xml)|*.pop.xml");
            if (string.IsNullOrEmpty(popFilePath))
            {
                return;
            }

            try
            {
                // Get the currently selected experiment.
                IExperiment<NeatGenome> experiment = GetSelectedExperiment();

                // Load population of genomes from file.
                List<NeatGenome> genomeList;
                using (var xr = XmlReader.Create(popFilePath))
                {
                    genomeList = experiment.LoadPopulation(xr);
                }

                if (genomeList.Count == 0)
                {
                    __log.WarnFormat("No genomes loaded from population file [{0}]", popFilePath);
                    return;
                }

                // Assign genome list and factory to class variables and update GUI.
                _genomeFactory = genomeList[0].GenomeFactory;
                _genomeList = genomeList;
                UpdateGuiState();
            }
            catch (Exception ex)
            {
                __log.ErrorFormat("Error loading population. Error message [{0}]", ex.Message);
            }
        }

        private void loadSeedGenomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = SelectFileToOpen("Load seed genome", "gnm.xml", "(*.gnm.xml)|*.gnm.xml");
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            // Parse population size from GUI field.
            var popSize = ParseInt(txtParamPopulationSize);
            if (null == popSize)
            {
                return;
            }

            try
            {
                // Get the currently selected experiment.
                IExperiment<NeatGenome> experiment = GetSelectedExperiment();

                // Load genome from file.
                List<NeatGenome> genomeList;
                using (var xr = XmlReader.Create(filePath))
                {
                    genomeList = experiment.LoadPopulation(xr);
                }

                if (genomeList.Count == 0)
                {
                    __log.WarnFormat("No genome loaded from file [{0}]", filePath);
                    return;
                }

                // Create genome list from seed, assign to local variables and update GUI.
                _genomeFactory = genomeList[0].GenomeFactory;
                _genomeList = _genomeFactory.CreateGenomeList(popSize.Value, 0u, genomeList[0]);
                UpdateGuiState();
            }
            catch (Exception ex)
            {
                __log.ErrorFormat("Error loading seed genome. Error message [{0}]", ex.Message);
            }
        }

        private void loadSeedGenomesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var popFilePath = SelectFileToOpen("Load seed genomes", "pop.xml", "(*.pop.xml)|*.pop.xml");
            if (string.IsNullOrEmpty(popFilePath))
            {
                return;
            }

            // Parse population size from GUI field.
            var popSize = ParseInt(txtParamPopulationSize);
            if (null == popSize)
            {
                return;
            }

            try
            {
                // Get the currently selected experiment.
                IExperiment<NeatGenome> experiment = GetSelectedExperiment();

                // Load genome from file.
                List<NeatGenome> genomeList;
                using (var xr = XmlReader.Create(popFilePath))
                {
                    genomeList = experiment.LoadPopulation(xr);
                }

                if (genomeList.Count == 0)
                {
                    __log.WarnFormat("No seed genomes loaded from file [{0}]", popFilePath);
                    return;
                }

                // Create genome list from seed genomes, assign to local variables and update GUI.
                _genomeFactory = genomeList[0].GenomeFactory;
                _genomeList = _genomeFactory.CreateGenomeList(popSize.Value, 0u, genomeList);
                UpdateGuiState();
            }
            catch (Exception ex)
            {
                __log.ErrorFormat("Error loading seed genomes. Error message [{0}]", ex.Message);
            }
        }

        private void savePopulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var popFilePath = SelectFileToSave("Save population", "pop.xml", "(*.pop.xml)|*.pop.xml");
            if (string.IsNullOrEmpty(popFilePath))
            {
                return;
            }

            try
            {
                // Get the currently selected experiment.
                IExperiment<NeatGenome> experiment = GetSelectedExperiment();

                // Save genomes to xml file.
                using (var xw = XmlWriter.Create(popFilePath, _xwSettings))
                {
                    experiment.SavePopulation(xw, _genomeList);
                }
            }
            catch (Exception ex)
            {
                __log.ErrorFormat("Error saving population. Error message [{0}]", ex.Message);
            }
        }

        private void saveBestGenomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = SelectFileToSave("Save champion genome", "gnm.xml", "(*.gnm.xml)|*.gnm.xml");
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            try
            {
                // Get the currently selected experiment.
                var experiment = GetSelectedExperiment();

                // Save genome to xml file.
                using (var xw = XmlWriter.Create(filePath, _xwSettings))
                {
                    experiment.SavePopulation(xw, new[] {_ea.CurrentChampGenome});
                }
            }
            catch (Exception ex)
            {
                __log.ErrorFormat("Error saving genome. Error message [{0}]", ex.Message);
            }
        }

        #endregion

        #region GUI Wiring [Menu Bar - Views - Graphs]

        private void fitnessBestMeansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            // Create data sources.
            var _dsList = new List<TimeSeriesDataSource>();


            _dsList.Add(new TimeSeriesDataSource("Best", TimeSeriesDataSource.DefaultHistoryLength, 0, Color.Red,
                delegate {
                    return new Point2DDouble(_ea.CurrentGeneration, _ea.Statistics._maxFitness);
                }));

            _dsList.Add(new TimeSeriesDataSource("Mean", TimeSeriesDataSource.DefaultHistoryLength, 0, Color.Black,
                delegate {
                    return new Point2DDouble(_ea.CurrentGeneration, _ea.Statistics._meanFitness);
                }));

            _dsList.Add(new TimeSeriesDataSource("Best (Moving Average)", TimeSeriesDataSource.DefaultHistoryLength, 0,
                Color.Orange, delegate {
                    return new Point2DDouble(_ea.CurrentGeneration, _ea.Statistics._bestFitnessMA.Mean);
                }));

            // Create a data sources for any auxiliary fitness info.
            var auxFitnessArr = _ea.CurrentChampGenome.EvaluationInfo.AuxFitnessArr;
            if (null != auxFitnessArr)
            {
                for (var i = 0; i < auxFitnessArr.Length; i++)
                {
                    // 'Capture' the value of i in a locally defined variable that has scope specific to each delegate creation (below). If capture 'i' instead then it will always have
                    // its last value in each delegate (which happens to be one past the end of the array).
                    var ii = i;
                    _dsList.Add(new TimeSeriesDataSource(_ea.CurrentChampGenome.EvaluationInfo.AuxFitnessArr[i]._name,
                        TimeSeriesDataSource.DefaultHistoryLength, 0, _plotColorArr[i%_plotColorArr.Length], delegate
                        {
                            return new Point2DDouble(_ea.CurrentGeneration,
                                _ea.CurrentChampGenome.EvaluationInfo.AuxFitnessArr[ii]._value);
                        }));
                }
            }

            // Create form.
            var graphForm = new TimeSeriesGraphForm("Fitness (Best and Mean)", "Generation", "Fitness",
                string.Empty, _dsList.ToArray(), _driver);
            _timeSeriesGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _timeSeriesGraphFormList.Remove(senderObj as TimeSeriesGraphForm);
                fitnessBestMeansToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            fitnessBestMeansToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void complexityBestMeansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            // Create data sources.
            var dsBestCmplx = new TimeSeriesDataSource("Best",
                TimeSeriesDataSource.DefaultHistoryLength, 0, Color.Red, delegate {
                    return new Point2DDouble(_ea.CurrentGeneration, _ea.CurrentChampGenome.Complexity);
                });

            var dsMeanCmplx = new TimeSeriesDataSource("Mean",
                TimeSeriesDataSource.DefaultHistoryLength, 0, Color.Black, delegate {
                    return new Point2DDouble(_ea.CurrentGeneration, _ea.Statistics._meanComplexity);
                });

            var dsMeanCmplxMA = new TimeSeriesDataSource("Mean (Moving Average)",
                TimeSeriesDataSource.DefaultHistoryLength, 0, Color.Orange, delegate {
                    return new Point2DDouble(_ea.CurrentGeneration, _ea.Statistics._complexityMA.Mean);
                });

            // Create form.
            var graphForm = new TimeSeriesGraphForm("Complexity (Best and Mean)", "Generation",
                "Complexity", string.Empty,
                new[] {dsBestCmplx, dsMeanCmplx, dsMeanCmplxMA}, _driver);
            _timeSeriesGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _timeSeriesGraphFormList.Remove(senderObj as TimeSeriesGraphForm);
                complexityBestMeansToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            complexityBestMeansToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void evaluationsPerSecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            // Create data sources.
            var dsEvalsPerSec = new TimeSeriesDataSource("Evals Per Sec",
                TimeSeriesDataSource.DefaultHistoryLength, 0, Color.Black, delegate {
                    return new Point2DDouble(_ea.CurrentGeneration, _ea.Statistics._evaluationsPerSec);
                });
            // Create form.
            var graphForm = new TimeSeriesGraphForm("Evaluations Per Second", "Generation",
                "Evaluations", string.Empty,
                new[] {dsEvalsPerSec}, _driver);
            _timeSeriesGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _timeSeriesGraphFormList.Remove(senderObj as TimeSeriesGraphForm);
                evaluationsPerSecToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            evaluationsPerSecToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void specieSizeByRankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsSpecieSizeRank = new SummaryDataSource("Specie Size", 0, Color.Red, delegate
            {
                // Ensure temp working storage is ready.
                var specieCount = _ea.SpecieList.Count;
                if (null == _specieDataArrayInt || _specieDataArrayInt.Length != specieCount)
                {
                    _specieDataArrayInt = new int[specieCount];
                }

                // Copy specie sizes into _specieSizeArray.
                for (var i = 0; i < specieCount; i++)
                {
                    _specieDataArrayInt[i] = _ea.SpecieList[i].GenomeList.Count;
                }

                // Build/create _specieSizePointArray from the _specieSizeArray.
                UpdateRankedDataPoints(_specieDataArrayInt, ref _specieDataPointArrayInt);

                // Return plot points.
                return _specieDataPointArrayInt;
            });
            // Create form.
            var graphForm = new SummaryGraphForm("Specie Size by Rank", "Species (largest to smallest)",
                "Size", string.Empty,
                new[] {dsSpecieSizeRank}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                specieSizeByRankToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            specieSizeByRankToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void specieChampFitnessByRankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsSpecieChampFitnessRank = new SummaryDataSource("Specie Fitness (Champs)", 0, Color.Red,
                delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie fitnesses into the data array.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].GenomeList[0].EvaluationInfo.Fitness;
                    }

                    // Build/create point array.
                    UpdateRankedDataPoints(_specieDataArray, ref _specieDataPointArray);

                    // Return plot points.
                    return _specieDataPointArray;
                });

            var dsSpecieMeanFitnessRank = new SummaryDataSource("Specie Fitness (Means)", 0, Color.Black,
                delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie fitnesses into the data array.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].CalcMeanFitness();
                    }

                    // Build/create point array.
                    UpdateRankedDataPoints(_specieDataArray, ref _specieDataPointArray);

                    // Return plot points.
                    return _specieDataPointArray;
                });


            // Create form.
            var graphForm = new SummaryGraphForm("Specie Fitness by Rank", "Species", "Fitness",
                string.Empty,
                new[] {dsSpecieChampFitnessRank, dsSpecieMeanFitnessRank}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                specieChampFitnessByRankToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            specieChampFitnessByRankToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void specieChampComplexityByRankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsSpecieChampComplexityRank = new SummaryDataSource("Specie Complexity (Champs)", 0,
                Color.Red, delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie complexity values into the data array.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].GenomeList[0].Complexity;
                    }

                    // Build/create point array.
                    UpdateRankedDataPoints(_specieDataArray, ref _specieDataPointArray);

                    // Return plot points.
                    return _specieDataPointArray;
                });

            var dsSpecieMeanComplexityRank = new SummaryDataSource("Specie Complexity (Means)", 0,
                Color.Black, delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie complexity values into the data array.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].CalcMeanComplexity();
                    }

                    // Build/create point array.
                    UpdateRankedDataPoints(_specieDataArray, ref _specieDataPointArray);

                    // Return plot points.
                    return _specieDataPointArray;
                });


            // Create form.
            var graphForm = new SummaryGraphForm("Specie Complexity by Rank", "Species", "Complexity",
                string.Empty,
                new[] {dsSpecieChampComplexityRank, dsSpecieMeanComplexityRank}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                specieChampComplexityByRankToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            specieChampComplexityByRankToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void genomeFitnessByRankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsGenomeFitnessRank = new SummaryDataSource("Genome Fitness", 0, Color.Red, delegate
            {
                // Ensure temp working storage is ready.
                var genomeCount = _ea.GenomeList.Count;
                if (null == _genomeDataArray || _genomeDataArray.Length != genomeCount)
                {
                    _genomeDataArray = new double[genomeCount];
                }

                // Copy genome fitness values into the data array.
                for (var i = 0; i < genomeCount; i++)
                {
                    _genomeDataArray[i] = _ea.GenomeList[i].EvaluationInfo.Fitness;
                }

                // Build/create point array.
                UpdateRankedDataPoints(_genomeDataArray, ref _genomeDataPointArray);

                // Return plot points.
                return _genomeDataPointArray;
            });
            // Create form.
            var graphForm = new SummaryGraphForm("Genome Fitness by Rank", "Genomes", "Fitness",
                string.Empty,
                new[] {dsGenomeFitnessRank}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                genomeFitnessByRankToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            genomeFitnessByRankToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void genomeComplexityByRankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsGenomeComplexityRank = new SummaryDataSource("Genome Complexity", 0, Color.Red,
                delegate
                {
                    // Ensure temp working storage is ready.
                    var genomeCount = _ea.GenomeList.Count;
                    if (null == _genomeDataArray || _genomeDataArray.Length != genomeCount)
                    {
                        _genomeDataArray = new double[genomeCount];
                    }

                    // Copy genome complexity values into the data array.
                    for (var i = 0; i < genomeCount; i++)
                    {
                        _genomeDataArray[i] = _ea.GenomeList[i].Complexity;
                    }

                    // Build/create point array.
                    UpdateRankedDataPoints(_genomeDataArray, ref _genomeDataPointArray);

                    // Return plot points.
                    return _genomeDataPointArray;
                });
            // Create form.
            var graphForm = new SummaryGraphForm("Genome Complexity by Rank", "Genomes", "Complexity",
                string.Empty,
                new[] {dsGenomeComplexityRank}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                genomeComplexityByRankToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            genomeComplexityByRankToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void specieSizeDistributionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsSpecieSizeDist = new SummaryDataSource("Specie Size Histogram", 0, Color.Red, delegate
            {
                // Ensure temp working storage is ready.
                var specieCount = _ea.SpecieList.Count;
                if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                {
                    _specieDataArray = new double[specieCount];
                }

                // Copy specie sizes into _specieSizeArray.
                for (var i = 0; i < specieCount; i++)
                {
                    _specieDataArray[i] = _ea.SpecieList[i].GenomeList.Count;
                }

                // Calculate a frequency distribution and retrieve it as an array of plottable points.
                var pointArr = CalcDistributionDataPoints(_specieDataArray);

                // Return plot points.
                return pointArr;
            });
            // Create form.
            var graphForm = new SummaryGraphForm("Specie Size Frequency Histogram", "Species Size",
                "Frequency", string.Empty,
                new[] {dsSpecieSizeDist}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                specieSizeDistributionToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            specieSizeDistributionToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void specieFitnessDistributionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsSpecieChampFitnessDist = new SummaryDataSource("Specie Fitness Histogram (Champ)", 0,
                Color.Red, delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie sizes into _specieSizeArray.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].GenomeList[0].EvaluationInfo.Fitness;
                    }

                    // Calculate a frequency distribution and retrieve it as an array of plottable points.
                    var pointArr = CalcDistributionDataPoints(_specieDataArray);

                    // Return plot points.
                    return pointArr;
                });

            var dsSpecieMeanFitnessDist = new SummaryDataSource("Specie Fitness Histogram (Mean)", 0,
                Color.Black, delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie sizes into _specieSizeArray.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].CalcMeanFitness();
                    }

                    // Calculate a frequency distribution and retrieve it as an array of plottable points.
                    var pointArr = CalcDistributionDataPoints(_specieDataArray);

                    // Return plot points.
                    return pointArr;
                });
            // Create form.
            var graphForm = new SummaryGraphForm("Specie Fitness Histogram", "Fitness", "Frequency",
                string.Empty,
                new[] {dsSpecieChampFitnessDist, dsSpecieMeanFitnessDist}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                specieFitnessDistributionsToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            specieFitnessDistributionsToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void specieComplexityDistributionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsSpecieChampComplexityDist = new SummaryDataSource(
                "Specie Complexity Histogram (Champ)", 0, Color.Red, delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie sizes into _specieSizeArray.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].GenomeList[0].Complexity;
                    }

                    // Calculate a frequency distribution and retrieve it as an array of plottable points.
                    var pointArr = CalcDistributionDataPoints(_specieDataArray);

                    // Return plot points.
                    return pointArr;
                });

            var dsSpecieMeanComplexityDist = new SummaryDataSource("Specie Complexity Histogram (Mean)", 0,
                Color.Black, delegate
                {
                    // Ensure temp working storage is ready.
                    var specieCount = _ea.SpecieList.Count;
                    if (null == _specieDataArray || _specieDataArray.Length != specieCount)
                    {
                        _specieDataArray = new double[specieCount];
                    }

                    // Copy specie sizes into _specieSizeArray.
                    for (var i = 0; i < specieCount; i++)
                    {
                        _specieDataArray[i] = _ea.SpecieList[i].CalcMeanComplexity();
                    }

                    // Calculate a frequency distribution and retrieve it as an array of plottable points.
                    var pointArr = CalcDistributionDataPoints(_specieDataArray);

                    // Return plot points.
                    return pointArr;
                });
            // Create form.
            var graphForm = new SummaryGraphForm("Specie Complexity Histogram", "Complexity", "Frequency",
                string.Empty,
                new[] {dsSpecieChampComplexityDist, dsSpecieMeanComplexityDist}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                specieComplexityDistributionsToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            specieComplexityDistributionsToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void genomeFitnessDistributionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsGenomeFitnessDist = new SummaryDataSource("Genome Fitness Histogram", 0, Color.Red,
                delegate
                {
                    // Ensure temp working storage is ready.
                    var genomeCount = _ea.GenomeList.Count;
                    if (null == _genomeDataArray || _genomeDataArray.Length != genomeCount)
                    {
                        _genomeDataArray = new double[genomeCount];
                    }

                    // Copy genome fitness values into the data array.
                    for (var i = 0; i < genomeCount; i++)
                    {
                        _genomeDataArray[i] = _ea.GenomeList[i].EvaluationInfo.Fitness;
                    }

                    // Calculate a frequency distribution and retrieve it as an array of plottable points.
                    var pointArr = CalcDistributionDataPoints(_genomeDataArray);

                    // Return plot points.
                    return pointArr;
                });
            // Create form.
            var graphForm = new SummaryGraphForm("Genome Fitness Histogram", "Fitness", "Frequency",
                string.Empty,
                new[] {dsGenomeFitnessDist}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                genomeFitnessDistributionToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            genomeFitnessDistributionToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        private void genomeComplexityDistributionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _ea) return;

            var dsGenomeComplexityDist = new SummaryDataSource("Genome Complexity Histogram", 0, Color.Red,
                delegate
                {
                    // Ensure temp working storage is ready.
                    var genomeCount = _ea.GenomeList.Count;
                    if (null == _genomeDataArray || _genomeDataArray.Length != genomeCount)
                    {
                        _genomeDataArray = new double[genomeCount];
                    }

                    // Copy genome fitness values into the data array.
                    for (var i = 0; i < genomeCount; i++)
                    {
                        _genomeDataArray[i] = _ea.GenomeList[i].Complexity;
                    }

                    // Calculate a frequency distribution and retrieve it as an array of plottable points.
                    var pointArr = CalcDistributionDataPoints(_genomeDataArray);

                    // Return plot points.
                    return pointArr;
                });
            // Create form.
            var graphForm = new SummaryGraphForm("Genome Complexity Histogram", "Complexity", "Frequency",
                string.Empty,
                new[] {dsGenomeComplexityDist}, _driver);
            _summaryGraphFormList.Add(graphForm);

            // Attach a event handler to update this main form when the graph form is closed.
            graphForm.FormClosed += delegate(object senderObj, FormClosedEventArgs eArgs)
            {
                _summaryGraphFormList.Remove(senderObj as SummaryGraphForm);
                genomeComplexityDistributionToolStripMenuItem.Enabled = true;
            };

            // Prevent creating more then one instance fo the form.
            genomeComplexityDistributionToolStripMenuItem.Enabled = false;

            // Show the form.
            graphForm.Show(this);
        }

        #endregion

        #region GUI Wiring [Misc Menu Bar & Button Event Handlers]

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmAboutBox = new AboutForm();
            frmAboutBox.ShowDialog(this);
        }

        private void btnCopyLogToClipboard_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Logger.LogItem item in lbxLog.Items)
            {
                sb.AppendLine(item.Message);
            }

            if (sb.Length > 0)
            {
                Clipboard.SetText(sb.ToString());
            }
        }

        #endregion

        #region GUI Wiring [Update/Pause Event Handlers + Form Close Handler]

        private void _ea_UpdateEvent(object sender, EventArgs e)
        {
            // Handle writing to log window. Switch execution to GUI thread if necessary.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    // Update stats on screen.
                    UpdateGuiState_EaStats();

                    // Write entry to log window.
                    __log.Info($"gen={_ea.CurrentGeneration:N0} bestFitness={_ea.Statistics._maxFitness:N6}");

                    // Check if we should save the champ genome to a file.
                    var champGenome = _ea.CurrentChampGenome;
                    if (chkFileSaveGenomeOnImprovement.Checked &&
                        champGenome.EvaluationInfo.Fitness > _champGenomeFitness)
                    {
                        _champGenomeFitness = champGenome.EvaluationInfo.Fitness;
                        var filename = string.Format(_filenameNumberFormatter,
                            "{0}_{1:0.00}_{2:yyyyMMdd_HHmmss}.gnm.xml",
                            txtFileBaseName.Text, _champGenomeFitness, DateTime.Now);

                        // Get the currently selected experiment.
                        IExperiment<NeatGenome> experiment = GetSelectedExperiment();

                        // Save genome to xml file.
                        using (var xw = XmlWriter.Create(filename, _xwSettings))
                        {
                            experiment.SavePopulation(xw, new[] {champGenome});
                        }
                    }
                }));
            }

            // Handle writing to log file.
            if (null != _logFileWriter)
            {
                var stats = _ea.Statistics;
                _logFileWriter.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", DateTime.Now, stats._generation, stats._maxFitness, stats._meanFitness, stats._meanSpecieChampFitness, _ea.CurrentChampGenome.Complexity, stats._meanComplexity, stats._maxComplexity, stats._totalEvaluationCount, stats._evaluationsPerSec, _ea.ComplexityRegulationMode);
                _logFileWriter.Flush();
            }
        }

        private void _ea_PausedEvent(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate {
                    UpdateGuiState();
                }));
            }
        }

        /// <summary>
        /// Gracefully handle application exit request.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != _ea && _driver.RunState == RunState.Running)
            {
                var result = MessageBox.Show("Evolution algorithm is still running. Exit anyway?", "Exit?",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    // Cancel closing of application.
                    e.Cancel = true;
                    return;
                }
            }

            if (null != _ea)
            {
                // Detach event handlers to prevent logging attempts to GUI as it is being torn down.
                _driver.UpdateEvent -= _ea_UpdateEvent;
                _driver.PausedEvent -= _ea_PausedEvent;

                if (_driver.RunState == RunState.Running)
                {
                    // Request algorithm to stop but don't wait.
                    _driver.RequestPause();
                }

                // Close log file.
                if (null != _logFileWriter)
                {
                    _logFileWriter.Close();
                    _logFileWriter = null;
                }
            }
        }

        #endregion

        #region Private Methods [Misc Helper Methods]

        /// <summary>
        /// Ask the user for a filename / path.
        /// </summary>
        private string SelectFileToOpen(string dialogTitle, string fileExtension, string filter)
        {
            var oDialog = new OpenFileDialog();
            oDialog.AddExtension = true;
            oDialog.DefaultExt = fileExtension;
            oDialog.Filter = filter;
            oDialog.Title = dialogTitle;
            oDialog.RestoreDirectory = true;

            // Show dialog and block until user selects a file.
            if (oDialog.ShowDialog() == DialogResult.OK)
            {
                return oDialog.FileName;
            }
            // No selection.
            return null;
        }

        /// <summary>
        /// Ask the user for a filename / path.
        /// </summary>
        private string SelectFileToSave(string dialogTitle, string fileExtension, string filter)
        {
            var oDialog = new SaveFileDialog();
            oDialog.AddExtension = true;
            oDialog.DefaultExt = fileExtension;
            oDialog.Filter = filter;
            oDialog.Title = dialogTitle;
            oDialog.RestoreDirectory = true;

            // Show dialog and block until user selects a file.
            if (oDialog.ShowDialog() == DialogResult.OK)
            {
                return oDialog.FileName;
            }
            // No selection.
            return null;
        }

        private int? ParseInt(TextBox txtBox)
        {
            int val;
            if (int.TryParse(txtBox.Text, out val))
            {
                return val;
            }
            __log.ErrorFormat("Error parsing value of text field [{0}]", txtBox.Name);
            return null;
        }

        private int ParseInt(TextBox txtBox, int defaultVal)
        {
            int val;
            if (int.TryParse(txtBox.Text, out val))
            {
                return val;
            }
            __log.ErrorFormat("Error parsing value of text field [{0}]", txtBox.Name);
            return defaultVal;
        }

        private double? ParseDouble(TextBox txtBox)
        {
            double val;
            if (double.TryParse(txtBox.Text, out val))
            {
                return val;
            }
            __log.ErrorFormat("Error parsing value of text field [{0}]", txtBox.Name);
            return null;
        }

        private double ParseDouble(TextBox txtBox, double defaultVal)
        {
            double val;
            if (double.TryParse(txtBox.Text, out val))
            {
                return val;
            }
            __log.ErrorFormat("Error parsing value of text field [{0}]", txtBox.Name);
            return defaultVal;
        }

        /// <summary>
        /// Updates an Point2DDouble array by sorting an array of values and copying the sorted values over the existing values in pointArr.
        /// Optionally creates the Point2DDouble array if it is null or is the wrong size.
        /// </summary>
        private void UpdateRankedDataPoints(int[] valArr, ref Point2DDouble[] pointArr)
        {

            // Sort values (largest first).
            Array.Sort(valArr, delegate(int x, int y)
            {
                if (x > y)
                {
                    return -1;
                }
                if (x < y)
                {
                    return 1;
                }
                return 0;
            });

            // Ensure point cache is ready.
            if (null == pointArr || pointArr.Length != valArr.Length)
            {
                pointArr = new Point2DDouble[valArr.Length];
                for (var i = 0; i < valArr.Length; i++)
                {
                    pointArr[i].X = i;
                    pointArr[i].Y = valArr[i];
                }
            }
            else
            {
                // Copy sorted values into _specieSizePointArray.
                for (var i = 0; i < valArr.Length; i++)
                {
                    pointArr[i].Y = valArr[i];
                }
            }
        }

        /// <summary>
        /// Updates an Point2DDouble array by sorting an array of values and copying the sorted values over the existing values in pointArr.
        /// Optionally creates the Point2DDouble array if it is null or is the wrong size.
        /// </summary>
        private void UpdateRankedDataPoints(double[] valArr, ref Point2DDouble[] pointArr)
        {

            // Sort values (largest first).
            Array.Sort(valArr, delegate(double x, double y)
            {
                if (x > y)
                {
                    return -1;
                }
                if (x < y)
                {
                    return 1;
                }
                return 0;
            });

            // Ensure point cache is ready.
            if (null == pointArr || pointArr.Length != valArr.Length)
            {
                pointArr = new Point2DDouble[valArr.Length];
                for (var i = 0; i < valArr.Length; i++)
                {
                    pointArr[i].X = i;
                    pointArr[i].Y = valArr[i];
                }
            }
            else
            {
                // Copy sorted values into _specieSizePointArray.
                for (var i = 0; i < valArr.Length; i++)
                {
                    pointArr[i].Y = valArr[i];
                }
            }
        }

        private Point2DDouble[] CalcDistributionDataPoints(double[] valArr)
        {
            // Square root is a fairly good choice for automatically determining the category count based on number of values being analysed.
            // See http://en.wikipedia.org/wiki/Histogram (section: Number of bins and width).
            var categoryCount = (int) Math.Sqrt(valArr.Length);
            var hd = NumericsUtils.BuildHistogramData(valArr, categoryCount);

            // Create array of distribution plot points.
            var pointArr = new Point2DDouble[hd.FrequencyArray.Length];
            var incr = hd.Increment;
            var x = hd.Min + (incr/2.0);

            for (var i = 0; i < hd.FrequencyArray.Length; i++, x += incr)
            {
                pointArr[i].X = x;
                pointArr[i].Y = hd.FrequencyArray[i];
            }
            return pointArr;
        }

        private Color[] GenerateNiceColors(int count)
        {
            var arr = new Color[count];
            var baseColor = ColorTranslator.FromHtml("#8A56E2");
            var baseHue = (new HSLColor(baseColor)).Hue;

            var colorList = new List<Color>();
            colorList.Add(baseColor);

            var step = (240.0/count);
            for (var i = 1; i < count; i++)
            {
                var nextColor = new HSLColor(baseColor);
                nextColor.Hue = (baseHue + step*i)%240.0;
                colorList.Add(nextColor);
            }
            return colorList.ToArray();
        }

        #endregion
    }
}
