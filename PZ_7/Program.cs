using System;
using System.Collections.Generic;

namespace PZ_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            errors stock = new errors();
            administrator bank = new administrator("Пользователь", stock);
            err systema = new err("Ошибка подключения", stock);

            stock.Market();

            systema.StopTrade();

            stock.Market();

            Console.Read();
        }
    }

    interface IObserver
    {
        void Update(Object ob);
    }

    interface IObservable
    {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers();
    }

    class errors : IObservable
    {
        StockInfo sInfo;

        List<IObserver> observers;
        public errors()
        {
            observers = new List<IObserver>();
            sInfo = new StockInfo();
        }
        public void RegisterObserver(IObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            observers.Remove(o);
        }

        public void NotifyObservers()
        {
            foreach (IObserver o in observers)
            {
                o.Update(sInfo);
            }
        }

        public void Market()
        {
            Random rnd = new Random();
            sInfo._errors = rnd.Next(0, 100);
            sInfo.Crit = rnd.Next(0, 30);
            NotifyObservers();
        }
    }

    class StockInfo
    {
        public int _errors { get; set; }
        public int Crit { get; set; }
    }

    class err : IObserver
    {
        public string Name { get; set; }
        IObservable stock;
        public err(string name, IObservable obs)
        {
            this.Name = name;
            stock = obs;
            stock.RegisterObserver(this);
        }
        public void Update(object ob)
        {
            StockInfo sInfo = (StockInfo)ob;

            if (sInfo._errors > 10)
                Console.WriteLine("Пользователь {0} Кол-во ошибок: {1}", this.Name, sInfo._errors);
            else
                Console.WriteLine("Ошибки не были обнаружены");
        }
        public void StopTrade()
        {
            stock.RemoveObserver(this);
            stock = null;
        }
    }

    class administrator : IObserver
    {
        public string Name { get; set; }
        IObservable stock;
        public administrator(string name, IObservable obs)
        {
            this.Name = name;
            stock = obs;
            stock.RegisterObserver(this);
        }
        public void Update(object ob)
        {
            StockInfo sInfo = (StockInfo)ob;

            if (sInfo.Crit > 10)
                Console.WriteLine("Администратор получил оповещение", sInfo.Crit);
            else
                Console.WriteLine("Администратор не получил оповещения");
        }
    }
}