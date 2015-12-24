using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;
using log4net.Config;
using System.Windows.Forms;

namespace KsFetch.GUI
{

    static class LoggerFactory
    {
        
        public static void Initialize()
        {
            var logConfig = new FileInfo(Path.Combine(Application.StartupPath, "log4net.config"));
            XmlConfigurator.Configure(logConfig);
        }

        public static ILog GetLogger(Type type)
        {
            if (!_loggers.ContainsKey(type))
            {
                var log = LogManager.GetLogger(type);
                _loggers.Add(type, log);
            }
            return _loggers[type];
        }

        private static Dictionary<Type, ILog> _loggers = new Dictionary<Type, ILog>();

    }

}
