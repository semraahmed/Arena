using System;

namespace ArenaGameEngine
{
    public abstract class Hero
    {
        public string Name { get; private set; }
        public int Health { get; protected set; }
        public int AttackPower { get; protected set; }
        public bool IsDead => Health <= 0;

        protected Hero(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
        }

        public virtual int Attack()
        {

            return AttackPower;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            }
        }
    }

    public class Merida : Hero
    {
        public Merida() : base("Merida", 100, 15) { }

        public override int Attack()
        {

            Random rnd = new Random();
            int damage = AttackPower;
            if (rnd.Next(0, 100) < 20)
            {
                damage *= 2;
                Console.WriteLine("Merida прави критичен удар!");
            }
            return damage;
        }
    }

    public class Mulan : Hero
    {
        public Mulan() : base("Mulan", 120, 10) { }

        public override int Attack()
        {

            Random rnd = new Random();
            int damage = AttackPower;
            if (rnd.Next(0, 100) < 25)
            {
                int heal = 5;
                Health += heal;
                Console.WriteLine("Mulan възстановява " + heal + " здраве!");
            }
            return damage;
        }
    }

    public interface GameEventListener
    {
        void GameRound(Hero attacker, Hero defender, int damage);
    }

    public class Arena
    {
        public Hero HeroA { get; private set; }
        public Hero HeroB { get; private set; }
        public GameEventListener EventListener { get; set; }

        public Arena(Hero a, Hero b)
        {
            HeroA = a;
            HeroB = b;
        }

        public Hero Battle()
        {
            Hero attacker = HeroA;
            Hero defender = HeroB;
            while (true)
            {
                int damage = attacker.Attack();
                defender.TakeDamage(damage);

                if (EventListener != null)
                {
                    EventListener.GameRound(attacker, defender, damage);
                }

                if (defender.IsDead) return attacker;

                Hero temp = attacker;
                attacker = defender;
                defender = temp;
            }
        }
    }

    public class ConsoleGameEventListener : GameEventListener
    {
        public void GameRound(Hero attacker, Hero defender, int damage)
        {
            Console.WriteLine($"{attacker.Name} атакува {defender.Name} и нанася {damage} щети. {defender.Name} има {defender.Health} здраве останало.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Hero merida = new Merida();
            Hero mulan = new Mulan();
            Arena arena = new Arena(merida, mulan);

            arena.EventListener = new ConsoleGameEventListener();

            Hero winner = arena.Battle();
            Console.WriteLine("Победителят е: " + winner.Name);
        }
    }
}