//
// Filename: Config.cs
// Project:  Assignment 3
// By: Mustafa Efiloglu
// Date: February 26, 2021
// Description:  Config Client
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3 {
    static public class Config {
        public const string wl_msg = "Enter a words, or type \"Help\" for help\n";
        public const string cm_hlp = "Help";
        public const string cm_nose = "Noise";
        public const string cm_mnl = "Manual";
        public const string cm_auto = "Auto";
        public const string cm_qt = "Quit";
        public const string cm_cnt = "";
        public const string cm_err_route = @"..\..\..\txtFolders\err_args.txt";
        public const string cm_hlp_route = @"..\..\..\txtFolders\hlp_file.txt";
        public const string cm_eut_route = @"..\..\..\txtFolders\aut_file.txt";
        public const string cm_tst_route = @"..\..\..\txtFolders\tst_logg.txt";
        public const bool cm_submit = true;
        public const bool cm_frmtted_lg = false;
        public const string cm_msfrmt_lg = "Fromatted is not true";
        public const string cm_invld_nmbr = "8785arentNum";
        public const string cm_lvl_hgh = "11";
        public const string cm_lvl_lw = "-2";
        public const string cm_lvl_invd = "OFF";
        public const string cm_plc_hldr = "Place Holder";
        public const int cm_mx_nmbr = 1321;
        public const string cm_fl_err = "File cannot open\n";
    }
}
