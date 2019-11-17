using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Collections;

namespace Qubechallenge
{
    class Program
    {
        static int header = 0,iterationcount=0;
        static List<Partners> Partnerlist = new List<Partners>();
        static List<capacities> Capacitylist = new List<capacities>();       
         
        public static void calculate(string deliveryid,int deliverysize,string theatreid,Dictionary<string,int> map, int check)
        {
            var Partnerreader = new StreamReader(File.OpenRead(@"C:\Users\Suryakumar\Desktop\Project\Python Projects\Qubechallenge\Qubechallenge\Data\partners.csv"));
            Partnerlist.Clear();
            while(!Partnerreader.EndOfStream)
            {
                if(header==0)
                {
                    var Line = Partnerreader.ReadLine();
                    header++;
                    continue;
                }

                if(header>0)
                {
                    Line = Partnerreader.ReadLine();
                    var row = Line.Split(',');
                    var minmax = row[1].Split('-');
                    if (row[0].TrimEnd().Equals(theatreid))
                    {
                        iterationcount++;
                        Partners P = new Partners()
                        {
                            theatreid = row[0],
                            min = Convert.ToInt32(minmax[0]),
                            max = Convert.ToInt32(minmax[1]),
                            mincost = Convert.ToInt32(row[2]),
                            costpergb = Convert.ToInt32(row[3]),
                            partnerid = row[4]
                        };
                        Partnerlist.Add(P);
                    }
                }
                
            }

            if(check==1)
            {
                checkdelivery(deliveryid, deliverysize, iterationcount, theatreid);

            }

            if (check == 2)
            {
                capacitycalculate(Partnerlist, deliveryid, deliverysize, theatreid, map);

            }

        }

        public static void checkdelivery(string deliveryid,int deliverysize,int iterationcount,string theatreid)
        {

            int amount = 0, curramount=int.MaxValue, count = 1;
            bool condition = false;
            bool flag = false;
            string partner = "";
            var csvwriter = new StringBuilder();
            var newLine = "";
            var filepath = @"C:\Users\Suryakumar\Desktop\Project\Python Projects\Qubechallenge\Qubechallenge\Data\output.csv";

            for (int i=0;i<Partnerlist.Count;i++)
            {
                string theatreIdObj = Partnerlist[i].theatreid;
                int minObj = Partnerlist[i].min;
                int maxObj = Partnerlist[i].max;
                int minCostObj = Partnerlist[i].mincost;
                int costPerGbObj = Partnerlist[i].costpergb;
                string partnerIdobj = Partnerlist[i].partnerid;

                if(deliverysize>minObj && deliverysize<maxObj)
                {
                    condition = true;
                    amount = (deliverysize * costPerGbObj);

                    if(amount<minCostObj)
                    {
                        amount = minCostObj;
                    }

                    if(amount<curramount)
                    {
                        curramount = amount;
                        partner = partnerIdobj;
                    }
                }

            }

            if(condition==true)
            {
                Console.WriteLine("{0} {1} {2} {3}", deliveryid,condition,partner,curramount);
                newLine = string.Format("{0},{1},{2},{3}", deliveryid, condition, partner, curramount);
                
            }

            else
            {
                Console.WriteLine("{0} {1} {2} {3}", deliveryid, condition, "", "");
                newLine = string.Format("{0},{1},{2},{3}", deliveryid, condition, "", "");
            }

            csvwriter.AppendLine(newLine);
            File.AppendAllText(filepath,csvwriter.ToString());
            
        }

       

        public static void capacitycalculate(List<Partners> T, string deliveryid, int deliverysize, string theatreid, Dictionary<string, int> map)
        {
           
            int amount = 0, curramount = int.MaxValue, count = 1;
            bool condition = false;
            string partner = "";
            var csvwriter = new StringBuilder();
            var newLine = "";
            var filepath = @"C:\Users\Suryakumar\Desktop\Project\Python Projects\Qubechallenge\Qubechallenge\Data\output2.csv";

            for (int i = 0; i < Partnerlist.Count; i++)
            {
                string theatreIdObj = Partnerlist[i].theatreid;
                int minObj = Partnerlist[i].min;
                int maxObj = Partnerlist[i].max;
                int minCostObj = Partnerlist[i].mincost;
                int costPerGbObj = Partnerlist[i].costpergb;
                string partnerIdobj = Partnerlist[i].partnerid;

                int capacity = map[partnerIdobj];

                if (deliverysize > minObj && deliverysize < maxObj && deliverysize<capacity)
                {
                    condition = true;
                    amount = (deliverysize * costPerGbObj);

                    if (amount < minCostObj)
                    {
                        amount = minCostObj;
                    }

                    if (amount < curramount)
                    {
                        curramount = amount;
                        partner = partnerIdobj;
                        map[partnerIdobj] = capacity - deliverysize;
                    }
                }

            }

            if (condition == true)
            {
                Console.WriteLine("{0} {1} {2} {3}", deliveryid, condition, partner, curramount);
                newLine = string.Format("{0},{1},{2},{3}", deliveryid, condition, partner, curramount);
            }
            else
            {
                Console.WriteLine("{0} {1} {2} {3}", deliveryid, condition, "", "");
                newLine = string.Format("{0},{1},{2},{3}", deliveryid, condition, "", "");
            }
            
            csvwriter.AppendLine(newLine);
            File.AppendAllText(filepath, csvwriter.ToString());
        }
        static void Main(string[] args)
        {
            List<input> inputarray = new List<input>(4);
            var reader = new StreamReader(File.OpenRead(@"C:\Users\Suryakumar\Desktop\Project\Python Projects\Qubechallenge\Qubechallenge\Data\input.csv"));

            var Capacityreader = new StreamReader(File.OpenRead(@"C:\Users\Suryakumar\Desktop\Project\Python Projects\Qubechallenge\Qubechallenge\Data\capacities.csv"));

            var map = new Dictionary<string, int>();
            int counter = 0;
            while (!Capacityreader.EndOfStream)
            {
                if(counter==0)
                {
                    var Line = Capacityreader.ReadLine();
                    counter++;
                }

                if(counter>0)
                {
                    var Line = Capacityreader.ReadLine();
                    var row = Line.Split(',');
                    map.Add(row[0].TrimEnd(), Convert.ToInt32(row[1]));
                   
                }
                
            }

            int pblm=1;
            Console.WriteLine("Enter the Problem number :");
            Console.WriteLine("1.Problem Statement-1 (Press 1)");
            Console.WriteLine("1.Problem Statement-2 (Press 2");
            pblm = Convert.ToInt32(Console.ReadLine());

            while (!reader.EndOfStream)
            {
                var Line = reader.ReadLine();
                var row = Line.Split(',');

                
                input ip = new input()
                {
                    deliveryid=row[0],
                    deliverysize=Convert.ToInt32(row[1]),
                    theatreid=row[2]
                };
                inputarray.Add(ip);

                calculate(row[0], Convert.ToInt32(row[1]), row[2],map,pblm);

            }
        }
    }

    public class Partners
    {
        public string theatreid { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int mincost { get; set; }
        public int costpergb { get; set; }
        public string partnerid { get; set; }

    }

    public class input
    {
        public string deliveryid { get; set; }
        public int deliverysize { get; set; }
        public string theatreid { get; set; }
        
    }

    public class capacities
    {
        public string partnerId { get; set; }

        public int capacity { get; set; }
    }
}
