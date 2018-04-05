// ------------------------------------------
// <copyright file="GraphvizTests.cs" company="Pedro Sequeira">
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
//    Project: Genetica.Graphviz.Tests
//    Last updated: 03/29/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
using Genetica.Elements;
using Genetica.Operators.Generation;
using Genetica.Trees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph.Graphviz.Dot;

namespace Genetica.Graphviz.Tests
{
    [TestClass]
    public class GraphvizTests
    {
        #region Static Fields & Constants

        private const string FILE_NAME = "file";
        private const string INFO_FILE_NAME = "info";
        private const string SYMB_FILE_NAME = "symb";
        private const string ORD_SYMB_FILE_NAME = "ord-symb";
        private const string SUB_PROG_FILE_NAME = "sub-prog";
        private const int WAIT_TIMEOUT = 1000;
        private const int NUM_PROGS = 10;
        private const int MAX_DEPTH = 2;

        private static readonly GraphvizImageType[] ImageTypes =
        {
            GraphvizImageType.Bmp,
            GraphvizImageType.Png,
            GraphvizImageType.Pdf,
            GraphvizImageType.Svg,
            GraphvizImageType.Svg,
            GraphvizImageType.Jpg
        };

        #endregion

        #region Public Methods

        [TestMethod]
        public void SaveFileDotTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, MAX_DEPTH);
            Console.WriteLine(prog);

            var fullPath = Path.GetFullPath(".");
            prog.ToGraphvizFile(fullPath, FILE_NAME, GraphvizImageType.Png, WAIT_TIMEOUT);
            var dotPath = Path.Combine(fullPath, $"{FILE_NAME}.dot");
            Console.WriteLine(dotPath);
            Assert.IsTrue(File.Exists(dotPath), $"Dot file should exist in {dotPath}");
        }

        [TestMethod]
        public void SaveFileImageTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, MAX_DEPTH);
            Console.WriteLine(prog);

            var fullPath = Path.GetFullPath(".");
            foreach (var imageType in ImageTypes)
            {
                var fileName = $"{FILE_NAME}-{imageType}";
                var dotPath = Path.Combine(fullPath, $"{fileName}.dot");
                var imgPath = $"{dotPath}.{imageType.ToString().ToLower()}";
                File.Delete(dotPath);
                File.Delete(imgPath);

                var filePath = prog.ToGraphvizFile(fullPath, fileName, imageType, WAIT_TIMEOUT);

                Console.WriteLine(dotPath);
                Assert.IsTrue(File.Exists(dotPath), $"Dot file should exist in {dotPath}.");
                Assert.AreEqual(filePath, dotPath, $"Dot file should be exist in {imgPath}");

                Console.WriteLine(imgPath);
                Assert.IsTrue(File.Exists(imgPath), $"Image file should exist in {imgPath}.");
                Assert.IsTrue(new FileInfo(imgPath).Length > 0, "Image size should be > 0 bytes.");

#if !DEBUG
                File.Delete(dotPath);
                File.Delete(imgPath);
#endif
            }
        }

        [TestMethod]
        public void SaveInfoTreeTest()
        {
            var generator = new FullProgramGenerator<MathProgram, double>();
            var tree = new InformationTree<MathProgram>();
            for (var i = 0; i < NUM_PROGS; i++)
            {
                var prog = generator.Generate(MathPrimitiveSets.Default, MAX_DEPTH);
                tree.AddProgram(prog);
                Console.WriteLine(prog);
            }

            var fullPath = Path.GetFullPath(".");
            var imgPath = Path.Combine(fullPath, $"{INFO_FILE_NAME}.dot.png");
            File.Delete(imgPath);

            var dotPath = tree.ToGraphvizFile(fullPath, INFO_FILE_NAME, GraphvizImageType.Png, WAIT_TIMEOUT);

            Console.WriteLine(imgPath);
            Assert.IsTrue(File.Exists(dotPath), $"Dot file with information tree should exist in {dotPath}");
            Assert.IsTrue(new FileInfo(dotPath).Length > 0, "Dot file size should be > 0 bytes.");
            Assert.IsTrue(File.Exists(imgPath), $"Image file with information tree should exist in {imgPath}");
            Assert.IsTrue(new FileInfo(imgPath).Length > 0, "Image size should be > 0 bytes.");

#if !DEBUG
            File.Delete(dotPath);
            File.Delete(imgPath);
#endif
        }

        [TestMethod]
        public void SaveOrdSymbTreeTest()
        {
            var generator = new FullProgramGenerator<MathProgram, double>();
            var tree = new OrderedSymbolTree<MathProgram>();
            for (var i = 0; i < NUM_PROGS; i++)
            {
                var prog = generator.Generate(MathPrimitiveSets.Default, MAX_DEPTH);
                tree.AddProgram(prog);
                Console.WriteLine(prog);
            }

            var fullPath = Path.GetFullPath(".");
            var imgPath = Path.Combine(fullPath, $"{ORD_SYMB_FILE_NAME}.dot.png");
            File.Delete(imgPath);

            var dotPath = tree.ToGraphvizFile(fullPath, ORD_SYMB_FILE_NAME, GraphvizImageType.Png, WAIT_TIMEOUT);

            Console.WriteLine(imgPath);
            Assert.IsTrue(File.Exists(dotPath), $"Dot file with information tree should exist in {dotPath}");
            Assert.IsTrue(new FileInfo(dotPath).Length > 0, "Dot file size should be > 0 bytes.");
            Assert.IsTrue(File.Exists(imgPath), $"Image file with information tree should exist in {imgPath}");
            Assert.IsTrue(new FileInfo(imgPath).Length > 0, "Image size should be > 0 bytes.");

#if !DEBUG
            File.Delete(dotPath);
            File.Delete(imgPath);
#endif
        }

        [TestMethod]
        public void SaveSubProgTreeTest()
        {
            var generator = new FullProgramGenerator<MathProgram, double>();
            var tree = new SubProgramTree<MathProgram, double>();
            for (var i = 0; i < NUM_PROGS; i++)
            {
                var prog = generator.Generate(MathPrimitiveSets.Default, MAX_DEPTH);
                tree.AddProgram(prog);
                Console.WriteLine(prog);
            }

            var fullPath = Path.GetFullPath(".");
            var imgPath = Path.Combine(fullPath, $"{SUB_PROG_FILE_NAME}.dot.png");
            File.Delete(imgPath);

            var dotPath = tree.ToGraphvizFile(fullPath, SUB_PROG_FILE_NAME, GraphvizImageType.Png, WAIT_TIMEOUT);

            Console.WriteLine(imgPath);
            Assert.IsTrue(File.Exists(dotPath), $"Dot file with information tree should exist in {dotPath}");
            Assert.IsTrue(new FileInfo(dotPath).Length > 0, "Dot file size should be > 0 bytes.");
            Assert.IsTrue(File.Exists(imgPath), $"Image file with information tree should exist in {imgPath}");
            Assert.IsTrue(new FileInfo(imgPath).Length > 0, "Image size should be > 0 bytes.");

#if !DEBUG
            File.Delete(dotPath);
            File.Delete(imgPath);
#endif
        }

        [TestMethod]
        public void SaveSymbTreeTest()
        {
            var generator = new FullProgramGenerator<MathProgram, double>();
            var tree = new SymbolTree<MathProgram>();
            for (var i = 0; i < NUM_PROGS; i++)
            {
                var prog = generator.Generate(MathPrimitiveSets.Default, MAX_DEPTH);
                tree.AddProgram(prog);
                Console.WriteLine(prog);
            }

            var fullPath = Path.GetFullPath(".");
            var imgPath = Path.Combine(fullPath, $"{SYMB_FILE_NAME}.dot.png");
            File.Delete(imgPath);

            var dotPath = tree.ToGraphvizFile(fullPath, SYMB_FILE_NAME, GraphvizImageType.Png, WAIT_TIMEOUT);

            Console.WriteLine(imgPath);
            Assert.IsTrue(File.Exists(dotPath), $"Dot file with information tree should exist in {dotPath}");
            Assert.IsTrue(new FileInfo(dotPath).Length > 0, "Dot file size should be > 0 bytes.");
            Assert.IsTrue(File.Exists(imgPath), $"Image file with information tree should exist in {imgPath}");
            Assert.IsTrue(new FileInfo(imgPath).Length > 0, "Image size should be > 0 bytes.");

#if !DEBUG
            File.Delete(dotPath);
            File.Delete(imgPath);
#endif
        }

        #endregion
    }
}