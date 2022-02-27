//
// Filename: Program.cs
// Project:  Assignment 3
// By: Mustafa Efiloglu
// Date: February 26, 2021
// Description:  Client Program
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace A3 {
    class Program {
        static void Main(string[] args) {
            int iInstnc = GnrtD();
            int lgPt = 0;
            IPAddress iLogg = null;

            if (args.Length != 2 || !int.TryParse(args[0], out lgPt) || !IPAddress.TryParse(args[1], out iLogg)) {
                if (File.Exists(Config.cm_err_route)) {
                    using (StreamReader st_reader = File.OpenText(Config.cm_err_route)) {
                        string str;
                        while ((str = st_reader.ReadLine()) != null) {
                            Console.WriteLine(str);
                        }
                    }
                } else {
                    Console.WriteLine(Config.cm_fl_err);
                }
                return;
            }
            string uInpt = "";
            while (uInpt != Config.cm_qt) {
                uInpt = PrcsLg(lgPt, iLogg, iInstnc);
            }
        }
        static string PrcsLg(int lgPt, IPAddress iLogg, int iInstnc) {
            const int mnLvl = 0;
            const int mxLvl = 8;
            const int smPau = 60;
            var rndLvl = new Random();
            string uInpt;
            string date;

            Console.WriteLine("Enter valid words");
            uInpt = Console.ReadLine().Trim();

            if (string.Compare(uInpt, Config.cm_hlp) == 0) {
                if (File.Exists(Config.cm_hlp_route)) {
                    using (StreamReader st_reader = File.OpenText(Config.cm_hlp_route)) {
                        string str;
                        while ((str = st_reader.ReadLine()) != null)
                            Console.WriteLine(str);
                        return Config.cm_cnt;
                    }
                } else {
                    Console.WriteLine(Config.cm_fl_err);
                }
                return Config.cm_cnt;
            }

            else if (string.Compare(uInpt, Config.cm_auto) == 0) {
                if (File.Exists(Config.cm_tst_route)) {
                    using (StreamReader rdr = File.OpenText(Config.cm_tst_route)) {
                        string cm_ln;
                        Console.WriteLine("Writing logs.\n");

                        for (int i = mnLvl; i < mxLvl + 1; i++) {
                            date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                            cm_ln = rdr.ReadLine();
                            Console.WriteLine("Writing logs array " + i.ToString());
                            SdMsg(iLogg, lgPt, date, iInstnc.ToString(), i.ToString(), cm_ln, Config.cm_frmtted_lg);
                            Thread.Sleep(smPau);
                        }

                        date = Config.cm_invld_nmbr;
                        cm_ln = rdr.ReadLine();
                        Console.WriteLine("Writing wrong date logs");
                        SdMsg(iLogg, lgPt, date, iInstnc.ToString(), rndLvl.Next(mnLvl, mxLvl + 1).ToString(), cm_ln, Config.cm_frmtted_lg);
                        Thread.Sleep(smPau);

                        date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                        cm_ln = rdr.ReadLine();
                        Console.WriteLine("Writing logs not valid instance.");
                        SdMsg(iLogg, lgPt, date, Config.cm_invld_nmbr, rndLvl.Next(mnLvl, mxLvl + 1).ToString(), cm_ln, Config.cm_frmtted_lg);
                        Thread.Sleep(smPau);

                        date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                        cm_ln = rdr.ReadLine();
                        Console.WriteLine("Writing logs with lvl");
                        SdMsg(iLogg, lgPt, date, iInstnc.ToString(), Config.cm_lvl_invd, cm_ln, Config.cm_frmtted_lg);
                        Thread.Sleep(smPau);

                        date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                        cm_ln = rdr.ReadLine();
                        Console.WriteLine("Writing logs wtih lvl low");
                        SdMsg(iLogg, lgPt, date, iInstnc.ToString(), Config.cm_lvl_lw, cm_ln, Config.cm_frmtted_lg);
                        Thread.Sleep(smPau);

                        date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                        cm_ln = rdr.ReadLine();
                        Console.WriteLine("Writing logs with lvl hgh");
                        SdMsg(iLogg, lgPt, date, iInstnc.ToString(), Config.cm_lvl_hgh, cm_ln, Config.cm_frmtted_lg);
                        Thread.Sleep(smPau);

                        Console.WriteLine("Writing logs with rnd string.");
                        SdMsg(iLogg, lgPt, Config.cm_plc_hldr, Config.cm_plc_hldr, Config.cm_plc_hldr, Config.cm_msfrmt_lg, Config.cm_submit);
                        Thread.Sleep(smPau);

                        Console.WriteLine("Logger wriitng.\n");
                        return Config.cm_cnt;
                    }
                }
            }

            else if (string.Compare(uInpt, Config.cm_nose) == 0) {
                Console.WriteLine("Enter a number for req, or typed \"Noise\" to list all command lines.\n");
                uInpt = Console.ReadLine().Trim();
                int num_of_req = 0;

                if (int.TryParse(uInpt, out num_of_req) || uInpt == Config.cm_nose) {
                    if (uInpt != Config.cm_nose && num_of_req <= 0) {
                        Console.WriteLine("Number shouldnt be 0 or -1");
                        return Config.cm_cnt;
                    }

                    if (File.Exists(Config.cm_tst_route)) {
                        using (StreamReader rdr = File.OpenText(Config.cm_tst_route)) {
                            List<string> ns_txt = new List<string>();
                            string ln;
                            while ((ln = rdr.ReadLine()) != null) {
                                ns_txt.Add(ln);
                            }

                            if (uInpt == Config.cm_nose || num_of_req >= Config.cm_mx_nmbr) {
                                Console.WriteLine("All test log writing\n");
                                foreach (string str in ns_txt) {
                                    date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                                    SdMsg(iLogg, lgPt, date, iInstnc.ToString(), rndLvl.Next(mnLvl, mxLvl + 1).ToString(), str, Config.cm_frmtted_lg);
                                    Thread.Sleep(smPau);
                                    Console.WriteLine(str);
                                }
                            } else {
                                Console.WriteLine("Writing {0} log\n", num_of_req);
                                for (int i = 0; i < num_of_req; i++) {
                                    date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                                    SdMsg(iLogg, lgPt, date, iInstnc.ToString(), rndLvl.Next(mnLvl, mxLvl + 1).ToString(), ns_txt[i], Config.cm_frmtted_lg);
                                    Thread.Sleep(smPau);
                                    Console.WriteLine(ns_txt[i]);
                                }
                            }
                            Console.WriteLine("\nLogger Wrote.\n");
                        }
                        return Config.cm_cnt;
                    }
                } else {
                    Console.WriteLine("This is not a words or number \"Noise\".");
                    return Config.cm_cnt;
                }

            }

            else if (string.Compare(uInpt, Config.cm_mnl) == 0) {
                Console.WriteLine("Enter manual log\n");
                uInpt = Console.ReadLine().Trim();
                Console.WriteLine("\nWriting log");
                SdMsg(iLogg, lgPt, Config.cm_plc_hldr, Config.cm_plc_hldr, Config.cm_plc_hldr, uInpt, Config.cm_submit);
                Console.WriteLine(uInpt);
                Console.WriteLine("Logger wrote.");
            }

            else if (string.Compare(uInpt, Config.cm_qt) == 0) {
                Console.WriteLine("Date\n");
                return Config.cm_qt;
            }

            else {
                Console.WriteLine("something wrong. Enter \"Help\" to get listed lines.\n");
                return Config.cm_cnt;
            }
            return Config.cm_cnt;
        }
        static int GnrtD() {
            string iInstncString = DateTime.UtcNow.ToString("HHmmssfff");
            int iInstnc = 0;
            int.TryParse(iInstncString, out iInstnc);
            return iInstnc;
        }
        static void SdMsg(IPAddress logg, int pt, string date, string iInstnc, string lgLvl, string logging, bool cm_frmtted_lg) {
            try {
                TcpClient clnt = new TcpClient(logg.ToString(), pt);
                string frmtLg = "";

                if (cm_frmtted_lg == Config.cm_submit) {
                    frmtLg = logging;
                } else {
                    frmtLg = date + "-";
                    frmtLg += iInstnc + "-";
                    frmtLg += lgLvl.ToString() + "-";
                    frmtLg += logging;
                }
                Byte[] by_data = Encoding.ASCII.GetBytes(frmtLg);
                NetworkStream st = clnt.GetStream();
                st.Write(by_data, 0, by_data.Length);
                st.Close();
                clnt.Close();
            } catch (ArgumentNullException e) {
                Console.WriteLine("ArgExp: " + e.ToString() + "\n");
            } catch (SocketException e) {
                Console.WriteLine("SckExp: " + e.ToString() + "\n");
            }
        }
    }
}