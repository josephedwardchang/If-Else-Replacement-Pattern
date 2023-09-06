using System;

namespace ConsoleApp1
{
    public class StandardHandler : MsgDataHandler
    {
        protected override bool CanHandle(MessageData data)
        {
            return (!string.IsNullOrEmpty(data.OrigMsg) &&
                (data.OrigMsg[0] != '{' && data.OrigMsg[0] != '$' && data.OrigMsg[0] != 'c'));
        }

        protected override void HandleIt(ref MessageData data)
        {
            data.Filename = "standard data parsed";
        }
    }

    /// <summary>
    /// json data handler/parser
    /// </summary>
    public class JsonHandler : MsgDataHandler
    {
        protected override bool CanHandle(MessageData data)
        {
            return (!string.IsNullOrEmpty(data.OrigMsg) && data.OrigMsg[0] == '{');
        }

        /// <summary>
        /// Handler to parse the json object.
        /// </summary>
        /// <param name="data">original json data</param>
        protected override void HandleIt(ref MessageData data)
        {
            data.Filename = "json data parsed";
        }

    }

    public class GPSHandler : MsgDataHandler
    {
        protected override bool CanHandle(MessageData data)
        {
            return (!string.IsNullOrEmpty(data.OrigMsg) && data.OrigMsg.StartsWith("$GP", StringComparison.CurrentCultureIgnoreCase));
        }

        protected override void HandleIt(ref MessageData data)
        {
            data.Filename = "GPS data parsed";
        }
    }

    public class DataHandler : MsgDataHandler
    {
        protected override bool CanHandle(MessageData data)
        {
            return string.IsNullOrEmpty(data.OrigMsg);
        }

        protected override void HandleIt(ref MessageData data)
        {
            data.Filename = "null data parsed";
        }
    }
}
