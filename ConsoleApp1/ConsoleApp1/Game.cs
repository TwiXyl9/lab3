using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    class Game
    {
        private string[] moves;
        private byte[] HMAC_key;
        private byte[] HMAC;
        public Game(string[] moves)
        {
            if (moves.Length % 2 != 0 && moves.Length >= 3 && (moves.Distinct().Count() == moves.Length))
            {
                this.moves = moves;
                Menu();
            }
            else {
                throw new Exception("Incorrect data! A small or even number of commands have been entered, or the commands contain repetitions! Try again!"); ;
            }
        }
        public void Move_Generation()
        {
            int comp_move = Computer_Move();
            Console.WriteLine("HMAC: " + BitConverter.ToString(this.HMAC).Replace("-", string.Empty).ToLower());
            Console.Write("Enter your move: ");
            int move = Convert.ToInt32(Console.ReadLine());
            if (move >= 0 && move <= this.moves.Length)
            {
                if (move != 0)
                {
                    Console.WriteLine("Your move: " + this.moves[move - 1]);
                    Console.WriteLine("Computer move: " + this.moves[comp_move - 1]);
                    Console.WriteLine(WhoIsWinner(move, comp_move));
                    Console.WriteLine("HMAC key: " + BitConverter.ToString(this.HMAC_key).Replace("-", string.Empty).ToLower());
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Incorrect command!");
                Menu();
            }
        }
        public int Computer_Move()
        {
            Random rd = new Random();
            int move = rd.Next(1, this.moves.Length+1);
            byte[] key = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(key);
            this.HMAC_key = key;
            byte[] bmove =  BitConverter.GetBytes(move);
            var hash = new HMACSHA256(key);
            this.HMAC=hash.ComputeHash(bmove);
            return move;
        }
        public int[] LosePositions(int mid, int move)
        {
            int[] lose_pos = new int[mid];
            int check = 1;
            for (int i = 0; check<=mid; i++)
            {
                lose_pos[i] = move + check>moves.Length ? move + check - moves.Length : move + check;
                check++;
            }
            return lose_pos;
        }
        public string WhoIsWinner(int user_move, int comp_move)
        {
            if(user_move==comp_move)return "Draw!";
            int mid = this.moves.Length / 2;
            if (LosePositions(mid, user_move).Contains(comp_move)) return "You win!";
            return "Computer win!";
        }
        public void Menu()
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < this.moves.Length; i++)
            {
                Console.WriteLine("{0} - {1};",i+1,this.moves[i]);
            }
            Console.WriteLine("0 - Exit;");
            Move_Generation();
        }

    }
}
