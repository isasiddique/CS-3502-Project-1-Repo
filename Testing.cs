using System;
using System.Threading;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;

class TicketBooking
{
    private static int availableTickets = 1000; // Increased number of tickets for stress testing
    private static object lockObject = new object();

    public static void BookTicket()
    {
        lock (lockObject)
        {
            if (availableTickets > 0)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} booked a ticket.");
                availableTickets--;
                Console.WriteLine($"Tickets remaining: {availableTickets}");
            }
            else
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} found no tickets available.");
            }
        }
    }
}

class Testing
{
    static void Main(string[] args)
    {
        int numberOfThreads = 1000; // Increased number of threads for stress testing
        Thread[] threads = new Thread[numberOfThreads];
        for (int i = 0; i < numberOfThreads; i++)
        {
            threads[i] = new Thread(new ThreadStart(TicketBooking.BookTicket));
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("All threads have completed execution.");

        // IPC using Named Pipes
        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut))
        {
            Console.WriteLine("Named Pipe Server is waiting for connection...");
            pipeServer.WaitForConnection();

            using (StreamReader reader = new StreamReader(pipeServer))
            using (StreamWriter writer = new StreamWriter(pipeServer) { AutoFlush = true })
            {
                writer.WriteLine("Hello from server!");
                string message = reader.ReadLine();
                Console.WriteLine("Received from client: " + message);
            }
        }
    }
}