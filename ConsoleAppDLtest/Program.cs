using League.BL.Domein;
using League.BL.Exceptions;
using League.BL.Interfaces;
using League.BL.Managers;
using League.DL;
using System;

namespace ConsoleAppDLtest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string connString = @"Data Source=NB21-6CDPYD3\SQLEXPRESS;Initial Catalog=LeagueDB;Integrated Security=True";
            SpelerRepoADO r = new SpelerRepoADO(connString);
            SpelerManager m = new SpelerManager(r);
            Speler speler = m.RegistreerSpeler("Ellen", 172, null);
            TeamRepoADO tr = new TeamRepoADO(connString);
            TeamManager tm = new TeamManager(tr);
            
            //try
            //{
               tm.RegistreerTeam(1, "Antwerpen", "Great Old");
            //}
            //catch(TeamManagerException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //Console.WriteLine(s);
            Team nieuwTeam = new Team(1, "Antwerp");
            ITransferRepository transferRepository = new TransferRepoADO(connString);
            TransferManager transferManager=new TransferManager(transferRepository);
            transferManager.RegistreerTransfer(speler, nieuwTeam, 100);
        }
    }
}
