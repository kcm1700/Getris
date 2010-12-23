﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.Core
{
    static public class Logger
    {
        static System.Object thisLock;
        static Logger()
        {
            thisLock = new System.Object();
        }

        static public void Write(string msg) 
        {
            lock (thisLock)
            {
                System.IO.FileStream fs = System.IO.File.Open("C:\\Temp\\asdf.txt",System.IO.FileMode.Append);
                if (fs.CanWrite)
                {
                    fs.Write(new System.Text.ASCIIEncoding().GetBytes(msg), 0, msg.Length);
                    fs.Write(new System.Text.UTF8Encoding().GetBytes("\r\n"), 0, "\r\n".Length);
                }
                fs.Close();
            }
        }
    }
}
