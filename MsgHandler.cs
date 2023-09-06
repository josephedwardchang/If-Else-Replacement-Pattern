using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MessageData
    {
        public string OrigMsg { get; set; }
        public DateTime Timestamp { get; set; }
        public string Filename { get; set; }
    }

    public abstract class MsgDataHandler
    {
        protected MsgDataHandler successor = null;

        // HowTo: Accepts any class type T as long as they're derived from MsgDataHandler
        public void SetSuccessor<T>(ref T successor) where T : MsgDataHandler
        {
            this.successor = successor;
        }

        public virtual void InitializeChain()
        {
        }

        public void Execute(ref MessageData msgData)
        {
            HandleRequest(ref msgData);
        }

        protected void HandleRequest(ref MessageData data)
        {
            var bCanHandle = false;
            try
            {
                bCanHandle = CanHandle(data);
            }
            catch
            {

            }
            if (bCanHandle)
            {
                try
                {
                    HandleIt(ref data);
                }
                catch
                {

                }
            }
            else if (successor != null)
            {
                try
                {
                    successor.HandleRequest(ref data);
                }
                catch
                {

                }
            }
        }

        protected abstract void HandleIt(ref MessageData data);
        protected abstract bool CanHandle(MessageData data);
    }
}
