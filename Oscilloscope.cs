using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ivi.Visa.Interop; 

/*
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
    class Oscilloscope
    {
        public string csv_path;
        public string csv_name;
        public FormattedIO488 src;
        double[] d_data;
        public string IDN;
        public string Sample_rate;

            public Oscilloscope(string srcAddress)
            {
                try
                {
                    this.src = new FormattedIO488();
                    ResourceManager rMgr = new ResourceManager();

                    this.src.IO = (IMessage)rMgr.Open(srcAddress, AccessMode.NO_LOCK, 2000, null);
                    this.src.IO.Timeout = 2000;

                    // Console.Write("TG1C1-A " + srcAddress + " setup..");
                    this.src.IO.Clear();
                }
                catch
                {
                    throw;
                }

                /*Reset and Read OPC*/
                // this.src.WriteString("*RST");
                this.src.WriteString("*OPC?", true);
                string temp = this.src.ReadString();
              //  Console.Write("\n\n*OPC ->" + temp);  // reading back Operation Complete

                /*Read IDN*/
                this.src.WriteString("*IDN?", true);
                IDN = this.src.ReadString();
               // Console.Write("\n\n*IDN -> " + temp);  // reading back IDN
                //
                // this.src.WriteString(":AUToscale");
                this.src.WriteString(":STOP");
                this.src.WriteString(":RUN");
                //src.WriteString(":OUTP ON", true); 
                //this.get_waveform();
            }

            public double get_mean()
            {
                this.get_waveform();
                double d_mean = 0.0;

                for (int ii = 0; ii < d_data.Length; ii++)
                {
                    d_mean = d_mean + d_data[ii];
                }
                d_mean = d_mean / d_data.Length;
                return d_mean;

            }

        public string get_Sample_Rate()
        {
            this.src.WriteString(":ACQuire:SRATe?");
            return this.src.ReadString();
        }

            public double get_rms()
            {
                this.get_waveform();
                double d_rms = 0.0;

                for (int ii = 0; ii < d_data.Length; ii++)
                {
                    d_rms = d_rms + (d_data[ii]*d_data[ii]);
                }
                d_rms = d_rms / d_data.Length;
                d_rms = Math.Sqrt(d_rms);
                return d_rms;
            }

            
            public void save_csv()
            {
                get_waveform();
                System.Text.StringBuilder theBuilder = new System.Text.StringBuilder();
                foreach(Double d_element in d_data)
                {
                   theBuilder.Append(d_element); 
                   theBuilder.Append(";");
                }
 
                using (StreamWriter theWriter = new StreamWriter(csv_path + "\\" + csv_name + ".csv"))
                {
                   theWriter.Write(theBuilder.ToString());   
                }

 
            }

            public void turn_channel1_on()
            {
                src.WriteString(":CHANnel1:DISPlay 1");
            }

            public void turn_channel2_on()
            {
                src.WriteString(":CHANnel2:DISPlay 1");
            }

            public void get_waveform()
            {

                /* WAVE_FORMAT - Sets the data transmission mode for waveform
                * data output. This command controls how the data is
                * formatted when sent from the Oscilloscope and can be set
                * to WORD or BYTE format.
                */
                // Set waveform format to BYTE.
                src.WriteString(":WAVeform:FORMat ASCII");

                /* WAVE_POINTS - Sets the number of points to be transferred.
                * The number of time points available is returned by the
                * "ACQUIRE:POINTS?" query. This can be set to any binary
                * fraction of the total time points available.
                */
                src.WriteString(":WAVeform:POINts 1000");


                /* GET_PREAMBLE - The preamble contains all of the current
                * WAVEFORM settings returned in the form <preamble block><NL>
                * where the <preamble block> is:
                * FORMAT : int16 - 0 = BYTE, 1 = WORD, 4 = ASCII.
                * TYPE : int16 - 0 = NORMAL, 1 = PEAK DETECT,
                * 2 = AVERAGE.
                * POINTS : int32 - number of data points transferred.
                * COUNT : int32 - 1 and is always 1.
                * XINCREMENT : float64 - time difference between data
                * points.
                * XORIGIN : float64 - always the first data point in
                * memory.
                * XREFERENCE : int32 - specifies the data point associated
                * with the x-origin.
                * YINCREMENT : float32 - voltage difference between data
                * points.
                * YORIGIN : float32 - value of the voltage at center
                * screen.
                * YREFERENCE : int32 - data point where y-origin occurs.
                */

                Console.WriteLine("Reading preamble.");



                string temp;
                this.src.WriteString(":WAVeform:PREamble?", true);
                temp = this.src.ReadString();
                Console.WriteLine("\n\n" + temp);

                //Parsing the Preamble
                char[] delimiterChars = { ',', '\t' }; // add the delimiters as required


                string[] parsed_str = temp.Split(delimiterChars);
                Console.WriteLine("{0} words in text:", parsed_str.Length);


                Console.WriteLine("Preamble FORMat: {0:e}", parsed_str[0]);

                Console.WriteLine("Preamble TYPE: {0:e}", parsed_str[1]);

                Console.WriteLine("Preamble POINts: {0:e}", parsed_str[2]);

                Console.WriteLine("Preamble COUNt: {0:e}", parsed_str[3]);

                Console.WriteLine("Preamble XINCrement: {0:e}", parsed_str[4]);

                Console.WriteLine("Preamble XORigin: {0:e}", parsed_str[5]);

                Console.WriteLine("Preamble XREFerence: {0:e}", parsed_str[6]);

                Console.WriteLine("Preamble YINCrement: {0:e}", parsed_str[7]);

                Console.WriteLine("Preamble YORigin: {0:e}", parsed_str[8]);

                Console.WriteLine("Preamble YREFerence: {0:e}", parsed_str[9]);


                /* READ_WAVE_DATA - The wave data consists of two parts: the
                * header, and the actual waveform data followed by a
                * New Line (NL) character. The query data has the following
                * format:
                *
                * <header><waveform data block><NL>
                *
                * Where:
                *
                * <header> = #800002048 (this is an example header)
                *
                * The "#8" may be stripped off of the header and the remaining
                * numbers are the size, in bytes, of the waveform data block.
                * The size can vary depending on the number of points acquired
                * for the waveform which can be set using the
                * ":WAVEFORM:POINTS" command. You may then read that number
                * of bytes from the Oscilloscope; then, read the following NL
                * character to terminate the query.
                */

                int number_of_bytes;
                number_of_bytes = Convert.ToInt32(parsed_str[2]);

                this.src.WriteString(":WAVeform:DATA?");
                temp = this.src.ReadString();

                //Console.WriteLine("\n\n" + temp);

                string[] str_data = temp.Split(delimiterChars);
                d_data = new double[number_of_bytes];

                str_data[0] = str_data[0].Substring(Convert.ToInt32(str_data[0].Substring(1, 1)) + 2, str_data[0].Length - (Convert.ToInt32(str_data[0].Substring(1, 1)) + 2));
                //Console.WriteLine("\n\n" + str_data[0]);

                for (int ii = 0; ii < str_data.Length; ii++)
                {
                    // Console.WriteLine("\n " + ii + "    " + str_data[ii]);
                    d_data[ii] = Convert.ToDouble(str_data[ii]);
                    //Console.WriteLine("\n " + ii + "    " + d_data[ii]);
                }

            }

        


    }
}
