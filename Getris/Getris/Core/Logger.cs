﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.Core
{
    static public class Logger
    {
        static private string file;
        static private bool on;
        static System.Object thisLock;
        static Logger()
        {
            on = false;
            file="C:\\Temp\\foo.txt";
            thisLock = new System.Object();
        }
        static public string File
        {
            get
            {
                lock (thisLock)
                {
                    return file;
                }
            }
            set
            {
                lock (thisLock)
                {
                    file = value;
                }
            }
        }
        static public bool On
        {
            get
            {
                lock (thisLock)
                {
                    return on;
                }
            }
            set
            {
                lock (thisLock)
                {
                    on = value;
                }
            }
        }
        static public void WriteLine(string msg) 
        {
            if (on)
            {
                lock (thisLock)
                {
                    System.IO.FileStream fs = System.IO.File.Open(file, System.IO.FileMode.Append);
                    if (fs.CanWrite)
                    {
                        fs.Write(new System.Text.ASCIIEncoding().GetBytes(msg), 0, msg.Length);
                        fs.Write(new System.Text.UTF8Encoding().GetBytes("\r\n"), 0, "\r\n".Length);
                    }
                    fs.Close();
                }
            }
        }
        static public void Write(string msg)
        {
            if (on)
            {
                lock (thisLock)
                {
                    System.IO.FileStream fs = System.IO.File.Open("C:\\Temp\\asdf.txt", System.IO.FileMode.Append);
                    if (fs.CanWrite)
                    {
                        fs.Write(new System.Text.ASCIIEncoding().GetBytes(msg), 0, msg.Length);
                    }
                    fs.Close();
                }
            }
        }
    }
}
