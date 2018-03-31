// ------------------------------------------
// <copyright file="D3Tests.cs" company="Pedro Sequeira">
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
//    Project: Genesis.D3.Tests
//    Last updated: 03/29/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
using Genesis.Elements;
using Genesis.Operators.Generation;
using Genesis.Trees;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genesis.D3.Tests
{
    [TestClass]
    public class D3Tests
    {
        #region Static Fields & Constants

        private const string FILE_NAME = "file";
        private const string FILE_TREE_NAME = "tree-file";
        private const string INFO_FILE_NAME = "info";
        private const string SYMB_FILE_NAME = "symb";
        private const string ORD_SYMB_FILE_NAME = "ord-symb";
        private const string SUB_PROG_FILE_NAME = "sub-prog";

        #endregion

        #region Public Methods

        [TestMethod]
        public void SaveFileTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, 4);
            Console.WriteLine(prog);

            var fullPath = Path.Combine(Path.GetFullPath("."), FILE_NAME);
            File.Delete(fullPath);

            prog.ToD3JsonFile(fullPath);
            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"D3 json file should exist in {fullPath}");

            File.Delete(fullPath);
        }

        [TestMethod]
        public void SaveInfoTreeTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, 4);
            Console.WriteLine(prog);

            var tree = new InformationTree<MathProgram>();
            tree.AddProgram(prog);

            var fullPath = Path.Combine(Path.GetFullPath("."), INFO_FILE_NAME);
            File.Delete(fullPath);

            tree.ToD3JsonFile(fullPath);

            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"Image file with information tree should exist in {fullPath}");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "Image size should be > 0 bytes.");

            File.Delete(fullPath);
        }

        [TestMethod]
        public void SaveOrdSymbTreeTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, 4);
            Console.WriteLine(prog);

            var tree = new OrderedSymbolTree<MathProgram>();
            tree.AddProgram(prog);

            var fullPath = Path.Combine(Path.GetFullPath("."), ORD_SYMB_FILE_NAME);
            File.Delete(fullPath);

            tree.ToD3JsonFile(fullPath);

            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"Image file with information tree should exist in {fullPath}");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "Image size should be > 0 bytes.");

            File.Delete(fullPath);
        }

        [TestMethod]
        public void SaveSubProgTreeTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, 4);
            Console.WriteLine(prog);

            var tree = new SubProgramTree<MathProgram, double>();
            tree.AddProgram(prog);

            var fullPath = Path.Combine(Path.GetFullPath("."), SUB_PROG_FILE_NAME);
            File.Delete(fullPath);

            tree.ToD3JsonFile(fullPath);

            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"Image file with information tree should exist in {fullPath}");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "Image size should be > 0 bytes.");

            File.Delete(fullPath);
        }

        [TestMethod]
        public void SaveSymbTreeTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, 4);
            Console.WriteLine(prog);

            var tree = new SymbolTree<MathProgram>();
            tree.AddProgram(prog);

            var fullPath = Path.Combine(Path.GetFullPath("."), SYMB_FILE_NAME);
            File.Delete(fullPath);

            tree.ToD3JsonFile(fullPath);

            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"Image file with information tree should exist in {fullPath}");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "Image size should be > 0 bytes.");

            File.Delete(fullPath);
        }

        [TestMethod]
        public void SaveTreeFileTest()
        {
            var prog = new FullProgramGenerator<MathProgram, double>().Generate(MathPrimitiveSets.Default, 4);
            Console.WriteLine(prog);

            var fullPath = Path.Combine(Path.GetFullPath("."), FILE_TREE_NAME);
            File.Delete(fullPath);

            prog.ToD3TreeJsonFile(fullPath);
            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"D3 json file should exist in {fullPath}");

            File.Delete(fullPath);
        }

        #endregion
    }
}