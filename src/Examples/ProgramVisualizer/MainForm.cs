// ------------------------------------------
// <copyright file="MainForm.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: ProgramVisualizer
//    Last updated: 04/02/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Genetica.Elements;
using Genetica.Elements.Terminals;
using Genetica.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace ProgramVisualizer
{
    public partial class MainForm : Form
    {
        #region Static Fields & Constants

        private const GraphvizImageType GRAPHVIZ_IMAGE_TYPE = GraphvizImageType.Png;
        private const int PROPERTY_GRID_HELP_LINES = 8;

        #endregion

        #region Fields

        private readonly MathExpressionConverter _converter;
        private MathProgram _program;

        #endregion

        #region Constructors

        public MainForm()
        {
            this.InitializeComponent();

            // creates primitives
            var terminals = new List<MathProgram>();
            for (var c = 'a'; c <= 'z'; c++) terminals.Add(new Variable(c.ToString()));
            var primitives = new PrimitiveSet<MathProgram>(terminals, MathPrimitiveSets.Default.Functions);

            // creates converter
            this._converter = new MathExpressionConverter(primitives);
        }

        #endregion

        #region Private & Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            this.ResizeDescriptionArea(PROPERTY_GRID_HELP_LINES);
            this.OnShown(e);
        }

        #endregion

        #region Private & Protected Methods

        private void ClearBtnClick(object sender, EventArgs e)
        {
            this.expressionTxtBox.Text = string.Empty;
            this.pictureBox.Image = null;
            this.exportBtn.Enabled = false;
        }

        private void ExportBtnClick(object sender, EventArgs e)
        {
            if (this._program == null) return;

            // shows dialog
            if (this.saveImageFileDialog.ShowDialog() != DialogResult.OK) return;

            // gets file name
            var basePath = Path.GetDirectoryName(this.saveImageFileDialog.FileName);
            var fileName = Path.GetFileNameWithoutExtension(this.saveImageFileDialog.FileName);
            var extension = Path.GetExtension(this.saveImageFileDialog.FileName)?.Remove(0, 1) ??
                            GRAPHVIZ_IMAGE_TYPE.GetOutputFormatStr();
            if (!Enum.TryParse(extension, true, out GraphvizImageType imgType)) imgType = GRAPHVIZ_IMAGE_TYPE;

            // converts to Graphviz
            var dotFile = this._program.ToGraphvizFile(basePath, fileName, imgType);
            var imgFile = $"{dotFile}.{imgType.GetOutputFormatStr()}";

            // checks image file
            if (File.Exists(imgFile))
                MessageBox.Show($"File successfully exported to:\n{imgFile}", "Export successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Could not save file.\nCheck that you have Graphviz installed.", "Export failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void GenerateBtnClick(object sender, EventArgs e)
        {
            this.exportBtn.Enabled = false;
            this._program = null;

            // checks expression
            if (string.IsNullOrWhiteSpace(this.expressionTxtBox.Text))
            {
                MessageBox.Show(
                    "Expression cannot be empty.", "Expression error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // converts to program
            this._program = this.normalRadBtn.Checked
                ? this._converter.FromNormalNotation(this.expressionTxtBox.Text)
                : this._converter.FromPrefixNotation(this.expressionTxtBox.Text);

            // checks program
            if (this._program == null)
            {
                MessageBox.Show(
                    "Invalid expression provided.", "Expression error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // converts to Graphviz
            var dotFile = this._program.ToGraphvizFile(Path.GetFullPath("."), "program", GRAPHVIZ_IMAGE_TYPE);

            // checks dot
            if (!File.Exists(dotFile))
            {
                MessageBox.Show(
                    "Could not generate file.", "File output error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // updates program properties
            this.propertyGrid.SelectedObject = new ProgramInfo(this._converter, this._program);

            // checks and loads image
            var imageFile = $"{dotFile}.{GRAPHVIZ_IMAGE_TYPE.GetOutputFormatStr()}";
            this.pictureBox.Image?.Dispose();
            if (!File.Exists(imageFile))
            {
                MessageBox.Show(
                    "Could not generate image file.\nCheck that you have Graphviz installed.", "File output error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Image img;
                using (var bmpTemp = new Bitmap(imageFile))
                    img = new Bitmap(bmpTemp);
                this.pictureBox.Image = img;
                this.exportBtn.Enabled = true;
            }
        }

        /// <remarks>
        ///     <see href="https://stackoverflow.com/a/2254976" />
        /// </remarks>
        private void ResizeDescriptionArea(int lines)
        {
            try
            {
                var info = this.propertyGrid.GetType().GetProperty("Controls");
                if (info == null) return;
                var collection = (Control.ControlCollection) info.GetValue(this.propertyGrid, null);

                foreach (var control in collection)
                {
                    var type = control.GetType();
                    if ("DocComment" != type.Name) continue;

                    const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                    if (type.BaseType != null)
                    {
                        var field = type.BaseType.GetField("userSized", flags);
                        if (field != null) field.SetValue(control, true);
                    }

                    info = type.GetProperty("Lines");
                    if (info != null) info.SetValue(control, lines, null);

                    break;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region Nested type: ProgramInfo

        internal class ProgramInfo
        {
            #region Constructors

            internal ProgramInfo(MathExpressionConverter converter, MathProgram program)
            {
                this.NormalExpression = converter.ToNormalNotation(program);
                this.PrefixExpression = converter.ToPrefixNotation(program);
                this.Length = program.Length;
                this.Breadth = program.GetMaxBreadth();
                this.Depth = program.GetMaxDepth();
                this.SubPrograms = program.GetSubPrograms()
                    .Select(subProg => converter.ToNormalNotation((MathProgram) subProg)).ToList();
                this.SubCombinations = program.GetSubCombinations()
                    .Select(subProg => converter.ToNormalNotation((MathProgram) subProg)).ToList();
                this.Leaves = program.GetLeaves()
                    .ToDictionary(leaf => converter.ToNormalNotation((MathProgram) leaf.Key), count => count.Value);
                this.Primitives = program.GetPrimitives()
                    .ToDictionary(primitive => primitive.Key.Label, count => count.Value);
            }

            #endregion

            #region Properties & Indexers

            [Category("Properties")]
            [System.ComponentModel.Description(
                "The maximum breadth of the program, i.e., the number of leaves encountered starting from the program.")]
            public uint Breadth { get; set; }

            /// <summary>
            /// </summary>
            [Category("Properties")]
            [System.ComponentModel.Description(
                "The maximum depth of the program, i.e., the maximum number of child programs encountered starting from the " +
                "program until reaching a terminal program.")]
            public uint Depth { get; set; }

            [Category("Tree elements")]
            [System.ComponentModel.Description("Contains all the leaves of the given program and their count. " +
                                               "This corresponds to the leaf nodes of the expression tree of the program.")]
            [DisplayName("Leaves")]
            public Dictionary<string, uint> Leaves { get; }

            [Category("Properties")]
            [System.ComponentModel.Description(
                "Gets the program's length, i.e., the total number of descendant programs encountered starting from the program.")]
            public ushort Length { get; set; }

            [Category("Properties")]
            [System.ComponentModel.Description(
                "The expression in normal notation, i.e., where functions are written in the form func(arg1 arg2 ...) or (arg1 func arg2).")]
            [DisplayName("Normal Expression")]
            public string NormalExpression { get; set; }

            [Category("Properties")]
            [System.ComponentModel.Description(
                "The expression in prefix notation, i.e., where functions are written in the form (func arg1 arg2 ...).")]
            [DisplayName("Prefix Expression")]
            public string PrefixExpression { get; set; }

            [Category("Tree elements")]
            [System.ComponentModel.Description("Contains all the primitives in the program and their count.")]
            [DisplayName("Primitives")]
            public Dictionary<string, uint> Primitives { get; set; }

            [Category("Tree elements")]
            [System.ComponentModel.Description(
                "Contains all the sub-combinations of the given program. If the program is a leaf, there are no combinations, otherwise " +
                "returns all the possible combinations between the sub-programs of the children and also the sub-combinations of each child.")]
            [DisplayName("Sub-combinations")]
            public List<string> SubCombinations { get; }

            [Category("Tree elements")]
            [System.ComponentModel.Description("Contains all the descendant programs of the program.")]
            [DisplayName("Sub-programs")]
            public List<string> SubPrograms { get; }

            #endregion
        }

        #endregion
    }
}