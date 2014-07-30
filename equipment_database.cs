using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


/*
 * 
 * Class store the information about a list of test instruments based on an input file. 
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


namespace Instrument_Control
{
    class equipment_database
    {
        public string address, name;

        private string text_file = "config.txt";

        public equipment_database(string path) // "FULL PATH TO TEXT FILE CONTAINING A LSIT OF: < EQUIPMENT NAME > < GPIB/USB PORT >"
        {
            if (!File.Exists(path + text_file))
            {
                //MessageBox.Show("The Equipment DataBase Does Not Exist");
            }
            read_from_file();
            microbot_Address_1 = "COM5";
        }

        public void read_from_file()
        {
            StreamReader reader = new StreamReader(path + text_file);
            try
            {
                do
                {
                    set_address(reader.ReadLine());  // read a line and parse it to get the address of a specific equipment
                }
                while (reader.Peek() != -1);
            }

            catch
            {
                throw;
            }
            finally
            {
                reader.Close();  // done with the file so close it
            }

        }

        public void set_address(string str)
        {

            int add_indx = 0; // index of the address and equipment name separator space
            add_indx = str.IndexOf(" ");

            if (add_indx != 0)
            {

                name = str.Substring(0, add_indx);
                address = str.Substring(add_indx + 1, str.Length - (add_indx + 1)); // get the address and the equipment name
            }
        }
    }
}
