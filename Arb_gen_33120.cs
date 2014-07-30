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


namespace MicroBot_Controller_1._0
{
    class Arb_gen_33120
    {
        public Boolean rf_on;
        public FormattedIO488 src;
        public string IDN;

        public Arb_gen_33120(string srcAddress)
            {
                this.src = new FormattedIO488();
                ResourceManager rMgr = new ResourceManager();

                this.src.IO = (IMessage)rMgr.Open(srcAddress, AccessMode.NO_LOCK, 2000, null);
                this.src.IO.Timeout = 2000;

               // Console.Write("TG1C1-A " + srcAddress + " setup..");
                this.src.IO.Clear();

                /*Reset and Read OPC*/
                // this.src.WriteString("*RST");
                this.src.WriteString("*OPC?", true);
                string temp = this.src.ReadString();
              //  Console.Write("\n\n*OPC ->" + temp);  // reading back Operation Complete

                /*Read IDN*/
                this.src.WriteString("*IDN?", true);
                IDN = this.src.ReadString();

                this.turn_off();
                this.rf_on = false;
               // Console.Write("\n\n*IDN -> " +  temp);  // reading back IDN
                //
                // this.src.WriteString(":AUToscale");
                //this.src.WriteString(":STOP");
                //this.src.WriteString(":RUN");
                //src.WriteString(":OUTP ON", true); 
                //this.get_waveform();
            }


        public string get_IDN()
        {
            this.src.WriteString("*IDN?", true);
            return this.src.ReadString();
        }

        public void set_freq(string freq, string unit)
        {
            this.src.WriteString(":FREQ " + freq + unit, true);
        }


        public void set_amplitude(string amplitude, string unit)
        {
            this.src.WriteString(":VOLTage " + amplitude + unit, true);
        }

        public void turn_on()
        {
            this.src.WriteString(":OUTP ON", true);
        }

        public void turn_off()
        {
            this.src.WriteString(":OUTP OFF", true);
        }



    }




}
