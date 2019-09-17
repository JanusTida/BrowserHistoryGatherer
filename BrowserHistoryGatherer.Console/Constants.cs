using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer {
    static partial class Constants {

        public const string JsonElemName_Arguments = "Arguments";

        public const string JsonElemName_BrowserTypes = "BrowserTypes";


        public const string JsonElemName_BrowserType = "BrowserType";

        public const string JsonElemName_Commands = "Commands";

        public const string JsonElemName_Command = "Command";

        public const string JsonElemName_OutputFile = "OutputFile";

        


        public const string Command_GetHistory = "Get-History";
        public const string Command_GetCache = "Get-Cache";
        public const string Command_GetFavorite = "Get-Favorite";


    }
    static partial class Constants {
        public const string JsonElemName_Uri = "Uri";
        public const string JsonElemName_Title = "Title";
        public const string JsonElemName_LastVisitTime = "LastVisitTime";
        public const string JsonElemName_CreationTime = "CreationTime";
        public const string JsonElemName_Size = "Size";
        public const string JsonElemName_VisitCount = "VisitCount";
    }
}
