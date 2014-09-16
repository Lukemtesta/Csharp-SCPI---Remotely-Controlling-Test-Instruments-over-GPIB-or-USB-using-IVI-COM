using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Ivi.Visa.Interop;

/*
 * 
 * These classes should be using with .NET 3.5+ to establish a connection and remotely control an Agilent or Rhode and Schwarz test instruments over a GPIB network.
 * 
 * Note to User: National Instruments IVI-COM and VISA COM libraries must be installed before use. 
 * 
 */


/*

Copyright (c) 2014 Luke Marcus Biagio Testa
All rights reserved.

Redistribution and use in source and binary forms are permitted
provided that the above copyright notice and this paragraph are
duplicated in all such forms and that any documentation,
advertising materials, and other materials related to such
distribution and use acknowledge that the software was developed
by the Luke Marcus Biagio Testa. The name of the
Luke Marcus Biagio Testa may not be used to endorse or promote products derived
from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR
IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * 
 */


namespace Device
{
    class Control
    {
        private FormattedIO488 obj;
        public string ID;

        private Control() { }

        public Control(string address)
        {
            this.obj = new FormattedIO488();
            ResourceManager rMgr = new ResourceManager();

            this.obj.IO = (IMessage)rMgr.Open(srcAddress, AccessMode.NO_LOCK, 2000, null);
            this.obj.IO.Clear();

            Query("*OPC?", true);
            ID = Query("*IDN?", true); 
        }

        public virtual ~Control() { Console.WriteLine("Deleting instance of class [DEVICE]"); }

        public string Command(string msg) { this.src.WriteString(msg); }

        public string Query(string msg, bool flag)
        {
            obj.WriteString("*" + msg + "?", flag);
            return obj.ReadString();
        }
    }
}
