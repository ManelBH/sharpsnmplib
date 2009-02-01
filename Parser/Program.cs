﻿/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/10/2
 * Time: 17:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Lextm.SharpSnmpLib.Mib;
using System.Diagnostics;

namespace Lextm.SharpSnmpLib.Parser
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<string> files = new List<string>();
            string folder = null;
            string pattern = null;
            string root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules");
            const string FolderSwitch = "/folder:";
            const string PatternSwitch = "/pattern:";
            const string RootSwitch = "/root:";
            foreach (string arg in args)
            {
                if (arg.StartsWith(FolderSwitch))
                {
                    folder = arg.Substring(FolderSwitch.Length);
                }
                else if (arg.StartsWith(PatternSwitch))
                {
                    pattern = arg.Substring(PatternSwitch.Length);
                }
                else if (arg.StartsWith(RootSwitch))
                {
                    root = arg.Substring(RootSwitch.Length);
                }
                else
                {
                    files.Add(arg);
                }
            }
            
            if (folder != null && pattern != null) 
            {
                files.AddRange(Directory.GetFiles(folder, pattern));
            }
            
            Console.WriteLine(files.Count.ToString() + " files found");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            Mib.Parser parser = new Mib.Parser();
            IList<SharpMibException> errors;
            IEnumerable<MibModule> modules = parser.ParseToModules(files, out errors);
            Assembler assembler = new Assembler(root);
            assembler.Assemble(modules);
            Console.WriteLine("total time " + watch.ElapsedMilliseconds.ToString());
            watch.Stop();
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }       
    }
}