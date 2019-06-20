using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFrameworkTwo.Utilities
{
    public class Context
    {
        RunSettings RunSettings;
        public string RootURL;
        public string ApiRootURL;


        //Constructor
        public Context()
        {
            RunSettings = new RunSettings();
            RootURL = RunSettings.WebRoot;
        }

    }

}
