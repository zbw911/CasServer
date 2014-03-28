using System.Collections.Generic;

namespace CASServer.Core
{
    public class Messager
    {
        #region Instance Properties

        public List<string> MessageList { get; set; }

        public string redirectto { get; set; }

        public string return_msg { get; set; }
        public int time { get; set; }

        public string tips { get; set; }
        public string to_title { get; set; }

        #endregion
    }
}