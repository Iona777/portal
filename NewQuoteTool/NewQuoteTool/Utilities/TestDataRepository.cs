using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using FileHelpers;
using NewQuoteTool.TestData.Models;
using NewQuoteTool.Pages;

namespace NewQuoteTool.Utilities
{
    class TestDataRepository
    {
        /// <summary>
        /// Used for finding where code is executed from, this is at the project level
        /// So an example path might be AssemblyDirectory/TestDAta
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private GasMatrix[] gasMatrixRecords;
        private ElectricMatrix[] electricMatrixRecords;

        public void ReadGasMatrixFileIntoDataRepository(string dataFile)
        {
            var engine = new FileHelperEngine<GasMatrix>();
            var dataDirectoryPath = GetFilePath("TestData","Matrices");
            var dataFilePath = Path.Combine(dataDirectoryPath, dataFile);
            gasMatrixRecords = engine.ReadFile(dataFilePath);
        }

        public void ReadElectricMatrixFileIntoDataRepository(string dataFile)
        {
            var engine = new FileHelperEngine<ElectricMatrix>();
            var dataDirectoryPath = GetFilePath("TestData", "Matrices");
            var dataFilePath = Path.Combine(dataDirectoryPath, dataFile);
            electricMatrixRecords = engine.ReadFile(dataFilePath);
        }

        public GasMatrix[] GetGasMatrix(string dataFile)
        {
            ReadGasMatrixFileIntoDataRepository(dataFile);
            return gasMatrixRecords;
        }

        public ElectricMatrix[] GetElectricMatrix(string dataFile)
        {
            ReadElectricMatrixFileIntoDataRepository(dataFile);
            return electricMatrixRecords;
        }

        /// <summary>
        /// Will combine assembly directory with given target folder
        /// E.G. ./TestData
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <returns>string</returns>
        public string GetFilePath(string targetFolder)
        {
            return Path.Combine(Path.Combine(AssemblyDirectory, targetFolder));
        }

        public string GetFilePath(string targetFolder, string subFolder1)
        {
            return Path.Combine(Path.Combine(AssemblyDirectory, targetFolder, subFolder1));
        }

        /// <summary>
        /// Will combine assembly directory with given target folder and the environment
        /// E.G. ./TestData/QA
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <returns>string</returns>
        public string GetFilePathWithEnvironment(string targetFolder)
        {
            return Path.Combine(Path.Combine(AssemblyDirectory, targetFolder, new RunSettings().EnvironmentKey));
        }
    }
}
