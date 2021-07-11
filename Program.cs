using System;


namespace Crowbar
{
    public class Program
    {
        public static App TheApp;
        static void Main(string[] args)
        {
            
            Console.WriteLine(args.Length);
            if (args.Length != 2) {
                Console.WriteLine("Use only 2 args: \"input/path.mdl\" \"output/path/\"");
                return;
            }
            else
            {
                TheApp = new App();
                TheApp.Init();
                args[0] = args[0].Replace("\"","");
                args[1] = args[1].Replace("\"","");
                TheApp.SetOutputFolder(args[1]);
                TheApp.Decompiler.Decompile(args[0]);
            }
            
        }
    }
}

