using System;
namespace Maze{
    public static class Program{
        public static void Main(string[] args){
            var rand = new Random();
            var chars = new char[]{'/', '\\'};
            while(true){
                Console.Write(chars[rand.Next(2)]);
            }
        }
    }
}