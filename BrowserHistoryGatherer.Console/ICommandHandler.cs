using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BrowserHistoryGatherer {
    /// <summary>
    /// 命令处理器;
    /// </summary>
    public interface ICommandHandler {
        void Handle(JObject browserObject, CommandHandlerArgs commandHandlerArgs);
    }
}
