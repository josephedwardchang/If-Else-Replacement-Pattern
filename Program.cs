using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class MainObj : MsgDataHandler
    {
        // the msghandlers, see MsgHandlers.cs file
        StandardHandler Obj1;
        DataHandler Obj2;
        GPSHandler Obj3;
        JsonHandler Obj4;

        protected override bool CanHandle(MessageData data)
        {
            // Since this MainObj is the first handler, we can just thhrow exception or return false here
            // to indicate MainObj can't process the request
            //throw new NotImplementedException();
            return false;
        }

        protected override void HandleIt(ref MessageData data)
        {
            // This will never be called if CanHandle returns false
            throw new NotImplementedException();
        }

        public override void InitializeChain()
        {
            base.InitializeChain();

            Obj1 = new StandardHandler();
            Obj2 = new DataHandler();
            Obj3 = new GPSHandler();
            Obj4 = new JsonHandler();

            // you can re-arrange successors but make sure they
            // don't go into a circular loop or skip any handler
            SetSuccessor(ref Obj3);
            Obj4.SetSuccessor(ref Obj2); 
            Obj3.SetSuccessor(ref Obj1);
            Obj1.SetSuccessor(ref Obj4); 
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MessageData msgData = new MessageData();

            MainObj mainobj = new MainObj();
            mainobj.InitializeChain();

            // Test 1
            msgData.OrigMsg = "MZ{json:rest}";
            msgData.Filename = string.Empty;
            mainobj.Execute(ref msgData);
            Console.WriteLine("Test 1:" + msgData.Filename); // will show filename as StandardHandler "standard data parsed"

            // Test 2
            msgData.OrigMsg = "{json:unrest}";
            msgData.Filename = string.Empty;
            mainobj.Execute(ref msgData);
            Console.WriteLine("Test 2:" + msgData.Filename); // will show filename as JsonHandler "json data parsed"

            // Test 3
            msgData.OrigMsg = "$GP{json:unrest}";
            msgData.Filename = string.Empty;
            mainobj.Execute(ref msgData);
            Console.WriteLine("Test 3:" + msgData.Filename); // will show filename as GPSHandler "GPS data parsed"

            // Test 4
            msgData.OrigMsg = "";
            msgData.Filename = string.Empty;
            mainobj.Execute(ref msgData);
            Console.WriteLine("Test 4:" + msgData.Filename); // will show filename as DataHandler "null data parsed"

            // Test 5
            msgData.OrigMsg = "csv,unrest,col\n\r1,2,3\n\r4,5,6";
            msgData.Filename = string.Empty;
            mainobj.Execute(ref msgData);
            Console.WriteLine("Test 5:" + msgData.Filename); // will show filename as empty because none of the handlers can handle it

        }
    }
}
