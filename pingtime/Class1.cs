﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pingtime
{
    public class Class1
    {
        /// <summary>
        /// 
        /// 라이센스 갯수
        /// </summary>
        int sleep = 10000;

        public void pingthread()
        {
            try
            {
                while (true)
                {
                    DBCON.Class1 DBCON = new DBCON.Class1();
                    MySqlConnection CON = new MySqlConnection(DBCON.DBCON);

                    ///
                    ///특정날짜 넘어가면 아웃
                    ///
                    string SQL2 = "select date_format(now(),'%Y%m%d') as time";
                    MySqlDataAdapter ADT2 = new MySqlDataAdapter(SQL2, CON);
                    DataSet DBSET2 = new DataSet();
                    ADT2.Fill(DBSET2, "BD2");
                    string time = "";
                    foreach (DataRow row2 in DBSET2.Tables["BD2"].Rows)
                    {
                        Console.WriteLine("시간 ================================================================" + row2["time"].ToString());
                        time = row2["time"].ToString();
                    }

                    string[] serverip = { };
                    string tempip = "";
                    int count = 0;
                    MySqlDataAdapter ADT1 = new MySqlDataAdapter("Server_list_check", CON);
                    ADT1.SelectCommand.CommandType = CommandType.StoredProcedure;
                    ADT1.SelectCommand.Parameters.AddWithValue("@where1", " ");
                    ADT1.SelectCommand.Parameters.AddWithValue("@liesence", DBCON.Liesence);
                    DataSet DBSET1 = new DataSet();
                    ADT1.Fill(DBSET1, "BD1");
                    foreach (DataRow row in DBSET1.Tables["BD1"].Rows)
                    {
                        tempip += row["serverip"].ToString() + ",";
                        count++;
                    }

                    serverip = tempip.Split(',');

                    Thread[] thread = new Thread[count];

                    //작업갯수
                    int maxWorkingCount = count;
                    int workingRangeSize = 1;
                    if (maxWorkingCount > Environment.ProcessorCount)
                    {
                        workingRangeSize = maxWorkingCount / Environment.ProcessorCount;
                    }
                    //스레드 최대갯수
                    var options = new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = workingRangeSize
                    };
                    Parallel.For(0, count, options, i =>
                   {
                       pingtime(serverip[i]);

                   });

                    Thread.Sleep(sleep);


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("리스트 체크 " + e.Message);
            }
        }


        public class Part
        {
            public string Serverip { get; set; }
            public int status { get; set; }
        }

        public void pingtime(string serverip)
        {
            try
            {
                DBCON.Class1 DBCON = new DBCON.Class1();
                MySqlConnection CON = new MySqlConnection(DBCON.DBCON);
                string SQL = "";
                //while (false)
                //{
                SQL = "select distinct serverip, status from service where flag = '1' and serverip = '" + serverip + "' ";
                MySqlDataAdapter ADT = new MySqlDataAdapter(SQL, CON);
                DataSet DBSET = new DataSet();
                ADT.Fill(DBSET, "BD");
                foreach (DataRow row in DBSET.Tables["BD"].Rows)
                {
                    try
                    {
                        ///다운된 서버 메일보내기
                        if (row["status"].ToString().Contains("Disconnect") == true)
                        {
                            Console.WriteLine("★★★★★★★★★★" + serverip + " DOWN!!! (다운된 서버 메일보내기에 들어옴)");
                            ///열려있지 않으면 오픈
                            if (CON.State != ConnectionState.Open)
                            {
                                CON.Open();
                            }
                            MySqlCommand cmd = new MySqlCommand("server_down_log_sp", CON);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@serverip1", row["serverip"].ToString());
                            cmd.Parameters.AddWithValue("@status1", "Down");
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            cmd = null;
                            CON.Close();

                            try
                            {
                                server_down_mail send_mail = new server_down_mail();
                                send_mail.send_mail(row["serverip"].ToString(), "다운되었습니다");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("-------------------------------------------------------------이메일 보내기 안됨");
                            }

                        }



                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("여기1" + e.Message);
                    }

                    try
                    {
                        Ping ping = new Ping();
                        PingOptions options = new PingOptions();
                        options.DontFragment = true;
                        string data = "aaaaaaaaaaaaaaaaa";
                        byte[] buffer = ASCIIEncoding.ASCII.GetBytes(data);
                        int timeout = 1000;
                        PingReply reply = ping.Send(row["serverip"].ToString(), timeout, buffer, options);
                        int j = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            reply = ping.Send(row["serverip"].ToString(), timeout, buffer, options);
                            Thread.Sleep(3000);

                            if (reply.Status != IPStatus.Success)
                            {
                                j++;
                                Console.WriteLine("★★★★★★★★★★" + row["serverip"].ToString() + " 장비 " + j + " 번");
                            }
                        }


                        //if (reply.Status == IPStatus.Success) // 네트워크 사용 가능할 때~~
                        if (j != 5)
                        {
                            Process cmad = new Process();
                            cmad.StartInfo.FileName = "cmd.exe";
                            cmad.StartInfo.RedirectStandardInput = true;
                            cmad.StartInfo.RedirectStandardOutput = true;
                            cmad.StartInfo.CreateNoWindow = true;
                            cmad.StartInfo.UseShellExecute = false;
                            cmad.Start();
                            cmad.StandardInput.WriteLine("ping " + row["serverip"].ToString() + " -n 1");
                            cmad.StandardInput.Flush();
                            cmad.StandardInput.Close();
                            cmad.WaitForExit();
                            //Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                            string[] time = cmad.StandardOutput.ReadToEnd().Split('\n');
                            time = time[6].Split(':');
                            time = time[1].Split(' ');
                            string[] time2 = { };
                            if (time[2].Contains("=") == true)
                            {
                                time2 = time[2].Split('=');

                            }
                            else if (time[2].Contains("<") == true)
                            {
                                time2 = time[2].Split('<');
                            }
                            Console.WriteLine(row["serverip"].ToString() + " " + time2[1]);
                            if (row["status"].ToString().Contains("Disconnect") == true || row["status"].ToString().Contains("Time Out") == true)
                            {
                                server_down_mail send_mail = new server_down_mail();
                                send_mail.send_mail(row["serverip"].ToString(), " 복구되었습니다.");
                            }

                            ///열려있지 않으면 오픈
                            if (CON.State != ConnectionState.Open)
                            {
                                CON.Open();
                            }
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = CON;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "update service set pingtime = @pingtime, status = @status where serverip = @serverip ";
                            cmd.Parameters.Add("@pingtime", MySqlDbType.VarChar, 100).Value = time2[1];
                            cmd.Parameters.Add("@status", MySqlDbType.VarChar, 100).Value = "Server Connect";
                            cmd.Parameters.Add("@serverip", MySqlDbType.VarChar, 100).Value = row["serverip"].ToString();
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            cmd = null;
                            CON.Close();

                            string SQL2 = "select * from service_range WHERE  (INET_ATON('" + row["serverip"].ToString() + "') BETWEEN INET_ATON(startip) AND INET_ATON(endip))";
                            MySqlDataAdapter ADT3 = new MySqlDataAdapter(SQL2, CON);
                            DataSet DBSET3 = new DataSet();
                            ADT3.Fill(DBSET3, "BD");
                            foreach (DataRow row2 in DBSET3.Tables["BD"].Rows)
                            {
                                if (CON.State != ConnectionState.Open)
                                {
                                    CON.Open();
                                }
                                MySqlCommand cmd2 = new MySqlCommand("Auto_add_server", CON);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@serverip1", row["serverip"].ToString());
                                cmd2.Parameters.AddWithValue("@network_name1", row2["name"].ToString());
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();
                                cmd2 = null;
                                CON.Close();
                            }
                        }
                        else if (j == 5)
                        {


                            Console.WriteLine("============================= " + serverip + " DOWN!!!");
                            Console.WriteLine(row["serverip"].ToString() + "번 아웃");


                            ///열려있지 않으면 오픈
                            if (CON.State != ConnectionState.Open)
                            {
                                CON.Open();
                            }
                            MySqlCommand cmd2 = new MySqlCommand();
                            cmd2.Connection = CON;
                            cmd2.CommandType = System.Data.CommandType.Text;
                            cmd2.CommandText = "update service set  status = @status where serverip = @serverip ";
                            cmd2.Parameters.Add("@status", MySqlDbType.VarChar, 100).Value = "Server Disconnect/ping";
                            cmd2.Parameters.Add("@serverip", MySqlDbType.VarChar, 100).Value = row["serverip"].ToString();
                            cmd2.ExecuteNonQuery();
                            cmd2.Dispose();
                            cmd2 = null;
                            CON.Close();

                            //server_down_mail send_mail = new server_down_mail();
                            //send_mail.send_mail(row["serverip"].ToString(), "다운되었습니다");

                        }

                    }
                    catch (Exception e)
                    {
                        /////열려있지 않으면 오픈
                        //if (CON.State != ConnectionState.Open)
                        //{
                        //    CON.Open();
                        //}
                        //MySqlCommand cmd2 = new MySqlCommand();
                        //cmd2.Connection = CON;
                        //cmd2.CommandType = System.Data.CommandType.Text;
                        //cmd2.CommandText = "update service set  status = @status where serverip = @serverip ";
                        //cmd2.Parameters.Add("@status", MySqlDbType.VarChar, 100).Value = "Server Disconnect/ping";
                        //cmd2.Parameters.Add("@serverip", MySqlDbType.VarChar, 100).Value = row["serverip"].ToString();
                        //cmd2.ExecuteNonQuery();
                        //cmd2.Dispose();
                        //cmd2 = null;
                        //CON.Close();

                        Console.WriteLine("여기3" + e.Message);

                    }


                    //Thread.Sleep(sleep);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("여기2" + e.Message);
            }
        }
    }
}
