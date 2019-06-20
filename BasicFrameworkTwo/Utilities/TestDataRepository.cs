using System;
using System.IO;
using System.Linq;
using FileHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicFrameworkTwo.Templates
{

    class TestDataRepository
    {
        //Array of UserTemplates
        private UserTemplate[] userRecords;

        /// <summary>
        /// Reads in file of users into userRecords
        /// (could/should parameterise the filename rather than hardcoding)
        /// </summary>
        public void ReadFileIntoDataRepository()
        {
            var engine = new FileHelperEngine<UserTemplate>();
            var dataDirectoryPath = GetFilePath();
            var dataFilePath = Path.Combine(dataDirectoryPath, "TestUsers.csv");
            userRecords = engine.ReadFile(dataFilePath);
            
        }

        /// <summary>
        /// Gets the userRecord for the given userKey
        /// </summary>
        /// <param name="requiredUserKey"></param>
        /// <returns>UserTemplate</returns>
        public UserTemplate GetUserDetails(string requiredUserKey)
        {
            ReadFileIntoDataRepository();
            var selectedUser = userRecords.FirstOrDefault(user => user.UserKey == requiredUserKey);

            if (selectedUser == null)
                throw new ArgumentNullException(@"User", new Exception($"User {requiredUserKey} not found!"));

            return selectedUser;

        }

        /// <summary>
        /// Gets the userRecord for the given email
        /// </summary>
        /// <param name="requiredEmail"></param>
        /// <returns>UserTemplate</returns>
        public UserTemplate GetUserDetailsByEmail(string requiredEmail)
        {
            ReadFileIntoDataRepository();
            var selectedUser = userRecords.FirstOrDefault(user => user.Username == requiredEmail);

            if (selectedUser == null)
                throw new ArgumentNullException(@"User", new Exception($"User {requiredEmail} not found!"));

            return selectedUser;

        }

        /// <summary>
        /// Works out the filepath to the data directory, regardless of environment.
        /// </summary>
        /// <returns>string</returns>
        public string GetFilePath()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            var solutionPath = dir.Parent.Parent.FullName;
            var finalPath = $@"{solutionPath}\Data\";

            return finalPath;
        }

    }
}
