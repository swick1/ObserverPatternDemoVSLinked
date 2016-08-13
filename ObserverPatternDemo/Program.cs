using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;
using System.Collections;


/* Observer pattern defines a one-to-many dependency between objects so that when one object changes state, 
 * all its dependents are notified and updated automatically. 
 * 
 * The .NET Timer class can be used to demonstrate this pattern. 
 * 
 * A class called Stock which simulates a Stock quote is designed which starts a Timer internally. 
 * 
 * A parallel interface called Observer is designed. Any class which implements this interface 
 * and registers with a Stock object will be notified periodically of the stock quotes. 
 * 
 * The Stock class maintains the list of registered Observer objects in an ArrayList.
 * */

namespace ObserverPatternDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //a stock will fluctuate over time and generates new data every day
            Stock superStock = new Stock();
            //stock monitor will watch the stock periodically because it is registered to a 
            // particular stock when the stock price changes the stock monitor gets updated.
            StockMonitor stockMon = new StockMonitor();
            superStock.attach(stockMon);

           

            //since the stock object only generates new information periodically but doesn't block
            // the main thread, I am using readline to wait for user input rather than just closing
            Console.ReadLine();
        }
    }


    public class Stock
    {
        private ArrayList observers = new ArrayList(); //collection of all observers
        private Random rand = new Random();

        public Stock()
        {
            Timer timer = new Timer(1000);
            //Register an ElapsedEventHandler which will periodically triggere GeneratePrice
            timer.Elapsed += new ElapsedEventHandler(GeneratePrice);
            timer.Start();
        }

        public void attach(Observer observer)
        {
            observers.Add(observer);
        }

        private void GeneratePrice(object sender, ElapsedEventArgs e)
        {
            double price = rand.NextDouble() * 100;
            //notify all attached observers
            foreach (Object o in observers)
            {
                //cast the object 'o' contained in the observers 
                // arraylist as an Observer because that's what it is!
                Observer observer = o as Observer;
                observer.update(price);
            }
        }
    }
    //a contractual interface bewteen the Stock class and its observers
    public interface Observer
    {
        void update(Object data);
    }

    //StockMonitor implements the Observer interface
    public class StockMonitor : Observer
    {
        public void update(Object data)
        {
            Console.WriteLine("Price is " + data);
        }
    }


}
