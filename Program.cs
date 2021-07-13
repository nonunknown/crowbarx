using System;


namespace Crowbar
{
    public class Program
    {
        public static App TheApp;
        static void Main(string[] args)
        {
            
            if (args.Length < 2) {
                Console.WriteLine("Args are: \"input/path.mdl\" \"output/path/\"");
                Console.WriteLine("Extra args: -v (Verbose)");
                return;
            }
            else
            {
                bool verbose = false;
                for (int i = 2; i < args.Length; i++)
                {
                    switch (args[i]) {
                        case "-v":
                            verbose = true;
                        break;

                        default:
                            
                        break;
                    }
                }

                TheApp = new App();
                TheApp.Init();
                TheApp.Verbose = verbose;
                args[0] = args[0].Replace("\"","");
                args[1] = args[1].Replace("\"","");

                

                TheApp.SetOutputFolder(args[1]);
                TheApp.Decompiler.Decompile(args[0]);
            }
            
        }
    }
}

